/*
    * This file contains the user kernel, which contains all needed functions for managing the users of the program.
    * @license
    *
    * This program is free software; you can redistribute it and/or
    * modify it under the terms of the GNU General Public License as
    * published by the Free Software Foundation; either version 3 of
    * the License, or (at your option) any later version.
    *
    * This program is distributed in the hope that it will be useful, but
    * WITHOUT ANY WARRANTY; without even the implied warranty of
    * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
    * General Public License for more details at
    * http://www.gnu.org/copyleft/gpl.html
    *
    * @author lw1994 <luca@welkr.de>
    *
    * @todo - 2015-02-22: Replace method-argument type from string to enum
*/

using RedundancyLibrary.Domain;
using RedundancyLibrary.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RedundancyLibrary.Core
{
    public class RestfulAccessService
    {
        #region constructors

        public RestfulAccessService(Uri apiUri)
        {
            if (apiUri == null)
                throw new ArgumentNullException("apiUri");
            _apiUri = apiUri;

            _serializer = new Lazy<DataContractJsonSerializer>(() => { return new DataContractJsonSerializer(typeof(IEnumerable<string>)); });
        }

        #endregion

        #region constants

        private const string METHOD_POST = "POST";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string DATA_TEMPLATE = "Content-Disposition: form-data; name=\"{0}\"";

        #endregion

        #region properties

        private readonly Uri _apiUri;

        protected Uri ApiUri { get { return _apiUri; } }

        #region WebRequest

        private HttpWebRequest _webRequest;
        protected HttpWebRequest WebRequest
        {
            get
            {
                if (_webRequest == null || _webRequest.HaveResponse)
                    _webRequest = CreateWebRequest();
                return _webRequest;
            }
        }

        private HttpWebRequest CreateWebRequest()
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(ApiUri);
            request.Method = METHOD_POST;
            request.ContentType = CONTENT_TYPE;

            return request;
        }

        #endregion

        #region Serializer

        private readonly Lazy<DataContractJsonSerializer> _serializer;
        private DataContractJsonSerializer Serializer { get { return _serializer.Value; } }

        #endregion

        #endregion

        #region methods

        public void SendRequestWithRawResult(ApiModule module, string method, IEnumerable<string> arguments, Stream outputStream)
        {
            var statusCode = SendRequest(module, method, arguments, outputStream);
            if (statusCode != HttpStatusCode.OK)
                HandleError(outputStream);
        }

        public T SendRequest<T>(ApiModule module, string method, IEnumerable<string> arguments)
        {
            using (var responseStream = new MemoryStream())
            {
                var statusCode = SendRequest(module, method, arguments, responseStream);
                if (statusCode != HttpStatusCode.OK)
                    HandleError(responseStream);

                return GetResult<T>(responseStream);
            }
        }

        public void SendRequest(ApiModule module, string method, IEnumerable<string> arguments)
        {
            using (var responseStream = new MemoryStream())
            {
                var statusCode = SendRequest(module, method, arguments, responseStream);
                if (statusCode != HttpStatusCode.OK)
                    HandleError(responseStream);
            }
        }

        public T SendFileRequest<T>(ApiModule module, string method, IEnumerable<string> arguments, FileInfo inputFile)
        {
            using (var responseStream = new MemoryStream())
            {
                var statusCode = SendFileRequest(module, method, arguments, inputFile, responseStream);
                if (statusCode != HttpStatusCode.OK)
                    HandleError(responseStream);

                return GetResult<T>(responseStream);
            }
        }

        private HttpStatusCode SendRequest(ApiModule module, string method, IEnumerable<string> arguments, Stream outputStream)
        {
            var request = WebRequest;
            var data = new Dictionary<string, string>
            {
                {"module", module.GetModuleType()},
                {"method", method},
                {"args", GetJsonStringFromArgument(arguments)}
            };

            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                WriteRequest(data, boundary, requestStream);
                WriteValueToStream(boundary + "--", requestStream);
            }

            using (var response = request.TryGetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(outputStream);
                    outputStream.Position = 0;
                }
                return response.StatusCode;
            }
        }

        #region SendFileRequest

        private HttpStatusCode SendFileRequest(ApiModule module, string method, IEnumerable<string> arguments, FileInfo inputFile, Stream outputStream)
        {
            var request = WebRequest;
            var data = new Dictionary<string, string>
            {
                {"module", module.GetModuleType()},
                {"method", method},
                {"args", GetJsonStringFromArgument(arguments)}
            };

            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                WriteRequest(data, boundary, requestStream);
                WriteFileToStream(inputFile, boundary, requestStream);
                WriteValueToStream(boundary + "--", requestStream);
            }

            using (var response = request.TryGetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(outputStream);
                    outputStream.Position = 0;
                }
                return response.StatusCode;
            }
        }

        private void WriteFileToStream(FileInfo inputFile, string boundary, Stream requestStream)
        {
            WriteValueToStream(boundary + Environment.NewLine, requestStream);
            WriteValueToStream(String.Format(DATA_TEMPLATE + "; filename=\"{1}\"{2}", "file", inputFile.Name, Environment.NewLine), requestStream);
            WriteValueToStream(String.Format("Content-Type: {0}{1}{1}", MimeType.Get(inputFile.Extension), Environment.NewLine), requestStream);
            using (var fs = inputFile.OpenRead())
                fs.CopyTo(requestStream);
            WriteValueToStream(Environment.NewLine, requestStream);
        }

        private void WriteRequest(Dictionary<string, string> data, string boundary, Stream requestStream)
        {
            foreach (var d in data)
            {
                WriteValueToStream(boundary + Environment.NewLine, requestStream);
                WriteValueToStream(String.Format(DATA_TEMPLATE + "{1}{1}", d.Key, Environment.NewLine), requestStream);
                WriteValueToStream(d.Value + Environment.NewLine, requestStream);
            }
        }

        private void WriteValueToStream(string value, Stream stream)
        {
            var tmpBuf = Encoding.ASCII.GetBytes(value);
            stream.Write(tmpBuf, 0, tmpBuf.Length);
        }

        #endregion

        #endregion

        #region helper

        private void HandleError(Stream stream)
        {
            using (var sr = new StreamReader(stream))
                throw new WebException(sr.ReadToEnd());
        }

        private string GetJsonStringFromArgument(IEnumerable<string> arguments)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.WriteObject(ms, arguments);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                    return sr.ReadToEnd();
            }
        }

        private string GetStringForRequest(IReadOnlyDictionary<string, string> arguments)
        {
            var first = true;
            var sb = new StringBuilder();

            foreach (var arg in arguments)
            {
                if (first)
                {
                    sb.AppendFormat("{0}={1}", arg.Key, arg.Value);
                    first = false;
                }
                else
                    sb.AppendFormat("&{0}={1}", arg.Key, arg.Value);
            }

            return sb.ToString();
        }

        #endregion

        #region virtual methods

        protected virtual T GetResult<T>(Stream inputStream)
        {
            if (typeof(Stream).IsAssignableFrom(typeof(T)))
                return (T)((object)inputStream); // cast first to object, because you can't cast inputStream directly to (T)

            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("d. MMM yyyy - HH:mm", CultureInfo.GetCultureInfo("en-US"))
            });
            return (T)serializer.ReadObject(inputStream);
        }

        #endregion
    }
}
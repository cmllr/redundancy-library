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

using RedundancyAccessLibrary.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RedundancyAccessLibrary.Core
{
    public abstract class RestfulAccessService<T>
    {
        #region constructors

        public RestfulAccessService(Uri apiUri)
        {
            if (apiUri == null)
                throw new ArgumentNullException("apiUri");
            _apiUri = apiUri;

            _webRequest = new Lazy<HttpWebRequest>(CreateWebRequest);
            _serializer = new Lazy<DataContractJsonSerializer>(() => { return new DataContractJsonSerializer(typeof(IEnumerable<string>)); });
        }

        #endregion

        #region constants

        private const string METHOD_POST = "POST";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";

        #endregion

        #region properties

        private readonly Uri _apiUri;

        protected Uri ApiUri { get { return _apiUri; } }

        #region WebRequest

        private readonly Lazy<HttpWebRequest> _webRequest;
        protected HttpWebRequest WebRequest { get { return _webRequest.Value; } }

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

        public T SendRequest(ApiModule module, string method, IEnumerable<string> arguments)
        {
            var data = new Dictionary<string, string>
            {
                {"module", module.ToString()},
                {"method", method},
                {"args", GetJsonStringFromArgument(arguments)}
            };
            var dataString = GetStringForRequest(data);

            using (var requestStream = new StreamWriter(WebRequest.GetRequestStream()))
                requestStream.Write(dataString);

            using (var response = WebRequest.TryGetResponseStream())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        HandleError(responseStream);

                    return GetResult(responseStream);
                }
            }
        }

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

        protected abstract T GetResult(Stream responseStream);

        #endregion
    }
}
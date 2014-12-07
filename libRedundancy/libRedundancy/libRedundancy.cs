using libRedundancy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace libRedundancy
{
    public enum Error
    {
        NoResult,
        RequestError,
    }
    /// <summary>
    /// Redundancy C# client library
    /// Requires JSON.NET 
    /// </summary>
    public class libRedundancy
    {
        /// <summary>
        /// The target path to api.inc.php
        /// </summary>
        public Uri Target { get; set; }
        /// <summary>
        /// The last occured error code
        /// </summary>
        public int LastErrorCode { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="target">The target api path</param>
        public libRedundancy(Uri target)
        {
            this.Target = target;
        }
        /// <summary>
        /// Send POST Request to the server
        /// </summary>
        /// <param name="data">a Name Value collection of the data which should be send to the server</param>
        /// <returns>The response data</returns>
        private string POST(NameValueCollection data)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, String.Format("libRedundancy {0}", Assembly.GetEntryAssembly().GetName().Version.ToString()));
                var response = client.UploadValues(this.Target, data);
                var responseString = Encoding.Default.GetString(response);
                return responseString;
            }
        }
        /// <summary>
        /// Get the object from the JSON data
        /// </summary>
        /// <typeparam name="T">The type to be deserialized</typeparam>
        /// <param name="json">The JSON string</param>
        /// <returns>The object</returns>
        private T GetObject<T>(string json)
        {
            int dummy;
            if (int.TryParse(json, out dummy))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// Request the objects
        /// </summary>
        /// <typeparam name="T">The type which should be returned</typeparam>
        /// <param name="module">The module name</param>
        /// <param name="method">The method name</param>
        /// <param name="args">the arguments</param>
        /// <returns>The object</returns>
        public T Request<T>(string module, string method, string[] args)
        {
            string response = this.DoRequest(module, method, args);           
            return this.GetObject<T>(response);
        }
        /// <summary>
        /// Do the request itself
        /// </summary>
        /// <param name="module">The module name</param>
        /// <param name="method">The method name</param>
        /// <param name="args">The arguments</param>
        /// <returns>A string with the server output</returns>
        private string DoRequest(string module, string method, string[] args)
        {
            try
            {
                NameValueCollection postData = new NameValueCollection();
                postData["module"] = module;
                postData["method"] = method;
                postData["args"] = JsonConvert.SerializeObject(args);
                string response = this.POST(postData);
                if (string.IsNullOrEmpty(response))
                    return Error.NoResult.ToString();
                else
                    return response;
            }
            catch (WebException ex)
            {
                var response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                LastErrorCode = int.Parse(response);
                return response;
            }
        }
        /// <summary>
        /// Upload the file
        /// </summary>
        /// <param name="root">The root dir id</param>
        /// <param name="token">The session token</param>
        /// <param name="filePath">The absolute path to the file</param>
        /// <returns>The result of the upload. Errorcode stored in libRedundancy::LastErrorCode</returns>
        public bool UploadFile(int root,string token, string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            using (var fstream = File.Open(filePath, FileMode.Open))
            {
                UploadFile file = new UploadFile
                {
                    Name = "file",
                    Filename = new FileInfo(filePath).Name,
                    ContentType = MimeType.Get(new FileInfo(filePath).Extension.ToLower()),
                    Stream = fstream
                };
                var values = new NameValueCollection();
                values["module"] = "Kernel.FileSystemKernel";
                values["method"] = "UploadFile";
                values["args"] = JsonConvert.SerializeObject(new string[] { root.ToString(), token });      
                string response =UploadFiles(this.Target.ToString(), file, values);
                if (response == "true")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Upload a file. Temporary solution by http://dev.bratched.com/en/uploading-multiple-files-with-c/
        /// </summary>
        /// <param name="address">The target server </param>
        /// <param name="file">The file</param>
        /// <param name="values">The POST-Data</param>
        /// <returns>The server result</returns>
        private string UploadFiles(string address, UploadFile file, NameValueCollection values)
        {
            var request = HttpWebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }


                var bffr = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(bffr, 0, bffr.Length);
                bffr = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                requestStream.Write(bffr, 0, bffr.Length);
                bffr = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                requestStream.Write(bffr, 0, bffr.Length);
                file.Stream.CopyTo(requestStream);
                bffr = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(bffr, 0, bffr.Length);
                

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }
            try
            {
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var stream = new MemoryStream())
                {
                    responseStream.CopyTo(stream);
                    return System.Text.Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch (WebException ex)
            {
                var response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                LastErrorCode = int.Parse(response);
                return response;
            }
           
        }
    }
}

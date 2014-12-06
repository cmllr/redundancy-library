using libRedundancy.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Net;
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
        private string DoRequest(string module, string method,string[] args)
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
            catch
           {
               return Error.RequestError.ToString();
           }            
        }
        public bool UploadFile()
        {
            throw new NotImplementedException();
        }
    }
}

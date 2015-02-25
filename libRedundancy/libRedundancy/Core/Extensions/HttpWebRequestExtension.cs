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
    * @todo  
*/

using System.Net;

namespace RedundancyLibrary.Core
{
    /// <summary>
    /// This class is needed, cause the original "GetResponse"-method throws an exception if HttpStatusCode != 200
    /// </summary>
    internal static class HttpWebRequestExtension
    {
        public static HttpWebResponse TryGetResponseStream(this HttpWebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                    return (HttpWebResponse)ex.Response;

                throw;
            }
        }
    }
}

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

using RedundancyLibrary.Domain;
using System;
using System.IO;

namespace RedundancyLibrary.Core
{
    /// <summary>
    /// Baseclass for Kernel classes
    /// </summary>
    public abstract class Kernel
    {
        #region constructors

        public Kernel(ApiModule module, Uri apiUri)
        {
            _module = module;
            _apiUri = apiUri;
            _restfulAccessService = new Lazy<RestfulAccessService>(() => { return new RestfulAccessService(apiUri); });
        }

        #endregion

        #region fields

        private readonly ApiModule _module;

        #endregion

        #region properties

        private readonly Lazy<RestfulAccessService> _restfulAccessService;
        private RestfulAccessService RestfulAccessService { get { return _restfulAccessService.Value; } }

        private readonly Uri _apiUri;
        protected Uri ApiUri { get { return _apiUri; } }

        #endregion

        #region methods

        protected virtual void SendRequestWithRawResult(string method, Stream result, params string[] args)
        {
            RestfulAccessService.SendRequestWithRawResult(_module, method, args, result);
        }

        protected virtual T SendRequest<T>(string method, params string[] args)
        {
            return RestfulAccessService.SendRequest<T>(_module, method, args);
        }

        protected virtual void SendRequest(string method, params string[] args)
        {
            RestfulAccessService.SendRequest(_module, method, args);
        }

        protected virtual T SendFileRequest<T>(string method, FileInfo file, params string[] args)
        {
            return RestfulAccessService.SendFileRequest<T>(_module, method, args, file);
        }

        #endregion
    }
}
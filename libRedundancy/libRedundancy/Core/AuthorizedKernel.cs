/*
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
using System.Collections.Generic;
using System.Security;
namespace RedundancyLibrary.Core
{
    /// <summary>
    /// Baseclass for kernel classes with authorization
    /// </summary>
    public abstract class AuthorizedKernel : Kernel
    {
        #region constructors

        public AuthorizedKernel(ApiModule apiModule, Uri apiUri)
            : base(apiModule, apiUri)
        {
        }

        #endregion

        #region authentification methods

        protected void CheckAuthorization()
        {
            var token = Authentification.Token;

            if (token == null || !token.IsAlive())
                throw new SecurityException();
        }

        protected string GetTokenString()
        {
            CheckAuthorization();
            return Authentification.Token.TokenString;
        }

        #endregion

        #region override: Kernel

        protected override T SendRequest<T>(string method, IEnumerable<string> args)
        {
            CheckAuthorization();
            return base.SendRequest<T>(method, args);
        }

        #endregion
    }
}
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
using RedundancyLibrary.Kernels;
using System;

namespace RedundancyLibrary.Core
{
    public static class Authentification
    {
        public static bool Authorize(string username, string password, string apiUrl)
        {
            var userKernel = new UserKernel(apiUrl);
            var tokenString = userKernel.Login(username, password, false);

            if (!String.IsNullOrEmpty(tokenString))
                Token = new Token(tokenString, false);

            return Token != null;
        }

        #region fields

        internal static Token Token { get; private set; }

        #endregion
    }
}
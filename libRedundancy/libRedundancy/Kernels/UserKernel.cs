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

using RedundancyLibrary.Core;
using RedundancyLibrary.Domain;
using RedundancyLibrary.Models;
using System;

namespace RedundancyLibrary.Kernels
{
    public sealed class UserKernel : Kernel
    {
        #region constants

        private const string METHOD_LOGIN = "LogIn";
        private const string METHOD_REGISTERUSER = "RegisterUser";

        #endregion

        #region constructors

        public UserKernel(string apiUrl)
            : base(ApiModule.UserKernel, new Uri(apiUrl))
        {
        }

        #endregion

        #region methods

        internal string Login(string username, string password, bool stayLoggedIn = true)
        {
            var args = new string[] { username, password, stayLoggedIn.ToString() };
            return SendRequest<string>(METHOD_LOGIN, args);
        }

        public User CreateNewUser(UserCreationInfo creationInfo)
        {
            return SendRequest<User>(METHOD_REGISTERUSER, creationInfo.LoginName, creationInfo.DisplayName, creationInfo.EmailAddress, creationInfo.Password);
        }

        #endregion
    }
}
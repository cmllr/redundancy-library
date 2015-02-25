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

using System;

namespace RedundancyLibrary.Domain
{
    internal sealed class Token
    {
        #region constructors

        public Token(string tokenString, bool staysAlive)
        {
            if (String.IsNullOrEmpty(tokenString))
                throw new ArgumentNullException("tokenString");

            _tokenString = tokenString;
            _creationDate = DateTime.Now;
            _expiersIn = (staysAlive) ? -1 : TimeSpan.FromMinutes(20).Seconds;
        }

        #endregion

        #region properties

        private readonly string _tokenString;
        public string TokenString { get { return _tokenString; } }

        private readonly DateTime _creationDate;
        public DateTime CreationDate { get { return _creationDate; } }

        #endregion

        #region fields

        private int _expiersIn = TimeSpan.FromMinutes(20).Seconds; // in seconds

        #endregion

        #region methods

        public bool IsAlive()
        {
            return (_expiersIn == -1) || CreationDate.AddSeconds(_expiersIn) >= DateTime.Now;
        }

        #endregion
    }
}
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

using RedundancyLibrary.Core;
using RedundancyLibrary.Domain;
using RedundancyLibrary.Models;
using System;
using System.Collections.Generic;

namespace RedundancyLibrary.Kernels
{
    /// <summary>
    /// Provides functions to access Redundancys file system
    /// </summary>
    public sealed class FileSystemKernel : AuthorizedKernel
    {
        #region constants

        private const string METHOD_GETCONTENT = "GetContent";
        private const string METHOD_CREATEDIRECTORY = "CreateDirectory";
        private const string METHOD_UPLOADFILE = "UploadFile";

        #endregion

        #region constructors

        public FileSystemKernel(string apiUrl)
            : base(ApiModule.FileSystemKernel, new Uri(apiUrl))
        {
        }

        #endregion

        #region methods

        #region GetDirectoryContent

        /// <summary>
        /// Gets the content of root directory
        /// </summary>
        public IEnumerable<FileSystemItem> GetDirectoryContent()
        {
            return GetDirectoryContent("/"); // "/" -> root path
        }

        /// <summary>
        /// Gets the content in of given directory
        /// </summary>
        /// <param name="absolutDirPath">Absolut directory path</param>
        public IEnumerable<FileSystemItem> GetDirectoryContent(string absolutDirPath)
        {
            return SendRequest<List<FileSystemItem>>(METHOD_GETCONTENT, absolutDirPath, GetTokenString());
        }

        #endregion

        #region CreateDirectory

        public void CreateDirectory(string name, int rootDirectoryId)
        {
            SendRequest(METHOD_CREATEDIRECTORY, name, rootDirectoryId.ToString(), GetTokenString());
        }

        #endregion

        #region UploadFile

        public bool UploadFile(int rootDirectoryId)
        {
            return SendRequest<bool>(METHOD_UPLOADFILE, rootDirectoryId.ToString(), GetTokenString());
        }

        #endregion

        #endregion
    }
}
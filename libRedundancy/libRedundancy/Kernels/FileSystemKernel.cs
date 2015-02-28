﻿/*
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
using System.IO;

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
        private const string METHOD_DELETEDIRECTORY = "DeleteDirectory";
        private const string METHOD_GETFOLDERLIST = "GetFolderList";

        private const string METHOD_UPLOADFILE = "UploadFile";
        private const string METHOD_DELETEFILE = "DeleteFile";

        private const string METHOD_GETABSOLUTEPATHBYID = "GetAbsolutePathById";

        #endregion

        #region constructors

        public FileSystemKernel(string apiUrl)
            : base(ApiModule.FileSystemKernel, new Uri(apiUrl))
        {
        }

        #endregion

        #region methods

        public string GetAbsolutePath(int id)
        {
            return SendRequest<string>(METHOD_GETABSOLUTEPATHBYID, id.ToString(), GetTokenString());
        }

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

        #region Directory

        #region CreateDirectory

        public void CreateDirectory(string name, int rootDirectoryId)
        {
            SendRequest(METHOD_CREATEDIRECTORY, name, rootDirectoryId.ToString(), GetTokenString());
        }

        #endregion

        public IEnumerable<string> GetFolderList()
        {
            return SendRequest<IEnumerable<string>>(METHOD_GETFOLDERLIST, GetTokenString());
        }

        public bool DeleteDirectory(int entryId)
        {
            var path = GetAbsolutePath(entryId);
            return SendRequest<bool>(METHOD_DELETEDIRECTORY, path, GetTokenString());
        }

        public bool DeleteDirectory(string path)
        {
            return SendRequest<bool>(METHOD_DELETEDIRECTORY, path, GetTokenString());
        }

        #endregion

        #region File

        #region UploadFile

        public bool UploadFile(int rootDirectoryId, FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException();

            return SendFileRequest<bool>(METHOD_UPLOADFILE, file, rootDirectoryId.ToString(), GetTokenString());
        }

        public bool DeleteFile(int entryId)
        {
            var path = GetAbsolutePath(entryId);
            return SendRequest<bool>(METHOD_DELETEFILE, path, GetTokenString());
        }

        public bool DeleteFile(string path)
        {
            return SendRequest<bool>(METHOD_DELETEFILE, path, GetTokenString());
        }

        #endregion

        #endregion

        #endregion
    }
}
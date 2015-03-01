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
using System.IO;
using System.Text;

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
        private const string METHOD_GETCONTENTOFFILE = "GetContentOfFile";

        private const string METHOD_GETABSOLUTEPATHBYID = "GetAbsolutePathById";

        private const string METHOD_MOVEENTRYBYID = "MoveEntryById";
        private const string METHOD_MOVEENTRY = "MoveEntry";
        private const string METHOD_COPYENTRYBYID = "CopyEntryById";
        private const string METHOD_COPYENTRY = "CopyEntry";
        private const string METHOD_GETENTRYBYABSOLUTEPATH = "GetEntryByAbsolutePath";
        private const string METHOD_RENAMEENTRY = "RenameEntry";
        private const string METHOD_ISENTRYEXISTING = "IsEntryExisting";
        private const string METHOD_GETENTRYBYID = "GetEntryById";

        private const string METHOD_STARTZIPCREATION = "StartZipCreation";
        private const string METHOD_GETLASTCHANGEDOFFILESYSTEM = "GetLastChangesOfFileSystem";


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
        /// Gets the content of the given directory
        /// </summary>
        /// <param name="dirId">Directory-ID</param>
        /// <returns></returns>
        public IEnumerable<FileSystemItem> GetDirectoryContent(int dirId)
        {
            var path = GetAbsolutePath(dirId);
            return GetDirectoryContent(path);
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

        public Stream GetFileContent(int fileId)
        {
            var entry = GetEntry(fileId);
            if (entry == null)
                throw new FileNotFoundException(String.Format("File with ID \"{0}\" not found.", fileId));

            if (entry.MimeType == "innode/directory")
                throw new NotSupportedException("Entry isn't file.");

            var ms = new MemoryStream();
            SendRequestWithRawResult(METHOD_GETCONTENTOFFILE, ms, entry.Hash, GetTokenString());
            ms.Position = 0;
            return ms;
        }

        #endregion

        #region entries

        public bool MoveEntryById(int sourceId, int targetId)
        {
            var path = GetAbsolutePath(targetId);
            return MoveEntryById(sourceId, path);
        }

        public bool MoveEntryById(int sourceId, string targetPath)
        {
            return SendRequest<bool>(METHOD_MOVEENTRYBYID, sourceId.ToString(), targetPath, GetTokenString());
        }

        public bool MoveEntry(string sourcePath, string targetPath)
        {
            return SendRequest<bool>(METHOD_MOVEENTRY, sourcePath, targetPath, GetTokenString());
        }

        public bool CopyEntryById(int sourceId, int targetId)
        {
            var path = GetAbsolutePath(targetId);
            return CopyEntryById(sourceId, path);
        }

        public bool CopyEntryById(int sourceId, string targetPath)
        {
            return SendRequest<bool>(METHOD_COPYENTRYBYID, sourceId.ToString(), targetPath, GetTokenString());
        }

        public bool CopyEntry(string sourcePath, string targetPath)
        {
            return SendRequest<bool>(METHOD_COPYENTRY, sourcePath, targetPath, GetTokenString());
        }

        public FileSystemItem GetEntry(string absolutePath)
        {
            return SendRequest<FileSystemItem>(METHOD_GETENTRYBYABSOLUTEPATH, absolutePath, GetTokenString());
        }

        public FileSystemItem GetEntry(int id)
        {
            return SendRequest<FileSystemItem>(METHOD_GETENTRYBYID, id.ToString(), GetTokenString());
        }

        public bool RenameEntry(int id, string newName)
        {
            return SendRequest<bool>(METHOD_RENAMEENTRY, id.ToString(), newName, GetTokenString());
        }

        public bool ExistsEntry(string name, int rootDirId)
        {
            return SendRequest<bool>(METHOD_ISENTRYEXISTING, name, rootDirId.ToString(), GetTokenString());
        }

        #endregion

        #region filesystem
        
        public string CreateZip(int entryToZip, int rootFolderId)
        {
            return SendRequest<string>(METHOD_STARTZIPCREATION, entryToZip.ToString(), GetTokenString(), rootFolderId.ToString());
        } 

        public IEnumerable<FileSystemChangeInfo> GetLastChanges()
        {
            return SendRequest<IEnumerable<FileSystemChangeInfo>>(METHOD_GETLASTCHANGEDOFFILESYSTEM, GetTokenString());
        }

        #endregion

        #endregion
    }
}
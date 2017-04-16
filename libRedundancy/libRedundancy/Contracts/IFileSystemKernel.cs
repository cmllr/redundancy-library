namespace RedundancyLibrary.Contracts
{
    public interface IFileSystemKernel : IKernel
    {
        bool CopyEntry(string sourcePath, string targetPath);
        bool CopyEntryById(int sourceId, int targetId);
        bool CopyEntryById(int sourceId, string targetPath);
        void CreateDirectory(string name, int rootDirectoryId);
        string CreateZip(int entryToZip, int rootFolderId);
        bool DeleteDirectory(int entryId);
        bool DeleteDirectory(string path);
        bool DeleteFile(int entryId);
        bool DeleteFile(string path);
        bool ExistsEntry(string name, int rootDirId);
        string GetAbsolutePath(int id);
        System.Collections.Generic.IEnumerable<RedundancyLibrary.Models.FileSystemItem> GetDirectoryContent();
        System.Collections.Generic.IEnumerable<RedundancyLibrary.Models.FileSystemItem> GetDirectoryContent(int dirId);
        System.Collections.Generic.IEnumerable<RedundancyLibrary.Models.FileSystemItem> GetDirectoryContent(string absolutDirPath);
        RedundancyLibrary.Models.FileSystemItem GetEntry(int id);
        RedundancyLibrary.Models.FileSystemItem GetEntry(string absolutePath);
        System.IO.Stream GetFileContent(int fileId);
        System.Collections.Generic.IEnumerable<string> GetFolderList();
        System.Collections.Generic.IEnumerable<RedundancyLibrary.Models.FileSystemChangeInfo> GetLastChanges();
        bool MoveEntry(string sourcePath, string targetPath);
        bool MoveEntryById(int sourceId, int targetId);
        bool MoveEntryById(int sourceId, string targetPath);
        bool RenameEntry(int id, string newName);
        bool UploadFile(int rootDirectoryId, System.IO.FileInfo file);
    }
}


using RedundancyLibrary.Core;
using RedundancyLibrary.Kernels;
using System;

namespace RedundancyLibrary.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "ApiTest";
            string password = "apitest";
            string target = "http://localhost/Includes/api.inc.php";

            var authOk = Authentification.Authorize(userName, password, target);

            var fileKernel = new FileSystemKernel(target);
            var filesBefore = fileKernel.GetDirectoryContent();
            fileKernel.CreateDirectory("Test", -1);
            var filesAfter = fileKernel.GetDirectoryContent();


            Console.ReadLine();

            ////Initalize the api
            //RedundancyAccessLibrary lib = new RedundancyAccessLibrary(new Uri(target));
            ////Get the Version
            //string version = lib.Request<string>("Kernel", "GetVersion", new string[0]);
            ////Authentification sample. You will get the session token
            //string token = lib.Request<String>("Kernel.UserKernel", "LogIn", new string[] { userName, password, "true" });
            ////Get the user JSON data
            //User userObject = lib.Request<User>("Kernel.UserKernel", "GetUser", new string[] { token });
            ////Get the file entries of your root folder
            //List<FileSystemItem> entries = lib.Request<List<FileSystemItem>>("Kernel.FileSystemKernel", "GetContent", new string[] { "/", token });



            ////Create a new folder
            //bool newDir = lib.Request<bool>("Kernel.FileSystemKernel", "CreateDirectory", new string[] { "FileUploads", "-1", token });
            ////Rename it (get the ID, then rename)
            //FileSystemItem newFolder = lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { "/FileUploads/", token });
            ////Do the renaming itself
            //bool rename = lib.Request<bool>("Kernel.FileSystemKernel", "RenameEntry", new string[] { newFolder.ID.ToString(), "renamedFolder", token });
            ////Get the properties of the now renamed folder
            //FileSystemItem renamedFolder = lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { "/renamedFolder/", token });

            ////Upload a file to the renamed folder
            //bool upload = lib.UploadFile(renamedFolder.ID, token, @"C:\Users\rdcy\sample.doc");
        }
    }
}

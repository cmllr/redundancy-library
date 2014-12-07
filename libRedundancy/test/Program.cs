using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libRedundancy.Models;
namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "username";
            string password = "password";
            string target = "http://server//Includes/api.inc.php";

            //Initalize the api
            libRedundancy.libRedundancy lib = new libRedundancy.libRedundancy(new Uri(target));       
            //Get the Version
            string version = lib.Request<string>("Kernel", "GetVersion", new string[0]);
            //Authentification sample. You will get the session token
            string token = lib.Request<String>("Kernel.UserKernel", "LogIn", new string[] { userName, password, "true" });
            //Get the user JSON data
            User userObject = lib.Request<User>("Kernel.UserKernel", "GetUser", new string[] { token });
            //Get the file entries of your root folder
            List<FileSystemItem> entries =  lib.Request<List<FileSystemItem>>("Kernel.FileSystemKernel", "GetContent", new string[] {"/", token });



            //Create a new folder
            bool newDir = lib.Request<bool>("Kernel.FileSystemKernel", "CreateDirectory", new string[] { "FileUploads", "-1", token });
            //Rename it (get the ID, then rename)
            FileSystemItem newFolder = lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { "/FileUploads/", token });
            //Do the renaming itself
            bool rename = lib.Request<bool>("Kernel.FileSystemKernel", "RenameEntry", new string[] { newFolder.ID.ToString(), "renamedFolder", token });
            //Get the properties of the now renamed folder
            FileSystemItem renamedFolder = lib.Request<FileSystemItem>("Kernel.FileSystemKernel", "GetEntryByAbsolutePath", new string[] { "/renamedFolder/", token });

            //Upload a file to the renamed folder
            bool upload = lib.UploadFile(renamedFolder.ID, token, @"C:\Users\rdcy\sample.doc");
        }
    }
}

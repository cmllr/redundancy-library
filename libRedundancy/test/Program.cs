using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libRedundancy.Classes;
namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initalize the api
            libRedundancy.libRedundancy lib = new libRedundancy.libRedundancy(new Uri("http://redundancy.pfweb.eu/Demo/Includes/api.inc.php"));       
            //Get the Version
            string version = lib.Request<string>("Kernel", "GetVersion", new string[0]);
            //Authentification sample. You will get the session token
            string token = lib.Request<String>("Kernel.UserKernel", "LogIn", new string[] { "demo", "demo123", "true" });
            //Get the user JSON data
            User userObject = lib.Request<User>("Kernel.UserKernel", "GetUser", new string[] { token });
            //Get the file entries of your root folder
            List<FileSystemItem> entries =  lib.Request<List<FileSystemItem>>("Kernel.FileSystemKernel", "GetContent", new string[] {"/Test/", token });            
        }
    }
}

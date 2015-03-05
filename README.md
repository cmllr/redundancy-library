redundancy-library
=============
[![Build Status](https://travis-ci.org/Redundancycloud/redundancy-library.svg?branch=master)](https://travis-ci.org/Redundancycloud/redundancy-library)
![Image of R2](https://raw.githubusercontent.com/squarerootfury/redundancy/Lenticularis/nys/Views/img/logoWithText.png)

Core client library to communicate with Redundancy servers.

Requirements
============
* [JSON.NET]
* .NET Framework 4+
* Redundancy server version 1.9.15+

[JSON.NET]:http://james.newtonking.com/json

Further documentation
=====================

See http://redundancy.pfweb.eu/doc/1.9.15/index.html for more details

Sample
======

Authentification:
```C
string userName = "username";
string password = "password";
string target = "http://server//Includes/api.inc.php";

// this method authorizes the given user and returns true or false
var authOk = Authentification.Authorize(userName, password, target);
```

The access to the Redundancy-Server will be done with diffrent kernel classes.
At the moment there is only the "FileSystemKernel" implemented. Other ones following in next versions.

Example for accessing the Redundancy-FileSystem:
```C
var fileSystemKernel = new FileSystemKernel(target);

// creating new dir in root direcotry
fileSystemKernel.CreateDirectory(-1, "Dir-Name");

// move any entry (file or direcotry)
var result = fileSystemKernel.MoveEntryById(1, 2);

// Upload file
using (var fileInfo = new FileInfo(@"DataPath"))
{
  var result = fileSystemKernel.UploadFile(-1, fileInfo);
}
```

License
=======

See [LICENSE](LICENSE) for more details

# ThesisBank

_ThesisBank_ project in Analysis, Design, and Software Architecture (BSDA) 3. semester at IT University of Copenhagen.

## Project description

_ThesisBank_ is a project bank that allow students to collaborate with teachers on a thesis. 
This vertical slice allows students to find a thesis and to apply for it, 
it allows teachers to confirm or reject the given application,
and finally it allows the student to see the change in status and accept the collaboration on the chosen thesis. 

## Commands to run project
The project runs in a docker container, but some setup is still required.
The steps are outlined below.

### Powershell setup scripts
  
#### **1. Setup .locals folder**

To set up a .locals folder, simply run the powershell script   ```run_1.ps1``` for windows or ```run_mac_1.ps1```  for mac operating systems.<br>
After the script has ran, it is important to navigate to the folder that has been created ```.local``` access the file ```db_password.txt```:
  ```txt
  GUID

"----"

12312asd3-12asd4-215asd215--1215asd21
```

It is essential that the `GUID` text, and the `slashes` are _deleted_ before proceeding. This file should **only contain the sequence of characters and integers**, 
as shown here:

```txt
12312asd3-12asd4-215asd215--1215asd21
```

#### **2. Create and run the docker container**

Before running the next script, it is necessary to delete or pause any containers running on port 1433:1433.
Please delete the `server` image, if it has already been previously created.

Then run the  ```run_2.ps1``` command, for both windows and mac OS, and agree to the permissions that are asked for. 
Dependencies are then downloaded, and the docker container created.

The application can be accesed through  ```http://localhost:5077```.<br>
It will redirect you automatically to a secure protocol ```https://localhost:7213```.

In case, the above **does not work**; running the script: `setup-script.ps1` or `setup-script_mac.ps1` depending on the OS. 
Then navigate to the `Server` directory and write  ```dotnet run``` instead. 
You can connect to the web-application through the localhost ```http://localhost:7207``.

#### **3. Log in to the system**

**To log in as a _Student_:** Press log in in the the top right corner.

Enter the e-mail: `StudentPeterSWU@hotmail.com`.<br>
And the password: `SWUITU2021`. 

Press login. All the features as a student will now be available.

`Open a new page, but in a different browser` (or use private browsing, incognito mode) to esnure the possibily of seeing both a student and teacher point of view.

**Log in as a _Teacher_:** Press log in at the the top right corner.

Enter the e-mail: `TeacherAlbertJones@hotmail.com`.<br> 
And the password: `SWUITU2021`. 

Press login. All featues as a teacher will now be available. By applying for a thesis on the student side, the teacher page can be refreshed and the applicattion is now visisble. Accepting the thesis changes the student point of view and vice versa. 

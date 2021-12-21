# ThesisBank

_ThesisBank_ project in Analysis, Design, and Software Architecture (BSDA) 3. semester at IT University of Copenhagen.

## Project description

Starting from the Project bank description which involves teachers and students. Students find a thesis to apply, and teachers confirm or reject the given student. 

## Commands to run project
The project runs in a docker container, but some slight initial setup is still required.
The steps are outlined below.

### Powershell setup scripts
  
#### **1. Setup .locals folder**

To set up a .locals folder, simply run the powershell script   ```run_1.ps1``` or ```run_mac_1.ps1```  depending on what OS you are running.<br>
After the script has run, it is important to navigate to the ```db_password.txt``` file. It it as so:
  ```txt
  GUID

"----"

12312asd3-12asd4-215asd215--1215asd21
```

It is essential that the `GUID` text, and the slashes are _deleted_ before proceeding. This file should **only contain the sequence of characters and integers**, as seen at the bottom of the document.

#### **2. Create and run the docker container**

Before running the next script, it is needed to delete or pause any containers running on port 1433:1433.
Also delete the server image, if it is locally downloaded.

Then run the  ```run_2.ps1``` command, and agree to the permissions that are asked for. 
Dependencies is then be downloaded, and the container created.

The application can be accesed through  ```localhost:5077```.

#### **3. Log in to the system**

**To log in as a _Student_:** Press log in in the the top right corner.

Enter the e-mail: `StudentPeterSWU@hotmail.com`.<br>
And the password: `SWUITU2021`. 

Press login. All the features as a student will now be available.

Open a new browser, but make sure it is another browser (or use private browsing, incognito mode) to esnure the possibily of seeing both a student and teacher point of view.

**Log in as a _Teacher_:** Press log in at the the top right corner.

Enter the e-mail: `TeacherAlbertJones@hotmail.com`.<br> 
And the password: `SWUITU2021`. Press login. 

All featues as a teacher will now be available. By applying for a thesis on the student side, the teacher page can be refreshed and the applicattion is now visisble. Accepting the thesis changes the student point of view and vice versa. 

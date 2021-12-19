# ThesisBank

_ThesisBank_ project in Analysis, Design, and Software Architecture (BSDA) 3. semester at IT University of Copenhagen.

## Project description

Starting from the Project bank description which involves teachers and students. Students find a thesis to apply, and teachers confirm or reject the given student. 

## Commands to run project
The project runs in a docker container, but some slight initial setup is still required.
The steps are outlined below.

  **`Powershell setup scripts`**
  
    1. Setup .locals folder
  To set up a .locals folder, simply run the powershell script   ``` run_1.ps1 ``` or ```run_mac_1.ps1```  depending on what system you're running.
  
  Afer the script has been run, it is _very_ important that you navigate to the ```db_password.txt``` file. It will likely look like this:
  ```
  GUID

"----"

12312asd3-12asd4-215asd215--1215asd21(your db password)
```
It is essential that the GUID text, and the slashes are deleted before proceeding.  
**This file should only contain the sequence of characters and integers, at the bottom of the document**

    2. Create and run the docker container
Before running the next script, you will need to delete or pause any containers you have running on port 1433:1433.
Also delete the "server" image, if you have it locally downloaded.
Then run the  ``` run_2.ps1 ``` command, and give any permissions that it may ask. 
Dependencies will be downloaded, and the container will be created.

The application can be accesed through  ``` localhost:5077 ```

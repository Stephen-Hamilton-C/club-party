import os
import sys

## Start in script directory
scriptDir = os.path.dirname(__file__)
os.chdir(scriptDir)

configPath = os.path.join(scriptDir, "deploy-config.txt")

# Reset option
if len(sys.argv) > 1 and sys.argv[1] == "-r":
    print("Clearing configuration...")
    if os.path.exists(configPath):
        os.remove(configPath)

## Load config file
if not os.path.exists(configPath):
    # Get buildPath
    buildPath = ""
    # Do-while loop
    while True:
        buildPath = input("Where is the WebGL build folder? (Absolute path or relative to "+scriptDir+"): ")
        if os.path.exists(buildPath):
            break
        else:
            print("Build path is not valid.")

    print()
    print("Make sure you get these next prompts correct. Getting them wrong will result in errors.")
    print("You can clear these errors by either running this script again with -r or deleting deploy-config.txt")
    print()
    stageDir = input("Where is the staging directory on the remote server? (Absolate path): ")
    user = input("What is the name of your user on the remote server?: ")
    url = input("What is the url/ip address of the remote server?: ")
    remoteUrl = user+"@"+url
    with open(configPath, "w") as configFile:
        configFile.write(buildPath+"\n")
        configFile.write(stageDir+"\n")
        configFile.write(remoteUrl+"\n")
else:
    with open(configPath, "r") as configFile:
        buildPath = configFile.readline().strip()
        stageDir = configFile.readline().strip()
        remoteUrl = configFile.readline().strip()

print()
print("Build path is located at "+buildPath)
print("Remote staging directory is at "+stageDir)
print("Remote URL/IP Address is "+remoteUrl)
print()

## Compress WebGL build into zip
def get_all_file_paths(directory):
  
    # initializing empty file paths list
    file_paths = []
  
    # crawling through directory and subdirectories
    for root, directories, files in os.walk(directory):
        for filename in files:
            # join the two strings in order to form the full filepath.
            filepath = os.path.join(root, filename)
            file_paths.append(os.path.relpath(filepath))
  
    # returning all file paths
    return file_paths 

print("Compressing build into zip...")
from zipfile import ZipFile
os.chdir(buildPath)
zipArchive = "club-party_webgl.zip"
# Delete old zip if present
if os.path.exists(zipArchive):
    os.remove(zipArchive)

# Create zip
filePaths = get_all_file_paths(buildPath)
with ZipFile(zipArchive, "w") as zip:
    for file in filePaths:
        zip.write(file);
print("Created "+zipArchive)

os.chdir(scriptDir)

## Deploy to server
import subprocess
print("Uploading to server...")
try:
    print(zipArchive+", "+str(os.path.exists(zipArchive)))
    print(remoteUrl)
    print(stageDir)
    print(buildPath)

    scpProcess = subprocess.run(["scp", zipArchive, remoteUrl+":"+stageDir], cwd=buildPath)
    if scpProcess.returncode != 0:
        print()
        print("FATAL: scp failed. Perhaps there is no internet connection, or a bad staging directory was provided.")
        print("If this error persists, run this script again with -r to reset configuration.")
        sys.exit(scpProcess.returncode)

    print("Deploying to web server...")
    sshProcess = subprocess.run(["ssh", "-C", remoteUrl, "\""+stageDir+"/server-deploy.py\""])
    if sshProcess.returncode != 0:
        print()
        print("FATAL: ssh failed. Perhaps there is no internet connection, or a bad remote url or user was provided.")
        print("If this error persists, run this script again with -r to reset configuration.")
        sys.exit(sshProcess.returncode)
except FileNotFoundError:
    print()
    print("FATAL: Could not execute scp and/or ssh!")
    if os.name == "nt":
        print("Ensure you have Windows Subsystem for Linux installed.")
        print("See https://learn.microsoft.com/en-us/windows/wsl/install for instructions.")
    else:
        print("Ensure you have ssh and scp installed")
    sys.exit(1)

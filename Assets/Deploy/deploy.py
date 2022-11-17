import os
import sys

## Start in script directory
scriptDir = os.path.dirname(__file__)
os.chdir(scriptDir)

configPath = os.path.join(scriptDir, "deploy-config.txt")
secrets = "deploy-secrets.txt"

# Reset option
if len(sys.argv) > 1 and sys.argv[1] == "-r":
    print("Clearing configuration and secrets...")
    if os.path.exists(configPath):
        os.remove(configPath)
    if os.path.exists(secrets):
        os.remove(secrets)

## Get build path from config file or prompt user if invalid
def getBuildPath() -> str:
    buildPath = ""
    if os.path.exists(configPath):
        with open(configPath, "r") as configFile:
            buildPath = configFile.readline()

    while not os.path.exists(buildPath):
        print("Build path is not valid.")
        buildPath = input("Where is the WebGL build folder? (Relative to "+scriptDir+"): ")

    with open(configPath, "w") as configFile:
        configFile.write(buildPath)
    return buildPath

print("Getting build path...")
buildPath = os.path.abspath(getBuildPath())
print("Build path is located at "+buildPath)

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

## Load secrets
os.chdir(scriptDir)
if not os.path.exists(secrets):
    stageDir = input("Where is the staging directory on the remote server? (Absolate path): ")
    user = input("What is the name of your user on the remote server?: ")
    url = input("What is the url/ip address of the remote server?: ")
    remoteUrl = user+"@"+url
    with open(secrets, "w") as secretFile:
        secretFile.write(stageDir+"\n")
        secretFile.write(remoteUrl)
else:
    with open(secrets, "r") as secretFile:
        stageDir = secretFile.readline().strip()
        remoteUrl = secretFile.readline().strip()
print("Loaded secrets.")

## Deploy to server
import subprocess
print("Uploading to server...")
try:
    scpProcess = subprocess.run(["scp", zipArchive, remoteUrl+":"+stageDir], cwd=buildPath)
    if scpProcess.returncode != 0:
        print("scp failed. Perhaps a bad staging directory was provided?")
        print("If this error persists, run this script again with -r to reset configuration.")
        sys.exit(scpProcess.returncode)

    print("Deploying to web server...")
    sshProcess = subprocess.run(["ssh", "-C", remoteUrl, "\""+stageDir+"/server-deploy.py\""])
    if sshProcess.returncode != 0:
        print("ssh failed. Perhaps a bad remote url or user was provided?")
        print("If this error persists, run this script again with -r to reset configuration.")
        sys.exit(sshProcess.returncode)
except FileNotFoundError:
    print()
    print("Could not execute scp and/or ssh!")
    if os.name == "nt":
        print("Ensure you have Windows Subsystem for Linux installed")
    else:
        print("Ensure you have ssh and scp installed")
    sys.exit(1)

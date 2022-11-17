import os

## Start in script directory
scriptDir = os.path.dirname(__file__)
os.chdir(scriptDir)

## Get build path from config file or prompt user if invalid
configPath = os.path.join(scriptDir, "deploy-config.txt")

def getBuildPath() -> str:
    buildPath = ""
    if os.path.exists(configPath):
        with open(configPath, "r") as configFile:
            buildPath = configFile.readline()

    while(not os.path.exists(buildPath)):
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
secrets = "deploy-secrets.txt"
if not os.path.exists(secrets):
    print("No config-secrets.txt file could be found next to script. Unable to deploy to server!")
    print("Secret lines in order: stageDir, remoteUrl")
    import sys
    sys.exit(1)
with open(secrets, "r") as secretFile:
    stageDir = secretFile.readline().strip()
    remoteUrl = secretFile.readline().strip()
print("Loaded secrets.")

## Deploy to server
import subprocess
print("Uploading to server...")
subprocess.run(["scp", zipArchive, remoteUrl+":"+stageDir], cwd=buildPath)
print("Deploying to web server... You will be asked for sudo password.")
print("If this stage hangs, do this manually.")
os.system("ssh -C "+remoteUrl+" \"sudo -S "+stageDir+"/deploy.bash\"")

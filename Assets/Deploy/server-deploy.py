#!/bin/python3

print()
print()
print("server-deploy.py")
print("================")

import os

scriptDir = os.path.dirname(__file__)
os.chdir(scriptDir)
configPath = "server-deploy_config.txt"

def getDeployDir() -> str:
    deployDir = ""
    if os.path.exists(configPath):
        with open(configPath, "r") as configFile:
            return configFile.readline().strip()
    
    # Do-while loop
    while True:
        deployDir = input("Where do you want to deploy the WebGL build? (Relative to "+scriptDir+"): ")
        if os.path.exists(deployDir) and os.path.isdir(deployDir):
            break
        else:
            print("Deploy directory is not valid.")

    with open(configPath, "w") as configFile:
        configFile.write(deployDir+"\n")
    
    return deployDir

print("Getting deploy path...")
deployDir = getDeployDir()
print("Deploy path is located at "+deployDir)

print("Copying zip archive to "+deployDir+"...")
# Get build archive
import glob
zipBuildName = glob.glob("*.zip")[0]

# Copy archive to deploy dir
import shutil
shutil.copy2(zipBuildName, deployDir)

print("Unzipping...")
# Unzip archive
from zipfile import ZipFile
os.chdir(deployDir)
with ZipFile(zipBuildName, "r") as zipBuildFile:
    zipBuildFile.extractall()
os.remove(zipBuildName)
print("Build deployed!")


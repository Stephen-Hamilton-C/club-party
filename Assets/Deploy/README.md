# Deploy WebGL Tool
These scripts assist in deploying a WebGL build to your web server.

## Known Issues
- This tool cannot run `sudo` yet (this is pure laziness, I'll get around to it eventually). This means that standard webservers may not work with this tool. This was designed for a webserver in a Docker container with a bind mount volume.

## Setup
1. Install Python 3 or newer on your computer and your server
2. Create a directory on your server and place `server-deploy.py` on it.
3. Make a WebGL build using Unity (File -> Build Settings -> WebGL)
4. Run `deploy.py` on your machine with Python 3 or newer

## Usage
The script is meant to be a fire-and-forget tool after the first run. All you need to do to deploy a new WebGL build is run `python3 deploy.py`.

## First Run
On first run, the script will prompt you for a bunch of information.
1. First, it will ask for the WebGL build folder. This is the folder containing the `index.html` file for your Unity WebGL build.
2. Then, it will ask for the staging directory on the remote server. This is the directory that contains the `server-deploy.py` script. It's recommended to make this an absolute path (e.g. `/home/serveruser/game-staging`).
3. Next, it will ask for your server username. This is just the name of the user that can place files into the webserver's `html` directory.
4. After that, you'll be asked for the server's URL or IP address.
5. If your stage directory, username, and URL/IP address are correct, you should see the `server-deploy.py` header. This will ask where the WebGL build should be deployed. This is typically your webserver's `html` directory is. E.g. for Apache, this is `/var/www/html`, or for Nginx, `/usr/share/nginx/html`

## Config
After the first run, you'll see a `deploy-config.txt` file next to the `deploy.py` script. On your server, you'll also see `server-deploy_config.txt` next to `server-deploy.py` script. **Don't** commit the config files, as they are not portable between computers, and can expose your server's IP address. The `.gitignore` should already have `deploy-config.txt` in it.

## Troubleshooting
If you are getting errors when deploying, first ensure you are connected to the internet. A good test is seeing if running the command `ping 8.8.8.8` responds.

If you have confirmed you have an internet connection and are still getting errors, ensure your server responds to ssh.

If it responds, run `deploy.py -r` to clear the config. You'll have to go through the first run again.

If even that fails, try deleting the `server-deploy_config.txt` on the server.

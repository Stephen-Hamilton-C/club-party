# Club Party
Club Party is a recreation of the beloved Putt Party from Discord's activities.
However, it was recently paywalled behind Discord Nitro.
So, this project was created, and it remains open source and free forever.

## Setup
1. Install Unity 2021.3 or later - https://unity3d.com/get-unity/download
2. Clone this repo
3. Club Party uses Photon Unity Networking v2 for multiplayer. You'll need to create an account at https://dashboard.photonengine.com.
4. Once your account has been created, click on the grey `Create A New App` button.
5. Name it whatever you wish, but make sure the `Photon Type` is set to `PUN`. Then click `Create`.
6. Click on the App ID and copy it. We'll need it later.
7. Open the repo with Unity
8. A PUN Setup window should appear. If it doesn't, Go to Window -> Photon Unity Networking -> PUN Wizard -> Setup Project
9. Paste your App ID into this window and click `Setup Project`.
10. A `PhotonServerSettings` asset should have just been highlighted in the project assets tab. You'll notice there is a picture in the same directory. Those are the settings I use for Club Party, if you wish to copy them.

## Automatic WebGL Deploy
You may notice the [deploy.py](https://github.com/Stephen-Hamilton-C/club-party/blob/main/Assets/deploy.py) Python script sitting in the Assets directory.
This is a special tool to upload WebGL builds to a webserver.
However, in its current state, it is quite unportable.
This will be fixed soon. See [#1](https://github.com/Stephen-Hamilton-C/club-party/issues/1).

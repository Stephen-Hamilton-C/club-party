using System;
using System.Linq;
using System.Reflection;
using ParrelSync;
using SHamilton.ClubParty.Network;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SHamilton.ClubParty.Dev {
    /// <summary>
    /// Editor script to automatically connect to an offline room if this script starts without being connected
    /// </summary>
    public class EditorAutoConnect : MonoBehaviour {
    
#if UNITY_EDITOR

        private static bool _performedConnection;
    
        private void Start() {
            if (!NetworkManager.IsConnected) {
                // Not connected
                Log("Not connected. Destroying current GameManager");
                Destroy(GetComponent<GameManager>());
                foreach (Transform child in transform) {
                    Destroy(child.gameObject);
                }

                gameObject.name = "AutoConnect NetworkManager";

                Log("Creating Network Manager");
                gameObject.AddComponent<NetworkManager>();

                Log("Setting expected CustomProperties...");
                // NetworkManager.LocalPlayerProperties.CharacterColor = Color.magenta;
                NetworkManager.LocalPlayer.CustomProperties["CharacterColor"] = Color.magenta;

                // Tried to connect online for clones, but it's not working, I don't have time to look into this rn
                // Check if a clone is running
                // var isCloneRunning = ClonesManager.IsClone() ||
                //     ClonesManager.GetCloneProjectsPath().Exists(ClonesManager.IsCloneProjectRunning);

                NetworkManager.onJoinedRoom += ReloadScene;
                // if (isCloneRunning) {
                //     Log("Clone detected as running, connecting online...");
                //     NetworkManager.ConnectAndJoinRoom();
                // } else {
                    Log("Connecting offline...");
                    NetworkManager.ConnectOfflineAndJoinRoom();
                // }
            } else if(_performedConnection) {
                // Connected via this script
                Log("Level loaded. Ignore all errors above this line.");
            
                // Try to clear console
                // Source: http://answers.unity.com/answers/1318322/view.html
                Assembly assembly = Assembly.GetAssembly (typeof(SceneView));
                Type logEntries = assembly.GetType ("UnityEditor.LogEntries");
                MethodInfo clearConsoleMethod = logEntries.GetMethod ("Clear");
                clearConsoleMethod?.Invoke(new object(), null);
            }
        }

        private void ReloadScene() {
            NetworkManager.onJoinedRoom -= ReloadScene;
            Log("Connected. Reloading level...");
            _performedConnection = true;
            var scene = SceneManager.GetActiveScene();
            NetworkManager.LoadLevel(scene);
        }

        private void Log(string msg) {
            Debug.Log("<b>"+msg+"</b>", this);
        }
    
#endif
    
    }
}

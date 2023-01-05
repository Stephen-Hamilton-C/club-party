using System;
using System.Reflection;
using Network;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev {
    /// <summary>
    /// Editor script to automatically connect to an offline room if this script starts without being connected
    /// </summary>
    public class EditorAutoConnect : MonoBehaviour {
    
#if UNITY_EDITOR

        private static bool _performedConnection;
    
        private void Start() {
            if (!PhotonNetwork.IsConnected) {
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
                NetworkManager.LocalPlayerProperties.CharacterColor = Color.magenta;
            
                Log("Connecting offline...");
                NetworkManager.onJoinedRoom += ReloadScene;
                NetworkManager.ConnectOfflineAndJoinRoom();
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
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            Log("Connected. Reloading level...");
            _performedConnection = true;
            PhotonNetwork.LoadLevel(sceneIndex);
        }

        private void Log(string msg) {
            Debug.Log("<b>"+msg+"</b>", this);
        }
    
#endif
    
    }
}

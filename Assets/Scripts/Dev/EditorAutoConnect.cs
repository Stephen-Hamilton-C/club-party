using System;
using System.Reflection;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev {
    public class EditorAutoConnect : MonoBehaviour {
    
#if UNITY_EDITOR

        private static bool _performedConnection;
    
        private void Start() {
            if (!PhotonNetwork.IsConnected) {
                Log("Not connected. Destroying current GameManager");
                Destroy(GetComponent<GameManager>());
            
                Log("Creating Network Manager");
                gameObject.AddComponent<NetworkManager>();
            
                Log("Setting expected CustomProperties...");
                PhotonNetwork.LocalPlayer.CustomProperties["CharacterColor"] = "1,0,1";
            
                Log("Connecting offline...");
                NetworkManager.onJoinedRoom += ReloadScene;
                NetworkManager.ConnectOfflineAndJoinRoom();
            } else if(_performedConnection) {
                Log("Level loaded. Ignore all errors above this line.");
            
                // Attempt to clear console
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

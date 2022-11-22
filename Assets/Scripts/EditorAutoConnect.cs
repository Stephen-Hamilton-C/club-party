using System;
using System.Reflection;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class EditorAutoConnect : MonoBehaviour {
    
    #if UNITY_EDITOR

    private static bool _performedConnection;
    
    private void Start() {
        if (!PhotonNetwork.IsConnected) {
            Debug.Log("<b>Not connected. Destroying current GameManager</b>");
            Destroy(GetComponent<GameManager>());
            Debug.Log("<b>Creating Network Manager</b>");
            gameObject.AddComponent<NetworkManager>();
            Debug.Log("<b>Connecting offline...</b>");
            NetworkManager.onJoinedRoom += ReloadScene;
            NetworkManager.ConnectOfflineAndJoinRoom();
        } else if(_performedConnection) {
            Debug.Log("<b>Level loaded. Ignore all errors above this line.</b>");
            
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
        Debug.Log("<b>Connected. Reloading level...</b>");
        _performedConnection = true;
        PhotonNetwork.LoadLevel(sceneIndex);
    }
    
    #endif
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.SceneManagement;
public class NetworkManagerHandler : MonoBehaviour
{
    [SerializeField]
    bool IsDedicatedServer = false;
    void Start()
    {

        if (IsDedicatedServer)
        {
            print("Starting Server");
            NetworkManager.Singleton.StartServer();   
            NetworkSceneManager.SwitchScene("LobbyMenu");
        }
    }

    void OnEnable()
    {
        if (IsDedicatedServer)
        {
            NetworkManager.Singleton.OnServerStarted += ServerStarted;
        }
    }

    void OnDisable()
    {
        if (IsDedicatedServer)
        {
            NetworkManager.Singleton.OnServerStarted -= ServerStarted;
        }
    }

    public void ServerStarted()
    {
        print("Server Started");
    }    
}

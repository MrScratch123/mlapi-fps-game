using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

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
        print("The Server Has Started :D");
    }    
}

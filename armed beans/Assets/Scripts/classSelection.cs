using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class classSelection : MonoBehaviour
{
    public NetworkPlayerManager myNetworkPlayerManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void SpawnPlayerUI(string playerType)
    {
        if (playerType == "rifle")
            myNetworkPlayerManager.SpawnPlayerServerRPC(PlayerType.Rifle);
        else if (playerType == "shotgun")
            myNetworkPlayerManager.SpawnPlayerServerRPC(PlayerType.Shotgun);
        else if (playerType == "sniper")
            myNetworkPlayerManager.SpawnPlayerServerRPC(PlayerType.Sniper);

        Destroy(gameObject);    

        
    }
    
}

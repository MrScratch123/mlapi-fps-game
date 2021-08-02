using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class connect : NetworkBehaviour
{
    [SerializeField] 
    GameObject uiCam;
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
        uiCam.SetActive(false);
    }
    public void Client()
    {
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
        uiCam.SetActive(false);
    }
    public void Server()
    {
        NetworkManager.Singleton.StartServer();   
        gameObject.SetActive(false);
        uiCam.SetActive(false);
    }
}

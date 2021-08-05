using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.SceneManagement;
public class connect : NetworkBehaviour
{
    [SerializeField] 
    GameObject uiCam;
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        NetworkSceneManager.SwitchScene("LobbyMenu");
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
        NetworkSceneManager.SwitchScene("LobbyMenu");
        gameObject.SetActive(false);
        uiCam.SetActive(false);
    }
}

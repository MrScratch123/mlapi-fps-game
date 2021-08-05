using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.SceneManagement;

public class VotingHandler : NetworkBehaviour
{
    public static VotingHandler activeVotingHandler;

    [SerializeField]
    Timer timer;
    ServerState State = ServerState.Standby;
    void OnEnable()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        }
    }

    void OnDisable()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnected;
        }
    }

    void Start()
    {
        NetworkPlayerManager[] networkPlayerManagers = FindObjectsOfType<NetworkPlayerManager>();
        activeVotingHandler = this;
       
        if (IsHost)
        {
            StartVoting();
        
        }
        else if (IsServer)
        {
            if (networkPlayerManagers.Length == 0)
            {
                State = ServerState.Standby;
            }
            else 
            {
                StartVoting();
            }

        }
    }



    void StartVoting()
    {
        print("started cycle");
        State = ServerState.menu;
        timer.StartTimer();
    }

    void ClientConnected(ulong ULONG)
    {
        print("client connected");
        if (State == ServerState.Standby && IsServer)
        {
            StartVoting();
        }
    }

    public void TimerEnded()
    {
        NetworkSceneManager.SwitchScene("Game");
    }

}

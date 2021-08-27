using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;
public class NetworkPlayerManager : NetworkBehaviour
{


    [HideInInspector]
    public NetworkVariable<string> PlayerName = new NetworkVariable<string>();

    

    [HideInInspector]
    public NetworkVariable<int> PlayerID = new NetworkVariable<int>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly});
    

    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField]
    GameObject DeadPlayerPrefab;


    [SerializeField]
    Camera myCam;

    [HideInInspector]
    public NetworkVariable<GameObject> CurrentPlayer = new NetworkVariable<GameObject>();
    GameObject cam;

    public NetworkVariable<int> Kills = new NetworkVariable<int>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly});
    public NetworkVariable<int> Deaths = new NetworkVariable<int>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly});
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (IsServer)
        {
            PlayerID.Value = Random.Range(0,10000000);
            
        }
    }
    void OnEnable()
    {
        NetworkSceneManager.OnSceneSwitched += SceneSwitched;
    }

    void OnDisable()
    {
        NetworkSceneManager.OnSceneSwitched -= SceneSwitched;
    }
   
    void Start()
    {
        if (IsOwner)
        {
            myCam.enabled = true;
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {    
                myCam.enabled = false;
                SpawnPlayerCamera();
            }
        }
        else
        {
            myCam.enabled = false;
        }
        if (IsServer)
        {
            Kills.Value = 0;
            Deaths.Value = 0;
        }
    }

   void SceneSwitched()
   {
        if (!IsOwner) {return;}
       if (SceneManager.GetActiveScene().buildIndex == 2)
       {    
            myCam.enabled = false;
            SpawnPlayerCamera();
       }
       else if (SceneManager.GetActiveScene().buildIndex == 1)
       {

            myCam.enabled = true;
       }
   }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRPC(PlayerType playerType)
    {
        CurrentPlayer.Value = GamemodeBase.CurrentGameMode.SpawnPlayer(PlayerID.Value, playerType);
        ulong ClientID = GetComponent<NetworkObject>().OwnerClientId;
        CurrentPlayer.Value.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        CurrentPlayer.Value.GetComponent<Player>().MyPlayerManager.Value = this.gameObject;
    }
    

    [ClientRpc]
    public void SpawnDeadPlayerCameraClientRPC()
    {
        if (!IsOwner)
            return;
        cam = Instantiate(DeadPlayerPrefab, new Vector3(-52, 22, 61), Quaternion.LookRotation(new Vector3(22, 131, 0)));
        cam.GetComponent<classSelection>().myNetworkPlayerManager = this;
    }

    void SpawnPlayerCamera()
    {
        if (!IsOwner)
            return;
        cam = Instantiate(DeadPlayerPrefab, new Vector3(-52, 22, 61), Quaternion.LookRotation(new Vector3(22, 131, 0)));
        cam.GetComponent<classSelection>().myNetworkPlayerManager = this;
    }

}

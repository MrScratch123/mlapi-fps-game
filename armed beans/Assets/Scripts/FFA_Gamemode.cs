using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
public class FFA_Gamemode : GamemodeBase
{    
    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField]
    GameObject[] SpawnPoints;



    void Start()
    {
        GamemodeBase.CurrentGameMode = this;
        NetworkManager.Singleton.OnClientConnectedCallback += UpdateKillList;
        NetworkManager.Singleton.OnClientDisconnectCallback += UpdateKillList;
    }
    public override void RequestDamage(int DamagedPlayerID, int InstigatorID, float Damage)
    {
        DamagePlayerServerRPC(DamagedPlayerID, InstigatorID, Damage);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DamagePlayerServerRPC(int DamagedPlayerID, int InstigatorID, float Damage) 
    {

        NetworkPlayerManager DamagedPlayer = GetPlayerFromID(DamagedPlayerID);
        NetworkPlayerManager Instigator = GetPlayerFromID(InstigatorID);
        if (DamagedPlayer && Instigator && Instigator.CurrentPlayer.Value)
        {
            DamagedPlayer.CurrentPlayer.Value.GetComponent<Player>().Health.Value -= Damage;
            DamagedPlayer.CurrentPlayer.Value.GetComponent<Player>().LastPlayerWhoDamaged = Instigator;
        }
    }

    public NetworkPlayerManager GetPlayerFromID(int PlayerID)
    {
        NetworkPlayerManager[] networkPlayers = FindObjectsOfType<NetworkPlayerManager>();

        foreach(NetworkPlayerManager player in networkPlayers)
        {
            if (player.PlayerID.Value == PlayerID)
            {
                return player;
            }
        }
        return null;
    }

    public override GameObject SpawnPlayer(int SpawningPlayer, PlayerType playerType)
    {
        GameObject player;
        if (playerType == PlayerType.Rifle)
            player = Instantiate(PlayerPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].transform.position, Quaternion.identity);
        else if (playerType == PlayerType.Shotgun)
            player = Instantiate(PlayerPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].transform.position, Quaternion.identity);
        else
            player = Instantiate(PlayerPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].transform.position, Quaternion.identity);
        return player;
    }

    public override void HandleDeath(int DeadPlayerID, int InstigatorID)
    {
        if (!IsServer) {return;}
        NetworkPlayerManager DeadPlayer = GetPlayerFromID(DeadPlayerID);
        NetworkPlayerManager Instigator = GetPlayerFromID(InstigatorID);

        if (DeadPlayer && Instigator)
        {
            DeadPlayer.Deaths.Value = DeadPlayer.Deaths.Value + 1;
            Instigator.Kills.Value = Instigator.Kills.Value + 1;
            Destroy(DeadPlayer.CurrentPlayer.Value);
            DeadPlayer.CurrentPlayer.Value.GetComponent<NetworkObject>().Despawn();
            print("someone died :skull:");
            DeadPlayer.SpawnDeadPlayerCameraClientRPC();
            UpdateKillsList();
        }

    }

    void UpdateKillList(ulong ULONG)
    {
        UpdateKillsList();
    }

    void UpdateKillsList()
    {
        // if (!IsServer) {return;}
        // var playerManagers = FindObjectsOfType<NetworkPlayerManager>();
        // KillList.Value.RemoveRange(0, KillList.Value.Count);
        // var sortedPlayerManagers = playerManagers.OrderBy(playerManagers => playerManagers.Kills).ToArray<NetworkPlayerManager>();
        // foreach(NetworkPlayerManager playerManager in sortedPlayerManagers)
        // {
        //     KillList.Value.Add(playerManager.gameObject);
        // }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using System;

public class GamemodeBase : NetworkBehaviour
{
    public static GamemodeBase CurrentGameMode;
    public virtual void RequestDamage(int DamagedPlayerID, int InstigatorID, float Damage){}
    public virtual void HandleDeath(int DeadPlayerID, int InstigatorID){}
    public virtual GameObject SpawnPlayer(int SpawningPlayer, PlayerType playerType) { return null;}


}

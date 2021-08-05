using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
public class Player : NetworkBehaviour
{
    [HideInInspector]
    public NetworkVariable<float> Health = new NetworkVariable<float>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly}); 
    public NetworkVariable<GameObject> MyPlayerManager = new NetworkVariable<GameObject>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly});
    
    [HideInInspector]
    public NetworkPlayerManager LastPlayerWhoDamaged;

    void Start()
    {
        if (IsServer)
        {
            Health.Value = 100;
        }
    }

    void OnEnable()
    {
        if (!IsServer) {return;}
        Health.OnValueChanged += HealthChanged;
    }

    void OnDisable()
    {
        if (!IsServer) {return;}
        Health.OnValueChanged -= HealthChanged;
    }

    void HealthChanged(float OldValue, float NewValue)
    {
        if (Health.Value <= 0)
        {
            GamemodeBase.CurrentGameMode.HandleDeath(MyPlayerManager.Value.GetComponent<NetworkPlayerManager>().PlayerID.Value, LastPlayerWhoDamaged.PlayerID.Value);
        }
    }
}

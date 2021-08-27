using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;
public class Player : NetworkBehaviour
{
    [HideInInspector]
    public NetworkVariable<float> Health = new NetworkVariable<float>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly}); 

    [HideInInspector]
    public NetworkVariable<GameObject> MyPlayerManager = new NetworkVariable<GameObject>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.ServerOnly});
    
    [HideInInspector]
    public NetworkPlayerManager LastPlayerWhoDamaged;

    [SerializeField]
    Text Kills;

    [SerializeField]
    Text Deaths;

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

    void Update()
    {
        if (!IsOwner) {return;}
        Kills.text = "Kills : " + MyPlayerManager.Value.GetComponent<NetworkPlayerManager>().Kills.Value.ToString();
        Deaths.text = "Deaths : " + MyPlayerManager.Value.GetComponent<NetworkPlayerManager>().Deaths.Value.ToString();
    }
}

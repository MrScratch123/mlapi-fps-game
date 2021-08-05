using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
public class gun_switcher : NetworkBehaviour
{
    NetworkVariable<int> CurrentGunIndex = new NetworkVariable<int>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly});

    [SerializeField]
    List<gun> guns = new List<gun>();

    void Start()
    {
        CurrentGunIndex.Value = 0;
    }

    void OnEnable()
    {
        CurrentGunIndex.OnValueChanged += GunIndexChanged;
    }

    void OnDisable()
    {
        CurrentGunIndex.OnValueChanged -= GunIndexChanged;
    }

    void Update()
    {
        if (!IsOwner) {return;}
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentGunIndex.Value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentGunIndex.Value = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentGunIndex.Value = 2;
        }
    
    }

    void GunIndexChanged(int OldIndex, int NewIndex)
    {

        for (int i = 0; i < guns.Count; i++)
        {
            if (NewIndex == i)
            {
                guns[i].GetComponent<Renderer>().enabled = true;
                guns[i].isShootingEnabled = true;

            }
            else 
            {
                guns[i].GetComponent<Renderer>().enabled = false;
                guns[i].isShootingEnabled = false;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class ADS : NetworkBehaviour
{
    [SerializeField]
    bool CanAim = true;

    [SerializeField]
    Vector3 AimLocation;

    [SerializeField]
    Vector3 NormalLocation;

    [SerializeField]
    float speed;

    bool isAiming;


    void Start()
    {
        transform.localPosition = NormalLocation;
    }
    void Update()
    {
        if (!IsOwner) {return;}

        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, AimLocation , Time.deltaTime * speed);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, NormalLocation , Time.deltaTime * speed);
        }
    }
}

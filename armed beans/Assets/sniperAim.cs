using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class sniperAim : NetworkBehaviour
{
    [SerializeField] 
    GameObject SniperImage;

    Renderer rendererComp;

    [SerializeField]
    Camera camera;
    gun gun;

    public float NormalFOV;
    public float ZoomedFOV = 40;

    [SerializeField]
    float speed = 10f;

    void Start()
    {
        rendererComp = GetComponent<Renderer>();
        gun = GetComponent<gun>();
    }


    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButton(1) && gun.isShootingEnabled)
        {
            rendererComp.enabled = false;
            SniperImage.SetActive(true);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, ZoomedFOV , speed * Time.deltaTime);
        }
        else if (!Input.GetMouseButton(1) && gun.isShootingEnabled)
        {
            rendererComp.enabled = true;
            SniperImage.SetActive(false);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, NormalFOV , speed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class gun : NetworkBehaviour
{
    [HideInInspector]
    public bool isShootingEnabled;
    [SerializeField] float shootSpeed;
    [SerializeField] float damage;
    [SerializeField] float distance;
    [SerializeField] float accuracy;
    bool canShoot = true;
    [SerializeField] Camera mainCamera;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem sparkles;
    [SerializeField] Transform gunTip;
    [SerializeField] GameObject line;
    [SerializeField] AudioClip sfx;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject player;

    [SerializeField]
    GameObject playerMesh;

    New_Weapon_Recoil_Script recoil;
    Vector3 hitPos;
    LineRenderer lineInstanceLineRenderer;
    GameObject sparksInstance;

    int MyPlayerManageID;


    private void Start()
    {
        TryGetComponent<New_Weapon_Recoil_Script>( out recoil);
        MyPlayerManageID = player.GetComponent<Player>().MyPlayerManager.Value.GetComponent<NetworkPlayerManager>().PlayerID.Value;
    }

    void enableShoot()
    {
        if (!IsOwner) {return;}
        canShoot = true;
        playerMesh.layer = 2;
        player.layer = 2;
    }
    private void Update()
    {
        if (!IsOwner) {return;}
        if (Input.GetMouseButton(0) && canShoot && isShootingEnabled)
        {
            StartCoroutine(processShoot());
        }
    }
    
    IEnumerator processShoot()
    {
        Vector3 posToShoot = new Vector3(mainCamera.transform.position.x + Random.Range(-accuracy, accuracy), mainCamera.transform.position.y + Random.Range(-accuracy, accuracy), mainCamera.transform.position.z);
        
        if(Physics.Raycast(posToShoot, mainCamera.transform.forward, out RaycastHit hit, distance))
        {

            if (hit.collider.tag == "Player" && hit.transform.gameObject != player)
            {
                print("i hit a player");
                int DamagedPlayerID = hit.transform.gameObject.GetComponent<Player>().MyPlayerManager.Value.GetComponent<NetworkPlayerManager>().PlayerID.Value;
                GamemodeBase.CurrentGameMode.RequestDamage(DamagedPlayerID, MyPlayerManageID , damage);
            }

            Quaternion sparkelRotation = Quaternion.LookRotation(hit.normal);
            // add sparkle
            hitPos = hit.point;
            playVFXServerRPC(hitPos);


        }
        //playSFXServerRPC();
        recoil?.Fire();
        canShoot = false;
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }

    [ServerRpc]
    private void playSFXServerRPC()
    {
        playSFXClientRPC();
    }

    [ClientRpc]
    private void playSFXClientRPC()
    {
        audioSource.PlayOneShot(sfx);
    }

    [ServerRpc]
    private void playVFXServerRPC(Vector3 _hitpos)
    {
        playVFXClientRPC(_hitpos);
    }

    [ClientRpc]
    private void playVFXClientRPC(Vector3 _hitpos)
    {
        var lineInstance = Instantiate(line, hitPos, Quaternion.Euler(0, 0, 0));
        lineInstanceLineRenderer = lineInstance.GetComponent<LineRenderer>();
        lineInstanceLineRenderer.positionCount = 2;
        lineInstanceLineRenderer.SetPosition(0, gunTip.position);
        lineInstanceLineRenderer.SetPosition(1, _hitpos);
        Destroy(lineInstance, 0.1f);
        Destroy(sparksInstance, 0.5f);
    }
}
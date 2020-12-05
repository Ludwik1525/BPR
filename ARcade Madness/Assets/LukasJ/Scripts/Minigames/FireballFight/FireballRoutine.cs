using UnityEngine;
using Photon.Pun;
using System.IO;

public class FireballRoutine : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody rb;

    private PhotonView PV;


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    // what happens when the fireball enters another collider that is a trigger
    private void OnTriggerEnter(Collider other)
    {
        // if it's a plyer, then kill him
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<JoystickScript>().Die();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SmallExplosion"), this.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlasmaExplosion"), this.transform.position, Quaternion.identity);
        }
        // destroy the fireball
        FindObjectOfType<FireballSetupManager>().GetComponent<PhotonView>().RPC("PlayFireballExplodeSound", RpcTarget.AllBuffered);
        PV.RPC("KillMe", RpcTarget.AllBuffered);

    }

    // destroying the fireball
    [PunRPC]
    void KillMe()
    {
        Destroy(gameObject);
    }
}

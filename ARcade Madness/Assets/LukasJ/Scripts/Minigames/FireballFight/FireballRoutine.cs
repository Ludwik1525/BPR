using System.Collections;
using System.Collections.Generic;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponentInChildren<Animator>().gameObject.GetComponent<FireBallAnimator>().Die();
            PhotonNetwork.Instantiate(Path.Combine("SmallExplosion", "Player"), this.transform.position, Quaternion.identity);
        }

        PhotonNetwork.Instantiate(Path.Combine("PlasmaExplosion", "Player"), this.transform.position, Quaternion.identity);
        PV.RPC("KillMe", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void KillMe()
    {
        Destroy(gameObject);
    }
}

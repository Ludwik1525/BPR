using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroyMe : MonoBehaviour
{
    private PhotonView PV; 

    void Start()
    {
        PV = GetComponent<PhotonView>();
        StartCoroutine(DestroyMeAfterTime(3));
    }
    
    IEnumerator DestroyMeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        PV.RPC("KillMe", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void KillMe()
    {
        Destroy(this.gameObject);
    }
}

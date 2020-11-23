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
        StartCoroutine(DestroyMeAfterTime(1.5f));
    }
    
    IEnumerator DestroyMeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        KillMe();
    }
    
    void KillMe()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}

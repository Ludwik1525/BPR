using System.Collections;
using UnityEngine;
using Photon.Pun;

public class DestroyMe : MonoBehaviour
{
    private PhotonView PV; 


    // script for destroying the particle effect that appears when a fireball hits some other object
    void Start()
    {
        PV = GetComponent<PhotonView>();
        StartCoroutine(DestroyMeAfterTime(1.5f));
    }
    
    // wait until the particle effect finishes
    IEnumerator DestroyMeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        PV.RPC("KillMe", RpcTarget.AllBuffered);
    }

    // destroying the particle effect
    [PunRPC]
    void KillMe()
    {
        Destroy(this.gameObject);
    }
}

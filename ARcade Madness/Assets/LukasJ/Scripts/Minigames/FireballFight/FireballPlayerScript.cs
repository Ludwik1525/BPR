using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireballPlayerScript : MonoBehaviour
{
    private PhotonView PV;

    void Start()
    {
        
    }

    [PunRPC]
    private void SetName(string name)
    {
        transform.GetChild(1).GetChild(0).GetComponent<TextMesh>().text = name;
    }
}

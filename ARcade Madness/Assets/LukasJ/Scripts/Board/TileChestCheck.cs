using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TileChestCheck : MonoBehaviour
{
    private bool iHaveAChest;
    public PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SetToTrue()
    {
        iHaveAChest = true;
    }

    [PunRPC]
    public void SetToFalse()
    {
        iHaveAChest = false;
    }
    
    public bool Check()
    {
        return iHaveAChest;
    }
}

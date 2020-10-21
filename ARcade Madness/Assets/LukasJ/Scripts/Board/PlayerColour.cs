using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerColour : MonoBehaviour
{
    private PhotonView PV;
    private ColourPalette colours;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        colours = FindObjectOfType<ColourPalette>();
    }
    
    [PunRPC]
    void RPC_AssignColour()
    {
        if (PV.IsMine)
        {
            GetComponent<SkinnedMeshRenderer>().material = colours.colours[(int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]];
        }
    }
}

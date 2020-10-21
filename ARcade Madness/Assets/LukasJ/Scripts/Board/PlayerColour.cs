using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerColour : MonoBehaviour
{
    private PhotonView PV;
    private ColourPalette colours;
    
    
    [PunRPC]
    void RPC_AssignColour()
    {
        PV = GetComponent<PhotonView>();
        colours = FindObjectOfType<ColourPalette>();

        if (PV.IsMine)
        {
            GetComponent<SkinnedMeshRenderer>().material = colours.colours[(int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]];
        }
    }
}

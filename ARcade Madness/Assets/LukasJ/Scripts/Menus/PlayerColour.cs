using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerColour : MonoBehaviour
{
    private PhotonView PV;
    private ColourPalette colours;

    [PunRPC]
    void RPC_AssignColour(int colourNo)
    {
        PV = GetComponent<PhotonView>();
        colours = FindObjectOfType<ColourPalette>();
        
        GetComponent<SkinnedMeshRenderer>().material = colours.colours[colourNo];
    }
}

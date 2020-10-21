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
    void RPC_AssignColour()
    {
        PV = GetComponent<PhotonView>();
        colours = FindObjectOfType<ColourPalette>();

        for (int i = GameSetupController.players.Count - 1; i >= 0; i--)
        {
            GameSetupController.players[i].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material =
                colours.colours[(int)PhotonNetwork.PlayerList[i].CustomProperties["ColourID"]];
        }
    }
}

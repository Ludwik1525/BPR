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

        //for (int i = 0; i < GameSetupController.players.Count; i++)
        //{
            //GameSetupController.players[i].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material =
                //colours.colours[(int)PhotonNetwork.PlayerList[i].CustomProperties["ColourID"]];
        if(PV.IsMine)
            GetComponent<SkinnedMeshRenderer>().material = colours.colours[(int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]];
        //}
    }
}

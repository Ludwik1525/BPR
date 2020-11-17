using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ColourManager : MonoBehaviour
{
    public Color32[] coloursArray;

    private PhotonView PV;
    
    private Transform playersContainer;

    void Start()
    {
    }

    [PunRPC]
    void RPC_ApplyColour(int colourNo, string myName)
    {
        playersContainer = GameObject.Find("PlayerDisplayer").transform;

        PV = GetComponent<PhotonView>();

        for (int i = playersContainer.childCount - 1; i >= 0; i--)
            {
                if (playersContainer.GetChild(i).GetChild(0).GetComponent<Text>().text == myName)
                {
                    playersContainer.GetChild(i).GetChild(1).GetComponent<Image>().color = coloursArray[colourNo];
                }
            }
    }
}

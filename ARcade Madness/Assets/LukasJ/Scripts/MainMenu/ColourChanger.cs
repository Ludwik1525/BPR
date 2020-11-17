﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour
{
    private int colourNo = 0;
    private string myName;
    private int randomIndex = 0;

    private PhotonView PV;
    private PhotonView PV1;
    private Button colourButton;

    private Transform playersContainer;

    private Player thisPlayer;

    public Color32[] coloursArray;

    void Start()
    {
        colourButton = GameObject.Find("ButtonColour").GetComponent<Button>();
        playersContainer = GameObject.Find("PlayerDisplayer").transform;

        myName = PlayerPrefs.GetString("NickName");

        PV = GetComponent<PhotonView>();
        PV1 = this.transform.GetChild(1).GetComponent<PhotonView>();

        colourButton.onClick.AddListener(ChangeColour);

        thisPlayer = PhotonNetwork.LocalPlayer;

        if (PV.IsMine)
        {
            if (thisPlayer.CustomProperties["ColourID"] != null)
            {
                colourNo = (int)thisPlayer.CustomProperties["ColourID"];
            }
            SetColour();
        }
    }

    public void ChangeColour()
    {
        if (PV.IsMine)
        {
            bool isColourUsed = true;

            while (isColourUsed)
            {
                isColourUsed = false;

                colourNo++;
                if (colourNo == 12)
                {
                    colourNo = 0;
                }

                for (int i = playersContainer.childCount - 1; i >= 0; i--)
                {
                    if (playersContainer.GetChild(i).GetChild(0).GetComponent<Text>().text != myName)
                    {
                        if (playersContainer.GetChild(i).GetChild(1).GetComponent<Image>().color == coloursArray[colourNo])
                        {
                            isColourUsed = true;
                        }
                    }
                }
            }

            PV1.RPC("RPC_ApplyColour", RpcTarget.AllBuffered, colourNo, myName);

            ExitGames.Client.Photon.Hashtable thisPColour = new ExitGames.Client.Photon.Hashtable();
            thisPColour.Add("ColourID", colourNo);
            thisPlayer.SetCustomProperties(thisPColour);
        }
    }

    void SetColour()
    {
        if (PV.IsMine)
        {
            if (thisPlayer.CustomProperties["ColourID"] != null)
            {
                colourNo = (int)thisPlayer.CustomProperties["ColourID"];
            }
            else
            {
                bool isColourUsed = true;  // bool used to decide if the set colour is used by anybody else
                PV.RPC("GenerateColourIndex", RpcTarget.AllBuffered);
                colourNo = randomIndex;  // initial random colour index for a new player joining the room

                while (isColourUsed)  // loop checking if the colour is used by anybody
                {
                    isColourUsed = false;  // set to false at start, then try to check if it's actually false

                    for (int i = playersContainer.childCount - 1; i >= 0; i--)  // for every player from the player list
                    {
                        if (playersContainer.GetChild(i).GetChild(0).GetComponent<Text>().text != myName)  // if given player is not me
                        {
                            if (playersContainer.GetChild(i).GetChild(1).GetComponent<Image>().color == coloursArray[colourNo])
                            // if this player's colour had the colour I have chosen right now
                            {
                                colourNo++;  // increment the colour index by one
                                if (colourNo == 12)  // if the new colour index is the maximum one, zero it out
                                {
                                    colourNo = 0;
                                }
                                isColourUsed = true;  // set the bool to true to loop again with a new colour index, repeat the whole process
                            }
                        }
                    }
                }
            }

            PV1.RPC("RPC_ApplyColour", RpcTarget.AllBuffered, colourNo, myName);

            ExitGames.Client.Photon.Hashtable thisPColour = new ExitGames.Client.Photon.Hashtable();
            thisPColour.Add("ColourID", colourNo);
            thisPlayer.SetCustomProperties(thisPColour);

        }
    }

    [PunRPC]
    void GenerateColourIndex()
    {
        randomIndex = Random.Range(0, 12);
    }
}

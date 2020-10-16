using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour
{
    private int colourNo = 0;
    private string myName;

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
            bool isColourUsed = true;

            if (thisPlayer.CustomProperties["ColourID"] != null)
            {
                colourNo = (int)thisPlayer.CustomProperties["ColourID"];
            }
            else
            {
                colourNo = 0;
                while (isColourUsed)
                {
                    isColourUsed = false;

                    for (int i = playersContainer.childCount - 1; i >= 0; i--)
                    {
                        if (playersContainer.GetChild(i).GetChild(0).GetComponent<Text>().text != myName)
                        {
                            if (playersContainer.GetChild(i).GetChild(1).GetComponent<Image>().color == coloursArray[colourNo])
                            {
                                colourNo++;
                                if (colourNo == 12)
                                {
                                    colourNo = 0;
                                }
                                isColourUsed = true;
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
}

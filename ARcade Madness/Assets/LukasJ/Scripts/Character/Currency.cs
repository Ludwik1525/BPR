using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Currency : MonoBehaviour
{
    private ScoreInfo si;
    private PhotonView myPV;
    private int currency;

    Button[] buttons;
    Button buttonQuit;
    Button buttonQuit2;


    private void Start()
    {
        //Very bad code that might work
        buttons = GameObject.Find("Canvas").GetComponentsInChildren<Button>(true);
        foreach(Button b in buttons)
        {
            if(b.gameObject.name.Contains("Quit"))
            {
                buttonQuit = b;
            }
            if (b.gameObject.name.Contains("Confirm"))
            {
                buttonQuit2 = b;
            }
        }
        buttonQuit.onClick.AddListener(onQuit);
        buttonQuit2.onClick.AddListener(onQuit);

        si = FindObjectOfType<ScoreInfo>();
        myPV = GetComponent<PhotonView>();

        //if (PlayerPrefs.HasKey("Currency"))
        //{
        //    currency = PlayerPrefs.GetInt("Currency");
        //}
        //else
        //{
        //    currency = 0;
        //}

        if (PhotonNetwork.LocalPlayer.CustomProperties["Currency"] != null)
        {
            currency = (int)PhotonNetwork.LocalPlayer.CustomProperties["Currency"];
        }
        else
        {
            currency = 0;
            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", currency);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);
        }

        if (myPV.IsMine)
        {
            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"],
                       currency);
        }
    }


    public void setCurrency()
    {
        if (myPV.IsMine)
        {
            this.currency = (int)PhotonNetwork.LocalPlayer.CustomProperties["Currency"];
            this.currency++;

            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", currency);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    public void setCurrencyWithVar(int currencyVar)
    {
        if (myPV.IsMine)
        {
            this.currency = (int)PhotonNetwork.LocalPlayer.CustomProperties["Currency"];
            this.currency += currencyVar;
            
            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", currency);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    public void decreaseCurrency()
    {
        if (myPV.IsMine)
        {
            this.currency = (int)PhotonNetwork.LocalPlayer.CustomProperties["Currency"];
            this.currency--;
            
            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", currency);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    public int CheckHowManyHaveMoney(string myName)
    {
        int amount = 0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int tempCurrency = 0;
            if (PhotonNetwork.PlayerList[i].NickName != myName)
            {
                tempCurrency = (int)PhotonNetwork.PlayerList[i].CustomProperties["Currency"];
                if (tempCurrency > 1)
                {
                    amount += 2;
                }
                else if(tempCurrency > 0)
                {
                    amount++;
                }
            }
        }
        return amount;
    }

    public void decreaseCurrencyCoinMagnet(string myName)
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int tempCurrency = 0;
            if(PhotonNetwork.PlayerList[i].NickName != myName)
            {
                tempCurrency = (int)PhotonNetwork.PlayerList[i].CustomProperties["Currency"];
                if (tempCurrency > 1)
                {
                    tempCurrency -= 2;

                   
                }
                else if(tempCurrency > 0)
                {
                    tempCurrency--;
                }

                ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
                thisCurrency.Add("Currency", tempCurrency);
                PhotonNetwork.PlayerList[i].SetCustomProperties(thisCurrency);

                si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered,
                    (int)PhotonNetwork.PlayerList[i].CustomProperties["PlayerNo"], tempCurrency);
            }
        }
    }

    private void OnApplicationQuit()
    {
        onQuit();
    }

    void onQuit()
    {
        currency = 0;
        ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
        thisCurrency.Add("Currency", currency);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);
    }


}

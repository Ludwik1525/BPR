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

    private void Start()
    {
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
            currency = 2;
            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", currency);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);
        }


        //currency = 2;
        //PlayerPrefs.SetInt("Currency", currency);

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

            //PlayerPrefs.SetInt("Currency", currency);

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

            //PlayerPrefs.SetInt("Currency", currency);
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

            //PlayerPrefs.SetInt("Currency", currency);
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
                if (tempCurrency > 0)
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
                if (tempCurrency > 0)
                {
                    tempCurrency--;

                    ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
                    thisCurrency.Add("Currency", tempCurrency);
                    PhotonNetwork.PlayerList[i].SetCustomProperties(thisCurrency);

                    si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered,
                        (int)PhotonNetwork.PlayerList[i].CustomProperties["PlayerNo"], tempCurrency);
                }
            }
        }

        //foreach()
        //{
        //    if(player != gameObject.transform)
        //    {
        //        player.GetComponent<Currency>().decreaseCurrency();
        //        si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        //    }
        //}

        //if (!myPV.IsMine)
        //{
        //    if(currency >= 0)
        //    {
        //        this.currency--;
        //        PlayerPrefs.SetInt("Currency", currency);
        //        si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        //    }
        //}
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.SetInt("Currency", 0);
        currency = 0;
        ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
        thisCurrency.Add("Currency", currency);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);
    }


}

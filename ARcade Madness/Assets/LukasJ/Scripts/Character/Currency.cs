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

        if (PlayerPrefs.HasKey("Currency"))
        {
            currency = PlayerPrefs.GetInt("Currency");
        }
        else
        {
            currency = 0;
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
            this.currency++;

            PlayerPrefs.SetInt("Currency", currency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    public void setCurrencyWithVar(int currencyVar)
    {
        if (myPV.IsMine)
        {
            this.currency =+ currencyVar;

            PlayerPrefs.SetInt("Currency", currency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    public void decreaseCurrency()
    {
        if (myPV.IsMine)
        {
            this.currency--;

            PlayerPrefs.SetInt("Currency", currency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    public void decreaseCurrencyCoinMagnet()
    {
        if (!myPV.IsMine)
        {
            this.currency--;

            PlayerPrefs.SetInt("Currency", currency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Currency", 0);
    }


}

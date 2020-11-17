using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CoinMagnet : MonoBehaviour
{
    
    public bool isAvailable = true;
    private Button coinB;
    private Currency currency;
    private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GameManager>();
        currency = GetComponent<Currency>();
        coinB = GameObject.Find("ButtonCoinMagnet").GetComponent<Button>();
        coinB.onClick.AddListener(StealTheCoins);
        TurnOffCoinMagnet();
    }

    public void StealTheCoins()
    {
        if (GM.PV.IsMine)
        {
            TurnOffCoinMagnet();
            GM.hasUsedPowerUp = true;
        }

        currency.setCurrencyWithVar(currency.CheckHowManyHaveMoney(PhotonNetwork.LocalPlayer.NickName));
        currency.decreaseCurrencyCoinMagnet(PhotonNetwork.LocalPlayer.NickName);
        GM.PV.RPC("PlayCoinStealSound", RpcTarget.AllBuffered);
    }

    public void TurnOffCoinMagnet()
    {
        if (isAvailable)
        {
            isAvailable = false;
            coinB.interactable = false;
        } 
    }

    public void TurnOnCoinMagnet()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            coinB.interactable = true;
        }
    }

}

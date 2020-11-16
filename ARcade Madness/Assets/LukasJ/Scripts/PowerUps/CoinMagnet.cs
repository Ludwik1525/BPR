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

    // Start is called before the first frame update
    void Start()
    {
        currency = GetComponent<Currency>();
        coinB = GameObject.Find("ButtonCoinMagnet").GetComponent<Button>();
        coinB.onClick.AddListener(StealTheCoins);

        if (isAvailable)
        {
            coinB.interactable = true;
        }
        else
            coinB.interactable = false;
    }

    public void StealTheCoins()
    {
        currency.setCurrencyWithVar(currency.CheckHowManyHaveMoney(PhotonNetwork.LocalPlayer.NickName));
        currency.decreaseCurrencyCoinMagnet(PhotonNetwork.LocalPlayer.NickName);
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

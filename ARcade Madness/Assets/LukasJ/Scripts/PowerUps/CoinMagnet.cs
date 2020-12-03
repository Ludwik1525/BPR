using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CoinMagnet : MonoBehaviour
{
    public bool isAvailable = true;

    private Button coinB;

    private Currency currency;
    private GameManager GM;
    

    void Start()
    {
        GM = GetComponent<GameManager>();
        currency = GetComponent<Currency>();
        coinB = GameObject.Find("ButtonCoinMagnet").GetComponent<Button>();

        coinB.onClick.AddListener(StealTheCoins);

        TurnOffCoinMagnet();
    }

    // function to use steal coins from other players and give them for the player who used this power-up
    public void StealTheCoins()
    {
        if (GM.PV.IsMine)
        {
            TurnOffCoinMagnet();
            GM.hasUsedPowerUp = true;

            switch (PlayerPrefs.GetInt("MyPowerups"))
            {
                case 3:
                    PlayerPrefs.SetInt("MyPowerups", 0);
                    break;
                case 5:
                    PlayerPrefs.SetInt("MyPowerups", 1);
                    break;
                case 6:
                    PlayerPrefs.SetInt("MyPowerups", 2);
                    break;
                case 7:
                    PlayerPrefs.SetInt("MyPowerups", 4);
                    break;
            }
        }

        currency.setCurrencyWithVar(currency.CheckHowManyHaveMoney(PhotonNetwork.LocalPlayer.NickName));
        currency.decreaseCurrencyCoinMagnet(PhotonNetwork.LocalPlayer.NickName);
        GM.PV.RPC("PlayCoinStealSound", RpcTarget.AllBuffered);
    }

    // function to disable the power-up
    public void TurnOffCoinMagnet()
    {
        if (isAvailable)
        {
            isAvailable = false;
            coinB.interactable = false;
        } 
    }

    // function to enable the power-up
    public void TurnOnCoinMagnet()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            coinB.interactable = true;
        }
    }

}

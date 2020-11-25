using UnityEngine;
using Photon.Pun;

public class Currency : MonoBehaviour
{
    private int currency;

    private PhotonView myPV;

    private ScoreInfo si;


    private void Start()
    {
        si = FindObjectOfType<ScoreInfo>();
        myPV = GetComponent<PhotonView>();

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

    // increase my currency by one
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

    // increase my currecny with a specific value
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

    // decrease my currency with a specific value
    public void decreaseCurrency(int chestCost)
    {
        if (myPV.IsMine)
        {
            this.currency = (int)PhotonNetwork.LocalPlayer.CustomProperties["Currency"];
            this.currency -= chestCost;
            
            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", currency);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);

            si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
        }
    }

    // check how many other players have coins
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

    // decrese the amount of coins for other players who have it (while using the magnet power-up)
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

    // defice behaviour when closing the application
    private void OnApplicationQuit()
    {
        onQuit();
    }

    // zero out the amount of coins
    void onQuit()
    {
        currency = 0;
        ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
        thisCurrency.Add("Currency", currency);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);

        si.GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], currency);
    }
}

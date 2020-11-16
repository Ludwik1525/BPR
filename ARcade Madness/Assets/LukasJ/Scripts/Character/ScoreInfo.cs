using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ScoreInfo : MonoBehaviour
{
    private PhotonView PV;
    private GameObject winScores;
    private List<string> finalNames;
    private List<int> finalScores;
    private string[] namesToDisplay;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        winScores = GameObject.Find("Canvas").transform.GetChild(2).GetChild(2).gameObject;

        finalNames = new List<string>();
        finalScores = new List<int>();
        namesToDisplay = new string[PhotonNetwork.PlayerList.Length];

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("ActivateChildren", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void ActivateChildren()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public void SortPlayersOrder()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            finalNames.Add(this.gameObject.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text + "  " +
                this.gameObject.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text);
            finalScores.Add(int.Parse(this.gameObject.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text));
        }

        finalScores.Sort();
        finalScores.Reverse();

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            string checker = finalScores[i].ToString();
            for(int j = 0; j < finalNames.Count; j++)
            {
                if(finalNames[j].Contains(checker))
                {
                    namesToDisplay[i] = finalNames[j];
                }
            }
        }

        PV.RPC("SetFinalScores", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SetFinalScores()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i < 3)
            {
                winScores.transform.GetChild(i).gameObject.SetActive(true);

                winScores.transform.GetChild(i).GetComponent<Text>().text = (i + 1) + ".  " + namesToDisplay[i];
            }
        }
    }

    [PunRPC]
    void SetScore(int index, int score)
    {
        this.gameObject.transform.GetChild(index).GetChild(0).GetChild(2).GetComponent<Text>().text = "" + score;
    }

    [PunRPC]
    void SetCurrency(int index, int currency)
    {
        this.gameObject.transform.GetChild(index).GetChild(0).GetChild(1).GetComponent<Text>().text = "" + currency;
    }
}

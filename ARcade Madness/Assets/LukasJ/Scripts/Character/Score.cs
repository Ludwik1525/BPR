using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Score : MonoBehaviour
{
    private ScoreInfo si;
    private PhotonView myPV;
    private int score;

    private void Start()
    {
        si = FindObjectOfType<ScoreInfo>();
        myPV = GetComponent<PhotonView>();
        if(myPV.IsMine)
        {
            if(PhotonNetwork.LocalPlayer.CustomProperties["score"] != null)
                si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"],
                    (int)PhotonNetwork.LocalPlayer.CustomProperties["score"]);
            else
            {
                ExitGames.Client.Photon.Hashtable thisScore = new ExitGames.Client.Photon.Hashtable();
                thisScore.Add("score", 0);
                PhotonNetwork.LocalPlayer.SetCustomProperties(thisScore);
            }
        }
        
    }

    public void setScore(int score)
    {
        this.score += score;

        ExitGames.Client.Photon.Hashtable thisScore = new ExitGames.Client.Photon.Hashtable();
        thisScore.Add("score", score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisScore);

        if(myPV.IsMine)
            si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], this.score);
    }
    public int getScore()
    {
        return PlayerPrefs.GetInt("score");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("score", 0);
    }

}

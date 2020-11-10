using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Score : MonoBehaviour
{
    private ScoreInfo si;
    private PhotonView myPV;
    private int score = 0;

    private void Start()
    {
        si = FindObjectOfType<ScoreInfo>();
        myPV = GetComponent<PhotonView>();
        if(myPV.IsMine)
        {
            if (!PlayerPrefs.HasKey("score"))
            {
                PlayerPrefs.SetInt("score", score);
            }
            setScore(0);
        }
        
    }

    public void setScore(int score)
    {
        this.score += score;
        PlayerPrefs.SetInt("score", this.score);

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

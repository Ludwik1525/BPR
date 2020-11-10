using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Score : MonoBehaviour
{
    private ScoreInfo si;
    private PhotonView myPV;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
        }
        setScore(0);
        si = FindObjectOfType<ScoreInfo>();
        myPV = GetComponent<PhotonView>();
    }

    public void setScore(int score)
    {
        PlayerPrefs.SetInt("score", getScore() + score);

        if(myPV.IsMine)
            si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], getScore());
    }
    public int getScore()
    {
        return PlayerPrefs.GetInt("score");
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.SetInt("score", 0);
    }

}

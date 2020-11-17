using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
        }
        else
        {
            score = 0;
        }

        if(myPV.IsMine)
        {
            si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"],
                       PlayerPrefs.GetInt("Score"));
            print("My score " + PlayerPrefs.GetInt("Score"));
        }
    }

    public void setScore()
    {
        if (myPV.IsMine)
        {
            this.score++;

            PlayerPrefs.SetInt("Score", score);

            si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], PlayerPrefs.GetInt("Score"));
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Score", 0);
    }
}

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
        PlayerPrefs.SetInt("score", 0);
        si = FindObjectOfType<ScoreInfo>();
        myPV = GetComponent<PhotonView>();
    }

    public void setScore()
    {
        PlayerPrefs.SetInt("score", getScore() + 1);

        if(myPV.IsMine)
            si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], getScore());
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

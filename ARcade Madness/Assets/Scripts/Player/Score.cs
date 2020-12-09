using UnityEngine;
using Photon.Pun;

public class Score : MonoBehaviour
{
    private int score;

    private PhotonView myPV;

    private ScoreInfo si;


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
        }
    }

    // set my score
    public void setScore()
    {
        if (myPV.IsMine)
        {
            this.score++;

            PlayerPrefs.SetInt("Score", score);

            si.GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], PlayerPrefs.GetInt("Score"));
        }
    }

    // set my score to 0 while closing the application
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Score", 0);
    }
}

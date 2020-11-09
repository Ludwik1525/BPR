using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Score : MonoBehaviour
{
    private GameObject scoresParent;
    private GameObject score;

    private PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        InstantiateMyScore();
    }
    
    void InstantiateMyScore()
    {
        if (PV.IsMine)
        {
            GameObject score = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ScorePrefab"),
                new Vector3(0, -20 * (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0), Quaternion.identity);

            PV.RPC("SetScoresParent", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SetScoresParent()
    {
        if (PV.IsMine)
        {
            scoresParent = GameObject.Find("Scores");
            score.transform.SetParent(scoresParent.transform);
        }
    }

    private IEnumerator WaitAndSetParent()
    {
        yield return new WaitForSeconds(1f);
        PV.RPC("SetScoresParent", RpcTarget.AllBuffered);
    }
}

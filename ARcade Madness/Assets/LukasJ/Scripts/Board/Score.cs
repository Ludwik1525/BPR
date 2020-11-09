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
        scoresParent = GameObject.Find("Scores");
    }
    
    void InstantiateMyScore()
    {
        score = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ScorePrefab"),
            new Vector3(0, -20*(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0), Quaternion.identity);

        PV.RPC("SetParent", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SetParent()
    {
        score.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    void Update()
    {
        
    }
}

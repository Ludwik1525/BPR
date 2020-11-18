using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class AttackScript : MonoBehaviour
{
    public Button attackB;
    private Animator animator;
    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;

    void Start()
    {
        attackB.onClick.AddListener(CastFireball);
    }

    private void Update()
    {
        
    }

    private void CastFireball()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), this.gameObject.transform.position, Quaternion.identity);
        Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
    }
}

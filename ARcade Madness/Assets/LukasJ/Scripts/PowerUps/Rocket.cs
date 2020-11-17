using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Rocket : MonoBehaviour
{
    public bool isAvailable = true;
    private Button rocketB;
    private GameManager BPC;
    private int numberOfTilesToMove = 5;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        BPC = GetComponent<GameManager>();
        rocketB = GameObject.Find("ButtonRocket").GetComponent<Button>();
        rocketB.onClick.AddListener(UseRocket);
        TurnOffRocket();
    }

    public void UseRocket()
    {
        if (BPC.PV.IsMine)
        {
            BPC.hasUsedPowerUp = true;
            int randomNo = Random.Range(5, 8);
            PV.RPC("UnifyTheRandomNumber", RpcTarget.AllBuffered, randomNo);
            StartCoroutine(BPC.MoveWithRocket(numberOfTilesToMove));
        }
    }

    public void TurnOffRocket()
    {
        if (isAvailable)
        {
            isAvailable = false;
            rocketB.interactable = false;
        }
    }

    public void TurnOnRocket()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            rocketB.interactable = true;
        }
    }

    [PunRPC]
    void UnifyTheRandomNumber(int number)
    {
        numberOfTilesToMove = number;
    }
}

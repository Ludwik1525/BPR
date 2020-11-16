using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Rocket : MonoBehaviour
{
    public bool isAvailable = true;
    private Button rocketB;
    private BoardPlayerController BPC;

    // Start is called before the first frame update
    void Start()
    {
        BPC = GetComponent<BoardPlayerController>();
        rocketB = GameObject.Find("ButtonRocket").GetComponent<Button>();
        rocketB.onClick.AddListener(UseRocket);
        isAvailable = false;
        TurnOnRocket();
    }

    public void UseRocket()
    {
        if (BPC.PV.IsMine)
        {
            BPC.hasUsedPowerUp = true;
            StartCoroutine(BPC.MoveWithRocket(3));
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
}

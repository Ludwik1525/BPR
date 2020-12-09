using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class PlayerSetup : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //The player is local player
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            //The player is remote player
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void SetName(string name)
    {
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }
}

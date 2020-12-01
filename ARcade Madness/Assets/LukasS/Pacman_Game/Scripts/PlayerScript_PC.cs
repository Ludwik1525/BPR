using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript_PC : MonoBehaviourPun
{
    public TextMeshProUGUI score_txt;
    private int score;
    private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            //The player is local player
            transform.GetComponent<MovementController_PC>().enabled = true;
            transform.GetComponent<MovementController_PC>().joystick.gameObject.SetActive(true);
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            //The player is remote player
            transform.GetComponent<MovementController_PC>().enabled = false;
            transform.GetComponent<MovementController_PC>().joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(pv.IsMine)
            score_txt.text = "Score: " + score;
    }


    [PunRPC]
    private void SetName(string name)
    {
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PacmanPoint"))
        {
            score++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("PacmanGhost"))
            pv.RPC("KillMe", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void KillMe()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    void EnableEndScreen()
    {
        FindObjectOfType<BoardMenus>().TurnOnWinScreen();
    }
}


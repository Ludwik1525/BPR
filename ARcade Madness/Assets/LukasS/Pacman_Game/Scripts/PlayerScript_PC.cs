using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript_PC : MonoBehaviourPun
{
    public TextMeshProUGUI score_txt;
    public TextMeshProUGUI playerNameText;
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

        }
        else
        {
            //The player is remote player
            transform.GetComponent<MovementController_PC>().enabled = false;
            transform.GetComponent<MovementController_PC>().joystick.gameObject.SetActive(false);
        }

        SetPlayerName();
    }

    void Update()
    {
        if(pv.IsMine)
            score_txt.text = "Score: " + score;
    }

    void SetPlayerName()
    {
        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                //playerNameText.text = photonView.Owner.NickName;
            }
        }
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


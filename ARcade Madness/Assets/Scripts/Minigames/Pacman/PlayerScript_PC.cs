﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript_PC : MonoBehaviourPun
{
    public TextMeshProUGUI score_txt;
    private int score;
    private PhotonView PV;

    private GameObject winScreen;
    int wonPrize;
    public Sprite[] powerupImgs;
    private GameObject pointParent;
    public int placement;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        winScreen = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        pointParent = GameObject.Find("Dots");
        if (PV.IsMine)
        {
            //The player is local player
            //transform.GetComponent<MovementController_PC>().enabled = true;
            //transform.GetComponent<MovementController_PC>().joystick.gameObject.SetActive(true);
            transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            //The player is remote player
            //transform.GetComponent<MovementController_PC>().enabled = false;
            //transform.GetComponent<MovementController_PC>().joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(PV.IsMine)
            score_txt.text = "Score: " + score;

        if(pointParent.transform.childCount < 1)
        {
            SendMyInfo();
        }
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
            FindObjectOfType<GameManager_PC>().GetComponent<PhotonView>().RPC("PlayCoinCollectSound", RpcTarget.AllBuffered);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("PacmanGhost"))
        {
            FindObjectOfType<GameManager_PC>().GetComponent<PhotonView>().RPC("PlayDeathSound", RpcTarget.AllBuffered);
            PV.RPC("KillMe", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void KillMe()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
        SendMyInfo();
    }

    private void SendMyInfo()
    {
        if (PV.IsMine)
        {
            FindObjectOfType<GameManager_PC>().GetComponent<PhotonView>().RPC("SubstractPlayersLeft", RpcTarget.AllBuffered);
            FindObjectOfType<GameManager_PC>().GetComponent<PhotonView>().RPC("AddMeToLists", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName, score);
        }
    }

    [PunRPC]
    void EnableEndScreen()
    {
        FindObjectOfType<BoardMenus>().TurnOnWinScreen();
    }


    public void DisplayScore()
    {
        if (PV.IsMine)
        {
            PV.RPC("SetScores", RpcTarget.AllBuffered, placement - 1);
        }
    }


    [PunRPC]
    public void DisplayCoins()
    {
        if (PV.IsMine)
        {
            PV.RPC("SetCoins", RpcTarget.AllBuffered, placement - 1, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    [PunRPC]
    private void SetScores(int pos)
    {
        if (PV.IsMine)
        {
            int random = 0;

            if (pos == 0)
            {
                random = SetRandomPowerup();

                PV.RPC("SetPrizeWon", RpcTarget.AllBuffered, random);

                SaveMyPowerups();
            }

        }
    }


    [PunRPC]
    private void SetCoins(int pos, string name)
    {
        if (PV.IsMine)
        {
            if (wonPrize != 3)
            {
                PlayerPrefs.SetInt("PlaceFromLastMinigame", PhotonNetwork.PlayerList.Length - pos);
            }
            else
            {
                PlayerPrefs.SetInt("PlaceFromLastMinigame", PhotonNetwork.PlayerList.Length - pos + 1);
            }

            print("COINS I AM GETTING :" + PlayerPrefs.GetInt("PlaceFromLastMinigame"));
            print("MY POWERUPS: " + PlayerPrefs.GetInt("MyPowerups"));
        }

        if (pos == 0 && wonPrize == 3)
            winScreen.transform.GetChild(2).GetChild(pos).GetComponent<Text>().text = pos + 1 + ". " + name + ", " + (PhotonNetwork.PlayerList.Length - pos + 1);
    }


    [PunRPC]
    private void SetMyParent()
    {
        transform.SetParent(GameObject.Find("PlayersParent").transform);
    }

    [PunRPC]
    void SetPrizeWon(int no)
    {
        wonPrize = no;
        if (no != 3)
            winScreen.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = powerupImgs[wonPrize];
        else
            winScreen.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    int SetRandomPowerup()
    {
        switch (PlayerPrefs.GetInt("MyPowerups"))
        {
            case 0:
                return Random.Range(0, 3);
            case 1:
                return Random.Range(1, 3);
            case 2:
                int newRand = Random.Range(0, 2);
                if (newRand == 0)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            case 3:
                return Random.Range(0, 2);
            case 4:
                return 2;
            case 5:
                return 1;
            case 6:
                return 0;
            default:
                return 3;
        }
    }

    void SaveMyPowerups()
    {
        switch (wonPrize)
        {
            case 0:
                switch (PlayerPrefs.GetInt("MyPowerups"))
                {
                    case 0:
                        PlayerPrefs.SetInt("MyPowerups", 1);
                        break;
                    case 2:
                        PlayerPrefs.SetInt("MyPowerups", 4);
                        break;
                    case 3:
                        PlayerPrefs.SetInt("MyPowerups", 5);
                        break;
                    case 6:
                        PlayerPrefs.SetInt("MyPowerups", 7);
                        break;
                }
                break;
            case 1:
                switch (PlayerPrefs.GetInt("MyPowerups"))
                {
                    case 0:
                        PlayerPrefs.SetInt("MyPowerups", 2);
                        break;
                    case 1:
                        PlayerPrefs.SetInt("MyPowerups", 4);
                        break;
                    case 3:
                        PlayerPrefs.SetInt("MyPowerups", 6);
                        break;
                    case 5:
                        PlayerPrefs.SetInt("MyPowerups", 7);
                        break;
                }
                break;
            case 2:
                switch (PlayerPrefs.GetInt("MyPowerups"))
                {
                    case 0:
                        PlayerPrefs.SetInt("MyPowerups", 3);
                        break;
                    case 1:
                        PlayerPrefs.SetInt("MyPowerups", 5);
                        break;
                    case 2:
                        PlayerPrefs.SetInt("MyPowerups", 6);
                        break;
                    case 4:
                        PlayerPrefs.SetInt("MyPowerups", 7);
                        break;
                }
                break;
        }
    }
}


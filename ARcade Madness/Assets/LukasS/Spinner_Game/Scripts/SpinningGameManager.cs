﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class SpinningGameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject playerLoading_btn;

    private GameObject button;

    private List<GameObject> buttons = new List<GameObject>();

    public Button ready_btn;

    private PhotonView pv;

    public GameObject menu;

    public List<GameObject> players = new List<GameObject>();

    private Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var player in PhotonNetwork.PlayerList)
        {
            button = Instantiate(playerLoading_btn, menu.transform);
            button.transform.position += offset;
            button.transform.GetChild(0).gameObject.GetComponent<Text>().text = player.NickName;
            offset += new Vector3(0, -150, 0);

            buttons.Add(button);
        }

        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Player_Spinner(Clone)")
            {
                if(go.GetComponent<PhotonView>().IsMine)
                    pv = go.GetComponent<PhotonView>();
            }
                
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ready()
    {
        foreach(var butt in buttons)
        {
            if(butt.transform.GetChild(0).gameObject.GetComponent<Text>().text == PhotonNetwork.NickName)
            {
                print(PhotonNetwork.NickName);

                pv.RPC("ReadyIndication", RpcTarget.AllBuffered, butt);

                //butt.transform.GetChild(1).gameObject.SetActive(false);
                //butt.transform.GetChild(2).gameObject.SetActive(true);
                ready_btn.interactable = false;
            }
        }
    }

    [PunRPC]
    public void ReadyIndication(GameObject a)
    {
        print("aaa");
        a.transform.GetChild(1).gameObject.SetActive(false);
        a.transform.GetChild(2).gameObject.SetActive(true);
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RespawnChestNextToPlayer : MonoBehaviour
{
    public bool isAvailable = true;
    private Button respawnChestB;
    private GameManager BPC;
    private SpawnChest spawnChest;

    // Start is called before the first frame update
    void Start()
    {
        spawnChest = FindObjectOfType<SpawnChest>();
        BPC = GetComponent<GameManager>();
        respawnChestB = GameObject.Find("ButtonChestRespawn").GetComponent<Button>();
        respawnChestB.onClick.AddListener(UseChestRespawn);
        TurnOffChestRespawn();
    }

    public void UseChestRespawn()
    {
        if (BPC.PV.IsMine)
        {
            BPC.hasUsedPowerUp = true;
        }

        FindObjectOfType<SpawnChest>().PV.RPC("DestroyChest", RpcTarget.AllBuffered, true);

        if (BPC.PV.IsMine)
        {
            spawnChest.SpawningChestSequenceDetermined(PlayerPrefs.GetInt("totalPos") + 1);
        }
    }

    public void TurnOffChestRespawn()
    {
        if (isAvailable)
        {
            isAvailable = false;
            respawnChestB.interactable = false;
        }
    }

    public void TurnOnChestRespawn()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            respawnChestB.interactable = true;
        }
    }
}

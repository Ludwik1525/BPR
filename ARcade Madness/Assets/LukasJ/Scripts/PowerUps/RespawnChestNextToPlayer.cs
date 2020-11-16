using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnChestNextToPlayer : MonoBehaviour
{
    public bool isAvailable = true;
    private Button respawnChestB;
    private BoardPlayerController BPC;
    private SpawnChest spawnChest;

    // Start is called before the first frame update
    void Start()
    {
        spawnChest = FindObjectOfType<SpawnChest>();
        BPC = GetComponent<BoardPlayerController>();
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

        spawnChest.DestroyChest(true);
        spawnChest.SpawningChestSequence(true, PlayerPrefs.GetInt("totalPos") + 1);
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

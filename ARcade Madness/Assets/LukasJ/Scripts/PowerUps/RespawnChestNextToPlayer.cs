using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RespawnChestNextToPlayer : MonoBehaviour
{
    public bool isAvailable = true;

    private Button respawnChestB;

    private GameManager BPC;
    private SpawnChest spawnChest;
    

    void Start()
    {
        spawnChest = FindObjectOfType<SpawnChest>();
        BPC = GetComponent<GameManager>();
        respawnChestB = GameObject.Find("ButtonChestRespawn").GetComponent<Button>();

        respawnChestB.onClick.AddListener(UseChestRespawn);

        TurnOffChestRespawn();
    }

    // function to re-spawn the chest in front of the player
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

    // function to disable the power-up
    public void TurnOffChestRespawn()
    {
        if (isAvailable)
        {
            isAvailable = false;
            respawnChestB.interactable = false;
        }
    }

    // function to enable the power-up
    public void TurnOnChestRespawn()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            respawnChestB.interactable = true;
        }
    }
}

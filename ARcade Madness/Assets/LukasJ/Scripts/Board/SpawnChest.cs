using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnChest : MonoBehaviour
{
    private GameObject chest;
    private PhotonView PV;
    private Transform[] childObjects;
    private List<Transform> tilesToSpawnChestsOn;
    int rand;

    private void Start()
    {
        childObjects = FindObjectOfType<Route>().GetComponentsInChildren<Transform>();
        tilesToSpawnChestsOn = new List<Transform>();

        foreach (Transform child in childObjects)
        {
            if (child != this.transform)
            {
                if (child.transform.gameObject.tag == "Tile" && child.transform.gameObject.name.Contains("Simple"))
                {
                    tilesToSpawnChestsOn.Add(child);
                }

            }
        }

        PV = GetComponent<PhotonView>();
        SpawningChestSequence();
    }

    public void SpawnChests()
    {
        chest = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chest"), tilesToSpawnChestsOn[rand].GetChild(2).transform.position, tilesToSpawnChestsOn[rand].GetChild(2).transform.rotation);
    }

    [PunRPC]
    private void ChooseRandomNumber()
    {
        rand = Random.Range(1, tilesToSpawnChestsOn.Count);
        rand = 2;
        PlayerPrefs.SetInt("random", rand);
    }

    private IEnumerator WaitAndSetParent()
    {
        yield return new WaitForSeconds(1f);
        PV.RPC("SetChestsParent", RpcTarget.AllBuffered);
    }

    public void ResetChestPosition()
    {
        PlayerPrefs.SetInt("random", 0);
    }

    private void OnApplicationQuit()
    {
        ResetChestPosition();
    }

    [PunRPC]
    public void SetChestsParent()
    {
        if (chest == null)
            chest = GameObject.Find("Chest(Clone)");

        chest.transform.parent = tilesToSpawnChestsOn[rand].transform;
    }

    public void DestroyChest()
    {
        PlayerPrefs.SetInt("random", 0);
        Destroy(chest);
        SpawningChestSequence();

    }

    private void SpawningChestSequence()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            rand = PlayerPrefs.GetInt("random");
            if (rand == 0)
            {
                PV.RPC("ChooseRandomNumber", RpcTarget.AllBuffered);
            }
            SpawnChests();
        }

        StartCoroutine("WaitAndSetParent");
    }
}

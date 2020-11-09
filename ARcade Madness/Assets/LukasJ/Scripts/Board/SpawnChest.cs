using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class SpawnChest : MonoBehaviour
{
    private GameObject chest;
    private PhotonView PV;
    private Transform[] childObjects;
    private List<Transform> tilesToSpawnChestsOn;
    int rand;
    int numberToIncrease;

    private void Start()
    {
        numberToIncrease = 0;
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
        //rand = UnityEngine.Random.Range(1, tilesToSpawnChestsOn.Count);
        rand = UnityEngine.Random.Range(0, 2);

        foreach (int tileNumber in GameController.gc.currentPositions)
        {
            if (rand == tileNumber)
                ChooseRandomNumber();
        }

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

        transform.parent = gameObject.transform;
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

    public int GetRealTileNo()
    {
        numberToIncrease = 0;
        int max = rand;
        int check = 0;

        GameObject[] allTiles = new GameObject[FindObjectOfType<Route>().transform.childCount - 1];

        for (int i = 0; i < FindObjectOfType<Route>().transform.childCount - 1; i++)
        {
            if (i < 9)
            {
                allTiles[i] = FindObjectOfType<Route>().transform.GetChild(i).gameObject;
            }
            else
            {
                allTiles[i] = FindObjectOfType<Route>().transform.GetChild(i + 1).gameObject;
            }
        }

        while (check <= max)
        {
            if (allTiles[check].transform.gameObject.tag == "Tile")
            {
                if (!allTiles[check].transform.gameObject.name.Contains("Simple"))
                {
                    numberToIncrease++;
                    max++;
                }
            }
            check++;
        }

        //for (int i = 0; i < rand + numberToIncrease; i++)
        //{
        //    if (allTiles[i].transform.gameObject.tag == "Tile")
        //        {
        //            if (!allTiles[i].transform.gameObject.name.Contains("Simple"))
        //            {
        //                numberToIncrease++;
        //            }
        //        }
        //}

        return rand + numberToIncrease;
    }
}

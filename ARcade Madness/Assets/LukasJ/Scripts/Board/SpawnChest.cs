using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnChest : MonoBehaviour
{
    private GameObject chest;
    public PhotonView PV;
    private Transform[] childObjects;
    private List<Transform> tilesToSpawnChestsOn;
    int rand;
    int numberToIncrease;
    bool isNOTRand;

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
    private void ChooseRandomNumber(int number)
    {
        rand = number;
        PlayerPrefs.SetInt("random", rand);
    }

    private IEnumerator WaitAndSetParent()
    {
        yield return new WaitForSeconds(1f);
        PV.RPC("SetChestsParent", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(1f);
        print("Number saved: " + PlayerPrefs.GetInt("random"));
        
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

        chest.transform.parent = this.gameObject.transform;
    }

    [PunRPC]
    public void DestroyChest(bool isRand)
    {
            Destroy(GameObject.FindGameObjectWithTag("Chest"));

        if (!isRand)
            {
                SpawningChestSequence();
            }
    }
    
    public void SpawningChestSequence()
    {
        rand = PlayerPrefs.GetInt("random");

        if (PhotonNetwork.IsMasterClient)
        {
            if (rand == 0)
            {
                bool isTileTaken = true;

                while (isTileTaken)
                {
                    isTileTaken = false;
                    rand = Random.Range(1, tilesToSpawnChestsOn.Count);
                    if (rand > FindObjectOfType<Route>().transform.childCount - 1)
                    {
                        rand = 0;
                    }

                    foreach (int tileNumber in GameController.gc.currentPositions)
                    {
                        if (GetRealTileNo(true) == tileNumber)
                            isTileTaken = true;
                    }
                }

                PV.RPC("ChooseRandomNumber", RpcTarget.AllBuffered, rand);
            }
            SpawnChests();
        }
        StartCoroutine("WaitAndSetParent");
    }

    public void SpawningChestSequenceDetermined(int tileToSpawnOn)
    {
        rand = tileToSpawnOn;
        rand = GetRealTileNo(false) - 1;
        bool isTileTaken = true;

        while (isTileTaken)
        {
            isTileTaken = false;
            rand = rand + 1;
            if(rand > FindObjectOfType<Route>().transform.childCount - 1)
            {
                rand = 0;
            }

            foreach (int tileNumber in GameController.gc.currentPositions)
            {
                if (GetRealTileNo(true) == tileNumber)
                    isTileTaken = true;
            }
        }

        PV.RPC("ChooseRandomNumber", RpcTarget.AllBuffered, rand);

        SpawnChests();
        StartCoroutine("WaitAndSetParent");
    }


    public int GetRealTileNo(bool convertingToChest)
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

        if(convertingToChest)
        {
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
        }
        else
        {
            if(rand == allTiles.Length - 1)
            {
                return 0;
            }

            for(int i = 0; i < max; i++)
            {
                if (allTiles[i].transform.gameObject.tag == "Tile")
                {
                    if (!allTiles[i].transform.gameObject.name.Contains("Simple"))
                    {
                        numberToIncrease--;
                    }
                }
            }
        }

        return rand + numberToIncrease;
    }
}

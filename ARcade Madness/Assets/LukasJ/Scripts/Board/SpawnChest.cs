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

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("ChooseRandomNumber", RpcTarget.AllBuffered);
            SpawnChests();
        }

        StartCoroutine("WaitAndSetParent");

        
    }

    public void SpawnChests()
    {

        chest = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chest"), tilesToSpawnChestsOn[rand].GetChild(2).transform.position, tilesToSpawnChestsOn[rand].GetChild(2).transform.rotation);
    }

    [PunRPC]
    private void ChooseRandomNumber()
    {
        rand = Random.Range(0, tilesToSpawnChestsOn.Count);
    }

    private IEnumerator WaitAndSetParent()
    {
        yield return new WaitForSeconds(1f);
        PV.RPC("SetChestsParent", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetChestsParent()
    {
        if (chest == null)
            chest = GameObject.Find("Chest(Clone)");

        transform.parent = gameObject.transform;
        if (tilesToSpawnChestsOn[rand].transform.parent.GetComponent<TileChestCheck>() != null)
            tilesToSpawnChestsOn[rand].transform.parent.GetComponent<TileChestCheck>().iHaveAChest = true;
    }
}

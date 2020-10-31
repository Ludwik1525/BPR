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
        PV = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("ChooseRandomNumber", RpcTarget.AllBuffered);
            SpawnChests();
        }

        StartCoroutine("WaitAndSetParent");

        childObjects = GetComponentsInChildren<Transform>();

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
    }

    public void SpawnChests()
    {

        chest = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chest"), tilesToSpawnChestsOn[rand].transform.position, tilesToSpawnChestsOn[rand].transform.rotation);
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
        tilesToSpawnChestsOn[rand].transform.parent.GetComponent<TileChestCheck>().iHaveAChest = true;
    }
}

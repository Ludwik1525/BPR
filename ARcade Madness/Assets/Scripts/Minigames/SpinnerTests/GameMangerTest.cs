using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameMangerTest : MonoBehaviour
{
    private GameObject player;
    public Transform[] spawnPositions;

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        //Vector3 instantiatePosition = spawnPositions[0].position;
        //UnityEngine.Object pPrefab = Resources.Load("Assets/Resources/Player_Spinner 1"); // note: not .prefab!

        //var pPrefab = AssetDatabase.LoadAssetAtPath("Assets/Resources/Player_Spinner 1",typeof(GameObject)); // note: not .prefab!
        //print(pPrefab);
        //player = (GameObject)GameObject.Instantiate(pPrefab, spawnPositions[0].position, Quaternion.identity);

        player = (GameObject)Instantiate(Resources.Load("Player_Spinner 1"),new Vector3(0.2f,0.14f,0.12f),Quaternion.identity);
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}

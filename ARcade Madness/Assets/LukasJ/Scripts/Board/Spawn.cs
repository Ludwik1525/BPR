using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private SpawnPosition[] spawnPositions;

    private void Awake()
    {
        spawnPositions = GetComponentsInChildren<SpawnPosition>();
    }

    public Transform AssignSpawnPosition()
    {
       foreach(SpawnPosition pos in spawnPositions)
       {
            if(pos.isFree)
            {
                pos.isFree = false;
                return pos.transform;
            }
       }
        return null;
    }

    public void LeaveSpawnPosition(SpawnPosition position)
    {
        foreach (SpawnPosition pos in spawnPositions)
        {
            if(pos == position)
            {
                pos.isFree = true;
            }
        }
    }



}

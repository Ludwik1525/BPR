using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public SpawnPosition[] spawnPositions;

    private void Awake()
    {
        spawnPositions = GetComponentsInChildren<SpawnPosition>();
    }

    public Transform AssignSpawnPosition(int index)
    {
       for(int i = 0; i < spawnPositions.Length; i++)
       {
            if(spawnPositions[i].isFree && index == i)
            {
                spawnPositions[i].isFree = false;
                return spawnPositions[i].transform;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playersRoot;
    public Transform[] startPositions;
    [HideInInspector]
    public Transform[] players;



    private void Start()
    {
        players = playersRoot.GetComponentsInChildren<Transform>();
        SetTurns();
    }

    void SetTurns()
    {
        for(int i = 0; i < players.Length; i++)
        {
            for(int j = 0; j < startPositions.Length; j++)
            {
                if (players[i] == startPositions[j])
                {
                    players[i].GetComponent<BoardPlayerController>().SetTurn(j+1);
                }
            }
                
        }
    }

}

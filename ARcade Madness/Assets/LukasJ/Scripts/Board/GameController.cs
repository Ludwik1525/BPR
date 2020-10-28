using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public GameObject playersRoot;
    public Transform[] startPositions;
    public Transform[] players;
    public bool doesHavePosition = false;
    public int[] currentPositions;


    
    public int currentTurn = 1;

    private void OnEnable()
    {
        if (gc == null)
        {
            gc = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gc.gameObject);
        StartCoroutine(SetTurnsCo());
    }

    void SetTurns()
    {
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < startPositions.Length; j++)
            {
                if (players[i].position == startPositions[j].position)
                {
                    players[i].GetComponent<BoardPlayerController>().SetTurn(j + 1);
                }
            }

        }
    }
    IEnumerator SetTurnsCo()
    {
        yield return new WaitForSeconds(1f);
        print(playersRoot.transform.childCount);
        players = new Transform[playersRoot.transform.childCount];
        for (int i = 0; i < playersRoot.transform.childCount; i++)
        {
            players[i] = playersRoot.transform.GetChild(i);
        }
        SetTurns();
        currentPositions = new int[players.Length];
    }

    public void SaveCurrentPlayerPositions()
    {
        for(int i = 0; i < players.Length; i++)
        {
            currentPositions[i] = players[i].gameObject.GetComponent<BoardPlayerController>().routePosition;
        }
        doesHavePosition = true;
    }
}

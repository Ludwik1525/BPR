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

    private ReferenceHolder rh;

    
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
        rh = FindObjectOfType<ReferenceHolder>();
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
        rh.InitializeArray(players.Length);
    }

    public void SaveCurrentPlayerPositions()
    {
        for(int i = 0; i < players.Length; i++)
        {
            rh.playerPositions[i] = players[i].transform;
        }
        doesHavePosition = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class BoardPlayerController : MonoBehaviour
{
    
    int routePosition;
    bool isMoving;
    private PhotonView PV;
    public int turn;

    public Route currentRoute;
    public int steps;
    public float speed = 2f;
    public GameObject dice;

    //Events
    [HideInInspector]
    public UnityEvent onStartMoving;
    [HideInInspector]
    public UnityEvent onStopMoving;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        onStartMoving = new UnityEvent();
        onStopMoving = new UnityEvent();
        print(PV.ViewID);
    }

    private void Update()
    {
        //if space is pressed and player is not moving, roll the dice
        if(PV.IsMine)
        {
            if(turn == GameController.gc.currentTurn)
            {
                PV.RPC("TurnOnTheDice", RpcTarget.AllBuffered);
                if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
                {
                    steps = Random.Range(1, 7);
                    Debug.Log("Dice Rolled: " + steps);
                    StartCoroutine(Move());
                }
            }
        }
    }

    IEnumerator Move()
    {
        if (isMoving)
        {
            //if the player is already moving return
            yield break;
        }

        //set bool value to true and invoke start moving event
        PV.RPC("TurnOffTheDice", RpcTarget.AllBuffered);
        onStartMoving.Invoke();
        yield return new WaitForSeconds(2.2f);
        isMoving = true;
        
        while (steps > 0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while(MoveToNextNode(nextPos))
            {
                yield return null;
            }
            steps--;
        }
        isMoving = false;
        onStopMoving.Invoke();
        PV.RPC("IncrementTurn", RpcTarget.AllBuffered);
    }

    bool MoveToNextNode(Vector3 target)
    {
        transform.rotation = Quaternion.LookRotation(transform.position - target);
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void SetTurn(int turn)
    {
        this.turn = turn;
    }

    [PunRPC]
    public void IncrementTurn()
    {
        GameController.gc.currentTurn++;
    }
}

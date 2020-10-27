using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BoardPlayerController : MonoBehaviour
{
    
    int routePosition;
    bool isMoving;
    private PhotonView PV;
    private PhotonView dicePV;
    private bool diceGuard = false;

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
        dicePV = transform.GetChild(2).GetComponent<PhotonView>();
        onStartMoving = new UnityEvent();
        onStopMoving = new UnityEvent();
    }

    private void Update()
    {
        //if space is pressed and player is not moving, roll the dice
        if(PV.IsMine)
        {
            if(turn == GameController.gc.currentTurn)
            {
                if(!diceGuard)
                {
                    dicePV.RPC("TurnOnTheDice", RpcTarget.AllBuffered);
                    diceGuard = true;
                }
                    
                if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
                {
                    steps = Random.Range(1, 7);
                    Debug.Log("Dice Rolled: " + steps);
                    StartCoroutine(Move());
                }

                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(raycast, out raycastHit))
                    {
                        if (raycastHit.collider.name == "DiceModel")
                        {
                            steps = Random.Range(1, 7);
                            Debug.Log("Dice Rolled: " + steps);
                            StartCoroutine(Move());
                        }
                    }
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
        dicePV.RPC("TurnOnTheDice", RpcTarget.AllBuffered);
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
        diceGuard = false;
        PV.RPC("ResetTurnVar", RpcTarget.AllBuffered);
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

    [PunRPC]
    public void ResetTurnVar()
    {
        if (GameController.gc.currentTurn == GameController.gc.players.Length + 1)
        {
            GameController.gc.currentTurn = 1;
            SceneManager.LoadScene("AssetScene");
        }
    }
}

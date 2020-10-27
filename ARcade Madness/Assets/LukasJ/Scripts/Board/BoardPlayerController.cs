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

    public Route currentRoute;
    public int turn;
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
    }

    private void Update()
    {
        //if space is pressed and player is not moving, roll the dice
        if(PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
            {
                steps = Random.Range(1, 7);
                Debug.Log("Dice Rolled: " + steps);
                StartCoroutine(Move());
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
        dice.SetActive(false);
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
        dice.SetActive(true);
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
}

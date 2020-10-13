using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardPlayerController : MonoBehaviour
{
    public Route currentRoute;
    int routePosition;
    bool isMoving;

    public int steps;
    public float speed = 2f;

    //Events
    [HideInInspector]
    public UnityEvent onStartMoving;
    [HideInInspector]
    public UnityEvent onStopMoving;

    private void Awake()
    {
        onStartMoving = new UnityEvent();
        onStopMoving = new UnityEvent();
    }

    private void Update()
    {
        //if space is pressed and player is not moving, roll the dice
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Dice Rolled: " + steps);
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        if(isMoving)
        {
            //if the player is already moving return
            yield break;
        }

        //set bool value to true and invoke start moving event
        isMoving = true;
        onStartMoving.Invoke();

        while(steps > 0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while(MoveToNextNode(nextPos))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
        }
        isMoving = false;
        onStopMoving.Invoke();
    }

    bool MoveToNextNode(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
    }
}

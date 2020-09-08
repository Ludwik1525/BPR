using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlayerController : MonoBehaviour
{
    public Route currentRoute;
    int routePosition;
    bool isMoving;

    public int steps;
    public float speed = 2f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Dice Rolled: " + steps);

            StartCoroutine(Move());

            //if(routePosition + steps < currentRoute.childNodeList.Count)
            //{
            //    StartCoroutine(Move());
            //}
            //else
            //{
            //    Debug.Log("Rolled number is too high");
            //}
        }
    }

    IEnumerator Move()
    {
        if(isMoving)
        {
            yield break;
        }

        isMoving = true;
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
            //routePosition++;
        }

        isMoving = false;
    }

    bool MoveToNextNode(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
    }
}

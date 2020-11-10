using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostMovement_PC : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] points;
    bool toMove = true;
    public int random;

    public float speed = 0.02f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(toMove)
        {
            Move();
        }
    }

    void Move()
    {



        transform.rotation = Quaternion.RotateTowards(transform.rotation, points[random].transform.rotation , 360 * Time.deltaTime);

  
        transform.position = Vector3.MoveTowards(transform.position, points[random].transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PacmanPatrolPoints"))
        {
            points = other.gameObject.GetComponent<PossiblePatrolPoints_PC>().points;
            random = Random.Range(0, points.Length);
        }    
    }
}

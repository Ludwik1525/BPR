using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostMovement_PC : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] points;

    bool toMove = true;
    private int random = 0;

    public float RotationSpeed = 10f;

    public float speed = 0.02f;

    Vector3 _direction;
    Quaternion _lookRotation;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
       
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, RotationSpeed * Time.deltaTime);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, points[random].transform.rotation , 360 * Time.deltaTime);

        print("ranodm " + random);
        transform.position = Vector3.MoveTowards(transform.position, points[random].transform.position, speed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, playersParent.transform.GetChild(random).position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {


        //if(other.gameObject.CompareTag("PacmanPatrolPoints"))
        if (other.gameObject.CompareTag("PacmanPatrolPoints"))
        {
            points = other.gameObject.GetComponent<PossiblePatrolPoints_PC>().points;
            random = Random.Range(0, points.Length);

            //_direction = (playersParent.transform.GetChild(random).transform.position - transform.position);
            _direction = (points[random].transform.position - transform.position);
            _lookRotation = Quaternion.LookRotation(_direction);
        }
    }
}

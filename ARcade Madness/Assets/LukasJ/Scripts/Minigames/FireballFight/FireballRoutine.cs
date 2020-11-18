using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballRoutine : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    public GameObject parent;

    private void Awake()
    {
        parent = GameObject.Find("Player_Fireball").transform.GetChild(0).gameObject;
        StartCoroutine(Parent());
    }

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //rb.velocity = -transform.forward * speed;// * Time.deltaTime;
        rb.AddForce(-parent.transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        //this.gameObject.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        //stransform.Translate()
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }

        Destroy(this.gameObject);
    }

    IEnumerator Parent ()
    {
        transform.parent = parent.transform;
        yield return new WaitForSeconds(0.1f);
        transform.parent = null;
    }
}

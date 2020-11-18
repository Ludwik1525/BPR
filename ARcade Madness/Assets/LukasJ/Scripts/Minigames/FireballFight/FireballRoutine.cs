using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballRoutine : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //rb.velocity = -transform.forward * speed;// * Time.deltaTime;
        rb.AddForce(-transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        //this.gameObject.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        //stransform.Translate()
    }
}

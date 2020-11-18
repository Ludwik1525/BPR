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
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }

        Destroy(gameObject);
    }
}

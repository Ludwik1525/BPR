using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballRoutine : MonoBehaviour
{
    public float speed = 0.01f;

    void Start()
    {
        
    }
    
    void Update()
    {
        this.gameObject.transform.Translate(-Vector3.forward * Time.deltaTime * speed);
    }
}

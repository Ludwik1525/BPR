using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController_PC : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 2f;
    public float maxVelocityChange = 4f;

    float dr = 0f;

    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Joystick inputs
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        if (_xMovementInput != 0) dr = 90 * _xMovementInput;
        if (_zMovementInput < 0) dr = 180;
        else if (_zMovementInput > 0) dr = 0;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, dr, 0), 360 * Time.deltaTime);

        transform.position += new Vector3(_xMovementInput, 0, _zMovementInput).normalized * Time.deltaTime * speed;
    }


}

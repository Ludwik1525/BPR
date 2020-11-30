using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController_PC : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 0.8f;

    float dr = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        //Joystick inputs
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        if (_xMovementInput != 0) dr = 90 * _xMovementInput;
        if (_zMovementInput < 0) dr = 180;
        else if (_zMovementInput > 0) dr = 0;

        Vector3 newPosition = new Vector3(_xMovementInput, 0.0f, _zMovementInput);
        transform.LookAt(-newPosition + transform.position); 


        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_xMovementInput, 0, _zMovementInput), 360 * Time.deltaTime);

        transform.position += new Vector3(_xMovementInput, 0, _zMovementInput).normalized * Time.deltaTime * speed;
    }
}

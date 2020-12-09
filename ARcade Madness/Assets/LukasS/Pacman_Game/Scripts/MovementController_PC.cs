using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController_PC : MonoBehaviour
{
    public Joystick joystick;
   // public float speed = 0.8f;


    private PhotonView PV;
    private Vector3 velocityVector = Vector3.zero;
    public float speed = 2f, maxVelocityChange = 4f, tiltAmount = 10f;
    private Rigidbody rb;


    float dr = 0f;

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    //Joystick inputs
    //    float _xMovementInput = joystick.Horizontal;
    //    float _zMovementInput = joystick.Vertical;

    //    if (_xMovementInput != 0) dr = 90 * _xMovementInput;
    //    if (_zMovementInput < 0) dr = 180;
    //    else if (_zMovementInput > 0) dr = 0;

    //    Vector3 newPosition = new Vector3(_xMovementInput, 0.0f, _zMovementInput);
    //    //transform.LookAt(-newPosition + transform.position);
    //    transform.GetChild(0).LookAt(-newPosition + transform.position); // rotate just the model itself which is a child


    //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_xMovementInput, 0, _zMovementInput), 360 * Time.deltaTime);

    //    transform.position += new Vector3(_xMovementInput, 0, _zMovementInput).normalized * Time.deltaTime * speed;
    //}

    void Start()
    {
        PV = GetComponent<PhotonView>();
        joystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        if (PV.IsMine)
        {
            //Joystick inputs
            float _xMovementInput = joystick.Horizontal;
            float _zMovementInput = joystick.Vertical;

            //Velocity vectores
            Vector3 _movementHorizontal = transform.right * _xMovementInput;
            Vector3 _movementVertical = transform.forward * _zMovementInput;

            //Final movement velocity vector
            Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;
            Vector3 newPosition = new Vector3(_xMovementInput, 0.0f, _zMovementInput);


            Move(_movementVelocityVector);


            transform.GetChild(0).LookAt(-newPosition + transform.position); // rotate just the model itself which is a child
        }

    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            velocityChange.y = 0f;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    public void StopMe()
    {
        velocityVector = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
}

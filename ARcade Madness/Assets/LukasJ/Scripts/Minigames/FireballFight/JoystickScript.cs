﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JoystickScript : MonoBehaviour
{
    private PhotonView PV;
    private Joystick joystick;
    public float speed = 2f;
    public float maxVelocityChange = 4f;
    public float tiltAmount = 10f;
    public bool isPerformingAnAction;
    public bool isAlive = true;

    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        joystick = FindObjectOfType<FixedJoystick>();
        isPerformingAnAction = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if(isAlive)
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

                if (!transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<FireBallAnimator>().isBlocking && !transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<FireBallAnimator>().isCastingSpell)
                {
                    Move(_movementVelocityVector);
                }
                transform.GetChild(0).LookAt(-newPosition + transform.position);
            }
            
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

                //velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                //velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0f;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
    }

    public void StopMe()
    {
        velocityVector = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    public void Die()
    {
        isAlive = false;
    }

}

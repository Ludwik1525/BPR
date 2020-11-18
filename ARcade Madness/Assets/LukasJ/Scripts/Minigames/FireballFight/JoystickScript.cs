using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickScript : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 2f;
    public float maxVelocityChange = 4f;
    public float tiltAmount = 10f;
    public bool isPerformingAnAction;

    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        isPerformingAnAction = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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
        transform.GetChild(0).LookAt(-newPosition + transform.position);

        if(-newPosition + transform.position != transform.position)
            transform.GetChild(1).LookAt(-newPosition + transform.position);

        if(!isPerformingAnAction)
            transform.Translate(newPosition * speed * Time.deltaTime, Space.World);

        Move(_movementVelocityVector);

    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

}

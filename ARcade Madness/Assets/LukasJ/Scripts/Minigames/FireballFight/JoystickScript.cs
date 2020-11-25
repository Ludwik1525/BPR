using UnityEngine;
using Photon.Pun;

public class JoystickScript : MonoBehaviour
{
    public bool isPerformingAnAction;

    public float speed = 2f, maxVelocityChange = 4f, tiltAmount = 10f;

    private Vector3 velocityVector = Vector3.zero;

    private Rigidbody rb;

    private PhotonView PV;
    private Joystick joystick;
    private FireBallAnimator playerScript;

    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        joystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody>();
        playerScript = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<FireBallAnimator>();

        isPerformingAnAction = false;
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

                if (!playerScript.isBlocking && !playerScript.isCastingSpell)
                {
                    Move(_movementVelocityVector);
                }

                transform.GetChild(0).LookAt(-newPosition + transform.position); // rotate just the model itself which is a child
        }
    }

    // assigning a new velocity vector
    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    // adding force to the rigidbody
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

    // stopping the player when the player doesn't touch the joystick anymore
    public void StopMe()
    {
        velocityVector = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    // function called when the player gets hit by a fireball
    public void Die()
    {
        PV.RPC("DisableMyCollider", RpcTarget.AllBuffered);
    }

    // function defining whet happens when the player dies
    [PunRPC]
    void DisableMyCollider()
    {
        // playing an animation
        GetComponentInChildren<Animator>().gameObject.GetComponent<FireBallAnimator>().Die();
        // disabling the collider
        GetComponent<Collider>().enabled = false;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        if(PV.IsMine)
        {
            // disabling the canvas buttons
            playerScript.attackB.gameObject.SetActive(false);
            playerScript.blockB.gameObject.SetActive(false);
            GameObject.Find("FixedJoystick").SetActive(false);
        }

        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }
}

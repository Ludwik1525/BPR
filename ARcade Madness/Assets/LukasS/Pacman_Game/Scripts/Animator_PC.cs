using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_PC : MonoBehaviour
{
    public Transform myParent;

    private GameObject joystick;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>().transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.transform.localPosition == Vector3.zero)
        {
            RunAnimStop();
            myParent.GetComponent<MovementController_PC>().StopMe();
        }
        else
        {
            RunAnimStart();
        }
    }

    private void RunAnimStart()
    {
        if (!animator.GetBool("isRunning"))
        {
            animator.SetBool("isRunning", true);
        }
    }

    // stopping running
    private void RunAnimStop()
    {
        if (animator.GetBool("isRunning"))
        {
            animator.SetBool("isRunning", false);
        }
    }
}

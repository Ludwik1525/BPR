using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_PC : MonoBehaviour
{
    [SerializeField]
    private GameObject joystick;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.transform.GetChild(0).localPosition == Vector3.zero)
        {
            RunAnimStop();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallAnimator : MonoBehaviour
{
    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;
    public Button attackB, blockB;
    private Animator animator;
    public GameObject joystick;
    public GameObject shield;
    private bool isBlocking;

    void Start()
    {
        isBlocking = false;
        animator = GetComponent<Animator>();
        attackB.onClick.AddListener(CastFireballAnimStart);
        blockB.onClick.AddListener(Block);
    }

    private void Update()
    {
        if (joystick.transform.localPosition == Vector3.zero)
        {
            RunAnimStop();
        }
        else
            RunAnimStart();
          
    }

    private void CastFireballAnimStart()
    {
        animator.SetBool("isThrowingSpell", true);
    }

    private void CastFireballAnimStop()
    {
        animator.SetBool("isThrowingSpell", false);
    }

    private void CastShieldAnimStart()
    {
        animator.SetBool("isBlocking", true);
    }

    private void CastShieldAnimStop()
    {
        animator.SetBool("isBlocking", false);
    }

    private void RunAnimStart()
    {
        if(!animator.GetBool("isRunning"))
            animator.SetBool("isRunning", true);
    }

    private void RunAnimStop()
    {
        if (animator.GetBool("isRunning"))
            animator.SetBool("isRunning", false);
    }

    public void SpawnFireBall()
    {
        Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
    }

    private void Block()
    {
        if(!isBlocking)
        {
            isBlocking = true;
            CastShieldAnimStart();
            shield.SetActive(true);
        }
        else
        {
            isBlocking = false;
            CastShieldAnimStop();
            shield.SetActive(false);
        }
    }
}

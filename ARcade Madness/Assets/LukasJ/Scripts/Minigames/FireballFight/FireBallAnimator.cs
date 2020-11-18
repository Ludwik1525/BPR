using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallAnimator : MonoBehaviour
{
    public Transform myParent;
    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab, fireballGuiderPrefab;
    public Button attackB, blockB;
    private Animator animator;
    public GameObject joystick;
    public GameObject shield;
    private bool isBlocking, isCastingSpell;

    void Start()
    {
        isCastingSpell = false;
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
        myParent.GetComponent<JoystickScript>().isPerformingAnAction = true;
        isCastingSpell = true;
    }

    private void CastFireballAnimStop()
    {
        animator.SetBool("isThrowingSpell", false);
        myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
        isCastingSpell = false;
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
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, transform.rotation);
    }

    private void Block()
    {
        if (!isCastingSpell)
        {
            if (!isBlocking)
            {
                isBlocking = true;
                CastShieldAnimStart();
                shield.SetActive(true);
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = true;
            }
            else
            {
                isBlocking = false;
                CastShieldAnimStop();
                shield.SetActive(false);
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
            }
        }
    }
}

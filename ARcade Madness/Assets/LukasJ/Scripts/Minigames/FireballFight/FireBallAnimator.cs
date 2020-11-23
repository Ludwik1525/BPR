using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FireBallAnimator : MonoBehaviour
{
    private PhotonView PV;
    public Transform myParent;
    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;
    public Button attackB, blockB;
    private Animator animator;
    private GameObject joystick;
    public GameObject shield;
    private bool isBlocking, isCastingSpell;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        isCastingSpell = false;
        isBlocking = false;
        animator = GetComponent<Animator>();
        attackB = GameObject.Find("ButtonAttack").GetComponent<Button>();
        blockB = GameObject.Find("ButtonBlock").GetComponent<Button>();

        attackB.onClick.AddListener(CastFireballAnimStart);
        blockB.onClick.AddListener(Block);
        joystick = FindObjectOfType<FixedJoystick>().transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (joystick.transform.localPosition == Vector3.zero)
        {
            RunAnimStop();
        }
        else
        {
            RunAnimStart();
        }

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
        {
            attackB.interactable = false;
            blockB.interactable = false;
            animator.SetBool("isRunning", true);
        } 
    }

    private void RunAnimStop()
    {
        if (animator.GetBool("isRunning"))
        {

            attackB.interactable = true;
            blockB.interactable = true;
            animator.SetBool("isRunning", false);
        }  
    }
    
    public void SpawnFireBall()
    {
        if (PV.IsMine)
        {
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, transform.rotation);
        }
    }

    private void Block()
    {
        if (!isCastingSpell)
        {
            if (!isBlocking)
            {
                isBlocking = true;
                CastShieldAnimStart();
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = true;
                if(PV.IsMine)
                {
                    PV.RPC("ShowShield", RpcTarget.AllBuffered);
                }
            }
            else
            {
                isBlocking = false;
                CastShieldAnimStop();
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
                if (PV.IsMine)
                {
                    PV.RPC("HideShield", RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    void HideShield()
    {
        shield.SetActive(false);
    }

    [PunRPC]
    void ShowShield()
    {
        shield.SetActive(true);
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
    }

    public void EnableShield()
    {
        shield.SetActive(true);
    }
}

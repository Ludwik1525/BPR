using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class FireBallAnimator : MonoBehaviour
{
    public bool isBlocking, isCastingSpell;

    public Transform myParent, fireballSpawnPoint;

    private GameObject joystick;
    public GameObject fireballPrefab, shield;

    public Button attackB, blockB;

    private Animator animator;

    private PhotonView PV;

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
            myParent.GetComponent<JoystickScript>().StopMe();
        }
        else
        {
            RunAnimStart();
        }
    }

    // casting the fireball
    private void CastFireballAnimStart()
    {
        animator.SetBool("isThrowingSpell", true);
        myParent.GetComponent<JoystickScript>().isPerformingAnAction = true;
        isCastingSpell = true;
    }

    // finishing casting the fireball
    private void CastFireballAnimStop()
    {
        animator.SetBool("isThrowingSpell", false);
        myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
        isCastingSpell = false;
    }

    // casting the shield
    private void CastShieldAnimStart()
    {
        animator.SetBool("isBlocking", true);
    }

    // finishing casting the shield
    private void CastShieldAnimStop()
    {
        animator.SetBool("isBlocking", false);
    }

    // starting running
    private void RunAnimStart()
    {
        if(!animator.GetBool("isRunning"))
        {
            attackB.interactable = false;
            blockB.interactable = false;
            animator.SetBool("isRunning", true);
        } 
    }

    // stopping running
    private void RunAnimStop()
    {
        if (animator.GetBool("isRunning"))
        {
            if (!isBlocking)
                attackB.interactable = true;
            blockB.interactable = true;
            animator.SetBool("isRunning", false);
        }  
    }
    
    // instantiating the fireball
    public void SpawnFireBall()
    {
        if (PV.IsMine)
        {
            GameObject fireball = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), fireballSpawnPoint.position, transform.rotation);
        }
    }

    // blocking with the shield
    private void Block()
    {
        if (!isCastingSpell)
        {
            if (!isBlocking)
            {
                isBlocking = true;
                CastShieldAnimStart();
                attackB.interactable = false;
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
                attackB.interactable = true;
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
                if (PV.IsMine)
                {
                    PV.RPC("HideShield", RpcTarget.AllBuffered);
                }
            }
        }
    }

    // hiding the shield
    [PunRPC]
    void HideShield()
    {
        shield.SetActive(false);
    }

    // enabling the shield
    [PunRPC]
    void ShowShield()
    {
        shield.SetActive(true);
    }

    // dying
    public void Die()
    {
        animator.SetBool("isDead", true);
    }

    public void EnableShield()
    {
        shield.SetActive(true);
    }
}

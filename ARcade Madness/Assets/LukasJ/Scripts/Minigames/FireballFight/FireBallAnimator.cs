using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallAnimator : MonoBehaviour
{
    public Transform fireballSpawnPoint;
    public GameObject fireballPrefab;
    public Button attackB;
    private Animator animator;
    public GameObject joystick;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackB.onClick.AddListener(CastFireballAnimStart);
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
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), this.gameObject.transform.position, Quaternion.identity);
    }

    private void CastFireballAnimStop()
    {
        animator.SetBool("isThrowingSpell", false);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), this.gameObject.transform.position, Quaternion.identity);
    }

    private void CastShieldAnimStart()
    {

    }

    private void RunAnimStart()
    {
        if(!animator.GetBool("isRunning"))
            animator.SetBool("isRunning", true);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), this.gameObject.transform.position, Quaternion.identity);
    }

    private void RunAnimStop()
    {
        if (animator.GetBool("isRunning"))
            animator.SetBool("isRunning", false);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), this.gameObject.transform.position, Quaternion.identity);
    }



    public void SpawnFireBall()
    {
        //animator
        Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
    }
}

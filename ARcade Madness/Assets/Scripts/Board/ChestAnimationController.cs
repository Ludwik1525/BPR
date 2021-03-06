﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChestAnimationController : MonoBehaviour
{
    private Animator anim;
    private Currency currency;
    public bool taken = false, doesWantChest = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        taken = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartBoolAnimationByName("OpenChest");
        }
    }

    void StartBoolAnimationByName(string animationName)
    {
        anim.SetBool(animationName, true);
    }

    void StopBoolAnimationByName(string animationName)
    {
        anim.SetBool(animationName, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.GetComponent<Currency>() != null)
        {
            currency = other.gameObject.transform.parent.GetComponent<Currency>();

                if (doesWantChest)
                {
                    StartBoolAnimationByName("OpenChest");

                    if (other.gameObject.transform.parent.GetComponent<Score>() != null)
                    {
                        other.gameObject.transform.parent.GetComponent<Score>().setScore();
                    }
                }
        }

    }

    public void DestroyTheChest()
    {

        FindObjectOfType<SpawnChest>().PV.RPC("DestroyChest", RpcTarget.AllBuffered, false); 
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("HEYO");
    //    if (collision.gameObject.GetComponent<Currency>() != null)
    //    {
    //        currency = collision.gameObject.GetComponent<Currency>();
    //    }

    //    if (currency.GetCurrencyAmount() > 9)
    //    {
    //        StartBoolAnimationByName("OpenChest");
    //    }
    //}
}

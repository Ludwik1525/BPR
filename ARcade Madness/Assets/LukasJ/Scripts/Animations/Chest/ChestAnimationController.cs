using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimationController : MonoBehaviour
{
    private Animator anim;
    private Currency currency;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
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
        //print("HEYO");
        //if(other.GetComponent<Currency>() != null)
        //{
        //    currency = other.GetComponent<Currency>();
        //}

        //if(currency.currencyAmount > 9)
        //{
        //    StartBoolAnimationByName("OpenChest");
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("HEYO");
        if (collision.gameObject.GetComponent<Currency>() != null)
        {
            currency = collision.gameObject.GetComponent<Currency>();
        }

        if (currency.currencyAmount > 9)
        {
            StartBoolAnimationByName("OpenChest");
        }
    }
}

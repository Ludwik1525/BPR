using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : MonoBehaviour
{
    public bool isDead = false;
    public SpinTest spinnerScript;
    private float startSpinSpeed;
    private float currentSpinSpeed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Comparing the speeds of the SPinnerTops
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if (mySpeed > otherPlayerSpeed)
            {


                //Apply dmg to slower player
                collision.collider.gameObject.GetComponent<BattleTest>().DoDamage(300);

            }
        }
    }

    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            spinnerScript.health -= _damageAmount;
            currentSpinSpeed = spinnerScript.health;

            if (currentSpinSpeed < 100)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        isDead = true;
        spinnerScript.health = 0f;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 100;

    public void TakeDamage(int amount)
    {
        if (health > amount)
        {
            health -= amount;
        }
        else
            health = 0;
    }

    public void Heal(int amount)
    {
        if (100 - health > amount)
        {
            health = 100;
        }
        else
            health += amount;
    }
    
}

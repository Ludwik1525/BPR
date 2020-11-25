using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 100;


    // for taking damage
    public void TakeDamage(int amount)
    {
        if (health > amount)
        {
            health -= amount;
        }
        else
            health = 0;
    }

    // fore healing
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

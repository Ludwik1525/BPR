using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public int currencyAmount = 10;

    public void AddCurrency(int amount)
    {
        currencyAmount += amount;
    }

    public void SubtractCurrency(int amount)
    {
        if(amount > currencyAmount)
        {
            currencyAmount = 0;
        }
        else
        {
            currencyAmount -= amount;
        }
    }

    public int GetCurrencyAmount()
    {
        return currencyAmount;
    }
}

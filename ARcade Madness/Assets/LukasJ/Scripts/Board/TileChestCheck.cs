using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChestCheck : MonoBehaviour
{
    private bool iHaveAChest;

    public void SetToTrue()
    {
        iHaveAChest = true;
    }

    public void SetToFalse()
    {
        iHaveAChest = false;
    }

    public bool Check()
    {
        return iHaveAChest;
    }
}

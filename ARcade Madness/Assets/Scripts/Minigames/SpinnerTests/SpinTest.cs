using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTest : MonoBehaviour
{
    public float health = 3600f;
    public bool doSpin = true;
    public GameObject playerGraphics;
    private void FixedUpdate()
    {
        if (doSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, health * Time.deltaTime, 0));
        }
    }
}

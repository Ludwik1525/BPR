using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceHolder : MonoBehaviour
{
    public Transform[] playerPositions;

    public void InitializeArray(int size)
    {
        playerPositions = new Transform[size];
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

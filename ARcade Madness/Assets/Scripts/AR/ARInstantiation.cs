using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInstantiation : MonoBehaviour
{
    public GameObject ARPrefab;

    private void Awake()
    {
        print("awake");

        GameObject go = GameObject.Find("ARFoundation(Clone)");
        print("go " + go);
        
        if (go == null)
        {

            print("helo");
            Instantiate(ARPrefab);
            print("helo there");

        }

        print("end");
    }

    private void Start()
    {
        print("start");
    }
}

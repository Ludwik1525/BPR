using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInstantiation : MonoBehaviour
{
    public GameObject ARPrefab;

    private void Awake()
    {
        GameObject go = FindObjectOfType<ArPersistence>().gameObject;
        if(go == null)
        {
            Instantiate(ARPrefab);
        }
    }
}

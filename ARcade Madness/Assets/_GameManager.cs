using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    public GameObject scriptHolder;

    private void Awake()
    {
        Debug.Log("YO");
        Instantiate(scriptHolder, this.transform);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}

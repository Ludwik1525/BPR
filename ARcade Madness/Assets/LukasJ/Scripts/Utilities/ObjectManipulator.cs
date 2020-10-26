using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject objectToManipulate;
    public GameObject[] objectsToManipulate;

    public void enableObject()
    {
        objectToManipulate.SetActive(true);
    }

    public void disableObject()
    {
        objectToManipulate.SetActive(false);
    }

    public void enableObjects()
    {
        foreach(GameObject go in objectsToManipulate)
        {
            go.SetActive(true);
        }
    }
    public void disableObjects()
    {
        foreach (GameObject go in objectsToManipulate)
        {
            go.SetActive(false);
        }
    }

}

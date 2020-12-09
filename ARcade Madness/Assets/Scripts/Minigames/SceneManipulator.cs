using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManipulator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CD());
    }
    IEnumerator CD()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("BoardSceneARFoundation");
    }
}

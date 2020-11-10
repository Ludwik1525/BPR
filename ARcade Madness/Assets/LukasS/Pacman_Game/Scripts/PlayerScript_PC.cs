using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript_PC : MonoBehaviour
{
    public TextMeshProUGUI score_txt;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score_txt.text = $"Score: {score}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PacmanPoint"))
        {
            score++;
            Destroy(other.gameObject);
        }
    }
}

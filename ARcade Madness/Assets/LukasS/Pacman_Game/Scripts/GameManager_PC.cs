using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_PC : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private TextMeshProUGUI startUi;

    [SerializeField]
    private GameObject[] ghosts;
    [SerializeField]
    private GameObject[] patrolPoints;

    private bool start = false;

    private float time = 4f;

    void Start()
    {
        SpawnGhosts();
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        time -= Time.deltaTime;
        if(time > 1)
        {
            startUi.text = $"{Mathf.CeilToInt(time -1)}";
        }
        if (time <= 1)
        {
            startUi.text = "GO!";
            yield return new WaitForSeconds(1);
           
            startUi.enabled = false;
            start = false;

            door.gameObject.SetActive(false);
        }
        
    }

    public void StartGame()
    {
        start = true;
    }

    private void SpawnGhosts()
    {
        Debug.Log(ghosts.Length);
        foreach(var ghost in ghosts)
        {
            int random = Random.Range(0, patrolPoints.Length);
            Debug.Log("r " + random);

            Instantiate(ghost, patrolPoints[random].transform.position, Quaternion.identity);
        }
    }
}

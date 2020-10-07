using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpinningGameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject uI_InformPanelGameobject;
    public TextMeshProUGUI uI_InformText;
    public GameObject searchForGamesButtonGameobject;
    public GameObject adjust_Button;
    public GameObject raycastCenter_Image;
    // Start is called before the first frame update
    void Start()
    {
        uI_InformPanelGameobject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

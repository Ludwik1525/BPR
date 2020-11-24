using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    ARPlacementManager m_ARPlacementManager;

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;
    public GameObject scaleSlider;
    public GameObject ARCanvas;

    private int readyPlayersCount = 0;
    private bool started = false;
    private GameSetupController gsc;
    private PhotonView pv;

    public TextMeshProUGUI informUIPanel_Text;


    private void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARPlacementManager = GetComponent<ARPlacementManager>();
        gsc = FindObjectOfType<GameSetupController>();
        pv = GetComponent<PhotonView>();

    }
    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        scaleSlider.SetActive(true);
        

        adjustButton.SetActive(false);
        searchForGameButton.SetActive(true);

        informUIPanel_Text.text = "Move phone to detect planes and place the Board!";
    }

    private void Update()
    {
        if(readyPlayersCount == PhotonNetwork.CountOfPlayers && !started)
        {
            gsc.CreatePlayer();
            started = true;
        }
    }

    public void DisableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = false;
        m_ARPlacementManager.enabled = false;

        SetAllPlanesActiveOrDeactive(false);

        scaleSlider.SetActive(false);

        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        searchForGameButton.SetActive(true);

        informUIPanel_Text.text = "Great! Now get ready!";
    }

    public void EnableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = true;
        m_ARPlacementManager.enabled = true;
        scaleSlider.SetActive(true);

        SetAllPlanesActiveOrDeactive(true);

        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanel_Text.text = "Move phone to detect planes and place the Board!";
    }

    private void SetAllPlanesActiveOrDeactive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }

    [PunRPC]
    private void Ready()
    {
        readyPlayersCount++;
    }

    public void ReadyButtonPress()
    {
        pv.RPC("Ready", RpcTarget.AllBuffered);
    }
}

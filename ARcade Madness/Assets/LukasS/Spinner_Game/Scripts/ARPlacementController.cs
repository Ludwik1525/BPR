using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARPlacementController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    ARPlacementManager m_ARPlacementManager;
    SpinningGameManager spinningGameManager;

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject readyButton;
    public GameObject scaleSlider;
    public GameObject ARCanvas;
    public GameObject mainCanvas;

    //Board buttons

    public List<GameObject> boardUI;


    private int readyPlayersCount = 0;
    private bool started = false;
    private PhotonView pv;

    public Text informUIPanel_Text;

    // Start is called before the first frame update
    void Start()
    {
        ARCanvas.SetActive(true);

        if (!GameController.gc.doesHavePosition)
        {
            foreach (GameObject go in boardUI)
            {
                go.GetComponent<Button>().interactable = false;
                go.GetComponent<Image>().enabled = false;
            }


            m_ARPlaneManager = FindObjectOfType<ARPlaneManager>();
            m_ARPlacementManager = GetComponent<ARPlacementManager>();
            spinningGameManager = FindObjectOfType<SpinningGameManager>();

            pv = GetComponent<PhotonView>();

            placeButton.SetActive(true);
            scaleSlider.SetActive(true);


            adjustButton.SetActive(false);
            readyButton.SetActive(false);

            informUIPanel_Text.text = "Move phone to detect planes and place the Board!";
        }
        else
        {
            placeButton.transform.parent.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (readyPlayersCount == GameSetupController.players.Count && !started)
        {
            started = true;
            ARCanvas.SetActive(false);
            spinningGameManager.LoadingPanel();
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
        readyButton.SetActive(true);

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
        readyButton.SetActive(false);

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
        placeButton.SetActive(false);
        scaleSlider.SetActive(false);
        adjustButton.SetActive(false);
        readyButton.SetActive(false);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    ARPlacementManager m_ARPlacementManager;

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

    public TextMeshProUGUI informUIPanel_Text;


    private void Awake()
    {
        if(GameController.gc.roundCount < 1)
        {
            foreach (GameObject go in boardUI)
            {
                go.GetComponent<Button>().interactable = false;
                go.GetComponent<Image>().enabled = false;
            }
            m_ARPlaneManager = GetComponent<ARPlaneManager>();
            m_ARPlacementManager = GetComponent<ARPlacementManager>();
            pv = GetComponent<PhotonView>();
        }
        else
        {
            placeButton.transform.parent.gameObject.SetActive(false);
        }


    }
    // Start is called before the first frame update
    void Start()
    {

        if (GameController.gc.roundCount < 1)
        {

            placeButton.SetActive(true);
            scaleSlider.SetActive(true);


            adjustButton.SetActive(false);
            readyButton.SetActive(true);

            informUIPanel_Text.text = "Move phone to detect planes and place the Board!";
        }
        else
        {
            placeButton.transform.parent.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        //print(GameSetupController.players.Count);
        if(readyPlayersCount == GameSetupController.players.Count && !started)
        {
            started = true;
            ARCanvas.SetActive(false);


            foreach (GameObject go in boardUI)
            {
                foreach(GameObject p in GameSetupController.players)
                {
                    print("GC CURRENT TURN : " + GameController.gc.currentTurn + " PLAYERS TURN : " + p.GetComponent<GameManager>().turn);
                    if (p.GetComponent<PhotonView>().IsMine)
                    {
                        print("IS ME");
                        if (GameController.gc.currentTurn == p.GetComponent<GameManager>().turn)
                        {
                            print("IS ME2");
                            go.GetComponent<Button>().interactable = true;
                        }
                    }
                }

                if(go.gameObject.name.Contains("Options"))
                {
                    go.GetComponent<Button>().interactable = true;
                }
                go.GetComponent<Image>().enabled = true;
            }
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

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARPlacementController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    ARPlacementManager m_ARPlacementManager;
    SpinningGameManager spinningGameManager;
    GameManager_PC gameManager_PC;
    FireballSetupManager fireballSetupManager;

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
        readyPlayersCount = 0;
        started = false;

        ARCanvas.SetActive(true);
        adjustButton.SetActive(false);
        readyButton.SetActive(false);

        pv= GetComponent<PhotonView>();
        m_ARPlaneManager = FindObjectOfType<ARPlaneManager>();
        m_ARPlacementManager = GetComponent<ARPlacementManager>();

        spinningGameManager = FindObjectOfType<SpinningGameManager>();
        fireballSetupManager = FindObjectOfType <FireballSetupManager> ();
        gameManager_PC = FindObjectOfType<GameManager_PC>();
    }


    private void Update()
    {
        if (readyPlayersCount == PhotonNetwork.PlayerList.Length && !started)
        {
            started = true;
            ARCanvas.SetActive(false);
            switch (SceneManager.GetActiveScene().name)
            {
                case "Spinner_Gameplay":
                    spinningGameManager.LoadingPanel();
                    break;

                case "Pacman_Gameplay":
                    gameManager_PC.LoadingPanel();
                    break;

                case "FireBallFightMiniGame":
                    fireballSetupManager.LoadingPanel();
                    break;
            }

            foreach (GameObject go in boardUI)
            {
                foreach (GameObject p in GameSetupController.players)
                {
                    if (p.GetComponent<PhotonView>().IsMine)
                    {
                        go.GetComponent<Image>().enabled = true;
                        go.GetComponent<Button>().interactable = true;
                    }
                }
            }
            FindObjectOfType<FixedJoystick>().enabled = true;

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

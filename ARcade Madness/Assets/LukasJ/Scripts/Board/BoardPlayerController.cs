using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BoardPlayerController : MonoBehaviour
{
    public int totalPos = 0;

    private PhotonView PV;
    private PhotonView dicePV;

    bool isMoving;
    private bool wasKeyPressed = false;
    private bool diceGuard = false;
    private bool wasDiceRolled = false;

    private GameObject decisionBox;
    private Button yesB, noB, rollB;

    public int routePosition;
    public int turn;
    public Route currentRoute;
    public int steps;
    public float speed = 2f;
    public GameObject dice;
    public int roll;

    //Events
    [HideInInspector]
    public UnityEvent onStartMoving;
    [HideInInspector]
    public UnityEvent onStopMoving;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("totalPos"))
            totalPos = PlayerPrefs.GetInt("totalPos");

        PV = GetComponent<PhotonView>();
        dicePV = transform.GetChild(2).GetComponent<PhotonView>();
        onStartMoving = new UnityEvent();
        onStopMoving = new UnityEvent();

        decisionBox = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        yesB = decisionBox.transform.GetChild(1).GetComponent<Button>();
        noB = decisionBox.transform.GetChild(2).GetComponent<Button>();
        rollB = GameObject.Find("ButtonRoll").GetComponent<Button>();
        rollB.interactable = false;

        yesB.onClick.AddListener(AcceptChest);
        noB.onClick.AddListener(DeclineChest);
        rollB.onClick.AddListener(Roll);
    }

    private void Start()
    {
        StartCoroutine(DelayForCurrency());
    }

    private void Roll()
    {
        if (PV.IsMine)
        {
            if (turn == GameController.gc.currentTurn)
            {
                if (!wasKeyPressed && !isMoving)
                {
                    wasKeyPressed = true;
                    steps = Random.Range(9, 10);
                    Debug.Log("Dice Rolled: " + steps);
                    StartCoroutine(Move());
                }
            }
        }
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (turn == GameController.gc.currentTurn)
            {
                if (dice.activeInHierarchy)
                {
                    rollB.interactable = true;
                }

                if (!diceGuard)
                {
                    if (wasDiceRolled)
                    {
                        wasDiceRolled = false;
                    }
                    else
                    {
                        dicePV.RPC("SwitchTheDice", RpcTarget.AllBuffered);
                        wasDiceRolled = true;
                    }
                    diceGuard = true;
                }

                if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
                {
                    if (!wasKeyPressed)
                    {
                        wasKeyPressed = true;
                        steps = Random.Range(9, 10);
                        Debug.Log("Dice Rolled: " + steps);
                        StartCoroutine(Move());
                    }
                }

                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(raycast, out raycastHit))
                    {
                        if (raycastHit.collider.name == "DiceModel")
                        {
                            steps = Random.Range(1, 7);
                            Debug.Log("Dice Rolled: " + steps);
                            StartCoroutine(Move());
                        }
                    }
                }
            }
        }
    }

    private void StopTimeAndOpenBox()
    {
        if (PlayerPrefs.GetInt("Currency") >= 1)
        {
            decisionBox.SetActive(true);
            PV.RPC("StopTheTime", RpcTarget.AllBuffered);
        }
    }

    private void AcceptChest()
    {
        PV.RPC("StartTheTimeAccept", RpcTarget.AllBuffered);
        decisionBox.SetActive(false);
        steps = 0;
        GetComponent<Currency>().decreaseCurrency();
    }

    private void DeclineChest()
    {
        PV.RPC("StartTheTimeDecline", RpcTarget.AllBuffered);
        decisionBox.SetActive(false);
    }

    bool MoveToNextNode(Vector3 target)
    {
        transform.rotation = Quaternion.LookRotation(transform.position - target);
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
    }

    public void SetTurn(int turn)
    {
        this.turn = turn;
    }

    IEnumerator Move()
    {
        if (isMoving)
        {
            //if the player is already moving return
            yield break;
        }

        //set bool value to true and invoke start moving event

        dicePV.RPC("SwitchTheDice", RpcTarget.AllBuffered);
        rollB.interactable = false;

        onStartMoving.Invoke();
        //Jump animation
        yield return new WaitForSeconds(2.2f);
        isMoving = true;

        int var = 0;

        while (steps > 0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            var = totalPos + routePosition;

            if (totalPos + routePosition >= currentRoute.childNodeList.Count)
            {
                var = totalPos + routePosition - currentRoute.childNodeList.Count;
            }
            if (var == FindObjectOfType<SpawnChest>().GetRealTileNo())
            {
                StopTimeAndOpenBox();
            }

            Vector3 nextPos = currentRoute.childNodeList[var].transform.GetChild(1).GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position;
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }
            steps--;
        }
        totalPos += routePosition;
        if (totalPos >= currentRoute.childNodeList.Count)
        {
            totalPos = var;
        }
        PlayerPrefs.SetInt("totalPos", totalPos);
        PV.RPC("SaveMyPos", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], totalPos);

        onStopMoving.Invoke();
        isMoving = false;
        diceGuard = false;
        wasKeyPressed = false;

        PV.RPC("IncrementTurn", RpcTarget.AllBuffered);
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator LoadSceneDelay()
    {
        if (FindObjectOfType<ChestAnimationController>().taken)
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("AssetScene");
        }
        else
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("AssetScene");
        }
    }

    IEnumerator DelayIfPlayerPicksUpChest(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameController.gc.currentTurn++;
        PV.RPC("ResetTurnVar", RpcTarget.AllBuffered);
    }

    IEnumerator DelayForCurrency()
    {
        yield return new WaitForSeconds(1f);
        if (GameController.gc.roundCount > 0)
        {
            GetComponent<Currency>().setCurrency();
        }
        GameController.gc.roundCount++;
    }

    [PunRPC]
    private void SetChestVariable()
    {
        PlayerPrefs.SetInt("random", 0);
    }

    [PunRPC]
    private void StopTheTime()
    {
        Time.timeScale = 0.01f;
    }

    [PunRPC]
    private void StartTheTimeAccept()
    {
        Time.timeScale = 1;
        PV.RPC("SetChestVariable", RpcTarget.AllBuffered);
        FindObjectOfType<ChestAnimationController>().doesWantChest = true;
        FindObjectOfType<ChestAnimationController>().taken = true;
    }

    [PunRPC]
    private void StartTheTimeDecline()
    {
        Time.timeScale = 1;
        FindObjectOfType<ChestAnimationController>().doesWantChest = false;
    }

    [PunRPC]
    public void ResetTurnVar()
    {
        if (GameController.gc.currentTurn == GameController.gc.players.Length + 1)
        {
            GameController.gc.currentTurn = 1;
            StartCoroutine(LoadSceneDelay());
        }
    }

    [PunRPC]
    public void SaveMyPos(int playerIndex, int tileIndex)
    {
        GameController.gc.currentPositions[playerIndex] = tileIndex;

        if (!GameController.gc.doesHavePosition)
            GameController.gc.doesHavePosition = true;
    }

    [PunRPC]
    public void IncrementTurn()
    {
        if (FindObjectOfType<ChestAnimationController>().taken)
        {
            StartCoroutine(DelayIfPlayerPicksUpChest(3));
        }
        else
        {
            GameController.gc.currentTurn++;
            PV.RPC("ResetTurnVar", RpcTarget.AllBuffered);
        }
    }

}

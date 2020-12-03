using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviourPun
{
    public Spinner spinnerScript;

    private Rigidbody rb;

    public GameObject uI_3D_Gameobject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameobject;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;

    private GameObject winScreen;

    private PhotonView pv;

    public bool isDead = false;

    public Sprite[] powerupsIMGs;

    private int wonPrize;

    [SerializeField]
    private float damage = 300.0f;

    public int placement;

    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    void Start()
    {
        winScreen = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        placement = 1;
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Comparing the speeds of the SPinnerTops
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if (mySpeed > otherPlayerSpeed)
            {

                //float default_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * doDamage_Coefficient;

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    //Apply dmg to slower player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, damage);
                }
            }
        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            spinnerScript.spinSpeed -= _damageAmount;
            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100)
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;

        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.spinSpeed = 0f;

        uI_3D_Gameobject.SetActive(false);

        if (pv.IsMine)
        {
            placement = FindObjectOfType<SpinningGameManager>().GetPlayersLeft();
            print(PhotonNetwork.LocalPlayer.NickName + " " + placement);
            pv.RPC("SubstructPlayersLeft", RpcTarget.AllBuffered);
        }

    }

    [PunRPC]
    void SubstructPlayersLeft()
    {
        FindObjectOfType<SpinningGameManager>().SubstuctPlayersLeft();
        print("pl left " + FindObjectOfType<SpinningGameManager>().GetPlayersLeft());
    }

    [PunRPC]
    private void DisplayScore()
    {
       if(pv.IsMine)
        {
            pv.RPC("SetScores", RpcTarget.AllBuffered, placement - 1, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    [PunRPC]
    private void SetScores(int pos, string name)
    {
        
        winScreen.transform.GetChild(2).GetChild(pos).gameObject.SetActive(true);
        winScreen.transform.GetChild(2).GetChild(pos).GetComponent<Text>().text = pos + 1 + ". " + name + ", " + (PhotonNetwork.PlayerList.Length - pos);
        winScreen.transform.GetChild(1).gameObject.SetActive(false);


        if(pv.IsMine)
        {
            int random = 0;

            if (pos == 0)
            {
                random = SetRandomPowerup();

                pv.RPC("SetPrizeWon", RpcTarget.AllBuffered, random);

                SaveMyPowerups();
            }

            if(random != 3)
            {
                PlayerPrefs.SetInt("PlaceFromLastMinigame", PhotonNetwork.PlayerList.Length - pos);
            }
            else
            {
                PlayerPrefs.SetInt("PlaceFromLastMinigame", PhotonNetwork.PlayerList.Length - pos + 1);
            }

            print("COINS I AM GETTING :" + PlayerPrefs.GetInt("PlaceFromLastMinigame"));
            print("MY POWERUPS: " + PlayerPrefs.GetInt("MyPowerups"));
        }
    }

    [PunRPC]
    void SetPrizeWon(int no)
    {
        wonPrize = no;
        if (no != 3)
            winScreen.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = powerupsIMGs[wonPrize];
        else
            winScreen.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    int SetRandomPowerup()
    {
        switch (PlayerPrefs.GetInt("MyPowerups"))
        {
            case 0:
                return Random.Range(0, 3);
            case 1:
                return Random.Range(1, 3);
            case 2:
                int newRand = Random.Range(0, 2);
                if(newRand == 0)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            case 3:
                return Random.Range(0, 2);
            case 4:
                return 2;
            case 5:
                return 1;
            case 6:
                return 0;
            default:
                return 3;
        }
    }

    void SaveMyPowerups()
    {
        switch(wonPrize)
        {
            case 0: 
                switch(PlayerPrefs.GetInt("MyPowerups"))
                {
                    case 0:
                        PlayerPrefs.SetInt("MyPowerups", 1);
                        break;
                    case 2:
                        PlayerPrefs.SetInt("MyPowerups", 4);
                        break;
                    case 3:
                        PlayerPrefs.SetInt("MyPowerups", 5);
                        break;
                    case 6:
                        PlayerPrefs.SetInt("MyPowerups", 7);
                        break;
                }
                break;
            case 1:
                switch (PlayerPrefs.GetInt("MyPowerups"))
                {
                    case 0:
                        PlayerPrefs.SetInt("MyPowerups", 2);
                        break;
                    case 1:
                        PlayerPrefs.SetInt("MyPowerups", 4);
                        break;
                    case 3:
                        PlayerPrefs.SetInt("MyPowerups", 6);
                        break;
                    case 5:
                        PlayerPrefs.SetInt("MyPowerups", 7);
                        break;
                }
                break;
            case 2:
                switch (PlayerPrefs.GetInt("MyPowerups"))
                {
                    case 0:
                        PlayerPrefs.SetInt("MyPowerups", 3);
                        break;
                    case 1:
                        PlayerPrefs.SetInt("MyPowerups", 5);
                        break;
                    case 2:
                        PlayerPrefs.SetInt("MyPowerups", 6);
                        break;
                    case 4:
                        PlayerPrefs.SetInt("MyPowerups", 7);
                        break;
                }
                break;
        }
    }
}

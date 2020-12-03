using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class FireBallAnimator : MonoBehaviour
{
    public bool isBlocking, isCastingSpell;

    public Transform myParent, fireballSpawnPoint;

    private GameObject joystick;
    public GameObject fireballPrefab, shield;

    public Button attackB, blockB;

    private Animator animator;
    private GameObject winScreen;

    private PhotonView PV;

    //For giving out the rewards
    int wonPrize, placement = 1;

    public Sprite[] powerupImgs;

    void Start()
    {
        placement = 1;
        PV = GetComponent<PhotonView>();
        winScreen = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        isCastingSpell = false;
        isBlocking = false;
        animator = GetComponent<Animator>();
        attackB = GameObject.Find("ButtonAttack").GetComponent<Button>();
        blockB = GameObject.Find("ButtonBlock").GetComponent<Button>();

        attackB.onClick.AddListener(CastFireballAnimStart);
        blockB.onClick.AddListener(Block);
        joystick = FindObjectOfType<FixedJoystick>().transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (joystick.transform.localPosition == Vector3.zero)
        {
            RunAnimStop();
            myParent.GetComponent<JoystickScript>().StopMe();
        }
        else
        {
            RunAnimStart();
        }
    }

    // casting the fireball
    private void CastFireballAnimStart()
    {
        animator.SetBool("isThrowingSpell", true);
        myParent.GetComponent<JoystickScript>().isPerformingAnAction = true;
        isCastingSpell = true;
    }

    // finishing casting the fireball
    private void CastFireballAnimStop()
    {
        animator.SetBool("isThrowingSpell", false);
        myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
        isCastingSpell = false;
    }

    // casting the shield
    private void CastShieldAnimStart()
    {
        animator.SetBool("isBlocking", true);
    }

    // finishing casting the shield
    private void CastShieldAnimStop()
    {
        animator.SetBool("isBlocking", false);
    }

    // starting running
    private void RunAnimStart()
    {
        if(!animator.GetBool("isRunning"))
        {
            attackB.interactable = false;
            blockB.interactable = false;
            animator.SetBool("isRunning", true);
        } 
    }

    // stopping running
    private void RunAnimStop()
    {
        if (animator.GetBool("isRunning"))
        {
            if (!isBlocking)
                attackB.interactable = true;
            blockB.interactable = true;
            animator.SetBool("isRunning", false);
        }  
    }
    
    // instantiating the fireball
    public void SpawnFireBall()
    {
        if (PV.IsMine)
        {
            GameObject fireball = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FireBall"), fireballSpawnPoint.position, transform.rotation);
        }
    }

    // blocking with the shield
    private void Block()
    {
        if (!isCastingSpell)
        {
            if (!isBlocking)
            {
                isBlocking = true;
                CastShieldAnimStart();
                attackB.interactable = false;
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = true;
                if(PV.IsMine)
                {
                    PV.RPC("ShowShield", RpcTarget.AllBuffered);
                }
            }
            else
            {
                isBlocking = false;
                CastShieldAnimStop();
                attackB.interactable = true;
                myParent.GetComponent<JoystickScript>().isPerformingAnAction = false;
                if (PV.IsMine)
                {
                    PV.RPC("HideShield", RpcTarget.AllBuffered);
                }
            }
        }
    }

    // hiding the shield
    [PunRPC]
    void HideShield()
    {
        shield.SetActive(false);
    }

    // enabling the shield
    [PunRPC]
    void ShowShield()
    {
        shield.SetActive(true);
    }

    // dying
    public void Die()
    {
        animator.SetBool("isDead", true);

        if (PV.IsMine)
        {
            placement = FindObjectOfType<FireballSetupManager>().GetPlayersLeft();
            PV.RPC("SubstructPlayersLeft", RpcTarget.AllBuffered);
        }
    }

    public void EnableShield()
    {
        shield.SetActive(true);
    }


    [PunRPC]
    void SubstructPlayersLeft()
    {
        FindObjectOfType<FireballSetupManager>().SubstractPlayersLeft();
        print("pl left " + FindObjectOfType<FireballSetupManager>().GetPlayersLeft());
    }

    [PunRPC]
    private void DisplayScore()
    {
        if (PV.IsMine)
        {
            PV.RPC("SetScores", RpcTarget.AllBuffered, placement - 1, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    [PunRPC]
    private void SetScores(int pos, string name)
    {
        winScreen.transform.GetChild(2).GetChild(pos).gameObject.SetActive(true);
        winScreen.transform.GetChild(2).GetChild(pos).GetComponent<Text>().text = pos + 1 + ". " + name + ", " + (PhotonNetwork.PlayerList.Length - pos);
        winScreen.transform.GetChild(1).gameObject.SetActive(false);
        if (PV.IsMine)
        {  
            int random = 0;

            if (pos == 0)
            {
                random = SetRandomPowerup();

                PV.RPC("SetPrizeWon", RpcTarget.AllBuffered, random);

                SaveMyPowerups();
            }

            if (random != 3)
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

        if (pos == 0 && wonPrize == 3)
            winScreen.transform.GetChild(2).GetChild(pos).GetComponent<Text>().text = pos + 1 + ". " + name + ", " + (PhotonNetwork.PlayerList.Length - pos + 1);
    }

    [PunRPC]
    void SetPrizeWon(int no)
    {
        wonPrize = no;
        if (no != 3)
            winScreen.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = powerupImgs[wonPrize];
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
                if (newRand == 0)
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
        switch (wonPrize)
        {
            case 0:
                switch (PlayerPrefs.GetInt("MyPowerups"))
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

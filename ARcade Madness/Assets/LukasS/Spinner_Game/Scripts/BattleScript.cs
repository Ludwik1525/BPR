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

    private PhotonView pv;

    public bool isDead = false;

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

        void Die()
        {
            isDead = true;

            GetComponent<MovementController>().enabled = false;
            rb.freezeRotation = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            spinnerScript.spinSpeed = 0f;

            uI_3D_Gameobject.SetActive(false);

            if(pv.IsMine)
            {
                placement = FindObjectOfType<SpinningGameManager>().GetPlayersLeft();
                print(PhotonNetwork.LocalPlayer.NickName + " " + placement);
                FindObjectOfType<SpinningGameManager>().SubstuctPlayersLeft();
            }
        }
    }


}

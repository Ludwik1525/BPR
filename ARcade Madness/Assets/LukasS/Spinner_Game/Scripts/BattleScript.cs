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

    public bool isDead = false;


    public float common_Demage_Coefficient = 0.04f;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_Coefficient = 10f;
    public float getDamaged_Coefficient = 1.2f;

    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

                float default_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * common_Demage_Coefficient * doDamage_Coefficient;

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    //Apply dmg to slower player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_Damage_Amount);
                }
            }
        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            _damageAmount *= getDamaged_Coefficient;

            if (_damageAmount > 1000)
            {
                _damageAmount = 400f;
            }

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

            //if (photonView.IsMine)
            //{
            //    StartCoroutine(ReSpawn());
            //}
        }

    }
}

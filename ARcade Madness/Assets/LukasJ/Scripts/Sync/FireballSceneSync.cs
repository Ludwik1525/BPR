﻿using Photon.Pun;
using UnityEngine;

public class FireballSceneSync : MonoBehaviour, IPunObservable
{
    private PhotonView pv;
    Vector3 _networkPosition;
    Quaternion _networkRotation;
    Rigidbody _rb;

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.rotation);
            stream.SendNext(_rb.velocity);
        }
        else
        {
            _networkPosition = (Vector3)stream.ReceiveNext();
            _networkRotation = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            _networkPosition += (_rb.velocity * lag);
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    public void FixedUpdate()
    {
        if (!pv.IsMine)
        {
            _rb.position = Vector3.MoveTowards(_rb.position, _networkPosition, Time.fixedDeltaTime);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, _networkRotation, Time.fixedDeltaTime * 100.0f);
        }
    }
}
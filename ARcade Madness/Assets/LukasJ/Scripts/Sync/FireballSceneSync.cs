using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireballSceneSync : MonoBehaviour, IPunObservable
{
    Transform trn;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;

    private void Awake()
    {
        trn = GetComponent<Transform>();
        photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        //networkedRotation = new Quaternion();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            // update "RemoteME"
            trn.position = Vector3.MoveTowards(trn.position, networkedPosition, distance * (1.0f / PhotonNetwork.SerializationRate));
            //rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Then, photonView is mine and I am the one who controls the player
            //should send postion, velocity etc. data to the other players 
            stream.SendNext(trn.position);
            //stream.SendNext(rb.rotation);

            if (synchronizeVelocity)
            {
                //stream.SendNext(trn.velocity);
            }

            //if (synchronizeAngularVelocity)
            //{
            //    stream.SendNext(rb.angularVelocity);
            //}
        }
        else
        {
            //Called on my player gameobject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext();
            //networkedRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(trn.position, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    trn.position = networkedPosition;
                }
            }

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    //trn.velocity = (Vector3)stream.ReceiveNext();

                    //networkedPosition += trn.velocity * lag;

                    distance = Vector3.Distance(trn.position, networkedPosition);
                }

                //if (synchronizeAngularVelocity)
                //{
                //    rb.angularVelocity = (Vector3)stream.ReceiveNext();

                //    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                //    angle = Quaternion.Angle(rb.rotation, networkedRotation);
                //}
            }
        }
    }
}

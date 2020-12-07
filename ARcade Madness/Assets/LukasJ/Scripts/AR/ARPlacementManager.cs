﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    ARRaycastManager m_ARRaycastManager;
    static List<ARRaycastHit> raycast_Hits = new List<ARRaycastHit>();

    public Camera aRCamera;

    public GameObject battleArenaGameobject;

    private ARAnchorManager anchorManager;

    Pose hitPose;


    private void Awake()
    {
        m_ARRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        aRCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
        anchorManager = FindObjectOfType<ARAnchorManager>();
        if (ArPersistence.anchor != null)
        {
            anchorManager.anchorPrefab = battleArenaGameobject;
            anchorManager.anchorPrefab.transform.position = ArPersistence.anchor.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ArPersistence.anchor == null)
        {
            Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
            Ray ray = aRCamera.ScreenPointToRay(centerOfScreen);

            if (m_ARRaycastManager.Raycast(ray, raycast_Hits, TrackableType.PlaneWithinPolygon))
            {
                hitPose = raycast_Hits[0].pose;

                Vector3 positionToBePlaced = hitPose.position;

                battleArenaGameobject.transform.position = positionToBePlaced;

             
            }
        }

    }

    private void OnDisable()
    {
        ARAnchor anchor = anchorManager.AddAnchor(hitPose);
        ArPersistence.anchor = anchor;
    }
}

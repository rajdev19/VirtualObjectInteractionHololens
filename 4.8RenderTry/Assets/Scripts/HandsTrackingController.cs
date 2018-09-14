// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;

namespace HoloLensHandTracking
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public class HandsTrackingController : MonoBehaviour
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        public GameObject TrackingObject;
        public TextMesh StatusText;
        public Color DefaultColor = Color.green;
        public Color TapColor = Color.blue;
        public Color HoldColor = Color.red;

        private HashSet<uint> trackedHands = new HashSet<uint>();
        private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
        private GestureRecognizer gestureRecognizer;
        private uint activeId;
        private bool IsPhysical = false;
        private int clickCount = 0;

        void Awake()
        {
            InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
            gestureRecognizer.Tapped += GestureRecognizerTapped;
            //gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;
            //gestureRecognizer.HoldCompleted += GestureRecognizer_HoldCompleted;
            //gestureRecognizer.HoldCanceled += GestureRecognizer_HoldCanceled;            
            gestureRecognizer.StartCapturingGestures();
            StatusText.text = "Airtap to finalize Spatial Mapping";
        }

        void ChangeObjectColor(GameObject obj, Color color)
        {            
            var rend = obj.GetComponentInChildren<Renderer>();
            if (rend)
            {
                rend.material.color = color;
                Debug.LogFormat("Color Change: {0}", color.ToString());
            }
        }


        //private void GestureRecognizer_HoldStarted(HoldStartedEventArgs args)
        //{
        //    uint id = args.source.id;            
        //    //StatusText.text = $"HoldStarted - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if (trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], HoldColor);
        //        StatusText.text += "-TRACKED";
        //    }
        //}

        //private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
        //{
        //    uint id = args.source.id;            
        //    //StatusText.text = $"HoldCompleted - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if(trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], DefaultColor);
        //        StatusText.text += "-TRACKED";
        //    }
        //}

        //private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)
        //{
        //    uint id = args.source.id;            
        //    //StatusText.text = $"HoldCanceled - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if (trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], DefaultColor);
        //        StatusText.text += "-TRACKED";
        //    }
        //}

        private void GestureRecognizerTapped(TappedEventArgs args)
        {            
            uint id = args.source.id;
            //StatusText.text = $"Tapped - Kind:{args.source.kind.ToString()} - Id:{id}";
            if (trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], TapColor);                
                if(!IsPhysical)
                {
                    makePhysical();
                    clickCount++;
                }
            }            
        }
        

        private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
        {
            uint id = args.state.source.id;
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }            
            trackedHands.Add(id);
            activeId = id;

            var obj = Instantiate(TrackingObject) as GameObject;
            Vector3 pos;
            Rigidbody RB= obj.AddComponent<Rigidbody>();
            RB.useGravity = false;
            RB.isKinematic = true;
            BoxCollider BC = obj.AddComponent<BoxCollider>();
            //var camera = Camera.main;
            //Vector3 headPosition = camera.transform.position;
            //Vector3 targetPosition = gameObject.transform.position;
            
            



            if (args.state.sourcePose.TryGetPosition(out pos))
            {
                //Vector3 positionDelta = pos - headPosition;
                //Vector3 factoredDelta = 0.5f * positionDelta;
                //obj.transform.position = pos+factoredDelta;
                obj.transform.position = pos;
            }

            trackingObject.Add(id, obj);
        }

        private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
        {
            uint id = args.state.source.id;
            Vector3 pos;
            Quaternion rot;

            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackingObject.ContainsKey(id))
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        //var camera = Camera.main;
                        //Vector3 headPosition = camera.transform.position;
                        //Vector3 positionDelta = pos - headPosition;
                        //Vector3 factoredDelta = 0.5f * positionDelta;
                        //trackingObject[id].transform.position = pos+factoredDelta;
                        trackingObject[id].transform.position = pos;

                    }

                    if (args.state.sourcePose.TryGetRotation(out rot))
                    {
                        trackingObject[id].transform.rotation = rot;
                    }
                }
            }
        }

        private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
        {
            uint id = args.state.source.id;
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            if (trackedHands.Contains(id))
            {
                trackedHands.Remove(id);
            }

            if (trackingObject.ContainsKey(id))
            {
                var obj = trackingObject[id];
                trackingObject.Remove(id);
                Destroy(obj);
            }
            if (trackedHands.Count > 0)
            {
                activeId = trackedHands.First();
            }
        }

        void OnDestroy()
        {                        
            InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;

            gestureRecognizer.Tapped -= GestureRecognizerTapped;
            //gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;
            //gestureRecognizer.HoldCompleted -= GestureRecognizer_HoldCompleted;
            //gestureRecognizer.HoldCanceled -= GestureRecognizer_HoldCanceled;
            gestureRecognizer.StopCapturingGestures();
        }
        void makePhysical()
        {

            if (clickCount == 0)
            {
                Debug.Log("click to disable updates");
                GameObject temp0 = GameObject.FindGameObjectWithTag("SpatialMap");
                SpatialMappingCollider SMC = temp0.GetComponent<SpatialMappingCollider>();
                SMC.freezeUpdates = true;
                SpatialMappingManager.Instance.StopObserver();
                StatusText.text = "Spatial Mapping finalised, tap to finalise object placement";
            }
            else if(clickCount==1)
            {
                //SurfaceMeshesToPlanes 


                Debug.Log("Requested to make physically interactable");
                //IsPhysical = true;
                //SpatialUnderstanding.Instance.RequestFinishScan();
                foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Targets"))
                {
                    temp.SetActive(false);

                }
                foreach (GameObject temp in GameObject.FindGameObjectsWithTag("PhysicalModel"))
                {
                    //Rigidbody RB = temp.AddComponent<Rigidbody>();
                    Rigidbody RB = temp.GetComponent<Rigidbody>();
                    RB.useGravity = true;
                    RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |RigidbodyConstraints.FreezeRotation;
                    //RB.isKinematic = true;
                }
                StatusText.text = "Object Placement finalised, tap to make interactable";
            }
            else if (clickCount == 2)
            {
                //SurfaceMeshesToPlanes 


                Debug.Log("Requested to make physically interactable");
                IsPhysical = true;
                //SpatialUnderstanding.Instance.RequestFinishScan();
                //foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Targets"))
                //{
                //    temp.SetActive(false);

                //}
                foreach (GameObject temp in GameObject.FindGameObjectsWithTag("PhysicalModel"))
                {
                    //Rigidbody RB = temp.AddComponent<Rigidbody>();
                    Rigidbody RB = temp.GetComponent<Rigidbody>();
                    RB.constraints = RigidbodyConstraints.None;
                    //RB.isKinematic = true;
                }
                StatusText.text = "Interaction Enabled";
            }

            //foreach (GameObject temp in DefaultTrackableEventHandler.PhysicalObjectArray)
            //{
            //    Rigidbody RB = temp.GetComponent<Rigidbody>();
            //    RB.useGravity = true;
            //    //MeshCollider MC = temp.AddComponent<MeshCollider>();
            //    //MC.sharedMesh = Mesh.
            //}
            //add spatial mapping freeze update to click 1 and rest to click 2
        }
    }
}
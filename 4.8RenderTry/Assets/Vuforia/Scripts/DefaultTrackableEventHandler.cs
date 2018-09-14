/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>

public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    
    //static float v = 0.5;
    //static Vector3 trans = new Vector3(v,v,v);
    
    #region PROTECTED_MEMBER_VARIABLES
    //int objectsDetected = 0;
    GameObject ObjectHologram;
    Vector3 ParentPosition = Vector3.one;
    Quaternion ParentRotation = Quaternion.identity;
    Quaternion InitialRotation;
    Vector3 InitialPosition;
    //bool trackingBool = false;
    //bool trackingLost = false;
    bool InstanceExists = false;
    Transform Parent;
    public static List<GameObject> PhysicalObjectArray;
    public List<GameObject> PhysicalObjectArraytemp;

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }
    void Update()
    {
        if(InstanceExists)
        {
            var camera = Camera.main;
            Vector3 headPosition = camera.transform.position;
            Vector3 targetPosition = gameObject.transform.position;
            Vector3 positionDelta = targetPosition - headPosition;
            Debug.Log(positionDelta.ToString());
            Vector3 factoredDelta = 0.2f * positionDelta;
            ParentPosition = Parent.position+factoredDelta;
            //var camera = Camera.main;
            //var headPosition = camera.transform.position;
            //var targetPosition = gameObject.transform.position;
            //var positionDelta = targetPosition - headPosition;
            //var factoredDelta = 0.5f * positionDelta;
            //gameObject.transform.position = targetPosition - factoredDelta;
            //ParentPosition = Parent.position;
            ParentRotation = Parent.rotation*InitialRotation;
            ObjectHologram.transform.position = ParentPosition;
            //ObjectHologram.transform.Translate(trans);
            ObjectHologram.transform.rotation = ParentRotation;
        }
        
        //if (trackingBool && !trackingLost)
        //{
            //ObjectHologram.transform.position = Parent.position;
            //ObjectHologram.transform.rotation = Parent.rotation;
            //ParentPosition = Parent.position;
            //ParentRotation = Parent.rotation;
            //ObjectHologram.transform.Rotate(+90, 0, 0);
            //ObjectHologram.transform.rotation = new Quaternion(Parent.rotation.x-90, Parent.rotation.y, Parent.rotation.z, Parent.rotation.w);
            //var ParentRectTransform = Parent.transform as RectTransform;
            //var Parentwidth = ParentRectTransform.rect.width;
            //newScale();
        //}
       // if(trackingBool && trackingLost)
        //{
        //    ObjectHologram.transform.position = ParentPosition;
        //    ObjectHologram.transform.rotation = ParentRotation;
        //}
        //cupp
        //Debug.Log("runs every frame");
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        if (!InstanceExists)
        {
            Debug.Log("Object in sight");
            var rendererComponents = GetComponentsInChildren<Renderer>(true);
            var colliderComponents = GetComponentsInChildren<Collider>(true);
            var canvasComponents = GetComponentsInChildren<Canvas>(true);
            var camera = Camera.main;
            Vector3 headPosition = camera.transform.position;
            Vector3 targetPosition = gameObject.transform.position;
            Vector3 positionDelta = targetPosition - headPosition;
            Vector3 factoredDelta = 0.1f * positionDelta;
            //gameObject.transform.position = targetPosition - factoredDelta;


            //trackingBool = true;
            //trackingLost = false;

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;

            Parent = gameObject.transform;
            ParentPosition = Parent.position;
            ParentRotation = Parent.rotation;
            //ParentRotation = new Quaternion(Parent.rotation.x-90, Parent.rotation.y, Parent.rotation.z, Parent.rotation.w);

            foreach (var comp in Behaviour.ScannedObjects)
            {
                if (comp.name == Parent.name)
                {
                    int index = Behaviour.ScannedObjects.IndexOf(comp);
                    InitialRotation = Behaviour.InitialObjectRotations[index];
                    InitialPosition = Behaviour.InitialObjectPositions[index];
                    //GameObject temp = this.par;
                    //Debug.Log("start for each loop");
                    Debug.Log(ParentPosition);
                    //ParentPosition.z = 100;
                    ParentPosition = Parent.position + InitialPosition+factoredDelta;
                    //ParentPosition = Parent.position;
                    ParentRotation = ParentRotation*InitialRotation;
                    ObjectHologram = Object.Instantiate(comp, ParentPosition, ParentRotation, null);
                    ObjectHologram.tag = "PhysicalModel";
                    InstanceExists = true;
                    //objectsDetected = 1;
                    ObjectHologram.transform.localPosition = ParentPosition;
                    //cuppy.AddComponent(typeof(Rigidbody));
                    //cuppy.AddComponent(typeof(BoxCollider));
                    //MeshCollider mc = cuppy.AddComponent(typeof(MeshCollider)) as MeshCollider;
                    //mc.sharedMesh = "default";
                    ObjectHologram.SetActive(true);
                    PhysicalObjectArraytemp.Add(ObjectHologram);
                    //Debug.Log("end for each loop");
                }
            }
        }
        else
        {
            Parent = this.GetComponentInParent<Transform>();
            ParentPosition = Parent.position;
            ParentRotation = Parent.rotation;
            ObjectHologram.transform.position = ParentPosition;
            ObjectHologram.transform.rotation = ParentRotation;
        }
        PhysicalObjectArray = PhysicalObjectArraytemp;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        //trackingLost = true;

        // Disable rendering:
        //foreach (var component in rendererComponents)
        //    component.enabled = false;

        // Disable colliders:
        //foreach (var component in colliderComponents)
        //    component.enabled = false;

        // Disable canvas':
        //foreach (var component in canvasComponents)
        //    component.enabled = false;

        //objectsDetected = 0;
    }
    public void newScale()
    {
        Vector2 trackedImage = Parent.GetComponent<ImageTargetBehaviour>().GetSize();
        float size = Parent.GetComponent<Renderer>().bounds.size.y;
        float newSize = ObjectHologram.GetComponent<Renderer>().bounds.size.y;

        Vector3 rescale = ObjectHologram.transform.localScale;

        rescale.y = newSize * rescale.y / size;

        ObjectHologram.transform.localScale = rescale;

    }

    #endregion // PROTECTED_METHODS
}

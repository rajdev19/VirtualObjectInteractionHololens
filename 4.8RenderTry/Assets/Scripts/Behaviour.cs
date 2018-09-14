using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Vuforia;

public class Behaviour : MonoBehaviour{
    GameObject ScanContainer;
    public static List<GameObject> ScannedObjects;
    public List<GameObject> ScannedObjectsTemp;
    public static List<Quaternion> InitialObjectRotations;
    public List<Quaternion> InitialObjectRotationsTemp;
    public static List<Vector3> InitialObjectPositions;
    public List<Vector3> InitialObjectPositionsTemp;
    // Use this for initialization
    void Start()
    {
        //GameObject.Destroy(GameObject.Find("TextureBufferCamera"));
        Transform[] transformObjects;
        //GameObject temp = GameObject.Find("Controller");
        //if (temp.name=="Controller")
        //{
        //    InputManager.Instance.PushFallbackInputHandler(temp);
        //}
        //else
        //    Debug.Log(message:"did not find controller"+temp.name);
        //InputManager.Instance.PushFallbackInputHandler(GameObject.Find("Controller"));

        ScanContainer = GameObject.Find("Scanned");
        transformObjects = ScanContainer.GetComponentsInChildren<Transform>();
        foreach (var comp in transformObjects)
        {
            Debug.Log("start of foreach");
            foreach (GameObject tempGO in GameObject.FindGameObjectsWithTag("Models"))
            {
                if (tempGO.name == comp.name)
                {
                    //GameObject tempGO = GameObject.Find(comp.name);
                    Debug.Log(message: "comp name" + comp.name);
                    //if ((comp.name != "Scanned") && (comp.name != "default"))
                    //{
                        //Debug.Log(comp.name);
                        Debug.Log(message: "is temp go active: " + tempGO.active);
                        Vector3 tempPos = Vector3.zero;
                        //foreach (var gameObj in GameObject.FindObjectsOfType(typeof(ObjectTarget)))
                        foreach (var gameObj in GameObject.FindGameObjectsWithTag("Targets"))
                        {
                            if (gameObj.name == comp.name)
                            {
                                GameObject tempPosgo = gameObj as GameObject;
                                tempPos = comp.transform.position - tempPosgo.transform.position;
                            }
                        }
                        ScannedObjectsTemp.Add(tempGO);
                        InitialObjectPositionsTemp.Add(tempPos);
                        InitialObjectRotationsTemp.Add(tempGO.transform.rotation);
                        Debug.Log("set inactive");
                        tempGO.SetActive(false);
                    //}
                }
            }


        }
        Debug.Log(InitialObjectPositionsTemp.ToString());
        ScannedObjects = ScannedObjectsTemp;
        InitialObjectRotations = InitialObjectRotationsTemp;
        InitialObjectPositions = InitialObjectPositionsTemp;

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
       // {
        //    Vector3 temp2 = Input.mousePosition;
         //   foreach (var comp in ScannedObjects)
          //  {
          //      Debug.Log(Input.mousePosition);
           //     Quaternion temp1 = Quaternion.identity;
          //      temp2.z = 1000;
           //     GameObject temp = Object.Instantiate(comp, temp2, temp1, null);
             //   temp.SetActive(true);


          //  }
      //  }

    }
}

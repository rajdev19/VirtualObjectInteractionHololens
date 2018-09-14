using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine.XR.WSA;
using Vuforia;

public class Meta : MonoBehaviour {

    // Use this for initialization
    GameObject a;
    float mTime;
	void Start () {
        mTime = Time.time + 30;
        Debug.Log("timeout");
        //a = GameObject.Find("ObjectMade");
        //a.SetActive(false);
        //GameObject temp;
        //temp = GameObject.Find("ARCamera");
        //Camera ar = Camera.main;
        
        
        //DontDestroyOnLoad(temp);
        //temp = GameObject.Find("SpatialMapping");
        //DontDestroyOnLoad(temp);

        //temp=GameObject.Find()
    }
	
	// Update is called once per frame
	void Update () {
        int temptime = (int)Time.time;
        if(Time.time>mTime && temptime%5==0)
        {
            //GameObject temp = Object.Instantiate(a);
            //temp.SetActive(true);
            Debug.Log(Time.time);
            //a = GameObject.Find("SpatialMapping");
            //SpatialMappingManager sm = a.GetComponent<SpatialMappingManager>() as SpatialMappingManager;
            //Destroy(sm);
            //SpatialMappingObserver so = a.GetComponent<SpatialMappingObserver>() as SpatialMappingObserver;
            //Destroy(so);

            //LoadNextSceneAsync();
        }
        if(Input.GetMouseButtonDown(0))
        {
            //a = GameObject.Find("SpatialMapping");

            //SpatialMappingManager sm = a.GetComponent<SpatialMappingManager>() as SpatialMappingManager;
            //Destroy(sm);
            //SpatialMappingCollider sm
            //SpatialMappingCollider sm = a.GetComponent<SpatialMappingCollider>() as SpatialMappingCollider;
            //sm.freezeUpdates=true;
            //Camera ARC = Camera.main;
            //CameraDevice.Instance.Stop();
            //SpatialMappingObserver so = a.GetComponent<SpatialMappingObserver>() as SpatialMappingObserver;
            //Destroy(so);
            //GameObject temp;
            //temp = GameObject.Find("Managers");
            //DestroyImmediate(temp);
            //temp = GameObject.Find("Controller");
            //DestroyImmediate(temp);
            //a = GameObject.Find("SpatialMapping");
            //Destroy(a);
            //a = GameObject.Find("ControllerSM");
            //Destroy(a);
            LoadNextSceneAsync();
            //Application.LoadLevel("2-Hololens");
        }
		
	}
  //  public void OnInputClicked(InputClickedEventData eventData)
  //  {
  //      Application.LoadLevel("2-Hololens");
  //LoadNextSceneAsync();
  //  }
    private void LoadNextSceneAsync()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        Application.LoadLevelAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

}

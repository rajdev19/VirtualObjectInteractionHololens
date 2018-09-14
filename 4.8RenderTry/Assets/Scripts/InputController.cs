using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class InputController : MonoBehaviour,IInputClickHandler {
    GameObject temp;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Clicked1");
        throw new System.NotImplementedException();
        Debug.Log("Clicked2");
    }

    //public void OnInputClicked(InputClickedEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //    Debug.Log("clicked");
    //    GameObject d = Object.Instantiate(temp);
    //    d.SetActive(true);
    //
    //}

    // Use this for initialization
    void Start () {
        InputManager.Instance.PushFallbackInputHandler(GameObject.Find("SpatialMapping"));
        temp = GameObject.Find("StarbucksCup");
        temp.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

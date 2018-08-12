using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

    private Vector3 m_startMousePosition;
    private bool m_isTouching;
    
    public float Shift
    {
        get;
        private set;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
        {
            m_isTouching = true;
            m_startMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            m_isTouching = false;

        if (m_isTouching)
        {
            Shift = (Input.mousePosition.x - m_startMousePosition.x) / Screen.width;
            m_startMousePosition = Input.mousePosition;
        }
	}
}

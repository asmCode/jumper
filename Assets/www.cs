using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class www : MonoBehaviour {

    public Dude s;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = s.m_lookTargetSmooth;

    }
}

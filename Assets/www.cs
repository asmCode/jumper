using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class www : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float s = Physics.GetRequiredSpeed(Mathf.PI / 4.0f, 100, 10.0f);
        s = 0.0f;
    }
}

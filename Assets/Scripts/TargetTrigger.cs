using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetTrigger : MonoBehaviour {

	public float delayTime = 0.0f;
	public Transform vanishEffect;
	public float shrinkScale = 0.1f;
	public float shrinkSpeed = 1.0f;
	public UnityEvent OnHit;
	private bool hit = false;

	public void Awake()
	{
		hit = false;
	}

	public void NotifyHit()
	{
		OnHit.Invoke ();
		Instantiate (vanishEffect, transform.position, transform.localRotation);
		hit = true;
		Destroy (this.gameObject, delayTime);
	}

	public void Update()
	{
		if(hit)
		{
			this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (shrinkScale, shrinkScale, shrinkScale), Time.deltaTime * shrinkSpeed);
		}
	}
}

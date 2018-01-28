using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetTrigger : MonoBehaviour {

	public float delayTime = 0.0f;
	public Transform vanishEffect;
	public UnityEvent OnHit;

	public void NotifyHit()
	{
		OnHit.Invoke ();
		vanishEffect.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
	}
}

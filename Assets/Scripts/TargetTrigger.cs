using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetTrigger : MonoBehaviour {

	public float delayTime = 0.0f;
	public Transform vanishEffect;
	public UnityEvent OnHit;

	private GameObject Gate;

	public void NotifyHit()
	{
		OnHit.Invoke();
		Gate.GetComponent<GateAnimation>().GateOpener(true);
		Gate.transform.GetChild(2).gameObject.SetActive(false);
		vanishEffect.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
	}

	public void AssignDepemdancie(GameObject x)
	{
		Gate = x;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetTrigger : MonoBehaviour {

	public Transform vanishEffect;
	public UnityEvent OnHit;

	private GameObject Gate;

	public void NotifyHit()
	{
		OnHit.Invoke();
		Gate.GetComponent<GateParams>().doors.GetComponent<GateAnimation>().GateOpener(true);
		Gate.GetComponent<GateParams>().doorsCollider.SetActive(false);
		vanishEffect.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
	}

	public void AssignDepemdancie(GameObject x)
	{
		Gate = x;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAnimation : MonoBehaviour {

	private Animator a_animator;

	private void Awake()
	{
		a_animator = GetComponent<Animator>();
	}

	public void GateOpener(bool state)
	{
		a_animator.SetBool ("Opened", state);
	}
}

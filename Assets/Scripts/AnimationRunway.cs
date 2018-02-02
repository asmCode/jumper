using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRunway : MonoBehaviour {

	public float animationSpeed = 0.5f;

	void Update () {

		float offsetY = Time.time * animationSpeed;
		GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (0.0f, offsetY);

	}
}

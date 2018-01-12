using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionFlicker : MonoBehaviour {

	private Material m_Material;
	public float floor = 0.5f;
	public float ceiling = 1.0f;
	public Color baseColor = Color.red;
	public int frequency = 1;

	private void Start()
	{
		m_Material = GetComponent<Renderer> ().material;
	}

	private void Update()
	{
		float emission = floor + Mathf.PingPong (Time.time*frequency, ceiling - floor);
		Color finalColor = baseColor * emission;
		m_Material.SetColor ("_EmissionColor", finalColor);
	}
}

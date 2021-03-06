﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diagnostic : MonoBehaviour
{
	public Dude2 m_dude;

	public Transform m_prevPlatform;
	public Transform m_nextPlatform;
	public Transform m_lookTargetSmooth;

	private void Update()
	{
		m_prevPlatform.position = m_dude.PrevPlatform.transform.position;
		m_nextPlatform.position = m_dude.NextPlatform.transform.position;
		m_lookTargetSmooth.position = m_dude.LookTargetSmooth;
	}
}

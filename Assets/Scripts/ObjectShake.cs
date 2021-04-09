﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// http://www.mikedoesweb.com/2012/camera-shake-in-unity/
/// This File was taken off of github from user: GuilleUCM

public class ObjectShake : MonoBehaviour
{

	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay = 0.2f;
	public float shake_intensity = .5f;

	private float temp_shake_intensity = 0;

	//void OnGUI()
	//{
	//	if (GUI.Button(new Rect(20, 40, 80, 20), "Shake"))
	//	{
	//		Shake();
	//	}
	//}

	void Update()
	{
		if (temp_shake_intensity > 0)
		{
			//transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
				originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
				originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
				originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
			temp_shake_intensity -= shake_decay;
		}
	}

	public void Shake()
	{
		originPosition = transform.position;
		originRotation = transform.rotation;
		temp_shake_intensity = shake_intensity;
	}
}

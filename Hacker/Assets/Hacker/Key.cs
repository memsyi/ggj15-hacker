﻿using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class Key : MonoBehaviour {
	[SerializeField] public AudioClip Pickup;

	void OnTriggerEnter2D(Collider2D other) {
		Platformer2DUserControl p = other.GetComponent<Platformer2DUserControl>();
		if (p == null || p.HasKey)
			return;
		if (Pickup != null) {
			AudioSource.PlayClipAtPoint(Pickup, Vector3.zero);
		}
		Destroy (GetComponent<Collider2D>());
		Transform t = other.gameObject.transform;
		transform.localPosition = t.position + new Vector3(-0.5f, 0.5f, 0.0f);
		transform.parent = t;
		p.HasKey = true;
	}
}

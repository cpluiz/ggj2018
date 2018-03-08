using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour{
	[SerializeField]
	private PruController player = null;

	void Awake() {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			player = other.GetComponent<PruController>();
		} else {
			if (player != null && other.CompareTag("Effect")) {
				player.ExecuteAction((other.name));
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			player = null;
		}
	}
}

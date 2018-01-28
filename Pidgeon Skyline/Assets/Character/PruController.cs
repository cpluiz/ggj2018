using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruController : MonoBehaviour {

	BoardManager board;
	int x,y;
	[SerializeField]
	Vector2 targetPosition;

	// Use this for initialization
	void Start () {
		x = 0;
		y = 1;
		board = GameObject.FindObjectOfType<BoardManager> ();
		transform.position = board.GetGridPosition (x-1, y);
		targetPosition = board.GetGridPosition (x, y);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Go") {
			x += 1;
			targetPosition = board.GetGridPosition (x, y);
		}
	}
}

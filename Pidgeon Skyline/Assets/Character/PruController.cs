using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets;

public class PruController : MonoBehaviour {

	BoardManager board;
	Vector2Int boardCoordinates;
	[SerializeField]
	Vector2 targetPosition;


	// Use this for initialization
	void Start () {
		boardCoordinates.x = 0;
		boardCoordinates.y = 1;
		board = GameObject.FindObjectOfType<BoardManager> ();
		transform.position = board.GetGridPosition (boardCoordinates);
		targetPosition = board.GetGridPosition (boardCoordinates);
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (MoveAlogBoard()) {
			transform.position = targetPosition;
		}
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
	}

	public void ExecuteAction(String action) {
		Debug.Log("Ação: " + action);
		switch (action) {
		case "Go":
			boardCoordinates = board.validateCoordinates (boardCoordinates + Vector2Int.right);
			targetPosition = board.GetGridPosition (boardCoordinates);
			AudioManager.PlayEffect ("avancar_pombo");
			break;
		case "Return":
			boardCoordinates = board.validateCoordinates(boardCoordinates + Vector2Int.left);
			targetPosition = board.GetGridPosition (boardCoordinates);
			AudioManager.PlayEffect ("voltar_pombo");
			break;
		case "Attack":
			AudioManager.PlayEffect ("ataque_pombo");
			break;
		case "Special":
			AudioManager.PlayEffect ("special_pombo");
			break;
		}
	}

	bool MoveAlogBoard() {
		Vector2 actualPosition = targetPosition;
		targetPosition = board.GetGridPosition (boardCoordinates);
		return actualPosition != targetPosition;
	}
}

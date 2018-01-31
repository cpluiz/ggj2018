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
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other){
		switch (other.name) {
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
}

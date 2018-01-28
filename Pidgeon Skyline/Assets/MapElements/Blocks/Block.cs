using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour {

	public Sprite blockImage;
	public string blockName;
	[Range(0.001f, 1f)]
	public float moveDetectionOffset = 0.05f;
	[SerializeField]
	private Vector2 anchoredPosition;
	[SerializeField]
	private Vector2Int index;
	private float moveOffset, halfSize;
	private SpriteRenderer render;
	GridPlay grid;
	public Color c;

	bool isMoving = false;
	bool isDeleting = false;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
		render.sprite = blockImage;
		gameObject.name = blockName;
		moveOffset = render.bounds.size.x;
		halfSize = render.bounds.extents.x;
		moveDetectionOffset = render.bounds.extents.x*.5f;
		anchoredPosition = transform.position;
		grid = GetComponentInParent<GridPlay> ();
	}

	public void SetIndex(int x, int y){
		index = new Vector2Int (x, y);
	}

	public void SetIndex(Vector2Int idx){
		index = idx;
	}

	public void ChangePosition(Vector2 newPosition){
		transform.position = anchoredPosition = newPosition;
	}

	public void CheckPosition(){
		if (transform.position.x > anchoredPosition.x + halfSize) {
			grid.ChangePosition (index, Vector2Int.right);
		} else if (transform.position.x < anchoredPosition.x - halfSize) {
			grid.ChangePosition (index, Vector2Int.left);
		}else if(transform.position.y > anchoredPosition.y + halfSize){
			grid.ChangePosition (index, Vector2Int.down);
		}else if(transform.position.y < anchoredPosition.y - halfSize){
			grid.ChangePosition (index, Vector2Int.up);
		}else {
			transform.position = anchoredPosition;
		}
	}

	
	// Update is called once per frame
	void Update () { 
		if (isDeleting) {
			render.color = c;
			return;
		}
		if (grid.deletingCol || grid.deletingRow)
			return;
		if (Input.GetMouseButtonUp (0) && isMoving) {
			CheckPosition ();
			render.sortingOrder = 0;
			isMoving = false;
		}
		if (!isMoving)
			return;
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		if (mousePos.x < anchoredPosition.x - moveDetectionOffset || mousePos.x > anchoredPosition.x + moveDetectionOffset) {
			mousePos.y = anchoredPosition.y;
			mousePos.x = Mathf.Clamp (mousePos.x, anchoredPosition.x - moveOffset, anchoredPosition.x + moveOffset);
		} else if (mousePos.y > anchoredPosition.y + moveDetectionOffset || mousePos.y < anchoredPosition.y - moveDetectionOffset) {
			mousePos.x = anchoredPosition.x;
			mousePos.y = Mathf.Clamp (mousePos.y, anchoredPosition.y - moveOffset, anchoredPosition.y + moveOffset);
		}else{
			mousePos = anchoredPosition;
		}
		mousePos.x = Mathf.Clamp (mousePos.x, grid.puzzleBounds.min.x + halfSize, grid.puzzleBounds.max.x - halfSize);
		mousePos.y = Mathf.Clamp (mousePos.y, grid.puzzleBounds.min.y + halfSize, grid.puzzleBounds.max.y - halfSize);
		transform.position = mousePos;
	}

	void OnMouseDown(){
		if (isDeleting || grid.deletingCol || grid.deletingRow)
			return;
		if (Input.GetMouseButton (0)) {
			isMoving = true;
			render.sortingOrder = 10;
		}
	}

	public bool DestroyBlock(){
		isDeleting = true;
		c = render.color;
		c.a -= 0.1f;
		return (c.a <= 0);
	}
}
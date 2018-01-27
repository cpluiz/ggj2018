using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlay : MonoBehaviour {

	[Header("Blocks to build a grid")]
	public Block[] blocks;
	//number of total puzzle columns in screen
	private int cols = 24;
	private int rows = 6;
	//GridMap position in pixel unitys of the screen
	private Vector2[,] point;
	private Block[,] block;
	CameraHelper cam;
	public Bounds puzzleBounds, mapBounds;

	// Use this for initialization
	void Start () {
		point = new Vector2[cols,rows];
		block = new Block[cols, rows];
		int pointIndex = 0;
		cam = Camera.main.GetComponent<CameraHelper>();
		cam.CalcCameraBounds ();
		puzzleBounds = new Bounds ();
		var blockWidth = (cam.screenWidth / cols);
		var blockWidthCenterOffset = blockWidth /2;
		for (int i = 0; i < cols; i++) {
			for (int j = 0; j < rows; j++) {
				point[i,j] = new Vector2 (cam.left + ( blockWidth * i ) + blockWidthCenterOffset, cam.top - (blockWidth * j) - blockWidthCenterOffset);
				Block toInstatiate = GetBlock (i, j);
				block[i,j] = Instantiate (toInstatiate, point [i,j], Quaternion.identity, gameObject.transform) as Block;
				block[i,j].SetIndex(i,j);
			}
		}
		puzzleBounds = new Bounds (new Vector2(0, point[0,4].y+blockWidth*1.5f) ,new Vector2 (blockWidth * cols, blockWidth * rows));
	}

	//TODO Actually initialize the puzzle to not start with a lot of events at the start
	Block GetBlock(int x, int y){
		Block toInstantiate = blocks [Random.Range (0, blocks.Length - 1)];
		if (y > 1) {
			if (block [x, y - 1].blockName == block [x, y - 2].blockName && block [x, y - 1].blockName == toInstantiate.blockName) {
				toInstantiate = GetBlock (x, y);
			}
		}
		if (x > 1) {
			if (block [x - 1, y].blockName == block [x - 2, y].blockName && block [x - 1, y].blockName == toInstantiate.blockName) {
				toInstantiate = GetBlock (x, y);
			}
		}
		return toInstantiate;
	}

//	bool CheckNeighboards(float x, float y){
//		
//	}
//
//	bool CheckPreviousHorizontalNeighboards(float x, float y){
//	}
//	bool CheckNextHorizontalNeighboards(float x, float y){
//	}
//	bool CheckPreviousVerticalNeighboards(float x, float y){
//	}
//	bool CheckNextVerticalNeighboards(float x, float y){
//	}

	public void ChangePosition (Vector2Int index, Vector2Int direction){
		Vector2Int index2 = index + direction;
		block [index.x, index.y].SetIndex (index2);
		block [index.x, index.y].ChangePosition(point[index2.x, index2.y]);
		block [index2.x, index2.y].SetIndex (index);
		block [index2.x, index2.y].ChangePosition(point[index.x, index.y]);
		Block temp = block [index.x, index.y];
		block [index.x, index.y] = block [index2.x, index2.y];
		block [index2.x, index2.y] = temp;
	}

	void Update () {
		
	}
}

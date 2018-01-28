using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	CameraHelper cam;

	public MazeTile[] tiles;

	private int boardOffsetX = 0;
	private MazeTile[,] tile;
	private Vector2 firstTileUp;

	private int cols = 6;
	private int rows = 3;

	private int mazeCols = 10;
	private int mazeRols = 3;

	float blockWidth, blockHeight;

	// Use this for initialization
	void Start () {
		tile = new MazeTile[mazeCols,mazeRols];
		cam = Camera.main.GetComponent<CameraHelper>();
		cam.CalcCameraBounds ();
		blockWidth = (cam.screenWidth / cols);
		blockHeight = (cam.screenHeight * .6f) / rows;
		var blockWidthCenterOffset = blockWidth /2;
		var blockHeightCenterOffset = blockHeight / 2;
		firstTileUp = new Vector2 (cam.left+blockWidthCenterOffset, cam.top-(cam.screenHeight*.4f)-blockHeightCenterOffset);
		for (int i = 0; i < mazeCols; i++) {
			for (int j = 0; j < mazeRols; j++) {
				tile[i,j] = Instantiate (tiles [0], new Vector2 (firstTileUp.x +(blockWidth * i), firstTileUp.y -(blockHeight * j)), Quaternion.identity, transform) as MazeTile;
			}
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			MoveBoardToRight (1);
		}
	}

	void MoveBoardToRight(int moveIndex){
		boardOffsetX += moveIndex;
		for (int y = 0; y < mazeRols; y++) {
			for (int x = 0; x < mazeCols; x++) {
				tile [x, y].transform.position = new Vector2 (firstTileUp.x + (blockWidth * (x - boardOffsetX)), firstTileUp.y - (blockHeight * y));
			}
		}
	}

}

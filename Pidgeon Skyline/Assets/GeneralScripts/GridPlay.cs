using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets;

public class GridPlay : MonoBehaviour {

	[Header("Blocks to build a grid")]
	public Block[] blocks;
	public EffectBehavior effect;
	//number of total puzzle columns in screen
	private int cols = 24;
	private int rows = 6;
	//GridM	ap position in pixel unitys of the screen
	private Vector2[,] point;
	[SerializeField]
	private Block[,] block;
	CameraHelper cam;
	public Bounds puzzleBounds;
	private List<Vector2Int> blocksToDestroy;
	public bool deletingRow, deletingCol = false;
	Vector2 effectPosition;
	bool showEffect;
	string effectName;

	// Use this for initialization
	void Start () {
		point = new Vector2[cols,rows];
		block = new Block[cols, rows];
		cam = Camera.main.GetComponent<CameraHelper>();
		cam.CalcCameraBounds ();
		puzzleBounds = new Bounds ();
		blocksToDestroy = new List<Vector2Int> ();
		var blockWidth = (cam.screenWidth / cols);
		var blockWidthCenterOffset = blockWidth /2;
		for (int i = 0; i < cols; i++) {
			for (int j = 0; j < rows; j++) {
				point[i,j] = new Vector2 (cam.left + ( blockWidth * i ) + blockWidthCenterOffset, cam.top - (blockWidth * j) - blockWidthCenterOffset);
				Block toInstatiate = GetBlock (i, j);
				block[i,j] = Instantiate (toInstatiate, point [i,j], Quaternion.identity, transform) as Block;
				block[i,j].SetIndex(i,j);
			}
		}
		puzzleBounds = new Bounds (new Vector2(0, point[0,4].y+blockWidth*1.5f) ,new Vector2 (blockWidth * cols, blockWidth * rows));
//		AudioManager.MusicVolume (0.5f);
	}

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

	void CheckNeighboards(Vector2Int index){
		CheckNeighboards (index.x, index.y);
	}

	void CheckNeighboards(int x, int y){
		if (deletingCol || deletingRow)
			return;
		int vertical, horizontal = 0;
		string blockName = block [x, y].blockName;
		vertical = CheckVertical (x, y, blockName);
		if (vertical >= 3) {
			effectName = blockName;
			showEffect = !blockName.Equals("NoAction");
			deletingCol = true;
			deletingRow = false;
			effectPosition = point [x, y];
		} else{
			horizontal = CheckHorizontal (x, y, blockName);
			if (horizontal >= 3) {
				effectName = blockName;
				showEffect = !blockName.Equals("NoAction");
				deletingRow = true;
				deletingCol = false;
				effectPosition = point [x, y];
			}
		}
	}

	void FillPuzzleBoard(){
		int added = 0;
		for (int i = 0; i < cols; i++) {
			for (int j = 0; j < rows; j++) {
				if (block [i, j] == null) {
					block [i, j] = Instantiate (blocks [Random.Range (0, blocks.Length)], point [i, j], Quaternion.identity, transform);
					block [i, j].SetIndex (i, j);
					added++;
				}
			}
		}
		if (added > 0) {
			CheckAllPuzzleBlocks ();
		}
	}

	void DestroyColumnBlocks(){
		int minY = rows;
		int x = blocksToDestroy [0].x;
		int rowsToFall = blocksToDestroy.Count;
		bool remove = true;
		int count = rowsToFall;
		foreach (Vector2Int index in blocksToDestroy) {
			remove = remove && RemoveBlock (index.x, index.y);
			if (minY > index.y-1) {
				minY = index.y-1;
			}
		}
		while (minY >= 0 && remove) {
			ChangePosition (x, minY, Vector2Int.up * rowsToFall);
			minY--;
		}
		if (remove) {
			deletingCol = false;
		}
	}

	bool RemoveBlock(int x, int y, int index = 1, int totalToRemove = 1){
		Block bloc = block [x, y];
		string complement = "";
		if (bloc == null)
			return true;
		if (bloc.DestroyBlock ()) {
			Destroy (bloc.gameObject);
			if (index == 0) {
				complement = "+1";
			} else if (index < totalToRemove) {
				complement = "+2";
			}
			AudioManager.PlayEffect ("Drop"+complement);
			block [x, y] = null;
			return true;
		}
		return false;
	}
		

	void DestroyRowBlocks(){
		int y = blocksToDestroy [0].y;
		int x = blocksToDestroy [0].x;
		bool remove = true;
		int total = blocksToDestroy.Count;
		foreach (Vector2Int index in blocksToDestroy) {
			remove = remove && RemoveBlock (index.x, index.y);
			if (!remove) {
				continue;
			}
		}
		int j = y-1;
		while (j >= 0 && remove) {
			ChangePosition (x, j, Vector2Int.up);
			j--;
		}
		if (remove) {
			deletingRow = false;
		}
	}

	void CheckAllPuzzleBlocks(){
		for (int i = 0; i < cols; i++) {
			for (int j = 0; j < rows; j++) {
				CheckNeighboards (i, j);
			}
		}
	}

	int CheckVertical(int x, int y, string blockName){
		blocksToDestroy.Clear ();
		blocksToDestroy.Add (new Vector2Int(x,y));
		int sequentialBlocks = 1;
		int prevY = y - 1;
		int nextY = y + 1;
		while(prevY>=0 && CheckBlockName(x,prevY,blockName)){
			blocksToDestroy.Add (new Vector2Int(x,prevY));
			sequentialBlocks++;
			prevY--;
		}
		while (nextY < rows && CheckBlockName(x, nextY, blockName)) {
			blocksToDestroy.Add (new Vector2Int(x,nextY));
			sequentialBlocks++;
			nextY++;
		}
		return sequentialBlocks;
	}

	int CheckHorizontal(int x, int y, string blockName){
		blocksToDestroy.Clear ();
		blocksToDestroy = new List<Vector2Int> ();
		blocksToDestroy.Add (new Vector2Int(x,y));
		int sequentialBlocks = 1;
		int prevX = x - 1;
		int nextX = x + 1;
		while(prevX>=0 && CheckBlockName(prevX,y,blockName)){
			blocksToDestroy.Add (new Vector2Int(prevX,y));
			sequentialBlocks++;
			prevX--;
		}
		while (nextX < cols && CheckBlockName(nextX, y, blockName)) {
			blocksToDestroy.Add (new Vector2Int(nextX,y));
			sequentialBlocks++;
			nextX++;
		}
		return sequentialBlocks;
	}

	bool CheckBlockName(int x, int y, string blockName){
		return block [x, y].blockName == blockName;
	}


	public void ChangePosition (Vector2Int index, Vector2Int direction){
		Vector2Int index2 = index + direction;
		block [index.x, index.y].SetIndex (index2);
		block [index.x, index.y].ChangePosition(point[index2.x, index2.y]);
		block [index2.x, index2.y].SetIndex (index);
		block [index2.x, index2.y].ChangePosition(point[index.x, index.y]);
		Block temp = block [index.x, index.y];
		block [index.x, index.y] = block [index2.x, index2.y];
		block [index2.x, index2.y] = temp;
		CheckNeighboards (index2);
		CheckNeighboards (index);
	}

	void ChangePosition(int x, int y, Vector2Int moveIndex){
		block [x, y].SetIndex (x + moveIndex.x, y + moveIndex.y);
		block [x, y].ChangePosition(point[x + moveIndex.x, y + moveIndex.y]);
		Block temp = block [x, y];
		block [x, y] = block [x + moveIndex.x, y + moveIndex.y];
		block [x + moveIndex.x, y + moveIndex.y] = temp;
	}


	//It's not preaty, but it works!!!
	void Update () {
		if (deletingCol) {
			DestroyColumnBlocks ();
			if (!deletingCol) {
				if(showEffect)
					Instantiate (effect, effectPosition, Quaternion.identity);
				FillPuzzleBoard ();
			}
		}else if (deletingRow) {
			DestroyRowBlocks ();
			if (!deletingRow) {
				if(showEffect)
					Instantiate (effect, effectPosition, Quaternion.identity);
				FillPuzzleBoard ();
			}
		}
	}
}

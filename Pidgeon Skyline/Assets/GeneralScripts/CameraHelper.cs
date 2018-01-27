using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour {
	Bounds cameraBounds;
	public float top, down, left, right;
	public float screenWidth;
	Camera cam;

	// Use this for initialization
	public void CalcCameraBounds () {
		cam = GetComponent<Camera> ();
		var lowerLeft = cam.ScreenToWorldPoint (Vector3.zero);
		var uperRight = cam.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));
		top = uperRight.y; down = lowerLeft.y; left = lowerLeft.x; right = uperRight.x;
		screenWidth = right - left;
	}
}

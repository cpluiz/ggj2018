﻿using UnityEngine;
using UnityEngine.SceneManagement;
using BaseAssets;

public class TitleManager : MonoBehaviour {

	public float audioVolume, effectVolume, uiVolume = 50f;

	// Use this for initialization
	void Start () {
		AudioManager.MusicVolume (audioVolume);
		AudioManager.EffectsVolume (effectVolume);
		AudioManager.EffectsVolume (uiVolume);
	}

	public void LoadNextScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex +1);
	}

}

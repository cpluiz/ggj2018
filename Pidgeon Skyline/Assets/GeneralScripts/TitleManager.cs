using UnityEngine;
using UnityEngine.SceneManagement;
using BaseAssets;

public class TitleManager : MonoBehaviour {

	public float audioVolume, effectVolume = 50f;

	// Use this for initialization
	void Start () {
		AudioManager.MusicVolume (audioVolume);
		AudioManager.EffectsVolume (effectVolume);
	}

	public void LoadNextScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex +1);
	}
	

}

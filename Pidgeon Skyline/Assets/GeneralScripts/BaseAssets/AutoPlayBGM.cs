using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseAssets {
    public class AutoPlayBGM : MonoBehaviour {
        void Awake() {
            SceneManager.sceneLoaded += AudioManager.instance.OnSceneLoad;
        }
        void OnDisable() {
            //SceneManager.sceneLoaded -= AudioManager.instance.OnSceneLoad;
        }
    }
}
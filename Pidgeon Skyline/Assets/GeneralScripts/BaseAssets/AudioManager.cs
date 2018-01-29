using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BaseAssets {
    public class AudioManager : MonoBehaviour {
        private static AudioManager _instance;
        public static AudioManager instance {
            get {
                if(_instance == null)
                    if(GameObject.FindObjectOfType<AudioManager>() == null)
                        CreateAudioManager();
                return _instance;
            }
        }

        private AudioSource musicSource, effectSource, uiSource;

        private static void CreateAudioManager() {
            GameObject audioManager = new GameObject();
            audioManager.name = "AudioManager";
            audioManager.AddComponent<AudioManager>();
        }

        //Create a instance of AudioManager object if there is no one
        void Awake() {
            if(instance == null) {
                //Create a instance of music source object
                GameObject musicSource = new GameObject();
                musicSource.name = "MusicSource";
                musicSource.AddComponent<AudioSource>();
                musicSource.transform.parent = this.transform;
                this.musicSource = musicSource.GetComponent<AudioSource>();
                this.musicSource.loop = true;
                this.musicSource.playOnAwake = false;
                //Create a instance of User Interface audio source object
                GameObject uiSource = new GameObject();
                uiSource.name = "UiSource";
                uiSource.AddComponent<AudioSource>();
                uiSource.transform.parent = this.transform;
                this.uiSource = uiSource.GetComponent<AudioSource>();
                this.uiSource.playOnAwake = false;
                //Create a instance of generic audio source object
                //not to be used if there is a 3D base positioned effect in the sound to be played
                GameObject effectSource = new GameObject();
                effectSource.name = "EffectSource";
                effectSource.AddComponent<AudioSource>();
                effectSource.transform.parent = this.transform;
                this.effectSource = effectSource.GetComponent<AudioSource>();
                this.effectSource.playOnAwake = false;
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
			if (this != _instance)
				Destroy (gameObject);
        }

        //Correcting object position
        void FixedUpdate() {
            instance.transform.position = Camera.main.transform.position;
            instance.transform.rotation = Camera.main.transform.rotation;
        }

        //Try to load background music whit the same name as loaded level
        public void OnSceneLoad(Scene scene, LoadSceneMode mode) {
            PlayMusic(scene.name);
        }

        //Function called to define music volume
        public static void MusicVolume(float volume) {
            instance.musicSource.volume = volume / 100;
        }
        //Function called to define global effects (effect and user interface) volume;
        public static void EffectsVolume(float volume) {
            AudioManager.EffectVolume(volume);
        }
		public static void UIVolume(float volume){
			AudioManager.UiVolume(volume);
		}
        //Function called to define
        private static void EffectVolume(float volume) {instance.effectSource.volume = volume/100;}
        private static void UiVolume(float volume) { instance.uiSource.volume = volume/100; }

        public static void PlayMusic(string musicName) {
            if(instance.musicSource.isPlaying)
                instance.musicSource.Stop();
            AudioClip backgroundMusic = Resources.Load<AudioClip>("Audio/Music/" + musicName);
            if(backgroundMusic == null) {
                Debug.LogError("Music \"" + musicName + "\" not found. The music file must be in the Resources/Audio/Music folder, and be in a Unity compatible audio format.");
                return;
            }
            instance.musicSource.clip = backgroundMusic;
            instance.musicSource.Play();
        }

        public static void PlayUiEffect(string uiEffectName) {
            AudioClip uiEffect = Resources.Load<AudioClip>("Audio/UI/" + uiEffectName);
            if(uiEffect == null) {
                Debug.LogError("UI effect: \"" + uiEffectName + "\" not found. The music file must be in the Resources/Audio/UI folder, and be in a Unity compatible audio format.");
                return;
            }
            instance.uiSource.PlayOneShot(uiEffect);
        }

        public static void PlayEffect(string effectName) {
            AudioClip effect = Resources.Load<AudioClip>("Audio/SFX/" + effectName);
            if(effect == null) {
                Debug.LogError("SFX effect: \"" + effectName + "\" not found. The music file must be in the Resources/Audio/Effects folder, and be in a Unity compatible audio format.");
                return;
            }
            instance.effectSource.PlayOneShot(effect);
        }

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace BaseAssets {

    public class LanguageManager : MonoBehaviour {
        private static LanguageManager _instance;
        public static LanguageManager instance {
            get {
                if(_instance == null)
                    if(GameObject.FindObjectOfType<LanguageManager>() == null)
                        CreateLanguageManager();
                return _instance;
            }
        }

        private static void CreateLanguageManager() {
            GameObject languageManager = new GameObject();
            languageManager.name = "LanguageManager";
            languageManager.AddComponent<LanguageManager>();
            languageManager.AddComponent<LanguageReader>();
            DontDestroyOnLoad(languageManager);
        }

        void Awake() {

            _instance = this;
        }

        private static string lang;
        public static  List<string> languages { get { return LanguageReader.languageStrings; } }
        public static string language { get { return lang; } }
        public static string GetTranslation(string identifier) {
            return LanguageReader.getText(identifier);
        }
        private static void initReader() {
            LanguageReader.instance.initLanguage();
        }
        public static void changeLanguage(string language) {
            lang = language;
            LanguageReader.changeLanguage(language);
            applyChange();
        }
        private static void applyChange() {
            //InterfaceLanguage[] interfaces = GameObject.FindObjectsOfType<InterfaceLanguage>();
            //Debug.Log("Changing language to "+language+" "+interfaces.Length+" Interfaces to apply");
            //for(int i = 0;i < interfaces.Length;i++) {
            //    interfaces[i].applyTranslation();
            //}
        }
    }
}

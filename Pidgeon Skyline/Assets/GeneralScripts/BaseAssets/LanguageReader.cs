using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BaseAssets {
    public class LanguageReader : MonoBehaviour {

        private static LanguageReader _instance;
        public static LanguageReader instance {
            get {
                if(_instance == null)
                    if(GameObject.FindObjectOfType<LanguageReader>() == null)
                        CreateLanguageReader();
                return _instance;
            }
        }

        private static void CreateLanguageReader() {
            GameObject languageReader = new GameObject();
            languageReader.name = "LanguageManager";
            languageReader.AddComponent<LanguageReader>();
            languageReader.AddComponent<LanguageManager>();
            DontDestroyOnLoad(languageReader);
            _instance = languageReader.GetComponent<LanguageReader>();
        }

        private Dictionary<string,Dictionary<string,string>> languages;
        private Dictionary<string,string> activeLanguage;
        private List<string> languageString;
        public static List<string> languageStrings { get { return instance.languageString; } }
        [SerializeField]
        public List<TextAsset> languageXml = new List<TextAsset>();

        public static bool isReady { get{ return (instance.activeLanguage != null && instance.languages != null); } }

        void Awake() {
            TextAsset[] initLanguage = Resources.LoadAll<TextAsset>("Language/");
            foreach(TextAsset language in initLanguage)
                languageXml.Add(language);
            languages = new Dictionary<string,Dictionary<string,string>>();
            languageString = new List<string>();
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void initLanguage() {
            if(LanguageReader.isReady)
                return;
            for(int i = 0;i < languageXml.Count;i++) {
                Dictionary<string,string> l = new Dictionary<string,string>();
                XmlDocument xmlDocument = new XmlDocument();
                try {
                    xmlDocument.LoadXml(languageXml[i].text);
                    XmlNodeList langTag = xmlDocument.GetElementsByTagName("LanguageTag");
                    if(languageString.Contains(langTag[0].Attributes.GetNamedItem("id").Value))
                        break;
                    languageString.Add(langTag[0].Attributes.GetNamedItem("id").Value);
                    XmlNodeList textList = langTag[0].ChildNodes;
                    foreach(XmlNode langString in textList) {
                        if(langString.NodeType != XmlNodeType.Comment) {
                            string identifier = langString.Attributes.GetNamedItem("identifier").Value;
                            string text = langString.InnerText;
                            l.Add(identifier,text);
                        }
                    }
                    languages.Add(languageString[i],l);
                } catch(XmlException e) {
                   // Debug.LogError(e);
                }
            }
            changeLanguage(languageString[0]);
        }

        public static string getText(string identifier) {
            if(!LanguageReader.isReady)
                instance.initLanguage();
            string text = "";
            instance.activeLanguage.TryGetValue(identifier,out text);
            return text;
        }
        public static void changeLanguage(string language) {
            instance.languages.TryGetValue(language,out instance.activeLanguage);
        }
    }
}

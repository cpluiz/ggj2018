using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BaseAssets {
    public class TextEffects : MonoBehaviour {
        private static TextEffects _instance;
        public static TextEffects instance {
            get {
                if(_instance == null)
                    if(GameObject.FindObjectOfType<TextEffects>() == null)
                        CreateTextEffects();
                return _instance;
            }
        }
        public static void CreateTextEffects() {
            if(_instance != null)
                return;
            GameObject textCanvas = new GameObject();
            textCanvas.name = "TextCanvas";
            textCanvas.AddComponent<TextEffects>();
        }
        private Text textComponent;
        private Canvas textCanvas;
        private CanvasRenderer textRenderer;
        private RectTransform rect;
        private Image frameImage, nextImage;

        // Use this for initialization
        void Awake() {
            if(instance == null) {
                textCanvas = this.gameObject.AddComponent<Canvas>();
                textCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                this.gameObject.AddComponent<CanvasScaler>();
                this.gameObject.AddComponent<GraphicRaycaster>();
                textRenderer = new GameObject().AddComponent<CanvasRenderer>();
                textRenderer.name = "TextRenderer";
                textRenderer.transform.SetParent(textCanvas.transform,false);
                frameImage = textRenderer.gameObject.AddComponent<Image>();
                rect = textRenderer.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0.5f,0.5f);
                rect.anchorMin = new Vector2(0,0);
                rect.anchorMax = new Vector2(1f,0.2f);
                rect.offsetMin = new Vector2(0,0);
                rect.offsetMax = new Vector2(0,0);

                nextImage = new GameObject().AddComponent<Image>();
                nextImage.name = "NextImage";
                nextImage.transform.SetParent(textRenderer.transform,false);
                nextImage.rectTransform.anchoredPosition = new Vector2(0.5f,0.5f);
                nextImage.rectTransform.anchorMin = new Vector2(0.965f,0.25f);
                nextImage.rectTransform.anchorMax = new Vector2(0.965f,0.25f);
                nextImage.rectTransform.offsetMin = new Vector2(-15f,-15f);
                nextImage.rectTransform.offsetMax = new Vector2(15f,15f);
                nextImage.color = Color.black;

                textComponent = new GameObject().AddComponent<Text>();
                textComponent.name = "Text";
                textComponent.font = Resources.Load<Font>("Fonts/CONTF___");
                //textComponent.font = Resources.GetBuiltinResource(typeof(Font),"Arial.ttf") as Font;
                textComponent.color = Color.black;
                textComponent.rectTransform.anchorMin = new Vector2(0.01f,0.1f);
                textComponent.rectTransform.anchorMax = new Vector2(0.99f,0.9f);
                textComponent.rectTransform.offsetMin = new Vector2(0,0);
                textComponent.rectTransform.offsetMax = new Vector2(0,0);
                textComponent.transform.SetParent(textRenderer.transform,false);
                _instance = this;
                DontDestroyOnLoad(_instance);
                DisableCanvas();
            }
        }


        private Color textColor = Color.black;

        private string fullText = "";
        private float slowSpeed = 0.2f;
        private float normalSpeed = 0.1f;
        private float rapidSpeed = 0.05f;
        private float speed = 0f;
        public bool showNext = false;
        private bool completedText = false;
        public static bool completed { get { return instance.completedText; } }

        public static void SetTextSpeed(TextSpeed spd) {
            switch(spd) {
                case TextSpeed.slow:
                    instance.speed = instance.slowSpeed;
                    break;
                case TextSpeed.normal:
                    instance.speed = instance.normalSpeed;
                    break;
                case TextSpeed.fast:
                    instance.speed = instance.rapidSpeed;
                    break;
                default:
                    instance.speed = instance.normalSpeed;
                    break;
            }
        }

        public static void SetTextColor(Color color) {
            instance.textColor = color;
        }

        public static void SetFrameImage(string frameName) {
            instance.frameImage.sprite = Resources.Load<Sprite>("Images/" + frameName) as Sprite;
        }

        public static void UpdateText(string txt) {
            instance.textComponent.text = txt;
        }
        public static void EnableShadow() {
            EnableShadow(Color.black);
        }
        public static void EnableShadow(Color color) {
            if(instance.textComponent.gameObject.GetComponents<Shadow>().Length <= 0) {
                Shadow shadow = instance.textComponent.gameObject.AddComponent<Shadow>();
                shadow.effectColor = color;
            } else {
                instance.textComponent.gameObject.GetComponent<Shadow>().enabled = true;
            }
        }
        public static void DisableShadow() {
            if(instance.textComponent.gameObject.GetComponents<Shadow>().Length != 0) {
                instance.textComponent.gameObject.GetComponent<Shadow>().enabled = false;
            }
        }
        public static void EnableOutline() {
            EnableOutline(Color.black);
        }
        public static void EnableOutline(Color color) {
            if(instance.textComponent.gameObject.GetComponents<Outline>().Length <= 0) {
                Outline outline = instance.textComponent.gameObject.AddComponent<Outline>();
                outline.effectColor = color;
            } else {
                instance.textComponent.gameObject.GetComponent<Outline>().enabled = true;
            }
        }
        public static void DisableOutline() {
            if(instance.textComponent.gameObject.GetComponents<Outline>().Length != 0) {
                instance.textComponent.gameObject.GetComponent<Outline>().enabled = false;
            }
        }
        public static void Skip() {
            if(instance.completedText) {
                DisableCanvas();
            } else {
                instance.StopAllCoroutines();
                UpdateText(instance.fullText);
                instance.completedText = true;
            }
        }


        public void WriteText(string text) {
            WriteText(text,false);
        }

        public void WriteText(string text, bool hasNext) {
            EnableCanvas();
            completedText = false;
            StopAllCoroutines();
            showNext = hasNext;
            StartCoroutine(WriteDelay(text,true));
            nextImage.gameObject.SetActive(hasNext);
        }

        public void WriteText(string text,Color textColor) {
            instance.textColor = textColor;
            WriteText(text,false);
        }
        public void WriteText(string text,bool hasNext,Color textColor) {
            instance.textColor = textColor;
            WriteText(text,hasNext);
        }

        public void WriteText(string text,string frameImage,Color textColor) {
            SetFrameImage(frameImage);
            WriteText(text,false,textColor);
        }
        public void WriteText(string text,bool hasNext,string frameImage, Color textColor) {
            SetFrameImage(frameImage);
            WriteText(text,hasNext,textColor);
        }

        private IEnumerator WriteDelay(string text,bool binary) {
            char[] newText;
            textComponent.color = textColor;
            if(binary) {
                newText = GenerateBinary(text.Length);
            } else {
                newText = new char[text.Length];
            }
            fullText = text;
            int count = 0;
            int i = 0;
            UpdateText(new string(newText));
            while (count < fullText.Length) {
                yield return new WaitForSeconds(speed);
                newText[i++] = fullText[count++];
                if(fullText[count-1]!=' ')
                    AudioManager.PlayUiEffect("WriteText");
                UpdateText(new string(newText));
            }
            instance.completedText = true;
        }

        public static void EnableCanvas() {
            instance.textCanvas.enabled = true;
        }

        public static void DisableCanvas() {
            instance.textCanvas.enabled = false;
            instance.completedText = false;
        }

        private char[] GenerateBinary(int lenght) {
            char[] binary = new char[lenght];
            for(int i = 0;i < lenght;i++) {
                binary[i] = Mathf.Abs(Random.Range(0,1.9f)).ToString()[0];
            }
            return binary;
        }

        public enum TextSpeed { slow,normal,fast }
    }
}
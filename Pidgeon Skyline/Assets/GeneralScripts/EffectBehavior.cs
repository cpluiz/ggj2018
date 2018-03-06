using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehavior : MonoBehaviour{

    public Sprite[] effectSprite;
    private SpriteRenderer _spriteRenderer;
    
    public void SetEffect(string effectName) {
        this.name = effectName;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (Sprite sprite in effectSprite) {
            if (sprite.name == effectName) {
                _spriteRenderer.sprite = sprite;
            }
        }
    }
}

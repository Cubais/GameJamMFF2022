using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rescale();
    }

    public void Rescale()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        var width = spriteRenderer.sprite.bounds.size.x;
        var height = spriteRenderer.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);
    }
}

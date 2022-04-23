using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType
{
    Desert
}

public class BackgroundManager : Singleton<BackgroundManager>
{
    [Header("Backgrounds")]
    [SerializeField] private Sprite desertBackground;

    [Header("Borders")]
    [SerializeField] BoxCollider2D leftBorder;
    [SerializeField] BoxCollider2D rightBorder;
    [SerializeField] BoxCollider2D bottomBorder;
    [SerializeField] BoxCollider2D topBorder;

    private SpriteRenderer spriteRenderer;
    private SpriteScaler spriteScaler;
        
    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteScaler = GetComponent<SpriteScaler>();
        ChangeBackground(BackgroundType.Desert);
    }

    public void ChangeBackground(BackgroundType type)
    {
        switch (type)
        {
            case BackgroundType.Desert:
                spriteRenderer.sprite = desertBackground;
                break;
            default:
                break;
        }

        spriteScaler.Rescale();
        SetupBorders();
    }

    private void SetupBorders()
    {
        var width = spriteRenderer.sprite.bounds.size.x;
        var height = spriteRenderer.sprite.bounds.size.y;

        topBorder.size = new Vector2(width, height / 4f);
        topBorder.transform.position = new Vector2(0f, height / 2f - topBorder.size.y);

        leftBorder.size = new Vector2(10f, height);
        leftBorder.transform.position = new Vector2(-width / 2f - 4f, 0f);

        bottomBorder.size = new Vector2(width, 10f);
        bottomBorder.transform.position = new Vector2(0f, -height / 2f - 4f);

        rightBorder.size = new Vector2(10f, height);
        rightBorder.transform.position = new Vector2(width / 2f + 4f, 0f);
    }
}

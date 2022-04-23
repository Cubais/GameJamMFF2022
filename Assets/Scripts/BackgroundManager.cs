using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType
{
    Desert
}

public class BackgroundManager : Singleton<BackgroundManager>
{
    public const float RESOLUTION_WIDTH = 19.2f;
    public const float RESOLUTION_HEIGHT = 10.8f;

    [Header("Desert Backgrounds")]
    [SerializeField] private List<Sprite> desertBackgrounds;

    [Header("Hangar Backgrounds")]
    [SerializeField] private List<Sprite> hangarBackgrounds;

    [Header("Lab Backgrounds")]
    [SerializeField] private List<Sprite> labBackgrounds;

    [Header("Desert Backgrounds")]
    [SerializeField] private List<SpriteRenderer> backgroundSprites;

    [Header("Borders")]
    [SerializeField] BoxCollider2D leftBorder;
    [SerializeField] BoxCollider2D rightBorder;
    [SerializeField] BoxCollider2D bottomBorder;
    [SerializeField] BoxCollider2D topBorder;
    
    private List<Sprite> backgroundMemory = new List<Sprite>();
    private BackgroundType currentBackgroundType;
        
    public void Init()
    {        
        ChangeBackground(BackgroundType.Desert);
        for (int i = 0; i < 3; i++)
        {
            var spriteXScale = backgroundSprites[i].transform.localScale.x;
            backgroundSprites[i].transform.position = new Vector2(i * RESOLUTION_WIDTH * spriteXScale, 0.0f);
            backgroundMemory.Add(backgroundSprites[i].sprite);
        }
    }

    private void Update()
    {
        MoveBorders();
        CheckBackgroundShift();
    }

    public void ChangeBackground(BackgroundType type)
    {
        switch (type)
        {
            case BackgroundType.Desert:
                for (int i = 0; i < 3; i++)                
                    backgroundSprites[i].sprite = desertBackgrounds[Random.Range(0, desertBackgrounds.Count)];                                
                break;
            default:
                break;
        }

        currentBackgroundType = type;
        Rescale();
        SetupBorders();        
    }

    private Sprite GetRandomBackgroundSprite(BackgroundType type)
    {
        switch (type)
        {
            case BackgroundType.Desert:
                return desertBackgrounds[Random.Range(0, desertBackgrounds.Count)];
            default:
                break;
        }

        return null;
    }

    private void Rescale()
    {
        foreach (var backgroundImg in backgroundSprites)
        {
            backgroundImg.GetComponent<SpriteScaler>().Rescale();
        }
    }

    private void CheckBackgroundShift()
    {
        var middlePos = backgroundSprites[1].transform.position.x;

        // Shift to right
        if (GameManager.instance.playerCharacter.transform.position.x > middlePos + 10)
        {
            var backgroundFirst = backgroundSprites[0];
            var index = Mathf.RoundToInt(backgroundSprites[2].transform.position.x / (RESOLUTION_WIDTH * backgroundSprites[2].transform.localScale.x)) + 1;
            if (index >= backgroundMemory.Count)
            {
                backgroundFirst.sprite = GetRandomBackgroundSprite(currentBackgroundType);
                backgroundMemory.Add(backgroundFirst.sprite);
                print("Adding new " + backgroundMemory.Count);
            }
            else
            {
                print("From memory " + index);
                backgroundFirst.sprite = backgroundMemory[index];
            }

            backgroundSprites[0] = backgroundSprites[1];
            backgroundSprites[1] = backgroundSprites[2];
            backgroundSprites[2] = backgroundFirst;
            backgroundSprites[2].transform.position = backgroundSprites[1].transform.position + new Vector3(backgroundSprites[1].transform.localScale.x * RESOLUTION_WIDTH, 0.0f, 0.0f);
        }
        // Shift left
        else if (GameManager.instance.playerCharacter.transform.position.x < middlePos - 10)
        {
            var backgroundLast = backgroundSprites[2];
            var index = Mathf.RoundToInt(backgroundSprites[0].transform.position.x / (RESOLUTION_WIDTH * backgroundSprites[0].transform.localScale.x)) - 1;
            backgroundLast.sprite = backgroundMemory[index];

            backgroundSprites[2] = backgroundSprites[1];
            backgroundSprites[1] = backgroundSprites[0];
            backgroundSprites[0] = backgroundLast;

            backgroundSprites[0].transform.position = backgroundSprites[1].transform.position - new Vector3(backgroundSprites[1].transform.localScale.x * RESOLUTION_WIDTH, 0.0f, 0.0f);
        }
    }

    private void MoveBorders()
    {
        var width = RESOLUTION_WIDTH;
        var height = RESOLUTION_HEIGHT;

        topBorder.size = new Vector2(width, height / 2f);
        topBorder.transform.position = new Vector2(GameManager.instance.playerCharacter.transform.position.x, 2 * topBorder.size.y / 4f);

        bottomBorder.size = new Vector2(width, 10f);
        bottomBorder.transform.position = new Vector2(GameManager.instance.playerCharacter.transform.position.x, -height / 2f - 4f);
    }

    private void SetupBorders()
    {
        var width = RESOLUTION_WIDTH * backgroundSprites[0].transform.localScale.x;
        var height = RESOLUTION_HEIGHT * backgroundSprites[0].transform.localScale.y;

        topBorder.size = new Vector2(width, height / 2f);
        topBorder.transform.position = new Vector2(0f, 2 * topBorder.size.y / 4f);

        leftBorder.size = new Vector2(width, height);
        leftBorder.transform.position = new Vector2(0f, 0f);

        bottomBorder.size = new Vector2(width, 10f);
        bottomBorder.transform.position = new Vector2(0f, -height / 2f - 4f);

        rightBorder.size = new Vector2(10f, height);
        rightBorder.transform.position = new Vector2(width / 2f + 4f, 0f);
    }
}

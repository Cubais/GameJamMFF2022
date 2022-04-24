using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType
{
    Desert,
    Hangar,
    Lab
}

public class BackgroundManager : Singleton<BackgroundManager>
{
    public const float RESOLUTION_WIDTH = 19.2f;
    public const float RESOLUTION_HEIGHT = 10.8f;

    [Header("Desert Backgrounds")]
    [SerializeField] private List<Sprite> desertBackgrounds;
    [SerializeField] private Sprite startLevelSprite;
    [SerializeField] private Sprite transitionDesertHangar;

    [Header("Hangar Backgrounds")]
    [SerializeField] private List<Sprite> hangarBackgrounds;
    [SerializeField] private Sprite firsthangar;
    [SerializeField] private Sprite transitionHangarLab;

    [Header("Lab Backgrounds")]
    [SerializeField] private List<Sprite> labBackgrounds;

    [Header("Desert Backgrounds")]
    [SerializeField] private List<SpriteRenderer> backgroundSprites;

    [Header("Borders")]
    [SerializeField] BoxCollider2D leftBorder;
    [SerializeField] BoxCollider2D rightBorder;
    [SerializeField] BoxCollider2D bottomBorder;
    [SerializeField] BoxCollider2D topBorder;

    public float screenWidth => RESOLUTION_WIDTH * backgroundSprites[0].transform.localScale.x;
    public BackgroundType CurrentLevel => currentBackgroundType;

    private List<Sprite> backgroundMemory = new List<Sprite>();
    private BackgroundType currentBackgroundType = BackgroundType.Desert;
        
    public void Init(LevelSettings levelSettings)
    {
        GenerateLevel(levelSettings);

        for (int i = 0; i < 3; i++)
        {
            var spriteXScale = backgroundSprites[i].transform.localScale.x;
            backgroundSprites[i].transform.position = new Vector2(i * RESOLUTION_WIDTH * spriteXScale, 0.0f);            
        }
    }

    private void Update()
    {
        MoveBorders();
        CheckBackgroundShift();
    }

    public void ChangebackgroundType(BackgroundType type)
    {
        currentBackgroundType = type;
    }

    private void GenerateLevel(LevelSettings levelSettings)
    {
        backgroundMemory.Add(startLevelSprite);
        for (int i = 0; i < levelSettings.DesertLevel.Count; i++)
        {
            backgroundMemory.Add(desertBackgrounds[Random.Range(0, desertBackgrounds.Count)]);
        }

        backgroundMemory.Add(transitionDesertHangar);
        backgroundMemory.Add(firsthangar);

        for (int i = 0; i < levelSettings.HangarLevel.Count - 1; i++)
        {
            backgroundMemory.Add(hangarBackgrounds[Random.Range(0, hangarBackgrounds.Count)]);
        }

        backgroundMemory.Add(transitionHangarLab);

        for (int i = 0; i < levelSettings.LabLevel.Count; i++)
        {
            backgroundMemory.Add(labBackgrounds[Random.Range(0, labBackgrounds.Count)]);
        }

        for (int i = 0; i < 3; i++)
        {
            backgroundSprites[i].sprite = backgroundMemory[i];
        }

        Rescale();
        SetupBorders(levelSettings);    
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
            index = (index < 0) ? 0 : index;
            backgroundLast.sprite = backgroundMemory[index];

            backgroundSprites[2] = backgroundSprites[1];
            backgroundSprites[1] = backgroundSprites[0];
            backgroundSprites[0] = backgroundLast;

            backgroundSprites[0].transform.position = backgroundSprites[1].transform.position - new Vector3(backgroundSprites[1].transform.localScale.x * RESOLUTION_WIDTH, 0.0f, 0.0f);
        }
    }

    public void MoveSideBorders(int leftIndex, int rightIndex)
    {
        leftBorder.edgeRadius = 0;
        leftBorder.transform.position = new Vector2(leftIndex * screenWidth, 0f);

        rightBorder.transform.position = new Vector2((rightIndex + 1.75f) * screenWidth, 0f);
    }

    private void MoveBorders()
    {
        var width = RESOLUTION_WIDTH;
        var height = RESOLUTION_HEIGHT;

        topBorder.size = new Vector2(width * 1000, height / 2f);
        topBorder.transform.position = new Vector2(GameManager.instance.playerCharacter.transform.position.x, 2 * topBorder.size.y / 4f);

        bottomBorder.size = new Vector2(width * 1000, 10f);
        bottomBorder.transform.position = new Vector2(GameManager.instance.playerCharacter.transform.position.x, -height / 2f - 4f);
    }

    private void SetupBorders(LevelSettings settings)
    {
        var width = RESOLUTION_WIDTH * backgroundSprites[0].transform.localScale.x;
        var height = RESOLUTION_HEIGHT * backgroundSprites[0].transform.localScale.y;

        topBorder.size = new Vector2(width, height / 2f);
        topBorder.transform.position = new Vector2(0f, 2 * topBorder.size.y / 4f);
                
        leftBorder.size = new Vector2(width, height);
        leftBorder.transform.position = new Vector2(-leftBorder.edgeRadius - 2, leftBorder.edgeRadius);

        bottomBorder.size = new Vector2(width, 10f);
        bottomBorder.transform.position = new Vector2(0f, -height / 2f - 4f);

        rightBorder.size = new Vector2(10f, height);
        rightBorder.transform.position = new Vector2(width * (settings.DesertLevel.Count + 1.75f), 0f);
    }
}

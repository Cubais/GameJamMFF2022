using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    [Header("NPCs desert")]
    [SerializeField] private GameObject desertMelee;
    [SerializeField] private GameObject desertRange;

    [Header("NPCs hangar")]
    [SerializeField] private GameObject hangarMelee;
    [SerializeField] private GameObject hangarRange;

    [Header("NPCs lab")]
    [SerializeField] private GameObject labMelee;
    [SerializeField] private GameObject labRange;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;

    public int LevelNPCCount => spawnedNPCs.Count;

    private HashSet<NPCControler> spawnedNPCs = new HashSet<NPCControler>();
    private List<NPCControler> menuNPCs = new List<NPCControler>();

    public void StartGame(LevelSettings settings)
    {
        foreach (var npc in menuNPCs)
        {
            npc.GetComponent<MenuNPC>().enabled = false;
            npc.enabled = true;
        }

        GenerateNpcs(settings);
    }
    public void SpawnMenuNPCs()
    {
        for (int j = 0; j < 2; j++)
        {
            var npc = Instantiate((j == 0) ? desertMelee : desertRange, GetRandomPosOnScreen(0, j == 0), Quaternion.identity).GetComponent<NPCControler>();
            npc.enabled = false;
            npc.gameObject.AddComponent<MenuNPC>();
            menuNPCs.Add(npc);
            spawnedNPCs.Add(npc);
        }
    }
    public void GenerateNpcs(LevelSettings levelSettings)
    {
        var screenIndex = 1;
        for (int i = 0; i < levelSettings.DesertLevel.Count; i++)
        {
            for (int j = 0; j < levelSettings.DesertLevel[i].NPCMeleeCount; j++)
            {
                var npc = Instantiate(desertMelee, GetRandomPosOnScreen(screenIndex, true), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
            }

            for (int j = 0; j < levelSettings.DesertLevel[i].NPCRangeCount; j++)
            {   
                var npc = Instantiate(desertRange, GetRandomPosOnScreen(screenIndex, false), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
            }

            screenIndex++;
        }

        for (int i = 0; i < levelSettings.DesertLevel.Count; i++)
        {

            screenIndex++;
        }

        for (int i = 0; i < levelSettings.DesertLevel.Count; i++)
        {

            screenIndex++;
        }
    }

    private Vector2 GetRandomPosOnScreen(int screenIndex, bool melee)
    {
        Vector2 position;
        if (melee)
        {
            position.x = Random.Range(screenIndex * BackgroundManager.instance.screenWidth,
                                        screenIndex * BackgroundManager.instance.screenWidth + BackgroundManager.instance.screenWidth / 4f);

            position.y = Random.Range(0f, -3.8f);
        }
        else
        {
            position.x = Random.Range(screenIndex * BackgroundManager.instance.screenWidth + BackgroundManager.instance.screenWidth / 4f,
                                       screenIndex * BackgroundManager.instance.screenWidth + BackgroundManager.instance.screenWidth / 2f);

            position.y = Random.Range(0f, -3.8f);
        }

        return position;
    }

    

    public void NPCDeath(NPCControler npc)
    {
        spawnedNPCs.Remove(npc);
    }
}

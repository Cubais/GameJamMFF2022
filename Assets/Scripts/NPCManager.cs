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

    private HashSet<NPCControler> spawnedNPCs = new HashSet<NPCControler>();
    private List<NPCControler> menuNPCs = new List<NPCControler>();
    private Dictionary<BackgroundType, int> npcCounts = new Dictionary<BackgroundType, int>();
    private BackgroundType currentLevel = BackgroundType.Desert;

    public void StartGame(LevelSettings settings)
    {
        foreach (var npc in menuNPCs)
        {
            npc.GetComponent<MenuNPC>().enabled = false;
            npc.enabled = true;
        }

        GenerateNpcs(settings);
    }

    public void SwitchLevel(BackgroundType newLevel)
    {
        currentLevel = newLevel;
    }
    public bool AllNPCKilled(BackgroundType level)
    {
        return npcCounts[level] == 0;
    }
    public void SpawnMenuNPCs()
    {
        menuNPCs.Clear();
        spawnedNPCs.Clear();
        if (!npcCounts.ContainsKey(BackgroundType.Desert))
            npcCounts.Add(BackgroundType.Desert, 0);
        else
            npcCounts[BackgroundType.Desert] = 0;

        for (int j = 0; j < 2; j++)
        {
            var npc = Instantiate((j == 0) ? desertMelee : desertRange, GetRandomPosOnScreen(0, j == 0), Quaternion.identity).GetComponent<NPCControler>();
            npc.enabled = false;
            npc.gameObject.AddComponent<MenuNPC>();
            menuNPCs.Add(npc);
            npcCounts[BackgroundType.Desert]++;
            spawnedNPCs.Add(npc);
        }
    }
    public void GenerateNpcs(LevelSettings levelSettings)
    {
        var screenIndex = 1;
        if (!npcCounts.ContainsKey(BackgroundType.Hangar))
            npcCounts.Add(BackgroundType.Hangar, 0);
        else
            npcCounts[BackgroundType.Hangar] = 0;

        if (!npcCounts.ContainsKey(BackgroundType.Lab))
            npcCounts.Add(BackgroundType.Lab, 0);
        else
            npcCounts[BackgroundType.Lab] = 0;

        for (int i = 0; i < levelSettings.DesertLevel.Count; i++)
        {
            for (int j = 0; j < levelSettings.DesertLevel[i].NPCMeleeCount; j++)
            {
                var npc = Instantiate(desertMelee, GetRandomPosOnScreen(screenIndex, true), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
                npcCounts[BackgroundType.Desert]++;
            }

            for (int j = 0; j < levelSettings.DesertLevel[i].NPCRangeCount; j++)
            {   
                var npc = Instantiate(desertRange, GetRandomPosOnScreen(screenIndex, false), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
                npcCounts[BackgroundType.Desert]++;
            }

            screenIndex++;
        }

        screenIndex++;

        for (int i = 0; i < levelSettings.HangarLevel.Count; i++)
        {
            for (int j = 0; j < levelSettings.HangarLevel[i].NPCMeleeCount; j++)
            {
                var npc = Instantiate(hangarMelee, GetRandomPosOnScreen(screenIndex, true), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
                npcCounts[BackgroundType.Hangar]++;
            }

            for (int j = 0; j < levelSettings.HangarLevel[i].NPCRangeCount; j++)
            {
                var npc = Instantiate(hangarRange, GetRandomPosOnScreen(screenIndex, false), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
                npcCounts[BackgroundType.Hangar]++;
            }

            screenIndex++;
        }

        screenIndex++;

        for (int i = 0; i < levelSettings.LabLevel.Count - 1; i++)
        {
            for (int j = 0; j < levelSettings.LabLevel[i].NPCMeleeCount; j++)
            {
                var npc = Instantiate(labMelee, GetRandomPosOnScreen(screenIndex, true), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
                npcCounts[BackgroundType.Lab]++;
            }

            for (int j = 0; j < levelSettings.LabLevel[i].NPCRangeCount; j++)
            {
                var npc = Instantiate(labRange, GetRandomPosOnScreen(screenIndex, false), Quaternion.identity);
                spawnedNPCs.Add(npc.GetComponent<NPCControler>());
                npcCounts[BackgroundType.Lab]++;
            }

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
        npcCounts[currentLevel]--;
        spawnedNPCs.Remove(npc);
    }

    internal void DeleteAll()
    {
        foreach (var item in spawnedNPCs)
        {
            Destroy(item.gameObject);
        }
    }
}

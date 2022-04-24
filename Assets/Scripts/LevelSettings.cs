using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Level/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    [Header("Level settings")]
    public List<ScreenNPCInfo> DesertLevel;
    public List<ScreenNPCInfo> HangarLevel;
    public List<ScreenNPCInfo> LabLevel;
}

[System.Serializable]
public struct ScreenNPCInfo
{
    public int NPCMeleeCount;
    public int NPCRangeCount;
}

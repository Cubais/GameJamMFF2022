using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Level/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    public int DesertCount = 3;
    public int HangarCount = 3;
    public int LabCount = 3;
}

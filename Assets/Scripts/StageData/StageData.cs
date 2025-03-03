using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public int stageNumber;
    public GameObject patrolGroupPrefab;
    public GameObject npcSpawnPointPrefab;
    public GameObject playerSpawnPointPrefab;
}



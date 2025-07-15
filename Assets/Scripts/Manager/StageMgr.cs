using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    public List<StageData> stageDataList;
    private static int currentStage;
    public int stageCount;

    public static int CurrentStage { get => currentStage; private set => currentStage = value; }
    public int StageCount => stageDataList.Count;

    private void Start()
    {
        stageCount = currentStage;
        currentStage = 0;
    }
    public void ResetStage()
    {
        currentStage = 0;
    }

    public void NextStage()
    {
        currentStage++;
        stageCount = currentStage;
    }

    public StageData GetCurrentStageData()
    {
        if (CurrentStage >= 0 && CurrentStage < stageDataList.Count)
        {
            return stageDataList[CurrentStage];
        }        
        return null;
    }
}

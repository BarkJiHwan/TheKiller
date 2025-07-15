using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [SerializeField] private ScoreMgr _scoreManager;
    [SerializeField] private TimerMgr _timerManager;
    [SerializeField] private StageMgr _stageManager;
    [SerializeField] private NPCManager _npcManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PlayerManager _playerManager;
        
    private GameObject currentPatrolGroupInstance; //현재 패트롤 그룹 인스턴스
    private GameObject currentNpcSpawnPointInstance; //현재 NPC 스폰 포인트 인스턴스
    private GameObject currentPlayerSpawnPointInstance; //현재 플레이어 스폰 포인트 인스턴스

    private bool isNextRoundTrigger;
    private bool isNextRoundProcessing;
    private bool isGameOver;

    private void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        isNextRoundProcessing = false;        
        _scoreManager.ResetScore();
        _stageManager.ResetStage();
        _npcManager.defeatedNpcs.Clear();
        Cursor.visible = false; //마우스 다시 비활성화
        Cursor.lockState = CursorLockMode.Locked;
        GameStart();
    }

    private void Update()
    {
        _uiManager.UpdateRemainingEnemiesUI(_npcManager.npcs.Count);
        if (_npcManager.CheckAllNPCsDead() && !isNextRoundTrigger && !isGameOver)
        {
            isNextRoundTrigger = true;
            NextRound();
        }
    }
        
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        _scoreManager.SaveScore();
        _uiManager.ShowEndGameUI(_scoreManager.Score, _npcManager.defeatedNpcs.Count, _scoreManager.LoadScores());
    }

    private void NextRound()
    {
        if (isNextRoundProcessing)
        {
            return;
        }

        isNextRoundProcessing = true;
                
        if (StageMgr.CurrentStage >= _stageManager.StageCount)
        {
            GameOver();
        }
        else
        {
            isNextRoundTrigger = false;            
            UpdateRound();
        }
    }

    public void GameStart()
    {        
        _npcManager.defeatedNpcs.Clear();
        _npcManager.npcs.Clear();

        _scoreManager.ResetScore();
        _stageManager.ResetStage();
        _timerManager.StartTimer(120f);

        InitializeStage();

        _uiManager.UpdateTimerUI(_timerManager.GetTime());
        _uiManager.UpdateScoreUI(_scoreManager.Score);
        _uiManager.UpdateStageUI(StageMgr.CurrentStage);
        _uiManager.ShowStageStartMessage(StageMgr.CurrentStage);
        var roundTexts = FindObjectsOfType<RoundText>();
        foreach (var rt in roundTexts)
            rt.UpdateRoundText(StageMgr.CurrentStage);
    }

    private void UpdateRound()
    {
        _stageManager.NextStage();
        _timerManager.StartTimer(120f);
        _npcManager.defeatedNpcs.Clear();
        _npcManager.npcs.Clear();
        ClearCurrentInstances();

        InitializeStage();

        _uiManager.UpdateTimerUI(_timerManager.GetTime());
        _uiManager.UpdateStageUI(StageMgr.CurrentStage);
        _uiManager.ShowStageStartMessage(StageMgr.CurrentStage);

        var roundTexts = FindObjectsOfType<RoundText>();
        foreach (var rt in roundTexts)
            rt.UpdateRoundText(StageMgr.CurrentStage);

        isNextRoundProcessing = false;
    }

    private void InitializeStage()
    {
        var stageData = _stageManager.GetCurrentStageData();
        if (stageData == null) return;

        // 패트롤 그룹 초기화
        if (stageData.patrolGroupPrefab != null)
        {
            currentPatrolGroupInstance = Instantiate(stageData.patrolGroupPrefab);
            currentPatrolGroupInstance.name = $"PatrolGroup_Stage_{StageMgr.CurrentStage + 1}";
        }

        //플레이어 스폰 포인트 초기화
        if (stageData.playerSpawnPointPrefab != null)
        {
            currentPlayerSpawnPointInstance = Instantiate(stageData.playerSpawnPointPrefab);
            _playerManager.MovePlayerToSpawnPoint(StageMgr.CurrentStage);
        }

        // NPC 초기화
        if (currentPatrolGroupInstance != null)
        {
            PatrolGroup patrolGroup = currentPatrolGroupInstance.GetComponent<PatrolGroup>();
            if (patrolGroup != null)
            {
                _npcManager.InitializeNPCs(patrolGroup);
            }
        }
    }
    private void ClearCurrentInstances()
    {
        // 현재 패트롤 그룹 인스턴스를 제거
        if (currentPatrolGroupInstance != null)
        {
            Destroy(currentPatrolGroupInstance);
            currentPatrolGroupInstance = null;
        }

        // 현재 NPC 스폰 포인트 인스턴스를 제거
        if (currentNpcSpawnPointInstance != null)
        {
            Destroy(currentNpcSpawnPointInstance);
            currentNpcSpawnPointInstance = null;
        }

        // 현재 플레이어 스폰 포인트 인스턴스를 제거
        if (currentPlayerSpawnPointInstance != null)
        {
            Destroy(currentPlayerSpawnPointInstance);
            currentPlayerSpawnPointInstance = null;
        }
    }
}

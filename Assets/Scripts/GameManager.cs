using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;
    private float timer;
    private bool isNextRoundTriggered;
    private bool isGameOver;
    private bool isRunningTime;
    private int stage;

    public List<NPCActions> npcs = new List<NPCActions>();//게임 시작 시 생성 될 npc의 수 죽은적 리무브
    public List<NPCActions> defeatedNpcs = new List<NPCActions>();//게임 시작 후 처치한 적의 수 누적
    public PatrolGroup[] patrolGroups;
    //public PatrolGroup patrolGroup2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        isGameOver = false;
        StartNewRound();
    }

    private void Update()
    {
        // 타이머 업데이트
        if (isRunningTime)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                isRunningTime = false;
                OnTimerEnd();
            }

            UIManager.Instance.UpdateTimerUI(timer);
            UIManager.Instance.UpdateRemainingEnemiesUI(npcs.Count);

            // 모든 NPC가 사망했는지 확인하고 NextRound가 이미 호출되지 않았는지 확인
            if (CheckAllNPCsDead() && !isNextRoundTriggered && isGameOver)
            {
                isGameOver = false;
                isNextRoundTriggered = true; // NextRound가 이미 호출되었음을 표시
                NextRound();
            }
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UIManager.Instance.UpdateScoreUI(score);
    }

    private void OnTimerEnd()
    {
        isGameOver = true;
        // 타임 슬레이트로 게임 멈춤
        Time.timeScale = 0f;

        // 점수를 JSON 파일로 저장
        SaveScore();

        // UI 활성화
        UIManager.Instance.ShowEndGameUI(score, defeatedNpcs.Count, GetTopScores());
    }

    private void SaveScore()
    {
        List<ScoreData> topScores = LoadScoresFromJson();
        topScores.Add(new ScoreData { score = score });

        // 상위 10위 점수만 유지
        topScores = topScores.OrderByDescending(s => s.score).Take(10).ToList();

        // JSON 데이터 생성
        ScoreDataList scoreDataList = new ScoreDataList { scores = topScores };

        // JSON 파일 경로 설정
        string filePath = Path.Combine(Application.persistentDataPath, "score.json");

        // 디렉토리 존재 여부 확인 및 생성
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // JSON 데이터 직렬화 및 파일로 저장
        string json = JsonUtility.ToJson(scoreDataList);
        File.WriteAllText(filePath, json);
    }

    private List<ScoreData> GetTopScores()
    {
        List<ScoreData> topScores = LoadScoresFromJson();
        topScores.Sort((a, b) => b.score.CompareTo(a.score));
        return topScores.Take(10).ToList();
    }

    private List<ScoreData> LoadScoresFromJson()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "score.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<ScoreDataList>(json).scores;
        }
        return new List<ScoreData>();
    }

    public void RemoveNPC(NPCActions npc)
    {
        if (npcs.Contains(npc))
        {
            defeatedNpcs.Add(npc);
            npcs.Remove(npc);
        }
    }

    private bool CheckAllNPCsDead()
    {
        foreach (NPCActions npc in npcs)
        {
            if (!npc.isDead)
            {
                return false;
            }
        }
        return true;
    }

    private void NextRound()
    {
        // 다음 라운드로 넘어가는 로직
        stage++;
        if (stage > patrolGroups.Length)
        {
            // 최종 스테이지를 완료했을 때 게임 종료
            OnGameEnd();
        }
        else
        {
            StartNewRound();
        }
    }

    public void StartNewRound()
    {
        isGameOver = false;
        // 게임 상태 초기화
        timer = 120f; // 타이머를 다시 2분(120초)으로 설정
        isRunningTime = true;
        isNextRoundTriggered = false; // 새로운 라운드를 시작할 때 초기화
        defeatedNpcs.Clear(); // 처치한 적 리스트 초기화

        Cursor.visible = false; //마우스 다시 비활성화
        Cursor.lockState = CursorLockMode.None;

        // NPC 상태 초기화
        foreach (NPCActions npc in npcs)
        {
            NPCPool.Instance.ReturnObject(npc.gameObject);
        }
        npcs.Clear(); // NPC 리스트 초기화

        // 새로운 NPC 생성
        PatrolGroup patrolGroup = GetPatrolGroup(stage);
        if (patrolGroup != null)
        {
            NPCManager.Instance.InitializeNPCs(patrolGroup);
        }
        PlayerManager.Instance.MovePlayerToSpawnPoint(0);

        UIManager.Instance.UpdateTimerUI(timer);
        UIManager.Instance.UpdateScoreUI(score);
        UIManager.Instance.UpdateStageUI(stage);
    }

    private void OnGameEnd()
    {
        // 게임 종료 시 처리 로직
        OnTimerEnd(); // OnTimerEnd 호출하여 게임 종료 시 UI 활성화 및 점수 저장
    }

    public int GetCurrentStage()
    {
        return stage;
    }

    public PatrolGroup GetPatrolGroup(int stage)
    {
        if (stage >= 1 && stage <= patrolGroups.Length)
        {
            return patrolGroups[stage - 1];
        }
        return null;
    }

    [System.Serializable]
    public class ScoreData
    {
        public int score;
    }

    [System.Serializable]
    private class ScoreDataList
    {
        public List<ScoreData> scores;
    }
}
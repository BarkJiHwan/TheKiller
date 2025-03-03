using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI remainingEnemiesText;

    public GameObject endGameUI;
    public TextMeshProUGUI[] topScoreTexts;
    public TextMeshProUGUI warningText;
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
        endGameUI.SetActive(false);
        warningText.gameObject.SetActive(false);
    }
    public void ShowStageStartMessage(int stage)
    {
        string message = $"Stage {stage} Start! \nEliminate as many enemies as possible!";
        ShowWarningMessage(message);
    }
    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }
    public void UpdateTimerUI(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }
    public void UpdateStageUI(int stage)
    {
        stageText.text = "Stage: " + stage;
    }

    public void UpdateRemainingEnemiesUI(int remainingEnemies)
    {
        remainingEnemiesText.text = "Enemies: " + remainingEnemies;
    }

    public void ShowEndGameUI(int finalScore, int defeatedEnemies, List<GameManager.ScoreData> topScores)
    {
        // 최종 점수 및 처치한 적 수 업데이트
        scoreText.text = "Final Score: " + finalScore;
        remainingEnemiesText.text = "Enemies Defeated: " + defeatedEnemies;

        // 1~10위까지의 점수 업데이트
        for (int i = 0; i < topScores.Count && i < topScoreTexts.Length; i++)
        {
            topScoreTexts[i].text = (i + 1) + ". " + topScores[i].score;
        }
        // 마우스 복구
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // EndGame UI 활성화
        endGameUI.SetActive(true);
    }

    public void HideEndGameUI()
    {
        // EndGame UI 비활성화 및 정보 초기화
        endGameUI.SetActive(false);

        scoreText.text = "";
        remainingEnemiesText.text = "";
    }
    public void ShowWarningMessage(string message)
    {
        if (warningText != null)
        {
            StartCoroutine(ShowWarning(message));
        }
    }

    private IEnumerator ShowWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true); // 경고 메시지 활성화

        Color originalColor = warningText.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float duration = 3f; // 깜박이는 시간

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.PingPong(t * 2f, 1f); // 알파값을 0에서 1 사이로 깜박이게 조절
            warningText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        warningText.color = originalColor; // 원래 색상으로 복구
        warningText.gameObject.SetActive(false); // 경고 메시지 비활성화
    }

    public void OnRestartButtonClicked()
    {
        // 타임 슬레이트 복구
        Time.timeScale = 1f;

        // 게임 재시작 로직 (게임 매니저 초기화 등)
        GameManager.Instance.StartNewRound();


        // EndGame UI 비활성화 및 초기화
        HideEndGameUI();
    }

    public void OnExitButtonClicked()
    {
        // 게임 종료
        Application.Quit();
    }
}
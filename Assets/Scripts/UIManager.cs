using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI remainingEnemiesText;

    public GameObject endGameUI;
    public TextMeshProUGUI[] topScoreTexts;
    public TextMeshProUGUI warningText;

    [SerializeField] private Coroutine _showWarningCor;
    
    private void Start()
    {
        ResetUI();
        endGameUI.SetActive(false);        
    }
    public void ResetUI()
    {
        scoreText.text = "Score: 0";
        timerText.text = "Time: 00:00";
        stageText.text = "Stage: 1";        
        remainingEnemiesText.text = "Enemies: 0";
        foreach (var t in topScoreTexts)
        {
            t.text = "";
        }
    }
    public void ShowStageStartMessage(int stage)
    {
        string message = $"Stage {stage + 1} Start! \nEliminate as many enemies as possible!";
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
        stageText.text = $"Stage: { stage + 1}";
    }

    public void UpdateRemainingEnemiesUI(int remainingEnemies)
    {
        remainingEnemiesText.text = "Enemies: " + remainingEnemies;
    }

    public void ShowEndGameUI(int finalScore, int defeatedEnemies, List<ScoreMgr.ScoreData> topScores)
    {
        // 최종 점수 및 처치한 적 수 업데이트
        scoreText.text = "Final Score: " + finalScore;
        remainingEnemiesText.text = "Enemies Defeated: " + defeatedEnemies;

        // 1~10위까지의 점수 업데이트
        for (int i = 0; i < topScores.Count && i < topScoreTexts.Length; i++)
        {
            if (topScores[i].score <= 0)
            {
                topScoreTexts[i].text = (i + 1) + ". ";
            }
            else
            {
                topScoreTexts[i].text = (i + 1) + ". " + topScores[i].score;
            }
        }
        // 마우스 복구
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // EndGame UI 활성화
        endGameUI.SetActive(true);
    }
    public void ShowEndGameUI(int finalScore, int defeatedEnemies, List<GameManager.ScoreData> topScores)
    {
        // 최종 점수 및 처치한 적 수 업데이트
        scoreText.text = "Final Score: " + finalScore;
        remainingEnemiesText.text = "Enemies Defeated: " + defeatedEnemies;

        // 1~10위까지의 점수 업데이트
        for (int i = 0; i < topScores.Count && i < topScoreTexts.Length; i++)
        {
            if (topScores[i].score <= 0)
            {
                topScoreTexts[i].text = (i + 1) + ". ";
            }
            else
            {
                topScoreTexts[i].text = (i + 1) + ". " + topScores[i].score;
            }
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
            _showWarningCor = StartCoroutine(ShowWarning(message));
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
        // 다시 시작
        SceneManager.LoadSceneAsync(1);
        //StartCoroutine(RestartCoroutine());
    }
    private IEnumerator RestartCoroutine()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(1);
        asyncOp.allowSceneActivation = false;

        // 진행률 표시
        while (asyncOp.progress < 0.9f)
        {            
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        asyncOp.allowSceneActivation = true;
    }
    public void OnExitButtonClicked()
    {
        // 게임 종료
        SceneManager.LoadScene(0);
    }    
}
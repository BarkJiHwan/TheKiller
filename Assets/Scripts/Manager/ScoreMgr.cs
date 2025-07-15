using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScoreMgr : MonoBehaviour
{
    private int score;
    private string filePath;
    [SerializeField] private UIManager _uiManager;
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "score.json");
    }

    public int Score => score;

    public void AddScore(int points)
    {
        score += points;
        _uiManager.UpdateScoreUI(score);
    }

    public void ResetScore()
    {
        score = 0;
        _uiManager.UpdateScoreUI(score);
    }

    public void SaveScore()
    {
        List<ScoreData> topScores = LoadScores();
        topScores.Add(new ScoreData { score = score });
        topScores = topScores.OrderByDescending(s => s.score).Take(10).ToList();

        ScoreDataList scoreDataList = new ScoreDataList { scores = topScores };
        string json = JsonUtility.ToJson(scoreDataList);
        File.WriteAllText(filePath, json);
    }

    public List<ScoreData> LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<ScoreDataList>(json).scores;
        }
        return new List<ScoreData>();
    }

    [System.Serializable]
    public class ScoreData
    {
        public int score;
    }

    [System.Serializable]
    public class ScoreDataList
    {
        public List<ScoreData> scores;
    }
}
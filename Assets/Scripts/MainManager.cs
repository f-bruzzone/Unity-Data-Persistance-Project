using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;

    private BestScore _bestScore;
    private string _name;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{_name} Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (CheckScore(m_Points)) 
            SaveScore();
    }

    private void SaveScore()
    {
        BestScore bestScore = new();
        bestScore.Name = _name;
        bestScore.Score = m_Points;

        string json = JsonUtility.ToJson(bestScore);
        File.WriteAllText("D:\\Unity\\Create_With_Code_Projects\\Unity-Data-Persistance-Project" + "/savefile.json", json);
    }

    class BestScore
    {
        public string Name;
        public int Score;
    }

    private void LoadScore()
    {
        _name = ScreenManager.Instance.Name;
        ScoreText.text = $"{_name} Score: {m_Points}";

        string path = "D:\\Unity\\Create_With_Code_Projects\\Unity-Data-Persistance-Project" + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _bestScore = JsonUtility.FromJson<BestScore>(json);

            BestScoreText.text = $"Best Score : {_bestScore.Name} : {_bestScore.Score}";
        }
        else
        {
            _name = ScreenManager.Instance.Name;
            BestScoreText.text = $"Best Score: {_name} : {m_Points}";
        }
    }

    private bool CheckScore(int score)
    {
        return _bestScore.Score < score;
    }
}

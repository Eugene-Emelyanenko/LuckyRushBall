using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelData
{
    public int levelNumber;
    public bool isUnlocked;
    public bool isCompleted;
    public float obstacleSpeed;
    public int scoreToComplete;

    public LevelData(int levelNumber, bool isUnlocked, bool isCompleted, float obstacleSpeed, int scoreToComplete)
    {
        this.levelNumber = levelNumber;
        this.isUnlocked = isUnlocked;
        this.isCompleted = isCompleted;
        this.obstacleSpeed = obstacleSpeed;
        this.scoreToComplete = scoreToComplete;
    }
}

public static class LevelDataManager
{
    private const string LevelDataKey = "LevelData";

    public static void SaveLevelData(List<LevelData> levelDataList)
    {
        string json = JsonUtility.ToJson(new LevelDataListWrapper { levels = levelDataList });
        PlayerPrefs.SetString(LevelDataKey, json);
        PlayerPrefs.Save();
    }

    public static List<LevelData> LoadLevelData()
    {
        if (PlayerPrefs.HasKey(LevelDataKey))
        {
            string json = PlayerPrefs.GetString(LevelDataKey);
            LevelDataListWrapper wrapper = JsonUtility.FromJson<LevelDataListWrapper>(json);
            return wrapper.levels;
        }
        return new List<LevelData>();
    }
}

[System.Serializable]
public class LevelDataListWrapper
{
    public List<LevelData> levels;
}
public class Levels : MonoBehaviour
{
    [SerializeField] private GameObject levelsMenu;

    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private Transform levelsContainer;

    public int levelsCount = 15;
    public float speedIncrement = 0.15f;
    public int scoreIncrement = 200;

    private List<LevelData> levelDataList = new List<LevelData>();

    private void Start()
    {
        EnableMenu(false);  
    }

    public void EnableMenu(bool isOpen)
    {
        levelsMenu.SetActive(isOpen);

        levelDataList = LevelDataManager.LoadLevelData();
        if (levelDataList.Count == 0)
            CreateDefaultLevelData();

        DisplayLevels();
    }

    private void CreateDefaultLevelData()
    {
        Debug.Log("Generate Default Data");

        float initialObstacleSpeed = 1.0f;
        int initialScoreToComplete = 500;

        for (int i = 1; i <= levelsCount; i++)
        {
            float currentObstacleSpeed = initialObstacleSpeed + (i - 1) * speedIncrement;
            int currentScoreToComplete = initialScoreToComplete + (i - 1) * scoreIncrement;
            levelDataList.Add(new LevelData(i, i == 1, false, currentObstacleSpeed, currentScoreToComplete));
        }

        LevelDataManager.SaveLevelData(levelDataList);
    }

    private void DisplayLevels()
    {
        foreach (Transform level in levelsContainer)
        {
            Destroy(level.gameObject);
        }

        foreach (LevelData data in levelDataList)
        {
            GameObject levelObject = Instantiate(levelPrefab, levelsContainer);
            Level level = levelObject.GetComponent<Level>();
            level.SetUp(data);
            level.button.onClick.RemoveAllListeners();
            level.button.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("SelectedLevel", data.levelNumber);
                SceneManager.LoadScene("Game");
            });
        }
    }
}

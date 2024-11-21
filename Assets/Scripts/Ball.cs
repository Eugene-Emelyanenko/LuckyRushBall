using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    public float gravity = 1.5f;

    public float forcePower = 1f;

    public float yScorePosForLevels = 480;

    public int scoreMultiplier = 1;
    public float scoreInterval = 0.25f;
    public float defaultObstacleSpeed = 3f;

    public float easyGravity = 1f;
    public float mediumGravity = 1.5f;
    public float hardGravity = 2f;

    public float easySpeed = 1f;
    public float mediumSpeed = 2f;
    public float hardSpeed = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int score = 0;
    private bool canInput = true;
    private bool isGameOver = false;
    private int bestScore = 0;
    private int selectedLevel;
    private List<LevelData> levelDataList = new List<LevelData>();
    private LevelData levelData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RectTransform scoreRectTransform = scoreText.gameObject.GetComponent<RectTransform>();

        levelDataList = LevelDataManager.LoadLevelData();

        rb.gravityScale = gravity;
        spriteRenderer.sprite = Resources.Load<Sprite>($"{0}");
        scoreRectTransform.anchoredPosition = Vector2.zero;

        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);

        if(selectedLevel == -1) //Default Game
        {
            obstacleSpawner.SetObstacleSpeed(defaultObstacleSpeed);
        }
        else if(selectedLevel == 0) //Super Game
        {
            int selectedSpeed = PlayerPrefs.GetInt("SelectedSpeed", 1);
            float speed = selectedSpeed == 1 ? easySpeed : selectedSpeed == 2 ? mediumSpeed : hardSpeed;
            obstacleSpawner.SetObstacleSpeed(speed);

            int selectedGravity = PlayerPrefs.GetInt("SelectedGravity", 1);
            float gravity = selectedGravity == 1 ? easyGravity : selectedGravity == 2 ? mediumGravity : hardGravity;
            rb.gravityScale = gravity;

            int selectedBall = PlayerPrefs.GetInt("SelectedBall", 1);
            spriteRenderer.sprite = Resources.Load<Sprite>($"{selectedBall}");
        }
        else //Level
        {
            scoreRectTransform.anchoredPosition = new Vector2(scoreRectTransform.anchoredPosition.x, yScorePosForLevels);

            levelData = levelDataList.Find(level => level.levelNumber == selectedLevel);

            obstacleSpawner.SetObstacleSpeed(levelData.obstacleSpeed);
            Debug.Log("Obstacle speed: " + levelData.obstacleSpeed);
            Debug.Log("Score to complete: " + levelData.scoreToComplete);
        }

        gameOverMenu.SetActive(false);
        score = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        StartCoroutine(Score());
    }

    IEnumerator Score()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(scoreInterval);
            score += scoreMultiplier;
            if(selectedLevel == 0 || selectedLevel == -1)
                scoreText.text = score.ToString();
            else
                scoreText.text = $"{score.ToString()}/{levelData.scoreToComplete}";
        }
    }

    private void Update()
    {
        if(!isGameOver)
        {
            if (Input.GetMouseButtonDown(0) && canInput)
            {
                Jump();
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        obstacleSpawner.canSpawn = false;
        canInput = false;
        gameOverMenu.SetActive(true);
        SoundManager.Instance.PlayClip(SoundManager.Instance.gameOverSound);
        if (PlayerPrefs.GetInt("Vibrate", 1) == 1)
            Handheld.Vibrate();
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        if (levelData != null && score >= levelData.scoreToComplete)
        {
            levelData.isCompleted = true;
            Debug.Log($"Level_{levelData.levelNumber} is completed");

            int nextLevelNumber = selectedLevel + 1;
            LevelData nextLevelData = levelDataList.Find(level => level.levelNumber == nextLevelNumber);
            if (nextLevelData != null)
            {
                nextLevelData.isUnlocked = true;
                Debug.Log($"Level_{nextLevelData.levelNumber} is unclocked");
            }

            LevelDataManager.SaveLevelData(levelDataList);
        }

        finalScoreText.text = $"Final Score: {score + 1}";
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * forcePower, ForceMode2D.Impulse);
    }
}

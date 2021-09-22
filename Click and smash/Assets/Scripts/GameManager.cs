using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _targets;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _pauseScreen;

    public GameObject titleScreen;

    private int _score;
    private float _spawnRate = 1.0f;

    public int lives;
    public bool isGameActive;
    public bool gameIsPaused;

   private void Update()
    {
        LivesTracker();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(_spawnRate);
            int index = Random.Range(0, _targets.Count);
            Instantiate(_targets[index]);
            
        }
    }

    private void PauseGame()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            _pauseScreen.SetActive(true);
        }
        else 
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            _pauseScreen.SetActive(false);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        _scoreText.text = "Score: " + _score;
    }

    public void LivesTracker()
    {
        _livesText.text = "Lives: " + lives;
    }

    public void GameOver()
    {
        _restartButton.gameObject.SetActive(true);
        _gameOverText.gameObject.SetActive(true);
        isGameActive = false;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        _score = 0;
        lives = 3;
        _spawnRate /= difficulty;
        UpdateScore(0);
        LivesTracker();
    }
}

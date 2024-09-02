using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject Snake;
    [SerializeField] private GameObject Slime;
    [SerializeField] private GameObject Snail;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text LivesText;
    [SerializeField] private TMP_Text LoseText;
    [SerializeField] private AudioSource TakeDamage;
    [SerializeField] private GameObject LoseUI;
    private float offset = 1.28f;
    private bool gameOngoing = true;
    private GameObject[] enemyList = new GameObject[3];
    private float timeBetweenSpawn = 1f;
    private int score = 0;
    private int lives = 3;
    private PlayerController pController;

    #endregion

    /// <summary>
    /// The GameManager first puts the three enemies that can spawn into an array. It'll then start the coroutine to
    /// actually spawn enemies, using the array to do so. It also controls the UI that appears when the player loses.
    /// </summary>
    void Start()
    {
        enemyList[0] = Snake;
        enemyList[1] = Slime;
        enemyList[2] = Snail;
        StartCoroutine(SpawnEnemy());
        LoseUI.SetActive(false);
        pController = GameObject.FindObjectOfType<PlayerController>();
    }

    /// <summary>
    /// Score is updated whenever an enemy dies (each enemy is only worth 100 points). This function also uses a
    /// PlayerPrefs to save a high score if the player gets a high score.
    /// </summary>
    public void UpdateScore()
    {
        score += 100;
        ScoreText.text = "Score: " + score;

        //if the current score is greater than a saved highscore value
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score); //the highscore value gets the current score
        }
    }

    /// <summary>
    /// Lives are updated whenever an enemy reaches the player or goes past the player (there isn't a way to gain lives
    /// so the player will always lose one when this function is called)
    /// </summary>
    public void UpdateLives()
    {
        lives--;
        LivesText.text = "Lives: " + lives;
        TakeDamage.Play(); //a sound effect is played after losing a life
        if(lives == 0)
        {
            LoseGame(); //a function that ends the game
        }
    }

    /// <summary>
    /// After opening a panel that tells the player that they lost, the game will tell the player their score and the
    /// high score. The player will also lose all controls after calling pController.Lose();.
    /// </summary>
    public void LoseGame()
    {
        LoseUI.SetActive(true);
        gameOngoing = false; //boolean that prevents enemies from spawning
        LoseText.text = "Game Over!\nFinal Score: " + score + "\nHigh Score: " + PlayerPrefs.GetInt("HighScore") + 
            "\nbtw the song is Battlefield3 by Peritune :)";
        pController.Lose();
    }

    /// <summary>
    /// These functions are not part of the players inputs, but actually two buttons that appear along with the lose
    /// screen
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// A coroutine that spawns enemies from an array, and also controls the position they appear in. The enemies 
    /// spawn slowly at first, and their spawnrate gradually increases. How bulky the enemy is also controls how long
    /// it takes for the next enemy to spawn (snail gives the longest time between spawn rate)
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemy()
    {
        while(gameOngoing)
        {
            int enemy = Random.Range(0, 3);
            int position = Random.Range(0, 5);
            Instantiate(enemyList[enemy], new Vector2(11, 0.85f - (offset * position)), Quaternion.identity);
            yield return new WaitForSeconds((enemy * 1f) + timeBetweenSpawn + 1f);
            if(timeBetweenSpawn > 0)
            {
                timeBetweenSpawn -= 0.1f; //enemies will continue to spawn faster after each enemy
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Snake;
    [SerializeField] private GameObject Slime;
    [SerializeField] private GameObject Snail;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text LivesText;
    private float offset = 1.28f;
    private bool gameOngoing = true;
    private GameObject[] enemyList = new GameObject[3];
    private float timeBetweenSpawn = 1f;
    private int score = 0;
    private int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        enemyList[0] = Snake;
        enemyList[1] = Slime;
        enemyList[2] = Snail;
        StartCoroutine(SpawnEnemy());
    }

    public void UpdateScore()
    {
        score += 100;
        ScoreText.text = "Score: " + score;
    }

    public void UpdateLives()
    {
        lives--;
        LivesText.text = "Lives: " + lives;
        if(lives == 0)
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {

    }

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
                timeBetweenSpawn -= 0.1f;
            }
        }
    }
}

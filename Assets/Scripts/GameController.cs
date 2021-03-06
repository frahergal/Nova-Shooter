﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    // Singleton
    public static GameController instance;

    // Enemy to spawn
    public Transform ufo;
    Transform bigEnemy;
    public Transform bigEnemy1;
    public Transform bigEnemy2;
    public Transform bigEnemy3;
    public Transform bigEnemy4;
    public Transform bigEnemy5;

    // Wave properties for level 1
    [Header("Wave Properties")]
    public float timeBeforeSpawning = 1.5f;
    public float timeBetweenEnemies = .25f;
    public float timeBeforeWaves = 2.0f;
    public int waveNumber = 0;
    int ufosPerWave;
    int ufosHealth;
    int bigEnemiesPerWave;
    int currentNumberOfEnemies;

    int score = 0;
    [Header("User Interface")]
    // GUI Text objects
    public Text scoreText;
    public Text waveText;
    public Text livesText;

    // Enemies wave-related attributes
    int waveMod;

    // Enemies colours
    string[] enemiesColours = { "blue", "red", "black" };
    int colourIndex;

    // Array of enemies to check
    GameObject[] enemiesActive;

    // Mouse
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    void Awake()
    {
        instance = this;
        currentNumberOfEnemies = 0;

        // Ignore collisions of the enemies (layer 9) and their lasers (layer 8)
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        livesText.text = "x " + PlayerBehaviour.instance.lives;
    }

    // Coroutine to spawn enemies
    IEnumerator SpawnEnemies()
    {
        // Give the player time before we start the game
        yield return new WaitForSeconds(timeBeforeSpawning);

        while (true)
        {
            // Don't spawn anything new until all of the previous wave's enemies are dead
            if (currentNumberOfEnemies <= 0)
            {
                enemiesActive = GameObject.FindGameObjectsWithTag("Enemy");
                // Double check if all enemies are dead
                if (enemiesActive.Length == 0)
                {

                    // Increases wave's number
                    waveNumber++;
                    waveText.text = "Wave: " + waveNumber;

                    // Checks the current level to modify attributes if necessary
                    CheckLevel();

                    // The ufos are spawned in odd waves
                    if (waveNumber % 2 != 0)
                    {
                        //Spawn 10 enemies in a random position
                        for (int i = 0; i < ufosPerWave; i++)
                        {
                            SpawnEnemy(ufo, ufosHealth);
                        }

                    }
                    // The "big enemies" are spawned in pair waves
                    else
                    {
                        for (int i = 0; i < bigEnemiesPerWave; i++)
                        {
                            SpawnEnemy(bigEnemy);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(timeBeforeWaves);

        }

    }

    void CheckLevel()
    {
        // 1st Level
        if (waveNumber < 11)
        {
            ufosPerWave = 10;
            ufosHealth = 1;
            bigEnemiesPerWave = 3;
            bigEnemy = bigEnemy1;
        }
        // 2nd Level
        else if (waveNumber > 10 && waveNumber < 21)
        {
            ufosPerWave = 15;
            ufosHealth = 2;
            bigEnemiesPerWave = 4;
            bigEnemy = bigEnemy2;
        }
        // 3rd level
        else if (waveNumber > 20 && waveNumber < 31)
        {
            ufosPerWave = 20;
            ufosHealth = 3;
            bigEnemiesPerWave = 5;
            bigEnemy = bigEnemy3;
        }
        // 4th level
        else if (waveNumber > 30 && waveNumber < 41)
        {
            ufosPerWave = 25;
            ufosHealth = 4;
            bigEnemiesPerWave = 6;
            bigEnemy = bigEnemy4;
        }
        // 5th level
        else if (waveNumber > 40 && waveNumber < 51)
        {
            ufosPerWave = 30;
            ufosHealth = 5;
            bigEnemiesPerWave = 7;
            bigEnemy = bigEnemy5;
        }
        // Extra levels (> 50 waves)
        else
        {
            ufosPerWave = (int)Random.Range(30, 51);
            ufosHealth = (int)Random.Range(1, 6);
            bigEnemiesPerWave = (int)Random.Range(7, 16);
            bigEnemy = bigEnemy5;
        }
    }

    // Spawn an enemy
    void SpawnEnemy(Transform enemyType, int ufoHealth = 2)
    {
        // We want the enemies to be off screen
        float signPosition = Random.Range(0, 2) * 2 - 1;
        float xPosition = Random.Range(-12, 12);
        float yPosition = Random.Range(6, 8);
        Vector3 enemyPos = this.transform.position;

        enemyPos.x += xPosition;
        enemyPos.y += yPosition * signPosition;
        // Spawn the enemy and increment the number of enemies spawned
        // (Instantiate Makes a clone of the first parameter
        // and places it at the second with a rotation of the third.)
        Transform enemy = Instantiate(enemyType, enemyPos, this.transform.rotation);
        // Set the enemy attributes depending on wave
        waveMod = waveNumber % 10;
        EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();

        // Attributes of enemies change dinamically with waves
        switch (waveMod)
        {
            // Wave 1: Green ufos
            case 1:
                enemyBehaviour.health = ufoHealth;
                break;
            // Wave 2: Green enemy
            case 2:
                break;
            // Wave 3: Blue ufos
            case 3:
                enemyBehaviour.ActivateBlueEnemy();
                enemyBehaviour.speed = 2.2f;
                enemyBehaviour.health = ufoHealth;
                break;
            // Wave 4: Blue enemy
            case 4:
                enemyBehaviour.ActivateBlueEnemy();
                enemyBehaviour.health += 1;
                break;
            // Wave 5: Red ufos
            case 5:
                enemyBehaviour.ActivateRedEnemy();
                enemyBehaviour.speed = 2.5f;
                enemyBehaviour.health = ufoHealth;
                break;
            // Wave 6: Red enemy
            case 6:
                enemyBehaviour.ActivateRedEnemy();
                enemyBehaviour.health += 2;
                break;
            // Wave 7: Yellow ufo
            case 7:
                enemyBehaviour.ActivateBlackEnemy();
                enemyBehaviour.speed = 2.8f;
                enemyBehaviour.health = ufoHealth;
                break;
            // Wave 8: Black enemy
            case 8:
                enemyBehaviour.ActivateBlackEnemy();
                enemyBehaviour.health += 3;
                break;
            // Wave 9: Combined ufo
            case 9:
                colourIndex = (int)Random.Range(0, 3);
                enemyBehaviour.ActivateEnemyByColour(enemiesColours[colourIndex]);
                enemyBehaviour.speed = 3f;
                enemyBehaviour.health = ufoHealth;
                break;
            // Wave 10: Combined enemies (colour)
            case 0:
                colourIndex = (int)Random.Range(0, 3);
                enemyBehaviour.ActivateEnemyByColour(enemiesColours[colourIndex]);
                enemyBehaviour.health += 4;
                break;
        }

        // Increases the count of enemies
        currentNumberOfEnemies++;
    }

    // Reduces the number of enemies in the counter
    public void KilledEnemy()
    {
        currentNumberOfEnemies--;
        //Debug.Log(currentNumberOfEnemies);
    }

    // Increases the score
    public void IncreaseScore(int increase)
    {
        score += increase;
        scoreText.text = "Score: " + score;
    }

    // Updates the UI with the current lives
    public void UpdateUILives(int lives)
    {
        livesText.text = "x " + lives;
    }

}

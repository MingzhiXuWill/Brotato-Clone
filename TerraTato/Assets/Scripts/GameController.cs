using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Spawner
    // Spawn Stats
    public float SpawnTimeMax;
    [HideInInspector]
    public float SpawnTimeCount;
    [HideInInspector]
    public bool CanSpawn;

    public float FruitSpawnRate;

    public GameObject FruitToSpawn;

    // Enemies
    public GameObject[] SpawnerEnemyList;

    public GameObject SpawnerMark;

    // Boundary
    public float xBoundary;
    public float yBoundary;
    public float BoundaryFix;
    #endregion

    #region Level
    [HideInInspector]
    public bool Spawnable;

    public GameObject Player;
    public PlayerController PlayerScript;

    [HideInInspector]
    public int CurrentLevel;

    public float LevelTimeMax;
    [HideInInspector]
    public float LevelTimeRemains;
    #endregion

    #region Text UI
    public GameObject TextLevelUI;
    public GameObject TextTimeUI;
    public GameObject TextHealthBarUI;
    public GameObject TextCoinNumberUI;
    [HideInInspector]
    public TextMeshProUGUI TextLevel;
    [HideInInspector]
    public TextMeshProUGUI TextTime;
    [HideInInspector]
    public TextMeshProUGUI TextHealthBar;
    [HideInInspector]
    public TextMeshProUGUI TextCoinNumber;

    public Image HealthBar;

    public GameObject PauseMenu;

    public GameObject ShopMenu;
    #endregion

    [HideInInspector]
    public static int GameState = 1; // 1 = Ingame // 2 = Paused // 3 = Shop

    void Start()
    {
        SpawnTimeCount = 0;
        CanSpawn = false;

        Spawnable = true;

        CurrentLevel = 0;

        LevelTimeRemains = LevelTimeMax;

        TextLevel = TextLevelUI.GetComponent<TextMeshProUGUI>();
        TextTime = TextTimeUI.GetComponent<TextMeshProUGUI>();
        TextHealthBar = TextHealthBarUI.GetComponent<TextMeshProUGUI>();
        TextCoinNumber = TextCoinNumberUI.GetComponent<TextMeshProUGUI>();

        PlayerScript = Player.GetComponent<PlayerController>();

        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimeUpdate();
        SpawnEnemy();

        GameUIUpdate();
        LevelTimeUpdate();
        LevelEndCheck();

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GameState == 1) 
            {
                PauseGame();

            }
            else if (GameState == 2)
            {
                ResumeGame();
            }
        }
    }

    public void GameUIUpdate() {
        TextLevel.text = "LEVEL " + (CurrentLevel + 1).ToString();
        TextTime.text = ((int)LevelTimeRemains).ToString();

        TextHealthBar.text = PlayerScript.CurrentHealth + " / " + PlayerScript.MaxHealth;

        TextCoinNumber.text = PlayerScript.TotalCoins + "";

        HealthBar.fillAmount = PlayerScript.CurrentHealth / PlayerScript.MaxHealth;
    }

    public void SpawnEnemy() {
        if (CanSpawn)
        {
            float xPos = Random.Range(-xBoundary + BoundaryFix, xBoundary - BoundaryFix);
            float yPos = Random.Range(-yBoundary + BoundaryFix, yBoundary - BoundaryFix);

            GameObject EnemyToSpawn = SpawnerEnemyList[Random.Range(0, SpawnerEnemyList.Length)];

            if (Random.value < FruitSpawnRate)
            {
                EnemyToSpawn = FruitToSpawn;

                Instantiate(EnemyToSpawn, new Vector2(xPos, yPos), Quaternion.identity);
            }
            else {
                GameObject thisSpawnerMark = Instantiate(SpawnerMark, new Vector2(xPos, yPos), Quaternion.identity);

                thisSpawnerMark.GetComponent<SpawnerMark>().EnemyToSpawn = EnemyToSpawn;
            }

            CanSpawn = false;
        }
    }

    public void SpawnTimeUpdate() {
        if (Spawnable) 
        {
            if (SpawnTimeCount <= SpawnTimeMax && !CanSpawn)
            {
                SpawnTimeCount += Time.deltaTime;
            }
            else if (SpawnTimeCount > SpawnTimeMax)
            {
                CanSpawn = true;
                SpawnTimeCount = 0;
            }
        }
    }

    public void LevelTimeUpdate() {
        if (LevelTimeRemains > 0)
        {
            LevelTimeRemains -= Time.deltaTime;
        }
    }

    public void LevelEndCheck() 
    {
        if (LevelTimeRemains <= 0)
        {
            // Destory Enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Enemy Enemy = enemy.GetComponent<Enemy>();

                if (Enemy != null)
                {
                    if (Enemy.RangeAttack != null)
                    {
                        DestroyAllParticles(Enemy.RangeAttack.GetComponent<ParticleSystem>());    
                    }
                }

                GameObject.Destroy(enemy);
            }

            // Destory Loot
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coins");
            foreach (GameObject coin in coins)
            {
                GameObject.Destroy(coin);
            }

            GoToShop();
        }
    }

    public void DestroyAllParticles(ParticleSystem ps, bool stop = true)
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);
        int count = ps.particleCount;
        for (int i = 0; i < count; i++)
        {
            particles[i].remainingLifetime = 0f;
        }
        ps.SetParticles(particles);

        if (stop) ps.Stop();
    }


    public void GoToShop() 
    {
        ShopMenu.SetActive(true);
        Time.timeScale = 0;

        GameState = 3;
    }

    public void GoToLevel()
    { 
    
    }

    public void PauseGame() 
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;

        GameState = 2;
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;

        GameState = 1;
    }

    public void GoToMainMenu() 
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;

        GameState = 1;
        //SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

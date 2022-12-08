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

    #region Wave
    [HideInInspector]
    public bool Spawnable;

    public float WavePercentage;

    public GameObject Player;
    [HideInInspector]
    public PlayerController PlayerScript;

    [HideInInspector]
    public int CurrentWave;

    public float WaveTimeMax;
    [HideInInspector]
    public float WaveTimeRemains;
    #endregion

    #region Text UI
    public GameObject TextWaveUI;
    public GameObject TextTimeUI;
    public GameObject TextHealthBarUI;
    public GameObject TextCoinNumberUI;
    [HideInInspector]
    public TextMeshProUGUI TextWave;
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

    ShopController ShopController;

    [HideInInspector]
    public static int GameState = 1; // 1 = Ingame // 2 = Paused // 3 = Shop

    void Start()
    {
        ShopController = GetComponent<ShopController>();

        SpawnTimeCount = 0;
        CanSpawn = false;

        Spawnable = true;

        CurrentWave = 0;

        WaveTimeRemains = WaveTimeMax;

        TextWave = TextWaveUI.GetComponent<TextMeshProUGUI>();
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
        WaveTimeUpdate();
        WaveEndCheck();

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
        TextWave.text = "Wave " + (CurrentWave + 1).ToString();
        TextTime.text = ((int)WaveTimeRemains).ToString();

        TextHealthBar.text = PlayerScript.CurrentHealth + " / " + PlayerScript.MaxHealth;

        TextCoinNumber.text = PlayerScript.TotalCoins + "";

        HealthBar.fillAmount = (float)PlayerScript.CurrentHealth / (float)PlayerScript.MaxHealth;
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
                thisSpawnerMark.GetComponent<SpawnerMark>().Wave = CurrentWave;
                thisSpawnerMark.GetComponent<SpawnerMark>().WavePercentage = WavePercentage;
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

    public void WaveTimeUpdate() {
        if (WaveTimeRemains > 0)
        {
            WaveTimeRemains -= Time.deltaTime;
        }
    }

    public void WaveEndCheck() 
    {
        if (WaveTimeRemains <= 0)
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

            // Destory Marks
            GameObject[] marks = GameObject.FindGameObjectsWithTag("SpawnerMark");
            foreach (GameObject mark in marks)
            {
                GameObject.Destroy(mark);
            }

            // Destory Marks
            GameObject[] texts = GameObject.FindGameObjectsWithTag("FloatingText");
            foreach (GameObject text in texts)
            {
                GameObject.Destroy(text);
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
        if (GameState == 1) {
            ShopMenu.SetActive(true);
            ShopController.ShopUpdate();

            Time.timeScale = 0;

            GameState = 3;
        }
    }

    public void GoToWave()
    {
        if (GameState == 3)
        {
            NextWave();
            ShopMenu.SetActive(false);

            Time.timeScale = 1;

            GameState = 1;
        }
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

    public void NextWave() 
    { 
        WaveTimeRemains = WaveTimeRemains = WaveTimeMax;

        SpawnTimeCount = 0;
        CanSpawn = false;

        CurrentWave++;

        Player.GetComponent<PlayerController>().WaveReset();
    }
}

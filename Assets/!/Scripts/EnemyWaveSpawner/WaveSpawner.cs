using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameEvent WaveChangedEvent;
    public List<Enemy> enemies = new List<Enemy>();
    private GameObject _enemyPrefab;
    public List<Transform> spawnLocations = new List<Transform>();
    public StatsSO StatsSO;
    private int _enemiesAllowedOnScreen;
    public int _enemiesSpawnedInCurrentWave;
    public int _numberOfEnemiesToKill;

    public IntVariableSO enemiesOnScreenSO;
    public int enemiesOnScreenSODisplay;

    public IntVariableSO currentWaveSO;
    public int currentWaveSODisplay;

    public IntVariableSO enemiesKilledSO;
    public int enemiesKilledSODisplay;

    //BOSS LOGIC
    public List<Boss> bosses = new List<Boss>();
    private GameObject _bossPrefab;
    private int _bossesAllowedOnScreen;
    private bool _isBossRound;

    void Start()
    {
        enemiesOnScreenSO.Value = 0;
        _enemiesAllowedOnScreen = 5;
        currentWaveSO.Value = 0;
        currentWaveSO.Value++;
        _numberOfEnemiesToKill = 10;
        enemiesKilledSO.Value = 0;
        StatsSO.CurrentStats.Health = StatsSO.BaseStats.Health;
        _enemiesSpawnedInCurrentWave = 0;

        //BOSS LOGIC
        _isBossRound = false;
        _bossesAllowedOnScreen = 1;

    }

    /// <summary>
    /// This Update method is temporary only so we can manually skip wave's. This can be deleted in the future it no longer needed
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TriggerNextWave();
        }
    }

    /// <summary>
    /// The main game loop for the Wave System. Essentially every 5 rounds a boss enemy spawns intead of regular zombies. 
    /// </summary>
    void FixedUpdate()
    {
        // these are only displays for the scriptable objects. Not needed for functinality.
        enemiesKilledSODisplay = enemiesKilledSO.Value;
        enemiesOnScreenSODisplay = enemiesOnScreenSO.Value;
        currentWaveSODisplay = currentWaveSO.Value;

        isBossRound();

        if (_isBossRound == true)
        {
            GenerateBossWave();
        }
        else
        {
            GenerateWave();
        }
    }

    /// <summary>
    ///  Logic for how the regular enemy wave is calculated. 
    /// </summary>
    private void GenerateWave()
    {
        if (_enemiesSpawnedInCurrentWave < _numberOfEnemiesToKill)
        {
            GenerateEnemies();
        }
        else
        {
            if (_numberOfEnemiesToKill <= enemiesKilledSO.Value)
            {
                TriggerNextWave();
            }
        }
    }

    /// <summary>
    /// The method below spawns the regular zombie prefabs.
    /// </summary>
    private void GenerateEnemies()
    {
        if (_enemiesAllowedOnScreen > enemiesOnScreenSO.Value)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            _enemyPrefab = enemies[randEnemyId].enemyPrefab;
            Transform randomSpawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];

            // Calculate random spawn position within the spawn area
            Vector2 spawnSize = randomSpawnLocation.localScale;
            Vector2 spawnCenter = randomSpawnLocation.position;

            float spawnX = spawnCenter.x + Random.Range(-spawnSize.x / 2f, spawnSize.x / 2f);
            float spawnY = spawnCenter.y + Random.Range(-spawnSize.y / 2f, spawnSize.y / 2f);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemiesOnScreenSO.Value++;
            _enemiesSpawnedInCurrentWave++;
        }
    }

    /// <summary>
    /// Method below contains logic for spawning next wave. 
    /// </summary>
    private void TriggerNextWave()
    {
        KillAllEnemies();
        enemiesOnScreenSO.Value = 0;
        currentWaveSO.Value++;
        MakeWaveHarderAlgorithm();
        enemiesKilledSO.Value = 0;
        _enemiesSpawnedInCurrentWave = 0;
        WaveChangedEvent?.Raise();
    }

    /// <summary>
    /// How zombie enemies become harder after each passing wave. This will get more sophisticated as the game progresses, it's fine for now.  
    /// </summary>
    private void MakeWaveHarderAlgorithm()
    {
        _numberOfEnemiesToKill += 5;
        StatsSO.CurrentStats.Health += 5f;
    }

    /// <summary>
    /// A method for killing all enemiies on screen. This is needed inside the TriggerNextWave().
    /// </summary>
    private void KillAllEnemies()
    {
        GameObject[] enemiesOnScreen = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject prefab in enemiesOnScreen)
        {
            Destroy(prefab);
        }
    }


    //BOSS LOGIC
    /// <summary>
    /// Similar to GenerateWave but this time for the Boss rounds. 
    /// </summary>
    private void GenerateBossWave()
    {
        if (_enemiesSpawnedInCurrentWave < _bossesAllowedOnScreen)
        {
            GenerateBosses();
        }
        else
        {
            if (_bossesAllowedOnScreen <= enemiesKilledSO.Value)
            {
                TriggerNextWave();
            }
        }
    }

    /// <summary>
    /// A check that runs int he FixedUpdate method to determine whether the current wave is a boss wave. 
    /// </summary>
    private void isBossRound()
    {
        if (currentWaveSO.Value % 5 == 0)
        {
            _isBossRound = true;
        }
        else
        {
            _isBossRound = false;
        }
    }

    /// <summary>
    /// Similar to Generate Enemies but for bosses.
    /// </summary>
    private void GenerateBosses()
    {
        if (_bossesAllowedOnScreen > enemiesOnScreenSO.Value)
        {
            int randEnemyId = Random.Range(0, bosses.Count);
            _bossPrefab = bosses[randEnemyId].bossPrefab;
            Transform randomSpawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];

            // Calculate random spawn position within the spawn area
            Vector2 spawnSize = randomSpawnLocation.localScale;
            Vector2 spawnCenter = randomSpawnLocation.position;

            float spawnX = spawnCenter.x + Random.Range(-spawnSize.x / 2f, spawnSize.x / 2f);
            float spawnY = spawnCenter.y + Random.Range(-spawnSize.y / 2f, spawnSize.y / 2f);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            Instantiate(_bossPrefab, spawnPosition, Quaternion.identity);
            enemiesOnScreenSO.Value++;
            _enemiesSpawnedInCurrentWave++;
        }
    }
}

/// <summary>
/// An enemy object used to create a list of enemies to hold the zombie prefabs. 
/// </summary>
[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
}

/// <summary>
/// An boss object used to create a list of boss to hold the zombie prefabs. 
/// </summary>
[System.Serializable]
public class Boss
{
    public GameObject bossPrefab;
}
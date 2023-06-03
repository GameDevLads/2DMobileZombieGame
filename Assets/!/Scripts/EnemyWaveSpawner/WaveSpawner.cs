using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    //List that holds all the enemy prefabs.
    public List<Enemy> enemies = new List<Enemy>();

    //Holds an enemy that in instantiated inside the GenereateEnemiesMethod().
    private GameObject _enemyPrefab;

    //Holds a list of all the spawn locations.
    public List<Transform> spawnLocations = new List<Transform>();

    //A check that allows you to disable the wavespawner from the editor.
    public bool spawnEnemies = false;

    //A scriptable object that keeps track of the current wave of enemies. This should be used to feed the wave number in the UI.
    public IntVariableSO currentWaveSO;
    // a tracker to see the current wave on the WaveSpawner script (Not needed for functinality)
    public int currentWave;

    //this gets initiliased to 1. Each time a new wave is triggered, the value of this is increased by 1. This value is passed onto EnemyWaveAlgorithm() to determine how many enemies spawn each new wave and how strong the zombies area.
    private int _newWaveValue = 1;

    //This variable is the starting point of the number of enemies in wave 1. This value is used to check if the number of enemies killed in a wave is equal to this number, if true, then a new wave is triggered. Each new Wave the value of this is incremented by 5 in EnemyWaveAlgorithm().
    private int _numberOFEnemies = 10;

    // A scriptable object for tracking the enemies killed per wave. This value is updated each time in the ApplyDamage() method in the enemy script when an enemy dies. This value along with _numberOfEnemies is used as a check in the TriggerNextWave() method to trigger the next wave.   
    public IntVariableSO enemiesKilledSO;
    // a tracker for the above (Not needed for functinality)
    public int enemiesKilled;

    // For storing the current Wave Value. Wave Value is used to determine how many enemies should spawn per wave. Each time a new wave gets triggered the _newWaveValue gets assigned to the _waveValue and that's how the enemies get harder and more of them spawn. 
    private int _enemiesToSpawnValue;

    //ScriptableObject for accessing the enemy stats.
    public StatsSO StatsSO;

    //Value that dictates how many zombies can be on screen at once. If a wave has 20 zombies only 5 will be on screen and if one zombie is killed than another will spawn to satisfy the below 5 enemy limit.  
    private int _enemiesAllowedOnScreen = 5;

    //Scriptable object that keeps track of how many zombies are on screen. Everytime an enemy spawns(GenerateEnemies()), this value increases, and any time a zombie dies (Enemy.ApplyDamage()) this value decreases. The value is always reset to 0 at the beginning of the game. 
    public IntVariableSO enemiesOnScreenSO;

    //keeps track of all the enemies that have been spawned per wave. This value does not change when the enemy dies. 
    private int _enemiesSpawnedInCurrentWave = 0;


    // How much it takes to spawn an enemy. When an enemy spawns, this value gets taken away from _wavevalue until _waveValue is euqal to 0. At this pint enemies stop spawning
    private int _spawnCost = 1;

    /// <summary>
    /// This resets most of the values to their initial states and spawns the first set of enemies. It's inside an if statement that is triggered by the public bool variable spawnEnemies.
    /// </summary>
    void Start()
    {
        if (spawnEnemies == true)
        {
            enemiesOnScreenSO.Value = 0;
            currentWaveSO.Value = 1;
            enemiesKilledSO.Value = 0;
            StatsSO.CurrentStats.Health = StatsSO.BaseStats.Health;
            GenerrateWave();
        }
    }

    /// <summary>
    /// Runs the GenerateWave() and riggerNextWave() on a loop. 
    /// </summary>
    void FixedUpdate()
    {
        //displays for scriptable objects (Not needed for functionality)
        enemiesKilled = enemiesKilledSO.Value;
        currentWave = currentWaveSO.Value;

        GenerrateWave();
        TriggerNextWave();
    }

    /// <summary>
    ///  Runs the code to spawn the enemies until it spawned all the enemies needed for the given wave. 
    /// </summary>
    public void GenerrateWave()
    {
        if (_enemiesSpawnedInCurrentWave <= _enemiesToSpawnValue)
        {
            _enemiesToSpawnValue = EnemyWaveAlgorithm(_newWaveValue);
            GenerateEnemies();
        }
    }

    /// <summary>
    /// Contains the logic for spawning the enemies. The code pics a random enemy from the enemies list and spawns it in a random x y cord and a random spawn location. Each time an enemy is spawned a few of the variables from above are updated accordigly to correctly keep track of the enemies, and wave state.
    /// </summary>
    public void GenerateEnemies()
    {
        if (_enemiesToSpawnValue > 0 && _enemiesAllowedOnScreen > enemiesOnScreenSO.Value)
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
            _enemiesToSpawnValue -= _spawnCost;
        }
    }

    /// <summary>
    /// The method below triggers a new wave under a condition that all the enemies of the specific wave have been killed. 
    /// </summary>
    public void TriggerNextWave()
    {
        if (enemiesKilledSO.Value == _numberOFEnemies)
        {
            enemiesKilledSO.Value = 0;
            _newWaveValue++;
            _enemiesSpawnedInCurrentWave = 0;
        }
    }

    /// <summary>
    /// Currently how enemies increase in power per wave. Will be modified in the future to make it more interesting. 
    /// </summary>
    public int EnemyWaveAlgorithm(int newWaveValue)
    {
        if (newWaveValue > currentWaveSO.Value)
        {
            currentWaveSO.Value = newWaveValue;
            StatsSO.CurrentStats.Health += 5f;
            _numberOFEnemies += 5;
        }
        return _numberOFEnemies;
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
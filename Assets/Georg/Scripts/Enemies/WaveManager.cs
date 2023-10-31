using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager _instance;

    [SerializeField] private GameObject flyingEnemyPrefab;
    [SerializeField] private GameObject groundEnemyPrefab;

    [SerializeField] private Transform[] flyingSpawnPoints;
    [SerializeField] private Transform[] groundSpawnPoints;

    [SerializeField] private float spawnDelay = 2; //Delay between each enemy in wave

    [SerializeField] private int initialWaitMin; //Wait before start of first wave
    [SerializeField] private int initialWaitMax;
    
    [Space(10)]
    
    [SerializeField] private int endWaveWaitMin; //Wait between waves
    [SerializeField] private int endWaveWaitMax;
    
    [Space(10)]

    [SerializeField] private int initialFlyingEnemyAmount; //Only initial amounts. Rest will be generated procedurally 
    [SerializeField] private int initialGroundEnemyAmount; //Only initial amounts. Rest will be generated procedurally 
    [SerializeField] private int initialWavesBeforeTown;

    [SerializeField] private int townTimer;

    private BoidManagerUpdated boidManager;

    [Space(10)]

    public int leftInWave; //To be decremented from enemies
    public List<Boid> boidsInScene;

    //private List<GameObject> enemies; //Spawned enemies

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        boidManager = FindObjectOfType<BoidManagerUpdated>();
        //boidManager.waveManager = this;
        StartCoroutine(WaveCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaveCycle()
    {
        //Initial wait
        int wait = Random.Range(initialWaitMin, initialWaitMax);
        Debug.Log("First wave starting in " + wait + " seconds");
        yield return new WaitForSeconds(wait);

        int wavesBeforeTown = initialWavesBeforeTown;

        int flyingInWave = initialFlyingEnemyAmount;
        int groundInWave = initialGroundEnemyAmount;

        //int leftInWave = initialFlyingEnemyAmount+initialGroundEnemyAmount;
        while (true)
        {
            for (int i = 0; i<wavesBeforeTown; i++)
            {
                Debug.Log("flyingInWave; "+flyingInWave);
                Debug.Log("groundInWave; "+groundInWave);
                leftInWave = flyingInWave + groundInWave;

                //This system spawns ALL flying enemies, then ALL ground enemies. Possibly alternate?
                for (int j = 0; j < flyingInWave; j++)
                {
                    GameObject enemy = SpawnEnemyAtPoint(flyingEnemyPrefab, flyingSpawnPoints);


                    Boid b = enemy.GetComponent<Boid>();
                    boidManager.InitialiseBoid(b);
                    boidsInScene.Add(b);

                    yield return new WaitForSeconds(spawnDelay);
                }
                boidManager.UpdateBoidList(boidsInScene);

                for (int j = 0;j < groundInWave; j++)
                {
                    /*GameObject enemy = */SpawnEnemyAtPoint(groundEnemyPrefab, groundSpawnPoints);
                    yield return new WaitForSeconds(spawnDelay);
                }

                Debug.Log("All enemies in wave spawned. Waiting...");
                while (leftInWave > 0) //While still enemies left in wave, wait
                {
                    yield return null;
                }

                if ((wavesBeforeTown- i) <= 1)
                {
                    break; //If last round, break immediately without waiting
                }

                else
                {
                    wait = Random.Range(endWaveWaitMin, endWaveWaitMax);
                    Debug.Log("Next wave starting in " + wait + " seconds");
                    yield return new WaitForSeconds(wait);
                }
            }

            Debug.Log("Stopping at town. " + townTimer + " second wait.");
            yield return new WaitForSeconds(townTimer);
            
        }
    }

    GameObject SpawnEnemyAtPoint(GameObject enemyPrefab, Transform[] spawns)
    {
        int selectedSpawn = Random.Range(0, spawns.Length); //Select random spawn
        GameObject enemy = Instantiate(enemyPrefab, spawns[selectedSpawn].position, Quaternion.identity); //Spawn enemy.

        enemy.GetComponent<DemoEnemy>().waveManager = this; //Make sure enemy can access this script

        return enemy;
    }

    public void RemoveBoidFromList(Boid b) //Technically, it seems to work without removing dead boids from the boids list, but for safety
    {
        boidsInScene.Remove(b);
        boidManager.UpdateBoidList(boidsInScene);
    }
}

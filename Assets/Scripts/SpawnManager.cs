using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private Text m_LevelText;

    [SerializeField]
    private GameObject[] m_EnemyPrefabs;

    [SerializeField]
    private GameObject m_SpawnerPrefab;

    [SerializeField]
    private int m_PlayfieldXsize = 5;

    [SerializeField]
    private int m_PlayfieldYsize = 5;

    [SerializeField]
    private int m_TileSize = 4;

    [SerializeField]
    private float m_ChallengeInterval = 30f;

    private int m_DifficultyCounter = 0;

    private float m_LastIncreaseTime = 0f;

    [SerializeField]
    private float m_SpawnIntervalMin = 5f;

    [SerializeField]
    private float m_SpawnIntervalMax = 15f;

    [SerializeField]
    private float m_SpawnDelay = 1.5f;

    private float m_TimeToNextSpawn = 0f;

    [Range(0.1f, 0.9f)]
    [SerializeField]
    private float m_DifficultyIncreaseFactor = 0.5f;

    // Use this for initialization
    void Start () {
        if (m_EnemyPrefabs.Length < 1) Debug.LogError("No enemys to spawn");
        m_LastIncreaseTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        m_TimeToNextSpawn -= Time.deltaTime;

        //spawn when its time
		if(m_TimeToNextSpawn <= 0f)
        {
            //TODO only border pieces
            int spawnX = Random.Range(0, m_PlayfieldXsize - 1);
            int spawnY = m_PlayfieldYsize-1; //Random.Range(0, m_PlayfieldYsize - 1);
            int spawnIndex = Random.Range(0, m_EnemyPrefabs.Length - 1);

            InitiateSpawnAt(spawnX * m_TileSize, spawnY * m_TileSize, spawnIndex);

            m_TimeToNextSpawn = Random.Range(m_SpawnIntervalMin, m_SpawnIntervalMax);

        }



        //increase spawn rate every x sec
        if ( (Time.time - m_LastIncreaseTime) >= m_ChallengeInterval)
        {
            m_DifficultyCounter++;

            m_LevelText.text = "Level: " + m_DifficultyCounter;
            
            m_SpawnIntervalMin = m_SpawnIntervalMin / (Mathf.Pow(m_DifficultyCounter, m_DifficultyIncreaseFactor));
            m_SpawnIntervalMax = m_SpawnIntervalMax / (Mathf.Pow(m_DifficultyCounter, m_DifficultyIncreaseFactor));

            m_LastIncreaseTime = Time.time;
        }

	}


    void InitiateSpawnAt(int x, int y, int prefabIndex)
    {
        //create spawn indicator
        Vector3 spawnerPos = new Vector3(x + m_TileSize / 2f, 0f, y + 1.5f * m_TileSize);
        GameObject spawnIndicator = Instantiate(m_SpawnerPrefab, spawnerPos, Quaternion.identity);

        Vector3 spawnPoint = new Vector3(x, 0.1f, y);

        Color enemyColor = m_EnemyPrefabs[prefabIndex].GetComponent<Renderer>().sharedMaterial.color;
        spawnIndicator.GetComponent<Spawner>().StartSpawn(m_SpawnDelay, enemyColor);

        //start spawn delayed
        StartCoroutine(SpawnDelayed(prefabIndex, spawnPoint, m_SpawnDelay));

    }

    IEnumerator SpawnDelayed(int index, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(m_EnemyPrefabs[index], pos, Quaternion.identity);
    }
}

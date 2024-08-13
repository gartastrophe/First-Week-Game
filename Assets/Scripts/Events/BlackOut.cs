using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackOut : MonoBehaviour
{
    public string lightTag = "Blackout";
    GameObject[] lightHolders;

    public float minSpawnTime, maxSpawnTime;
    public GameObject bossPrefab;
    private Transform bossSpawn;

    public static bool lightEnabled = true;

    LevelManager levelManager;
    void Start()
    {
        bossSpawn = GameObject.FindGameObjectWithTag("BossSpawn").transform;

        lightHolders = GameObject.FindGameObjectsWithTag(lightTag);

        foreach (GameObject obj in lightHolders)
        {
            Light light = obj.GetComponent<Light>();
            if (light != null)
            {
                light.enabled = false;
            }
        }

        levelManager = GameObject.FindObjectOfType<LevelManager>();

        StartCoroutine(SpawnEnemyCoroutine());
    }

    //I asked chatgpt for help with coroutine stuff
    private IEnumerator SpawnEnemyCoroutine()
    {
        float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
        yield return new WaitForSeconds(waitTime);
        if (!levelManager.gameWonTriggered)
        {
            Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation);
        }
    }
}

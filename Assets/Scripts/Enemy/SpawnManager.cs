using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private float spawnBuffer;
    [SerializeField] private int maxEnemies; //0125

    private int currentEnemyCount = 0; //0125
    
    public bool spawning;

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            spawnPoints.Add(gameObject.transform.GetChild(i).gameObject);
        }

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnBuffer);
        spawning = true;
        foreach (var spawnPoint in spawnPoints)
        {
            if (currentEnemyCount >= maxEnemies) //0125
                yield break; 

            Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            currentEnemyCount ++; //0125
        }
    }

    public void EnemyDied() //0125
    {
        currentEnemyCount --; 
    }
    
}

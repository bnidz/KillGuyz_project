using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
     
[Serializable]
public class WaveEnemy
{
    public int Amount;
    public Enemy Type;
  
}

[Serializable]
public class Wave
{
    public List<WaveEnemy> Enemy;
    public List <Transform> spawns;
}
public class Spawner : MonoBehaviour
{
    public List<Wave> Waves;
    public int CurrentWave = 0;

    //lets test autospawner
    public List<GameObject> activeEnemies;

    void CreateNextEnemyWave()
    {
        var wave = Waves.ElementAtOrDefault(CurrentWave);

        if (wave != null)
        {
            StartCoroutine(CreateEnemiesFor(wave));
            CurrentWave++;
        }
    }

    Transform RandomSpawn;
    public float spawnInterval = 2f;

    private bool waveInprogress = false;
    public IEnumerator CreateEnemiesFor(Wave wave)
    {
        
        if (!wave.Enemy.Any()) yield break;
        waveInprogress = true;
        foreach (var enemy in wave.Enemy)
        {
            for (int i = 0; i < enemy.Amount; i++)
            {
                yield return new WaitForSeconds(spawnInterval);
                RandomSpawn = wave.spawns[UnityEngine.Random.Range(0, wave.spawns.Count)];
                GameObject spawnie =  Instantiate(enemy.Type.baseEnemy, RandomSpawn.position, Quaternion.identity);
                spawnie.gameObject.GetComponent<NavMesh_Enemy>().thisEnemy = enemy.Type;
                activeEnemies.Add(spawnie);
            }
        }
        waveInprogress = false;
    }

    public GameCamera gamecam;
    public UIManager uiman;
    private int clearedWaves = 0;
    // To test press spacebar
    private bool isTrue = false;

    void Update()
    {
        //way to mark wave cleared and automaticly spawn next one..
        //maybe some sort of trigger (for ddebug at least)

        //if (Input.GetKeyDown(KeyCode.Space)) CreateNextEnemyWave();
        //autospawner

        if (activeEnemies.Count == 0 && !waveInprogress)
        {
            clearedWaves++;
            if(clearedWaves <= Waves.Count)
            {
                int currentwavestring = CurrentWave + 1;
                string uiText = "NEW WAVE INCOMING" + currentwavestring.ToString();
                StartCoroutine(uiman.ShowInfoTextCenter(uiText, 3f));
            
                CreateNextEnemyWave();
            }
            if(clearedWaves > Waves.Count)
            {
                if(!isTrue)
                {
                    isTrue = true;
                    string uiText = "LEVEL CLEAREDDD!!!! THANK YOU";
                    StartCoroutine(uiman.ShowInfoTextCenter(uiText, 3f));
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    gamecam.isAttacking = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.S))
        //{
        //    gamecam.isAttacking = false;
        //}
    }
}
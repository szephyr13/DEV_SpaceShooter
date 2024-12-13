using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    //waves
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject player;
    private Vector3 randomPosition;
    private bool wavesAreOver;
    private bool spawnBoss;


    void Start()
    {
        StartCoroutine(EnemySpawn());
        wavesAreOver = false;
        spawnBoss = false;

    }

    private void Update()
    {
        if (wavesAreOver)
        {
            if (!spawnBoss)
            {
                StopAllCoroutines();
                AudioManager.instance.PlayBGM("Boss");
                Instantiate(bossPrefab, transform.position, Quaternion.identity);
                spawnBoss = true;
            }


            
        }
    }



    IEnumerator EnemySpawn()
    {
        for (int i = 0; i < 3; i++) //levels
        {
            for (int j = 0; j < 5; j++) //waves
            {
                //ui text∫
                waveText.text = (i+1) + " - " + (j+1);
                if(j >= 1 && player.gameObject.GetComponent<Player>().lifes < 4)
                {
                    AudioManager.instance.PlaySFX("PowerUp");
                    player.gameObject.GetComponent<Player>().lifes++;
                    player.gameObject.GetComponent<Player>().UpdatePlayer();
                }
                yield return new WaitForSeconds(2f); //wait time for reading info
                waveText.text = "";

                //spwaning logic
                for (int k = 0; k < 10; k++) //enemies
                {
                    randomPosition = new Vector3(transform.position.x, Random.Range(-4.57f, 4.57f), 0);
                    Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

                    yield return new WaitForSeconds(1f); //enemy time separation
                }
                yield return new WaitForSeconds(2f); //wave time separation
            }
            yield return new WaitForSeconds(3f); //level time separation
        }
        wavesAreOver = true;
    }
}

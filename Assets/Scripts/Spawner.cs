using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI waveText;


    void Start()
    {
        StartCoroutine(EnemySpawn());
    }


    IEnumerator EnemySpawn()
    {
        for (int i = 0; i < 5; i++) //levels
        {
            for (int j = 0; j < 4; j++) //waves
            {
                //ui text∫
                waveText.text = (i+1) + " - " + (j+1);
                yield return new WaitForSeconds(2f); //wait time for reading info
                waveText.text = "";

                //spwaning logic
                for (int k = 0; k < 10; k++) //enemies
                {
                    Vector3 randomPosition = new Vector3(transform.position.x, Random.Range(-4.57f, 4.57f), 0);
                    Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                    yield return new WaitForSeconds(1f); //enemy time separation
                }
                yield return new WaitForSeconds(2f); //wave time separation
            }
            yield return new WaitForSeconds(3f); //level time separation
        }
    }
}

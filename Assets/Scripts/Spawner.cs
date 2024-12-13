using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    //waves
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private TextMeshProUGUI waveText;
    private Vector3 randomPosition;
    private bool wavesAreOver;

    //you won
    [SerializeField] private GameObject youWon;


    void Start()
    {
        StartCoroutine(EnemySpawn());
        wavesAreOver = false;

    }

    private void Update()
    {
        if (wavesAreOver)
        {
            AudioManager.instance.PlaySFX("YouWon");
            youWon.SetActive(true);
            this.gameObject.SetActive(false);
            Time.timeScale = 0f;
        }
    }



    IEnumerator EnemySpawn()
    {
        for (int i = 0; i < 1; i++) //levels
        {
            for (int j = 0; j < 2; j++) //waves
            {
                //ui text∫
                waveText.text = (i+1) + " - " + (j+1);
                yield return new WaitForSeconds(2f); //wait time for reading info
                waveText.text = "";

                //spwaning logic
                for (int k = 0; k < 3; k++) //enemies
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

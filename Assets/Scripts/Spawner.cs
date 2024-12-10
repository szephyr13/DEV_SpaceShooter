using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI waveText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawn()
    {
        for (int i = 0; i < 5; i++) //levels
        {
            for (int j = 0; j < 4; j++) //waves
            {
                waveText.text = (i+1) + " - " + (j+1);
                yield return new WaitForSeconds(2f);
                waveText.text = "";
                for (int k = 0; k < 6; k++) //enemies
                {
                    Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
        }
    }
}

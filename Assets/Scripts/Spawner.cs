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

    //pool
    private ObjectPool<Enemy> enemyPool;


    void Start()
    {
        enemyPool = new ObjectPool<Enemy>(CreateE, null, ReleaseE, DestroyE);
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
                    randomPosition = new Vector3(transform.position.x, Random.Range(-4.57f, 4.57f), 0);
                    Enemy enemyCopy = enemyPool.Get();
                    enemyCopy.transform.position = randomPosition;
                    enemyCopy.gameObject.SetActive(true);

                    yield return new WaitForSeconds(1f); //enemy time separation
                }
                yield return new WaitForSeconds(2f); //wave time separation
            }
            yield return new WaitForSeconds(3f); //level time separation
        }
    }

    //POOL LOGIC - creates bullet on position and stores on pool - destroys bullet - releases bullet
    private Enemy CreateE()
    {
        Enemy enemyCopy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        enemyCopy.MyPool = enemyPool;
        enemyCopy.gameObject.transform.SetParent(this.gameObject.transform);
        return enemyCopy;
    }

    private void DestroyE(Enemy obj)
    {
        Destroy(obj.gameObject);
    }

    public void ReleaseE(Enemy obj)
    {
        obj.gameObject.SetActive(false);
    }
}

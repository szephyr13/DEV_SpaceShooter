using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
    }


    IEnumerator Attack()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bulletPrefab, spawnPosition.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}

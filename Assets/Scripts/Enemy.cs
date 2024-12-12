using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    //movement and shooting
    [SerializeField] private float speed;
    [SerializeField] private Shooting bulletPrefab;
    [SerializeField] private GameObject spawnPosition;

    //pool
    private ObjectPool<Shooting> bulletPool;


    // creates pool, calls shooting coroutine
    void Start()
    {
        bulletPool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
        StartCoroutine(Attack());
    }



    //updating enemy position
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
    }



    //coroutine for enemy attacks
    private IEnumerator Attack()
    {
        while (true)
        {
            //take from pool, reubicate, activate bullet
            Shooting bulletCopy = bulletPool.Get();
            bulletCopy.transform.position = spawnPosition.transform.position;
            bulletCopy.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.8f);
        }
    }


    //triggering with bullets - destroys bullet 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }


    //POOL LOGIC - creates bullet on position and stores on pool - destroys bullet - releases bullet
    private Shooting CreateB()
    {
        Shooting bulletCopy = Instantiate(bulletPrefab, spawnPosition.transform.position, Quaternion.identity);
        bulletCopy.MyPool = bulletPool;
        return bulletCopy;
    }

    private void DestroyB(Shooting obj)
    {
        Destroy(obj.gameObject);
    }

    private void ReleaseB(Shooting obj)
    {
        obj.gameObject.SetActive(false);
    }

}

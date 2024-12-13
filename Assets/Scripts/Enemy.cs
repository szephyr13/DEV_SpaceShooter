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
    private GameObject player;
    //bullet pool
    private ObjectPool<Shooting> bulletPool;



    // finds player, creates pool, calls shooting coroutine
    void Start()
    {
        player = GameObject.Find("Player");
        bulletPool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
        StartCoroutine(Attack());
    }



    //updating enemy position, pool info
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);

        if (this.gameObject.transform.position.x <= -11f)
        {
            Destroy(this.gameObject);
        }
    }



    //coroutine for enemy attacks
    private IEnumerator Attack()
    {
        while (true)
        {
            //take from pool, reubicate, activate bullet
            Shooting bulletCopy = bulletPool.Get();
            bulletCopy.transform.position = this.gameObject.transform.position;
            bulletCopy.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.8f);
        }
    }


    //triggering with bullets - shows death animation - destroys bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            player.gameObject.GetComponent<Player>().score += 20;
            StartCoroutine(deathAnimation());
            Destroy(collision.gameObject);
        }
    }


    //enemy death animation
    IEnumerator deathAnimation()
    {
        Animator animator = this.transform.GetChild(0).GetComponent<Animator>();
        animator.Play("EnemyFighterDestruction");

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }


    //POOL LOGIC - creates bullet on position and stores on pool - destroys bullet - releases bullet
    private Shooting CreateB()
    {
        Shooting bulletCopy = Instantiate(bulletPrefab, this.gameObject.transform.position, Quaternion.identity);
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

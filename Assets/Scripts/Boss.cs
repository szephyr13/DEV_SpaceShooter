using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Boss : MonoBehaviour
{

    //movement and shooting
    [SerializeField] private float speed;
    [SerializeField] private Shooting bulletPrefab;
    [SerializeField] private Shooting torpedoPrefab;
    private GameObject player;
    private int lifes;
    //bullet pool
    private ObjectPool<Shooting> bulletPool;
    private ObjectPool<Shooting> torpedoPool;
    //movement
    [SerializeField] private bool initialMovement;
    [SerializeField] private bool border;
    [SerializeField] private Vector3 randomMovement;

    private bool startedDestroying;



    void Start()
    {
        initialMovement = true;
        border = true;
        lifes = 150;
        player = GameObject.Find("Player");
        bulletPool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
        torpedoPool = new ObjectPool<Shooting>(CreateT, null, ReleaseT, DestroyT);
        StartCoroutine(BulletAttack());
        StartCoroutine(TorpedoAttack());
        startedDestroying = false;
    }





    void Update()
    {
        //initial movement
        if (initialMovement)
        {
            if (gameObject.transform.position.x > 5)
            {
                transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
            } else
            {
                initialMovement = false;
            }
        }

        //boss movement
        if (initialMovement == false)
        {
            // if hits limit, activates border
            if(gameObject.transform.position.x < 2.2f
                || gameObject.transform.position.x > 7.3f
                || gameObject.transform.position.y < -3.5f
                || gameObject.transform.position.y > 3.5f)
            {
                border = true;
            }

            //if border is activated, changes direction
            if (border)
            {
                randomMovement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                border = false;
            }
            //moves
            transform.Translate(randomMovement.normalized * speed * Time.deltaTime);

            //movement limits
            float clampedX = Mathf.Clamp(transform.position.x, 2f, 7.5f);
            float clampedY = Mathf.Clamp(transform.position.y, -3.75f, 3.75f);
            transform.position = new Vector3(clampedX, clampedY, 0);

        }

        //redirects to destroy logic
        if (lifes <= 0 && startedDestroying == false)
        {
            transform.position = transform.position;
            StartCoroutine(StartDestroying());
            startedDestroying = true;
        }
    }


    //destroy logic (audio, animation, destroying
    IEnumerator StartDestroying()
    {
        player.gameObject.GetComponent<Player>().score += 100;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySFX("BossExplosion");
        Animator animator = this.transform.GetChild(0).GetComponent<Animator>();
        animator.Play("DestroyedBoss");

        yield return new WaitForSeconds(1f);

        player.gameObject.GetComponent<Player>().battleWon = true;
        Destroy(this.gameObject);
    }

    //coroutine for enemy attacks
    private IEnumerator BulletAttack()
    {
        while (true)
        {
            //take from pool, reubicate, activate bullet
            Shooting bulletCopy = bulletPool.Get();
            bulletCopy.transform.position = this.gameObject.transform.position;
            bulletCopy.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator TorpedoAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            //take from pool, reubicate, activate bullet
            Shooting torpedoCopy = torpedoPool.Get();
            torpedoCopy.transform.position = this.gameObject.transform.position;
            torpedoCopy.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.2f);
        }
    }

    //triggering with bullets - shows death animation - destroys bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            AudioManager.instance.PlaySFX("EnemyExplosion");
            player.gameObject.GetComponent<Player>().score += 20;
            Destroy(collision.gameObject);
            lifes -= 10;
        }
    }


    //POOL LOGIC - creates bullet on position and stores on pool - destroys bullet - releases bullet
    private Shooting CreateB()
    {
        Shooting bulletCopy = Instantiate(bulletPrefab, this.gameObject.transform.position, Quaternion.identity);
        bulletCopy.MyPool = bulletPool;
        return bulletCopy;
    }

    private Shooting CreateT()
    {
        Shooting torpedoCopy = Instantiate(torpedoPrefab, this.gameObject.transform.position, Quaternion.identity);
        torpedoCopy.MyPool = torpedoPool;
        return torpedoCopy;
    }

    private void DestroyB(Shooting obj)
    {
        Destroy(obj.gameObject);
    }
    private void DestroyT(Shooting obj)
    {
        Destroy(obj.gameObject);
    }

    private void ReleaseB(Shooting obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void ReleaseT(Shooting obj)
    {
        obj.gameObject.SetActive(false);
    }
}

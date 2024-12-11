using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float shootingRate;
    [SerializeField] private Shooting bulletPrefab;
    [SerializeField] private Shooting bullet2Prefab;
    [SerializeField] private GameObject gun0;
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject[] status1SpawnPoints;
    [SerializeField] private GameObject shipBase;
    [SerializeField] private Sprite base4lifes;
    [SerializeField] private Sprite base3lifes;
    [SerializeField] private Sprite base2lifes;
    [SerializeField] private Sprite base1life;

    [SerializeField] private Animator gun0Animator;
    [SerializeField] private Animator gun1Animator;

    private float timer = 0.2f;
    private float lifes = 4;
    private ObjectPool<Shooting> bulletPool;
    private ObjectPool<Shooting> projectilePool;
    private Shooting currentBullet;
    private GameObject currentGun;

    private int playerStatus = 0;
    //0 - normal; 1 - 2bullets; 


    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
        projectilePool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStatus == 0)
        {
            gun0.SetActive(true);
            gun1.SetActive(false);
        } else if (playerStatus == 1)
        {
            gun0.SetActive(false);
            gun1.SetActive(true);
        }
        PlayerMovement();
        MovementLimits();
        Shoot();
    }

    private void PlayerMovement()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * speed * Time.deltaTime);
        
    }
    private void MovementLimits()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -8.31f, 8.48f);
        float clampedY = Mathf.Clamp(transform.position.y, -4.57f, 4.57f);
        transform.position = new Vector3(clampedX, clampedY, 0);
    }
    private void Shoot()
    {
        timer += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && timer > shootingRate)
        {
            if(playerStatus == 0)
            {
                gun0Animator.Play("ShootingCannon");
                Shooting bulletCopy = bulletPool.Get();
                bulletCopy.transform.position = gun0.transform.position;
                bulletCopy.gameObject.SetActive(true);
            }
            else if (playerStatus == 1)
            {
                gun1Animator.enabled = true;
                gun1Animator.Play("Gun2Animation");
                for (int i = 0; i < 2; i++)
                {
                    Shooting bulletCopy = projectilePool.Get();
                    bulletCopy.gameObject.SetActive(true);
                    bulletCopy.transform.position = status1SpawnPoints[i].transform.position;
                }
            }
            timer = 0;
        }
        else
        {
            if(playerStatus == 0 && !Input.GetKey(KeyCode.Space))
            {
                gun0Animator.Play("IdleCannon");
            }
            else if(playerStatus == 1 && !Input.GetKey(KeyCode.Space))
            {
                gun1Animator.Play("Gun2Idle");
                //gun1.GetComponent<SpriteRenderer>.sprite = gun1.GetComponent
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && playerStatus == 0)
        {
            //2 spawn points
            playerStatus = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && playerStatus == 1)
        {
            //1 spawn point
            playerStatus = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy"))
        {
            lifes--;
            Destroy(collision.gameObject);
        }
        if (lifes == 4)
        {
            shipBase.gameObject.GetComponent<SpriteRenderer>().sprite = base4lifes;
        }
        else if (lifes == 3)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base3lifes;
        }
        else if (lifes == 2)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base2lifes;
        }
        else if (lifes == 1)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base1life;
        }
        else if (lifes == 0)
        {
            Destroy(gameObject);
        }
    }

    //BULLET POOL
    private Shooting CreateB()
    {
        if (playerStatus == 0)
        {
            currentBullet = bulletPrefab;
            currentGun = gun0;
        }
        else if (playerStatus == 1)
        {
            currentBullet = bullet2Prefab;
            currentGun = gun1;
        }

        Shooting bulletCopy = Instantiate(currentBullet, currentGun.transform.position, Quaternion.Euler(0, 0, -90));

        if (playerStatus == 0)
        {
            bulletCopy.MyPool = bulletPool;
        } else if (playerStatus == 1)
        {
            bulletCopy.MyPool = projectilePool;
        }
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

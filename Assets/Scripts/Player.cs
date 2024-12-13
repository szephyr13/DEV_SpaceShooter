using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    //movement and visualization
    [SerializeField] private float speed;
    [SerializeField] private GameObject shipBase;
    [SerializeField] private Sprite base4lifes;
    [SerializeField] private Sprite base3lifes;
    [SerializeField] private Sprite base2lifes;
    [SerializeField] private Sprite base1life;
    private int lifes = 4;
    public int score = 0;
    //ui
    [SerializeField] private GameObject[] lifesUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    //gun
    [SerializeField] private Animator gun0Animator;
    [SerializeField] private Animator gun1Animator;
    [SerializeField] private GameObject gun0;
    [SerializeField] private GameObject gun1;
    private int playerStatus = 0;   //0 - normal; 1 - 2bullets; 
    private GameObject currentGun;

    //shooting
    [SerializeField] private float shootingRate;
    [SerializeField] private Shooting bulletPrefab;
    [SerializeField] private Shooting bullet2Prefab;
    [SerializeField] private GameObject[] status1SpawnPoints;
    private float timer = 0.2f;
    private ObjectPool<Shooting> bulletPool;
    private ObjectPool<Shooting> projectilePool;
    private Shooting currentBullet;


    //game over screen
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseMenu;


    

    void Start()
    {
        //pause time for mainMenu
        Time.timeScale = 0f;
        //creating pools on start
        bulletPool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
        projectilePool = new ObjectPool<Shooting>(CreateB, null, ReleaseB, DestroyB);
        //draws lifes
        UpdateLifes();
    }




    void Update()
    {

        //just debugging - mode alt by tab 
        if (Input.GetKeyDown(KeyCode.Tab) && playerStatus == 0)
        {
            playerStatus = 1;  //2 spawn points
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && playerStatus == 1)
        {
            playerStatus = 0; //1 spawn point
        }


        //sets gun checking status and active gun
        if (playerStatus == 0 && gun1.activeSelf)
        {
            gun0.SetActive(true);
            gun1.SetActive(false);
        } else if (playerStatus == 1 && gun0.activeSelf)
        {
            gun0.SetActive(false);
            gun1.SetActive(true);
        }


        //pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == false)
        {
            AudioManager.instance.PlaySFX("UISelect");
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == true)
        {
            AudioManager.instance.PlaySFX("UISelect");
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }

        PlayerMovement();
        MovementLimits();
        Shoot();
        scoreText.text = "Score: " + score;
    }



    private void UpdateLifes()
    {
        //adds up lifes
        for (int i = 0; i <= lifes; i++)
        {
            if(lifesUI[i].activeSelf == false)
            {
                lifesUI[i].SetActive(true);
            }
        }

        //deletes lifes
        if (lifesUI[lifes].activeSelf == true)
        {
            lifesUI[lifes].SetActive(false);
        }

    }



    //movement logic - input + limits
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




    //shooting logic
    // 1. timer for shooting limit
    // 2. animation logic for shooting and non-shooting
    // 3. bullet spawning and storing on pool 
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
                AudioManager.instance.PlaySFX("PlayerBullet");
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
                AudioManager.instance.PlaySFX("EnemyBullet");
            }
            timer = 0;
        }
        else
        { //just animation
            if(playerStatus == 0 && !Input.GetKey(KeyCode.Space))
            {
                gun0Animator.Play("IdleCannon");
            }
            else if(playerStatus == 1 && !Input.GetKey(KeyCode.Space))
            {
                gun1Animator.Play("Gun2Idle");
            }
        }
    }


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //trigger logic: life susbtraction + enemy animation
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            lifes--;
            AudioManager.instance.PlaySFX("EnemyBullet");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            lifes--;
            score += 15;
            AudioManager.instance.PlaySFX("EnemyExplosion");
            StartCoroutine(deathAnimation(collision.gameObject));
        }

        //life animation (on ship + on ui)
        if (lifes >= 4)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base4lifes;
            UpdateLifes();
        }
        else if (lifes == 3)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base3lifes;
            UpdateLifes();
            
        }
        else if (lifes == 2)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base2lifes;
            UpdateLifes();
        }
        else if (lifes == 1)
        {
            shipBase.GetComponent<SpriteRenderer>().sprite = base1life;
            UpdateLifes();
        }
        else if (lifes == 0)
        {
            AudioManager.instance.PlaySFX("YouLose");
            gameOverScreen.SetActive(true);
            UpdateLifes();
            Time.timeScale = 0f;
        }
    }


    //enemy death animation
    IEnumerator deathAnimation (GameObject collision)
    {
        Animator animator = collision.transform.GetChild(0).GetComponent<Animator>();
        animator.Play("EnemyFighterDestruction");
        yield return new WaitForSeconds(0.5f);
        Destroy(collision.gameObject);
    }

    //POOL LOGIC - creates bullet on position and stores on pool - destroys bullet - releases bullet
    // !! if structure thinking on which gun is being used
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

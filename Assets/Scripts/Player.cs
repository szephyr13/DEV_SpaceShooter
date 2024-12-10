using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float shootingRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject shipBase;
    [SerializeField] private Sprite base4lifes;
    [SerializeField] private Sprite base3lifes;
    [SerializeField] private Sprite base2lifes;
    [SerializeField] private Sprite base1life;
    private float timer = 0.5f;
    private float lifes = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);
            timer = 0;
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
}

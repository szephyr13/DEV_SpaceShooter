using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float shootingRate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject gun;
    private float timer = 0.5f;

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
}

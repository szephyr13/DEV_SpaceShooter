using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Player movement
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(inputH, inputV, 0).normalized * speed * Time.deltaTime);

        //Player limits
        float clampedY = Mathf.Clamp(inputV, -4.57f, 4.57f);
        float clampedX = Mathf.Clamp(inputH, -8.31f, 8.48f);
        transform.position = new Vector3(clampedX, clampedY, 0);

    }
}

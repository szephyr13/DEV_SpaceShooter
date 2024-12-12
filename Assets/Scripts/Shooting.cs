using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooting : MonoBehaviour
{
    //movement
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;

    //pool
    private ObjectPool<Shooting> myPool;
    public ObjectPool<Shooting> MyPool { get => myPool; set => myPool = value; }


    //set movement, release logic
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (this.gameObject.transform.position.x >= 10f)
        {
            MyPool.Release(this);
        }
    }
}

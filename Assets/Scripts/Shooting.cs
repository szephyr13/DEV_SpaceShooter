using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;

    private ObjectPool<Shooting> myPool;
    public ObjectPool<Shooting> MyPool { get => myPool; set => myPool = value; }

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (this.gameObject.transform.position.x >= 10f)
        {
            MyPool.Release(this);
        }
    }
}

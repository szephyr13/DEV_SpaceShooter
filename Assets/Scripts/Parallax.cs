using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float width;
    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.position;
    }


    void Update()
    {
        //How much remains to reach the end of the image
        float module = (speed * Time.time) % width;
        //Position refreshes from initial adding module info in desired direction
        transform.position = initialPosition + module * direction;
    }
}

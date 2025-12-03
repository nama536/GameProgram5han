using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float _bulletSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = transform.right * _bulletSpeed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBullet : MonoBehaviour
{
    //誰の弾か
    public enum WhoBullet
    {
        PlayerOne,
        PlayerTwo
    }
    public WhoBullet BulletType;

    Rigidbody2D rb;
    //弾速
    [SerializeField] float _bulletSpeed;

    private EGame _eGame;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = transform.right * _bulletSpeed;
        _eGame = FindFirstObjectByType<EGame>();
    }

    void Update()
    {
        if (!_eGame.OnGame)
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EPlayer ePlayer = collision.gameObject.GetComponent<EPlayer>();

            if(ePlayer.ThisPlayerCount == EPlayer.PlayerCount.PlayerOne && BulletType == WhoBullet.PlayerTwo)
            {
                _eGame.HPDown(EPlayer.PlayerCount.PlayerOne);
                Destroy(gameObject);
            }
            if(ePlayer.ThisPlayerCount == EPlayer.PlayerCount.PlayerTwo && BulletType == WhoBullet.PlayerOne)
            {
                _eGame.HPDown(EPlayer.PlayerCount.PlayerTwo);
                Destroy(gameObject);
            }
        }
    }
}

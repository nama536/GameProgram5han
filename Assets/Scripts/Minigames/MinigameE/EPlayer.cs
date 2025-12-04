using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EPlayer : MonoBehaviour
{
    //コントローラーの左スティック入力受け取り
    Vector2 axis;
    //プレイヤーの物理
    Rigidbody2D rb;
    //プレイヤーの速度とジャンプ力
    [SerializeField] float _playerSpeed;
    [SerializeField] float _playerJump;
    //弾
    [SerializeField] GameObject _bullet;

    bool _isGround = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMove();
    }

    //左スティック取得
    void OnMove(InputValue value)
    {
        axis = value.Get<Vector2>();
    }

    //横移動
    void PlayerMove()
    {
        rb.velocity = new Vector2(axis.x * _playerSpeed,rb.velocity.y);
    }

    //ジャンプ
    void OnJump()
    {
        if(_isGround == true)
        {
            rb.velocity = new Vector2(rb.velocity.x,_playerJump);
            _isGround = false;
        }
    }

    //射撃
    void OnShoot()
    {
        Debug.Log("ファイア");
        Instantiate(_bullet,transform.position + new Vector3(0.6f,0.0f,0.0f),transform.rotation);
    }

    //地面についたら
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" )
        {
            //飛べるようにする
           _isGround = true;
        }
    }
}

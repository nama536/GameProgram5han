using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EManager : MonoBehaviour
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMove();
    }

    void OnMove(InputValue value)
    {
        axis = value.Get<Vector2>();
    }

    void PlayerMove()
    {
        rb.velocity = new Vector2(axis.x * _playerSpeed,rb.velocity.y);
    }

    void OnJump()
    {
        rb.velocity = new Vector2(rb.velocity.x,_playerJump);
    }

    void OnShoot()
    {
        Debug.Log("ファイア");
        Instantiate(_bullet,transform.position,transform.rotation);
    }
}

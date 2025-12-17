using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EPlayer : MonoBehaviour
{
    //プレイヤーの判別
    public enum PlayerCount
    {
        PlayerOne,
        PlayerTwo
    }
    public PlayerCount ThisPlayerCount;
    //インプット
    [SerializeField] PlayerInput _playerInput;
    //プレイヤーの見た目
    public SpriteRenderer SpriteRenderer;

    //コントローラーの左スティック入力受け取り
    private Vector2 _axis;
    //プレイヤーの物理
    private Rigidbody2D _rb;
    //プレイヤーの速度とジャンプ力
    [SerializeField] float _playerSpeed;
    [SerializeField] float _playerJump;
    //弾
    [SerializeField] GameObject _bullet;
    //クールタイム
    private bool _canShoot = true;
    [SerializeField] float _coolTime;

    //地面に着いているか
    private bool _isGround = true;
    //どちらを向いているか
    private bool _doRight;

    private EGame _eGame;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _eGame = FindFirstObjectByType<EGame>();

        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            _doRight = true;
        }
        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            _doRight = false;
        }
    }

    void Update()
    {
        if (_eGame.OnGame)
        {
            PlayerMove();
        }
    }

    void OnReady()
    {
        Debug.Log("レディ");
        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            _eGame.PlayerDataManagers[0].Ready = true;
        }

        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            _eGame.PlayerDataManagers[1].Ready = true;
        }

        _eGame.DoReady(ThisPlayerCount);
        _playerInput.SwitchCurrentActionMap("EPlayer");
    }

    //左スティック取得
    void OnMove(InputValue value)
    {
        _axis = value.Get<Vector2>();
    }

    //横移動
    void PlayerMove()
    {
        _rb.velocity = new Vector2(_axis.x * _playerSpeed,_rb.velocity.y);
        Vector3 nowPos = this.transform.position;
        nowPos.x = Mathf.Clamp(nowPos.x,-7.0f,7.0f);
        this.transform.position = nowPos;
        
        //プレイヤーの向きを変える
        if(_axis.x > 0.0f)
        {
            this.transform.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
            _doRight = true;
        }
        else if(_axis.x < 0.0f)
        {
            this.transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);
            _doRight = false;
        }
    }

    //ジャンプ
    void OnJump()
    {
        if(_isGround == true && _eGame.OnGame)
        {
            _rb.velocity = new Vector2(_rb.velocity.x,_playerJump);
            _isGround = false;
        }
    }

    //射撃
    void OnShoot()
    {
        if(_doRight == true && _eGame.OnGame && _canShoot)
        {
            var bullet = Instantiate(_bullet,transform.position + new Vector3(0.8f,0.0f,0.0f),transform.rotation);

            EBullet eBullet = bullet.GetComponent<EBullet>();
            if(ThisPlayerCount == PlayerCount.PlayerOne)
            {
                eBullet.BulletType = EBullet.WhoBullet.PlayerOne;
            }
            if(ThisPlayerCount == PlayerCount.PlayerTwo)
            {
                eBullet.BulletType = EBullet.WhoBullet.PlayerTwo;
            }

            _canShoot = false;
            Invoke("CanShoot",_coolTime);
        }
        else if(_doRight == false && _eGame.OnGame && _canShoot)
        {
            var bullet = Instantiate(_bullet,transform.position + new Vector3(-0.8f,0.0f,0.0f),transform.rotation);

            EBullet eBullet = bullet.GetComponent<EBullet>();
            if(ThisPlayerCount == PlayerCount.PlayerOne)
            {
                eBullet.BulletType = EBullet.WhoBullet.PlayerOne;
            }
            if(ThisPlayerCount == PlayerCount.PlayerTwo)
            {
                eBullet.BulletType = EBullet.WhoBullet.PlayerTwo;
            }

            _canShoot = false;
            Invoke("CanShoot",_coolTime);
        }
    }

    void CanShoot()
    {
        _canShoot = true;
    }

    //地面についたら
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && _eGame.OnGame)
        {
            //飛べるようにする
           _isGround = true;
        }
    }
}

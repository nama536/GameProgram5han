using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DGame : MonoBehaviour
{
    //最初に表示されるパネル
    [SerializeField] GameObject _firstPanel;
    //ラウンド数を出すテキスト
    [SerializeField] TextMeshProUGUI _roundText;
    //準備中かどうか
    private bool _nowWaitReady = false;
    //遊び方説明パネル
    [SerializeField] GameObject _howToPlayPanel;
    //プレイヤーの見た目
    [SerializeField] Sprite[] _playerSprites;
    //準備中ボタン画像
    [SerializeField] Image[] _waitReadyImage;
    //カウントダウンテキスト
    [SerializeField] TextMeshProUGUI _countDownText;
    //ゲーム中かどうか
    public bool OnGame = false;

    //ゲームパネル
    [SerializeField] GameObject _onGamePanel;
    //プレイヤーのキャラ
    [SerializeField] GameObject _playerPrefab;
    //スコアカウント
    private int _playerOneScore;
    private int _playerTwoScore;
    [SerializeField] TextMeshProUGUI[] _scoreCountText;
    //タイマー
    [SerializeField] float _gameTime;
    private float _gameTimeCount;
    [SerializeField] TextMeshProUGUI _gameTimeCountText;
    [SerializeField] Image _gameTimeCircle;

    //穴
    public GameObject[] Holes;
    //位置情報
    public enum Place
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }

    //モグラ
    [SerializeField] GameObject _mole;
    private GameObject _inGameMole;
    //キャンバス
    [SerializeField] Canvas _canvas;

    //リザルト勝敗テキスト
    [SerializeField] TextMeshProUGUI[] _resultText;
    //リザルトプレイヤー画像
    [SerializeField] Image[] _resultPlayerImage;

    private DPlayer[] _dPlayers = new DPlayer[2];
    public PlayerDataManager[] PlayerDataManagers;

    void Start()
    {
        if (MainModeManager.instance.OnMainMode)
        {
            _roundText.gameObject.SetActive(true);
            _roundText.text = "Round " + MainModeManager.instance.RoundCount.ToString();
        }

        Invoke("HowToPlay",3.0f);

        _gameTimeCount = _gameTime;

        PlayerDataManagers[0].Ready = false;
        PlayerDataManagers[1].Ready = false;
    }

    void Update()
    {
        WaitReady();
        Timer();
        if (OnGame)
        {
            ChangePointerColor();
        }
    }

    void HowToPlay()
    {
        _firstPanel.SetActive(false);

        var plOne = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[1].PlayerDevice);

        _dPlayers[0] = plOne.GetComponent<DPlayer>();
        _dPlayers[1] = plTwo.GetComponent<DPlayer>();

        _dPlayers[0].ThisPlayerCount = DPlayer.PlayerCount.PlayerOne;
        _dPlayers[1].ThisPlayerCount = DPlayer.PlayerCount.PlayerTwo;

        _nowWaitReady = true;
    }

    public void DoReady(DPlayer.PlayerCount playerCount)
    {
        //プレイヤー1の準備完了の動き
        if(playerCount == DPlayer.PlayerCount.PlayerOne)
        {
            _waitReadyImage[0].sprite = _playerSprites[0];
        }
        //プレイヤー2の準備完了の動き
        if(playerCount == DPlayer.PlayerCount.PlayerTwo)
        {
            _waitReadyImage[1].sprite = _playerSprites[1];
        }
    }

    async void WaitReady()
    {
        if (_nowWaitReady && PlayerDataManagers[0].Ready && PlayerDataManagers[1].Ready)
        {
            await Task.Delay(1000);
            _howToPlayPanel.SetActive(false);
            StartCoroutine("GameStart");
            _nowWaitReady = false;
        }
    }

    IEnumerator GameStart()
    {
        _countDownText.gameObject.SetActive(true);
        _countDownText.text = "3";
        yield return new WaitForSeconds(1f);

        _countDownText.text = "2";
        yield return new WaitForSeconds(1f);

        _countDownText.text = "1";
        yield return new WaitForSeconds(1f);

        _countDownText.text = "Start";
        OnGame = true;
        yield return new WaitForSeconds(1f);

        _countDownText.gameObject.SetActive(false);
    }

    void Timer()
    {
        if (OnGame)
        {
            _gameTimeCount -= Time.deltaTime;
            _gameTimeCircle.fillAmount = _gameTimeCount / _gameTime;
            _gameTimeCountText.text = _gameTimeCount.ToString("F0");
        }

        if(_gameTimeCount <= 0f && OnGame)
        {
            _countDownText.gameObject.SetActive(true);
            _countDownText.text = "End";
            StartCoroutine("GameEnd");
            OnGame = false;
        }
    }

    void ChangePointerColor()
    {
        if(_dPlayers[0].PlayerPlace == _dPlayers[1].PlayerPlace)
        {
            _dPlayers[0].PointerImage.sprite = _playerSprites[8];
            _dPlayers[1].PointerImage.sprite = _playerSprites[8];
        }
        else
        {
            _dPlayers[0].PointerImage.sprite = _playerSprites[6];
            _dPlayers[1].PointerImage.sprite = _playerSprites[7];
        }
    }

    void SummonMole()
    {
        _inGameMole = Instantiate(_mole,_canvas.transform);
    }

    public void Hit(Place playerPlace,DPlayer.PlayerCount playerCount)
    {
        DMole dMole = _inGameMole.GetComponent<DMole>();

        if(dMole.MorePlace == playerPlace)
        {
            if(playerCount == DPlayer.PlayerCount.PlayerOne)
            {
                if (!dMole.IsRare)
                {
                    _playerOneScore++;
                }
                else if (dMole.IsRare)
                {
                    _playerOneScore += 3;
                }

                _scoreCountText[0].text = _playerOneScore.ToString();
            }

            if(playerCount == DPlayer.PlayerCount.PlayerTwo)
            {
                if (!dMole.IsRare)
                {
                    _playerTwoScore++;
                }
                else if (dMole.IsRare)
                {
                    _playerTwoScore += 3;
                }

                _scoreCountText[1].text = _playerTwoScore.ToString();
            }

            Destroy(_inGameMole);
            SummonMole();
        }
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(3f);

        _onGamePanel.SetActive(false);

        if(_playerOneScore > _playerTwoScore)
        {
            _resultText[0].text = "勝ち";
            _resultText[1].text = "負け";
            _resultPlayerImage[0].sprite = _playerSprites[2];
            _resultPlayerImage[1].sprite = _playerSprites[5];

            PlayerDataManagers[0].MainModeScore++;
        }
        if(_playerOneScore < _playerTwoScore)
        {
            _resultText[0].text = "負け";
            _resultText[1].text = "勝ち";
            _resultPlayerImage[1].sprite = _playerSprites[3];
            _resultPlayerImage[0].sprite = _playerSprites[4];

            PlayerDataManagers[1].MainModeScore++;
        }
        
        yield return new WaitForSeconds(3f);

        if (!MainModeManager.instance.OnMainMode)
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            _resultText[0].text = PlayerDataManagers[0].MainModeScore.ToString();
            _resultText[1].text = PlayerDataManagers[1].MainModeScore.ToString();

            _resultPlayerImage[0].sprite = _playerSprites[0];
            _resultPlayerImage[1].sprite = _playerSprites[1];
        }

        yield return new WaitForSeconds(3f);

        if(PlayerDataManagers[0].MainModeScore == 3 || PlayerDataManagers[1].MainModeScore == 3)
        {
            SceneManager.LoadScene("Result");
        }
        else
        {
            MainModeManager.instance.RandomStage();
        }
    }
}

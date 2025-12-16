using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AGame : MonoBehaviour
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

    //プレイヤーのキャラ
    [SerializeField] GameObject _playerPrefab;
    //プレイヤーの見た目
    [SerializeField] Image[] _playerOnGameImages;
    //プッシュカウント
    private int _playerOnePushCount;
    private int _playerTwoPushCount;
    [SerializeField] TextMeshProUGUI[] _pushCountText;
    //タイマー
    [SerializeField] float _gameTime;
    private float _gameTimeCount;
    [SerializeField] TextMeshProUGUI _gameTimeCountText;
    [SerializeField] Image _gameTimeCircle;
    //ゲームパネル
    [SerializeField] GameObject _onGamePanel;
    //リザルト勝敗テキスト
    [SerializeField] TextMeshProUGUI[] _resultText;
    //リザルトプレイヤー画像
    [SerializeField] Image[] _resultPlayerImage;

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
    }

    void HowToPlay()
    {
        _firstPanel.SetActive(false);

        var plOne = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[1].PlayerDevice);

        APlayer aPlayerOne = plOne.GetComponent<APlayer>();
        APlayer aPlayerTwo = plTwo.GetComponent<APlayer>();

        aPlayerOne.ThisPlayerCount = APlayer.PlayerCount.PlayerOne;
        aPlayerTwo.ThisPlayerCount = APlayer.PlayerCount.PlayerTwo;

        _nowWaitReady = true;
    }

    public void DoReady(APlayer.PlayerCount playerCount)
    {
        //プレイヤー1の準備完了の動き
        if(playerCount == APlayer.PlayerCount.PlayerOne)
        {
            _waitReadyImage[0].sprite = _playerSprites[0];
        }
        //プレイヤー2の準備完了の動き
        if(playerCount == APlayer.PlayerCount.PlayerTwo)
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

    public void PushCount(APlayer.PlayerCount playerCount,InputAction.CallbackContext context)
    {
        if(playerCount == APlayer.PlayerCount.PlayerOne)
        {
            if (context.started)
            {
                _playerOnePushCount++;
                _pushCountText[0].text = _playerOnePushCount.ToString();

                _playerOnGameImages[0].sprite = _playerSprites[8];
            }
            else if(context.canceled)
            {
                _playerOnGameImages[0].sprite = _playerSprites[6];
            }
        }

        if(playerCount == APlayer.PlayerCount.PlayerTwo)
        {
            if (context.started)
            {
                _playerTwoPushCount++;
                _pushCountText[1].text = _playerTwoPushCount.ToString();

                _playerOnGameImages[1].sprite = _playerSprites[9];
            }
            else if(context.canceled)
            {
                _playerOnGameImages[1].sprite = _playerSprites[7];
            }
        }
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

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(3f);

        _onGamePanel.SetActive(false);

        if(_playerOnePushCount > _playerTwoPushCount)
        {
            _resultText[0].text = "勝ち";
            _resultText[1].text = "負け";
            _resultPlayerImage[0].sprite = _playerSprites[2];
            _resultPlayerImage[1].sprite = _playerSprites[5];

            PlayerDataManagers[0].MainModeScore++;
        }
        if(_playerOnePushCount < _playerTwoPushCount)
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

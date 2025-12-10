using System.Collections;
using System.Collections.Generic;
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
    //カウントダウンテキスト
    [SerializeField] TextMeshProUGUI _countDownText;
    //ゲーム中かどうか
    public bool OnGame = false;

    //プレイヤーのキャラ
    [SerializeField] GameObject _playerPrefab;
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

        plOne.gameObject.transform.position = new Vector3(-3.5f,0.0f,0.0f);
        plTwo.gameObject.transform.position = new Vector3(3.5f,0.0f,0.0f);
        plTwo.transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);

        APlayer aPlayerOne = plOne.GetComponent<APlayer>();
        APlayer aPlayerTwo = plTwo.GetComponent<APlayer>();

        aPlayerOne.ThisPlayerCount = APlayer.PlayerCount.PlayerOne;
        aPlayerTwo.ThisPlayerCount = APlayer.PlayerCount.PlayerTwo;

        PlayerDataManagers[0].Ready = false;
        PlayerDataManagers[1].Ready = false;

        _nowWaitReady = true;
    }

    void WaitReady()
    {
        if (_nowWaitReady && PlayerDataManagers[0].Ready && PlayerDataManagers[1].Ready)
        {
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

    public void PushCount(APlayer.PlayerCount playerCount)
    {
        if(playerCount == APlayer.PlayerCount.PlayerOne)
        {
            _playerOnePushCount++;
            _pushCountText[0].text = _playerOnePushCount.ToString();
        }

        if(playerCount == APlayer.PlayerCount.PlayerTwo)
        {
            _playerTwoPushCount++;
            _pushCountText[1].text = _playerTwoPushCount.ToString();
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

            PlayerDataManagers[0].MainModeScore++;
        }
        if(_playerOnePushCount < _playerTwoPushCount)
        {
            _resultText[0].text = "負け";
            _resultText[1].text = "勝ち";

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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EGame : MonoBehaviour
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
    //キャンバス
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject _onGameUI;
    //カウントダウンテキスト
    [SerializeField] TextMeshProUGUI _countDownText;
    //ゲーム中かどうか
    public bool OnGame = false;
    //プレイヤーのキャラ
    [SerializeField] GameObject _playerPrefab;
    //HP
    private int _playerOneHP = 3;
    private int _playerTwoHP = 3;
    [SerializeField] TextMeshProUGUI[] _hPText;
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

        PlayerDataManagers[0].Ready = false;
        PlayerDataManagers[1].Ready = false;
    }


    void Update()
    {
        WaitReady();
        JudgeGameEnd();
    }

    void HowToPlay()
    {
        _firstPanel.SetActive(false);

        var plOne = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[1].PlayerDevice);

        plOne.gameObject.transform.position = new Vector3(-6.2f,-0.9f,0.0f);
        plTwo.gameObject.transform.position = new Vector3(6.2f,-0.9f,0.0f);
        plTwo.transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);

        EPlayer ePlayerOne = plOne.GetComponent<EPlayer>();
        EPlayer ePlayerTwo = plTwo.GetComponent<EPlayer>();

        ePlayerOne.ThisPlayerCount = EPlayer.PlayerCount.PlayerOne;
        ePlayerTwo.ThisPlayerCount = EPlayer.PlayerCount.PlayerTwo;

        ePlayerOne.SpriteRenderer.sprite = _playerSprites[6];
        ePlayerTwo.SpriteRenderer.sprite = _playerSprites[1];

        PlayerDataManagers[0].Ready = false;
        PlayerDataManagers[1].Ready = false;

        _nowWaitReady = true;
    }

    public void DoReady(EPlayer.PlayerCount playerCount)
    {
        //プレイヤー1の準備完了の動き
        if(playerCount == EPlayer.PlayerCount.PlayerOne)
        {
            _waitReadyImage[0].sprite = _playerSprites[0];
        }
        //プレイヤー2の準備完了の動き
        if(playerCount == EPlayer.PlayerCount.PlayerTwo)
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
            _canvas.SetActive(false);
            StartCoroutine("GameStart");
            _nowWaitReady = false;
        }
    }

    IEnumerator GameStart()
    {
        _onGameUI.SetActive(true);
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

    public void HPDown(EPlayer.PlayerCount playerCount)
    {
        if (playerCount == EPlayer.PlayerCount.PlayerOne)
        {
            _playerOneHP--;
            _hPText[0].text = _playerOneHP.ToString();
        }
        if(playerCount == EPlayer.PlayerCount.PlayerTwo)
        {
            _playerTwoHP--;
            _hPText[1].text = _playerTwoHP.ToString();
        }
    }

    void JudgeGameEnd()
    {
        if (OnGame)
        {
            if (_playerOneHP == 0 || _playerTwoHP == 0)
            {
                _countDownText.gameObject.SetActive(true);
                _countDownText.text = "End";
                StartCoroutine("GameEnd");
                OnGame = false;
            }
        }
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(3f);

        _onGameUI.SetActive(false);
        _canvas.SetActive(true);

        if(_playerOneHP > _playerTwoHP)
        {
            _resultText[0].text = "勝ち";
            _resultText[1].text = "負け";
            _resultPlayerImage[0].sprite = _playerSprites[2];
            _resultPlayerImage[1].sprite = _playerSprites[5];

            PlayerDataManagers[0].MainModeScore++;
        }
        if(_playerOneHP < _playerTwoHP)
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

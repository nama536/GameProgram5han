using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    //キャンバス
    [SerializeField] GameObject _canvas;
    //カウントダウンテキスト
    [SerializeField] TextMeshProUGUI _countDownText;
    //ゲーム中かどうか
    public bool OnGame = false;
    //プレイヤーのキャラ
    [SerializeField] GameObject _playerPrefab;

    public PlayerDataManager[] PlayerDataManagers;

    void Start()
    {
        if (MainModeManager.instance.OnMainMode)
        {
            _roundText.gameObject.SetActive(true);
            _roundText.text = "Round " + MainModeManager.instance.RoundCount.ToString();
        }

        Invoke("HowToPlay",3.0f);
    }


    void Update()
    {
        WaitReady();
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

        PlayerDataManagers[0].Ready = false;
        PlayerDataManagers[1].Ready = false;

        _nowWaitReady = true;
    }

    void WaitReady()
    {
        if (_nowWaitReady && PlayerDataManagers[0].Ready && PlayerDataManagers[1].Ready)
        {
            _howToPlayPanel.SetActive(false);
            _canvas.SetActive(false);
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
}

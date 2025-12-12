using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TitleManager : MonoBehaviour
{
    //準備完了待ち画面
    [SerializeField] GameObject _waitReadyPanel;
    //準備完了画面中か
    private bool _nowWaitReady = true;
    //準備待ちUI
    [SerializeField] GameObject[] _waitReadyUI;
    //プレイヤーのプレファブ
    [SerializeField] GameObject _playerPrefab;
    //プレイヤーの見た目
    [SerializeField] Sprite[] _playerSprites;
    //最初のセレクト画面
    [SerializeField] GameObject _firstSelect;
    //セレクトモードを選んだ時の画面
    [SerializeField] GameObject _selectMode;
    //メインモードボタン
    [SerializeField] Button _mainMode;
    //ゲームAボタン
    [SerializeField] Button _gameA;
    //プレイヤー1のインプット
    private PlayerInput[] _playerInputs;

    [SerializeField] PlayerDataManager[] _playerDataManagers;

    void Start()
    {
        JoinDevice();
    }

    void JoinDevice()
    {
        //デバイスを解除
        _playerDataManagers[0].PlayerDevice = null;
        _playerDataManagers[1].PlayerDevice = null;

        Debug.Log(InputSystem.devices.Count);

        foreach(var device in InputSystem.devices)
        {
            //ゲームパッドが繋がってたら
            if (device.name.Contains("Gamepad"))
            {
                if(_playerDataManagers[0].PlayerDevice == null)
                {
                    //一つ目のデバイスを登録
                    _playerDataManagers[0].PlayerDevice = device;
                    Debug.Log(_playerDataManagers[0].PlayerDevice);
                    Debug.Log("プレイヤー１");

                    //透明のプレイヤーを召喚
                    _playerInputs[0] = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:_playerDataManagers[0].PlayerDevice);
                    _playerInputs[0].gameObject.transform.position = new Vector3(-6f,-2.5f,0.0f);
                }
                else if(_playerDataManagers[1].PlayerDevice == null)
                {
                    //二つ目のデバイスを登録
                    _playerDataManagers[1].PlayerDevice = device;
                    Debug.Log(_playerDataManagers[1].PlayerDevice);
                    Debug.Log("プレイヤー２");

                    //透明のプレイヤーを召喚
                    _playerInputs[1] = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:_playerDataManagers[1].PlayerDevice);
                    _playerInputs[1].gameObject.transform.position = new Vector3(6f,-2.5f,0.0f);
                }
            }
        }
    }

    public void DoReady(TitlePlayer.PlayerCount playerCount)
    {
        //プレイヤー1の準備完了の動き
        if(playerCount == TitlePlayer.PlayerCount.PlayerOne)
        {
            SpriteRenderer spriteRenderer = _playerInputs[0].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = _playerSprites[0];

            _waitReadyUI[0].SetActive(false);
        }
        //プレイヤー2の準備完了の動き
        if(playerCount == TitlePlayer.PlayerCount.PlayerTwo)
        {
            SpriteRenderer spriteRenderer = _playerInputs[1].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = _playerSprites[1];

            _waitReadyUI[1].SetActive(false);
        }
    }

    async Task WaitReady()
    {
        if (_nowWaitReady && _playerDataManagers[0].Ready && _playerDataManagers[1].Ready)
        {
            await Task.Delay(1000);
            //準備中パネルを消す
            _waitReadyPanel.SetActive(false);
            FirstSelect();
            _nowWaitReady = false;
            //プレイヤー1の機種をタイトル画面の操作に使用
            _playerInputs[0].SwitchCurrentActionMap("UI");
        }
    }

    public void DoMainMode()
    {
        //ラウンド数とスコアを0にする
        MainModeManager.instance.RoundCount = 0;
        _playerDataManagers[0].MainModeScore = 0;
        _playerDataManagers[1].MainModeScore = 0;
        //メインモードを開始する
        MainModeManager.instance.OnMainMode = true;
        MainModeManager.instance.StartMainMode();
    }

    public void DoSelectMode()
    {
        _selectMode.SetActive(true);
        _firstSelect.SetActive(false);

        _gameA.Select();
    }

    public void FirstSelect()
    {
        _firstSelect.SetActive(true);
        _selectMode.SetActive(false);

        _mainMode.Select();
    }

    public void DoGameA()
    {
        SceneManager.LoadScene("MinigameA");
    }

    public void DoGameB()
    {
        SceneManager.LoadScene("MinigameB");
    }

    public void DoGameC()
    {
        SceneManager.LoadScene("MinigameC");
    }

    public void DoGameD()
    {
        SceneManager.LoadScene("MinigameD");
    }
    public void DoGameE()
    {
        SceneManager.LoadScene("MinigameE");
    }

    public void DoGameF()
    {
        SceneManager.LoadScene("MinigameF");
    }

    public void DoGameG()
    {
        SceneManager.LoadScene("MinigameG");
    }
}

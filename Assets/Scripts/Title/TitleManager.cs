using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TitleManager : MonoBehaviour
{
    //最初のセレクト画面
    [SerializeField] GameObject _firstSelect;
    //セレクトモードを選んだ時の画面
    [SerializeField] GameObject _selectMode;
    //メインモードボタン
    [SerializeField] Button _mainMode;
    //ゲームAボタン
    [SerializeField] Button _gameA;
    //タイトル画面の操作
    [SerializeField] PlayerInput _titleInput;

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

                    //プレイヤー1の機種をタイトル画面の操作に使用
                    _titleInput.SwitchCurrentControlScheme(_playerDataManagers[0].PlayerDevice);
                }
                else if(_playerDataManagers[1].PlayerDevice == null)
                {
                    //二つ目のデバイスを登録
                    _playerDataManagers[1].PlayerDevice = device;
                    Debug.Log(_playerDataManagers[1].PlayerDevice);
                    Debug.Log("プレイヤー２");
                }
            }
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

    public void BackToFirstSelect()
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

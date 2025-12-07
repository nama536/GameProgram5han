using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject _firstSelect;
    [SerializeField] GameObject _selectMode;
    [SerializeField] Button _mainMode;
    [SerializeField] Button _gameA;
    [SerializeField] PlayerInput _titleInput;

    [SerializeField] PlayerDataManager[] _playerDataManagers;

    void Start()
    {
        JoinDevice();
    }

    void JoinDevice()
    {
        _playerDataManagers[0].PlayerDevice = null;
        _playerDataManagers[1].PlayerDevice = null;

        foreach(var device in InputSystem.devices)
        {
            if (device.name.Contains("Gamepad"))
            {
                if(_playerDataManagers[0].PlayerDevice == null)
                {
                    _playerDataManagers[0].PlayerDevice = device;
                    Debug.Log(_playerDataManagers[0].PlayerDevice);

                    _titleInput.SwitchCurrentControlScheme(_playerDataManagers[0].PlayerDevice);
                }
                else if(_playerDataManagers[1].PlayerDevice == null)
                {
                    _playerDataManagers[1].PlayerDevice = device;
                    Debug.Log(_playerDataManagers[1].PlayerDevice);
                }
            }
        }
    }

    public void DoMainMode()
    {
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

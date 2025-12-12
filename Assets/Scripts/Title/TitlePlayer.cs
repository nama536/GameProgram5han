using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitlePlayer : MonoBehaviour
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

    private TitleManager _titleManager;

    void Start()
    {
        _titleManager = FindFirstObjectByType<TitleManager>();
    }

    void OnReady()
    {
        _titleManager.DoReady(ThisPlayerCount);

        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            _titleManager.PlayerDataManagers[0].Ready = true;
        }

        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            _titleManager.PlayerDataManagers[1].Ready = true;
        }
    }
}

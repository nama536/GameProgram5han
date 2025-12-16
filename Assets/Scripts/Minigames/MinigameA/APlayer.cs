using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class APlayer : MonoBehaviour
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

    private AGame _aGame;

    void Start()
    {
        _aGame = FindFirstObjectByType<AGame>();
    }

    public void OnReady()
    {
        Debug.Log("レディ");
        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            _aGame.PlayerDataManagers[0].Ready = true;
        }

        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            _aGame.PlayerDataManagers[1].Ready = true;
        }

        _aGame.DoReady(ThisPlayerCount);
        _playerInput.SwitchCurrentActionMap("APlayer");
    }

    public void OnPush(InputAction.CallbackContext context)
    {
        if (_aGame.OnGame)
        {
            _aGame.PushCount(ThisPlayerCount,context);
        }
    }
}

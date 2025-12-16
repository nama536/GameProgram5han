using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BPlayer : MonoBehaviour
{
    //�v���C���[�̔���

    public enum PlayerCount

    {

        PlayerOne,

        PlayerTwo

    }

    public PlayerCount ThisPlayerCount;
    private Game game;

    [SerializeField] PlayerInput _playerInput;

    private void Start()
    {
        game = FindFirstObjectByType<Game>();
    }

    public void OnReady()
    {
        Debug.Log("レディ");
        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            game.PlayerDataManagers[0].Ready = true;
        }

        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            game.PlayerDataManagers[1].Ready = true;
        }

        game.DoReady(ThisPlayerCount);
        _playerInput.SwitchCurrentActionMap("BPlayer");
    }

    public void OnPush(InputAction.CallbackContext context)
    {
        // �v���C���[�ԍ���Game�ɑ���
        if (ThisPlayerCount == PlayerCount.PlayerOne)
        {
            game.WhoPush(ThisPlayerCount,context);
        }
        else
        {
            game.WhoPush(ThisPlayerCount,context);
        }
    }
}

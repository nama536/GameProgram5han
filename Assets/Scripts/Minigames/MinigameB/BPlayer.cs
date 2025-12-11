using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPlayer : MonoBehaviour
{
    //プレイヤーの判別

    public enum PlayerCount

    {

        PlayerOne,

        PlayerTwo

    }

    public PlayerCount ThisPlayerCount;
    private Game game;
    private void Start()
    {
        game = FindFirstObjectByType<Game>();

    }
    void OnPush()
    {
        // プレイヤー番号をGameに送る
        if (ThisPlayerCount == PlayerCount.PlayerOne)
        {
            game.WhoPush(1);
        }
        else
        {
            game.WhoPush(2);
        }
    }
}

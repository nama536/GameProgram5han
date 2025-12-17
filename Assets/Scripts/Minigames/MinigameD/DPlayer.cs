using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DPlayer : MonoBehaviour
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

    public DGame.Place PlayerPlace;

    [SerializeField] EventSystem _eventSystem;
    [SerializeField] RectTransform _rectTransform;

    public Image PointerImage;

    private Button[] _holes = new Button[2];

    private DGame _dGame;

    void Start()
    {
        _dGame = FindFirstObjectByType<DGame>();
        _holes[0] = _dGame.Holes[6].GetComponent<Button>();
        _holes[1] = _dGame.Holes[8].GetComponent<Button>();

        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            _holes[0].Select();
        }
        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            _holes[1].Select();
        }
    }

    void Update()
    {
        if (_dGame.OnGame)
        {
            MovePointer();
        }
    }

    public void OnReady()
    {
        Debug.Log("レディ");
        if(ThisPlayerCount == PlayerCount.PlayerOne)
        {
            _dGame.PlayerDataManagers[0].Ready = true;
        }

        if(ThisPlayerCount == PlayerCount.PlayerTwo)
        {
            _dGame.PlayerDataManagers[1].Ready = true;
        }

        _dGame.DoReady(ThisPlayerCount);
        _playerInput.SwitchCurrentActionMap("DPlayer");
    }

    void MovePointer()
    {
        if(_eventSystem.currentSelectedGameObject == _dGame.Holes[0])
        {
            PlayerPlace = DGame.Place.One;
            _rectTransform.anchoredPosition = new Vector2(-500f,250f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[1])
        {
            PlayerPlace = DGame.Place.Two;
            _rectTransform.anchoredPosition = new Vector2(0.0f,250f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[2])
        {
            PlayerPlace = DGame.Place.Three;
            _rectTransform.anchoredPosition = new Vector2(500f,250f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[3])
        {
            PlayerPlace = DGame.Place.Four;
            _rectTransform.anchoredPosition = new Vector2(-500f,-50f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[4])
        {
            PlayerPlace = DGame.Place.Five;
            _rectTransform.anchoredPosition = new Vector2(0.0f,-50f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[5])
        {
            PlayerPlace = DGame.Place.Six;
            _rectTransform.anchoredPosition = new Vector2(500f,-50f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[6])
        {
            PlayerPlace = DGame.Place.Seven;
            _rectTransform.anchoredPosition = new Vector2(-500f,-350f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[7])
        {
            PlayerPlace = DGame.Place.Eight;
            _rectTransform.anchoredPosition = new Vector2(0.0f,-350f);
        }
        else if(_eventSystem.currentSelectedGameObject == _dGame.Holes[8])
        {
            PlayerPlace = DGame.Place.Nine;
            _rectTransform.anchoredPosition = new Vector2(500f,-350f);
        }
    }

    public void OnHit()
    {
        if (_dGame.OnGame)
        {
            _dGame.Hit(PlayerPlace,ThisPlayerCount);
        }
    }
}

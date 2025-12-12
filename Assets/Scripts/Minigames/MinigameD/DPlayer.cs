using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DPlayer : MonoBehaviour
{
    public DGame.Place PlayerPlace;

    [SerializeField] EventSystem _eventSystem;
    [SerializeField] RectTransform _rectTransform;

    private DGame _dGame;

    void Start()
    {
        _dGame = FindFirstObjectByType<DGame>();
    }

    void Update()
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

    public void OnSubmit()
    {
        _dGame.Hit(PlayerPlace);
    }
}

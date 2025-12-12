using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMole : MonoBehaviour
{
    //モグラの位置
    public DGame.Place MorePlace;
    //レアモグラかどうか
    public bool IsRare = false;
    //見た目
    [SerializeField] Image _image;
    [SerializeField] Sprite _rareMoleSprite;

    [SerializeField] RectTransform _rectTransform;

    void Awake()
    {
        int i = Random.Range(0,9);
        switch (i)
        {
            case 0:
                MorePlace = DGame.Place.One;
                _rectTransform.anchoredPosition = new Vector2(-500f,250f);
                break;
            case 1:
                MorePlace = DGame.Place.Two;
                _rectTransform.anchoredPosition = new Vector2(0f,250f);
                break;
            case 2:
                MorePlace = DGame.Place.Three;
                _rectTransform.anchoredPosition = new Vector2(500f,250f);
                break;
            case 3:
                MorePlace = DGame.Place.Four;
                _rectTransform.anchoredPosition = new Vector2(-500f,-50f);
                break;
            case 4:
                MorePlace = DGame.Place.Five;
                _rectTransform.anchoredPosition = new Vector2(0f,-50f);
                break;
            case 5:
                MorePlace = DGame.Place.Six;
                _rectTransform.anchoredPosition = new Vector2(500f,-50f);
                break;
            case 6:
                MorePlace = DGame.Place.Seven;
                _rectTransform.anchoredPosition = new Vector2(-500f,-350f);
                break;
            case 7:
                MorePlace = DGame.Place.Eight;
                _rectTransform.anchoredPosition = new Vector2(0f,-350f);
                break;
            case 8:
                MorePlace = DGame.Place.Nine;
                _rectTransform.anchoredPosition = new Vector2(500f,-350f);
                break;
        }

        int x = Random.Range(0,10);
        if(x <= 1)
        {
            IsRare = true;
            //_image.sprite = _rareMoleSprite;
            _image.color = Color.yellow;
        }
    }
}

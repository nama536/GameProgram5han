using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DGame : MonoBehaviour
{
    //穴
    public GameObject[] Holes;
    //位置情報
    public enum Place
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }

    //モグラ
    [SerializeField] GameObject _hole;
    private List<GameObject> _moles = new List<GameObject>();
    //キャンバス
    [SerializeField] Canvas _canvas;

    void Start()
    {
        SummonMole();
    }

    void SummonMole()
    {
        GameObject i = Instantiate(_hole,_canvas.transform);
        _moles.Add(i);
    }

    public void Hit(Place playerPlace)
    {
        DMole dMole = _moles[0].GetComponent<DMole>();

        if(dMole.MorePlace == playerPlace)
        {
            Destroy(_moles[0]);
            SummonMole();
        }
    }
}

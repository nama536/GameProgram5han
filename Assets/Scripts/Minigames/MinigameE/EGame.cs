using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EGame : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;

    [SerializeField] PlayerDataManager[] _playerDataManagers;

    void Start()
    {
        var plOne = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:_playerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:_playerDataManagers[1].PlayerDevice);

        plOne.gameObject.transform.position = new Vector3(-6.2f,-0.9f,0.0f);
        plTwo.gameObject.transform.position = new Vector3(6.2f,-0.9f,0.0f);
        plTwo.transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);
    }
}

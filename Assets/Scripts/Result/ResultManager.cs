using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject _playerOneWinPanel;
    [SerializeField] GameObject _playerTwoWinPanel;

    [SerializeField] PlayerDataManager[] _playerDataManagers;

    void Start()
    {
        StartCoroutine(nameof(Result));
    }

    IEnumerator Result()
    {
        if(_playerDataManagers[0].MainModeScore > _playerDataManagers[1].MainModeScore)
        {
            _playerOneWinPanel.SetActive(true);
        }
        else if(_playerDataManagers[0].MainModeScore < _playerDataManagers[1].MainModeScore)
        {
            _playerTwoWinPanel.SetActive(true);
        }

        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("Title");
    }
}

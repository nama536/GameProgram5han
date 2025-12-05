using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject _firstSelect;
    [SerializeField] GameObject _selectMode;
    [SerializeField] Button _mainMode;
    [SerializeField] Button _gameA;

    private MainModeManager _mainModeManager;

    void Start()
    {
        _mainModeManager = FindFirstObjectByType<MainModeManager>();
    }

    public void DoMainMode()
    {
        _mainModeManager.StartMainMode();
    }

    public void DoSelectMode()
    {
        _selectMode.SetActive(true);
        _firstSelect.SetActive(false);

        _gameA.Select();
    }

    public void BackToFirstSelect()
    {
        _firstSelect.SetActive(true);
        _selectMode.SetActive(false);

        _mainMode.Select();
    }

    public void DoGameA()
    {
        SceneManager.LoadScene("MinigameA");
    }

    public void DoGameB()
    {
        SceneManager.LoadScene("MinigameB");
    }

    public void DoGameC()
    {
        SceneManager.LoadScene("MinigameC");
    }

    public void DoGameD()
    {
        SceneManager.LoadScene("MinigameD");
    }
    public void DoGameE()
    {
        SceneManager.LoadScene("MinigameE");
    }

    public void DoGameF()
    {
        SceneManager.LoadScene("MinigameF");
    }

    public void DoGameG()
    {
        SceneManager.LoadScene("MinigameG");
    }
}

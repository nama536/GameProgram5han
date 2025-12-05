using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainModeManager : MonoBehaviour
{
    static public MainModeManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public bool OnMainMode = false;

    [SerializeField] List<int> _stageNumber = new List<int>();
    private int _stageCount = 7;

    public void StartMainMode()
    {
        OnMainMode = true;

        for(int i = 0;i < _stageCount;i++)
        {
            _stageNumber.Add(i);
            Debug.Log(i);
        }

        RandomStage();
    }

    void RandomStage()
    {
        if(_stageNumber != null)
        {
            int i = Random.Range(0,_stageNumber.Count);

            switch (_stageNumber[i])
            {
                case 0:
                    SceneManager.LoadScene("MinigameA");
                    break;
                case 1:
                    SceneManager.LoadScene("MinigameB");
                    break;
                case 2:
                    SceneManager.LoadScene("MinigameC");
                    break;
                case 3:
                    SceneManager.LoadScene("MinigameD");
                    break;
                case 4:
                    SceneManager.LoadScene("MinigameE");
                    break;
                case 5:
                    SceneManager.LoadScene("MinigameF");
                    break;
                case 6:
                    SceneManager.LoadScene("MinigameG");
                    break;
            }

            _stageNumber.Remove(i);
        }
    }
}

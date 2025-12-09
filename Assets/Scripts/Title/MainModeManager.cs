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

    //今メインモードかどうか
    public bool OnMainMode = false;

    //ステージのID
    [SerializeField] List<int> _stageNumber = new List<int>();
    //ステージ数
    private int _stageCount = 7;
    //現在のラウンド数
    public int RoundCount = 0;

    public void StartMainMode()
    {
        _stageNumber.Clear();

        for(int i = 0;i < _stageCount;i++)
        {
            //ステージのIDをリストに入れる
            _stageNumber.Add(i);
            Debug.Log(i);
        }

        RandomStage();
    }

    void RandomStage()
    {
        //まだやってないステージがあったら
        if(_stageNumber != null)
        {
            //リストからランダムなシーンを読み込む
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

            //ラウンド数をプラス
            RoundCount++;
            //読み込んだステージをリストから削除
            _stageNumber.Remove(i);
        }
        else
        {
            //もうやってないステージがなかったらリストに全部また入れる
            StartMainMode();
        }
    }
}

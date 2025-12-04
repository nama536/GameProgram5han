using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "PlayerDataScriptableObject")]
public class PlayerDataManager : ScriptableObject
{
    //プレイヤーのID情報
    public int PlayerID;
    //プレイヤーのデバイス情報
    public InputDevice PlayerDevice;
    //プレイヤーの選択キャラクター
    public GameObject PlayerPrefab;
}

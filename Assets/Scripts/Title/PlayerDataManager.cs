using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "PlayerDataScriptableObject")]
public class PlayerDataManager : ScriptableObject
{
    //プレイヤーのデバイス情報
    public InputDevice PlayerDevice;
    //プレイヤーが準備できたか
    public bool Ready = false;
}

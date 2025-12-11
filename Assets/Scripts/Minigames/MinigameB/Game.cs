using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Star Settings")]
    [SerializeField] Image starImage;
    [SerializeField] Color normalColor;
    [SerializeField] Color glowColor;

    [Header("Player Images")]
    [SerializeField] Image player1Image;
    [SerializeField] Image player2Image;

    [Header("Star Move Targets")]
    [SerializeField] Transform player1Target;
    [SerializeField] Transform player2Target;

    [Header("Player Prefab")]
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] float drawThreshold = 0.06f;

    public PlayerDataManager[] PlayerDataManagers;

    private bool waiting = false;
    private bool canPress = false;
    private bool resultShown = false;

    private bool p1Penalty = false;
    private bool p2Penalty = false;

    private bool movingStar = false;
    private Transform moveTarget;
    private float p1PressTime = -1f;
    private float p2PressTime = -1f;

    private float startTime;

    void Start()
    {
        // プレイヤー生成
        var plOne = PlayerInput.Instantiate(_playerPrefab, pairWithDevice: PlayerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab, pairWithDevice: PlayerDataManagers[1].PlayerDevice);

        // プレイヤーの番号を割り当てる
        plOne.gameObject.GetComponent<BPlayer>().ThisPlayerCount = BPlayer.PlayerCount.PlayerOne;
        plTwo.gameObject.GetComponent<BPlayer>().ThisPlayerCount = BPlayer.PlayerCount.PlayerTwo;

        StartGame();
    }

    public void StartGame()
    {
        starImage.color = normalColor;
        starImage.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);

        waiting = true;
        canPress = false;
        resultShown = false;
        movingStar = false;

        float waitTime = Random.Range(3f, 15f);
        Invoke(nameof(LightUpStar), waitTime);
    }

    void LightUpStar()
    {
        if (!waiting) return;

        starImage.color = glowColor;
        canPress = true;
    }

    public void WhoPush(int player)
    {
        if (resultShown) return;

        // フライング
        if (waiting && !canPress)
        {
            ApplyPenalty(player);
            return;
        }

        // ペナルティ中は無効
        if (player == 1 && p1Penalty) return;
        if (player == 2 && p2Penalty) return;

        // 星が光っていない時は無効
        if (!canPress) return;

        // ★ 押した時間を記録
        float now = Time.time;
        if (player == 1) p1PressTime = now;
        else p2PressTime = now;

        // ★ 両者押したかチェック
        if (p1PressTime > 0 && p2PressTime > 0)
        {
            CheckDraw();
            return;
        }

        // まだ片方しか押していない → 保留
    }


    void DecideWinner(int player)
    {
        resultShown = true;
        waiting = false;
        canPress = false;

        if (player == 1)
            moveTarget = player1Target;
        else
            moveTarget = player2Target;

        movingStar = true;
    }

    void Update()
    {
        if (!movingStar) return;

        starImage.transform.position = Vector3.Lerp(
            starImage.transform.position,
            moveTarget.position,
            5f * Time.deltaTime
        );
    }

    void ApplyPenalty(int player)
    {
        if (player == 1 && !p1Penalty)
        {
            p1Penalty = true;
            player1Image.color = Color.gray;
            Invoke(nameof(ResetP1Penalty), 3f);
        }
        else if (player == 2 && !p2Penalty)
        {
            p2Penalty = true;
            player2Image.color = Color.gray;
            Invoke(nameof(ResetP2Penalty), 3f);
        }
    }

    void ResetP1Penalty()
    {
        p1Penalty = false;
        player1Image.color = Color.white;
    }

    void ResetP2Penalty()
    {
        p2Penalty = false;
        player2Image.color = Color.white;
    }
    void CheckDraw()
    {
        float diff = Mathf.Abs(p1PressTime - p2PressTime);

        if (diff <= drawThreshold)
        {
            // ★引き分け処理
            resultShown = true;
            waiting = false;
            canPress = false;

            Debug.Log("引き分け！");

            // 星を中央に戻して白にして終了など
            starImage.color = normalColor;
            movingStar = false;

            return;
        }

        // どちらが早いか判定
        if (p1PressTime < p2PressTime)
            DecideWinner(1);
        else
            DecideWinner(2);
    }
}

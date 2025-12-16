using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //最初に表示されるパネル
    [SerializeField] GameObject _firstPanel;
    //ラウンド数を出すテキスト
    [SerializeField] TextMeshProUGUI _roundText;
    //準備中かどうか
    private bool _nowWaitReady = false;
    //遊び方説明パネル
    [SerializeField] GameObject _howToPlayPanel;
    //プレイヤーの見た目
    [SerializeField] Sprite[] _playerSprites;
    //準備中ボタン画像
    [SerializeField] Image[] _waitReadyImage;
    //カウントダウンテキスト
    [SerializeField] TextMeshProUGUI _countDownText;
    //ゲーム中かどうか
    public bool OnGame = false;

    [Header("Star Settings")]
    [SerializeField] Image starImage;

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
    private float p1PressTime;
    private float p2PressTime;
    private float reactionTime;

    [SerializeField] TextMeshProUGUI[] _pushCountTexts;

    void Start()
    {
        /*// �v���C���[����
        var plOne = PlayerInput.Instantiate(_playerPrefab, pairWithDevice: PlayerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab, pairWithDevice: PlayerDataManagers[1].PlayerDevice);

        // �v���C���[�̔ԍ������蓖�Ă�
        plOne.gameObject.GetComponent<BPlayer>().ThisPlayerCount = BPlayer.PlayerCount.PlayerOne;
        plTwo.gameObject.GetComponent<BPlayer>().ThisPlayerCount = BPlayer.PlayerCount.PlayerTwo;

        StartGame();*/

        if (MainModeManager.instance.OnMainMode)
        {
            _roundText.gameObject.SetActive(true);
            _roundText.text = "Round " + MainModeManager.instance.RoundCount.ToString();
        }

        Invoke("HowToPlay",3.0f);

        PlayerDataManagers[0].Ready = false;
        PlayerDataManagers[1].Ready = false;
    }

    void Update()
    {
        /*if (!movingStar) return;

        starImage.transform.position = Vector3.Lerp(
            starImage.transform.position,
            moveTarget.position,
            5f * Time.deltaTime
        );*/

        WaitReady();
    }

    void HowToPlay()
    {
        _firstPanel.SetActive(false);

        var plOne = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[0].PlayerDevice);
        var plTwo = PlayerInput.Instantiate(_playerPrefab,pairWithDevice:PlayerDataManagers[1].PlayerDevice);

        BPlayer bPlayerOne = plOne.GetComponent<BPlayer>();
        BPlayer bPlayerTwo = plTwo.GetComponent<BPlayer>();

        bPlayerOne.ThisPlayerCount = BPlayer.PlayerCount.PlayerOne;
        bPlayerTwo.ThisPlayerCount = BPlayer.PlayerCount.PlayerTwo;

        _nowWaitReady = true;
    }

    public void DoReady(BPlayer.PlayerCount playerCount)
    {
        //プレイヤー1の準備完了の動き
        if(playerCount == BPlayer.PlayerCount.PlayerOne)
        {
            _waitReadyImage[0].sprite = _playerSprites[0];
        }
        //プレイヤー2の準備完了の動き
        if(playerCount == BPlayer.PlayerCount.PlayerTwo)
        {
            _waitReadyImage[1].sprite = _playerSprites[1];
        }
    }

    async void WaitReady()
    {
        if (_nowWaitReady && PlayerDataManagers[0].Ready && PlayerDataManagers[1].Ready)
        {
            await Task.Delay(1000);
            _howToPlayPanel.SetActive(false);
            StartCoroutine("GameStart");
            _nowWaitReady = false;
        }
    }

    IEnumerator GameStart()
    {
        _countDownText.gameObject.SetActive(true);
        _countDownText.text = "3";
        yield return new WaitForSeconds(1f);

        _countDownText.text = "2";
        yield return new WaitForSeconds(1f);

        _countDownText.text = "1";
        yield return new WaitForSeconds(1f);

        _countDownText.text = "Start";
        OnGame = true;

        float waitTime = Random.Range(3f, 15f);
        Invoke(nameof(LightUpStar), waitTime);
        yield return new WaitForSeconds(1f);

        _countDownText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        /*waiting = true;
        canPress = false;
        resultShown = false;
        movingStar = false;*/
    }

    void LightUpStar()
    {
        starImage.color = Color.yellow;
        canPress = true;
    }



    public void WhoPush(BPlayer.PlayerCount playerCount,InputAction.CallbackContext context)
    {
        /*if (resultShown) return;

        // �t���C���O
        if (waiting && !canPress)
        {
            ApplyPenalty(player);
            return;
        }

        // �y�i���e�B���͖���
        if (player == 1 && p1Penalty) return;
        if (player == 2 && p2Penalty) return;

        // ���������Ă��Ȃ����͖���
        if (!canPress) return;

        // �� ���������Ԃ��L�^
        float now = Time.time;
        if (player == 1) p1PressTime = now;
        else p2PressTime = now;

        // �� ���҉��������`�F�b�N
        if (p1PressTime > 0 && p2PressTime > 0)
        {
            CheckDraw();
            return;
        }

        // �܂��Е����������Ă��Ȃ� �� �ۗ�*/

        if(playerCount == BPlayer.PlayerCount.PlayerOne)
        {
            if (context.started)
            {
                if (canPress)
                {
                    player1Image.sprite = _playerSprites[8];

                    p1PressTime = reactionTime;
                    _pushCountTexts[0].text = p1PressTime.ToString();
                }
            }
        }

        if(playerCount == BPlayer.PlayerCount.PlayerTwo)
        {
            if (context.started)
            {
                if (canPress)
                {
                    player1Image.sprite = _playerSprites[9];

                    p2PressTime = reactionTime;
                    _pushCountTexts[1].text = p2PressTime.ToString();
                }
            }
        }
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
    /*void CheckDraw()
    {
        float diff = Mathf.Abs(p1PressTime - p2PressTime);

        if (diff <= drawThreshold)
        {
            // ��������������
            resultShown = true;
            waiting = false;
            canPress = false;

            Debug.Log("���������I");

            // ���𒆉��ɖ߂��Ĕ��ɂ��ďI���Ȃ�
            starImage.color = normalColor;
            movingStar = false;

            return;
        }

        // �ǂ��炪����������
        if (p1PressTime < p2PressTime)
            DecideWinner(1);
        else
            DecideWinner(2);
    }*/
}

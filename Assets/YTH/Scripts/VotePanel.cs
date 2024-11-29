using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    private PlayerType _playerType; // Duck�� ���Ǿ�

    [SerializeField] GameObject[] _panelList; // PlayerActorNumber �ε����� �̸� �����ص� �гε��� ����Ʈ�� ��� ����

    [SerializeField] GameObject[] _SkipAnonymImage; // ��ŵ �� ��ŭ �͸� �̹��� ����

    [SerializeField] GameObject[] _panelAnonymImage; // 2���� �迭 �̿��Ͽ� ���� ��ȹ

    [SerializeField] VoteScenePlayerData[] _playerData;

    [SerializeField] Button[] _voteButtons; // ��ǥ�ϱ� ���� ��ư��

    #region UI
    [Header("UI")]
    // [SerializeField] GameObject _characterImage; // ��ǥâ���� �� �÷��̾� ĳ���� �̹���

    //  [SerializeField] Image _voteSignImage; // ��ǥ�� �÷��̾� ǥ�� �̹���

    //  [SerializeField] Image _deadSignImage; // ���� ���� ǥ�� �̹���

    [SerializeField] TMP_Text _nickNameText; // �� �÷��̾� �г��� �ؽ�Ʈ

    [SerializeField] TMP_Text _stateText;

    [SerializeField] GameObject _votePanel; // ��ǥâ ��ü �г�

    [SerializeField] GameObject _playerPanel; // �� �÷��̾� �г�

    [SerializeField] public Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // �Ű��ڸ� ���� �� �ִ� �ð� ī��Ʈ

    [SerializeField] Slider _voteTimeCountSlider; // ��ǥ ���� �ð� ī��Ʈ
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _reportTimeCountSlider.maxValue = _voteData.ReportTimeCount;
        _voteTimeCountSlider.maxValue = _voteData.VoteTimeCount;

        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        _voteTimeCountSlider.value = _voteData.VoteTimeCount;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            SpawnPlayerPanel();
        }
     

        // ��ǥ�� ���� �� ��ǥ ���� false�� �ʱ�ȭ
        //for (int i = 0; i < 12; i++)
        //{
        //    _playerData[i].DidVote = false;
        //}
    }
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false; // ����� ��

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayerPanel();
    }


    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);

        CountTime();
    }

    // �� �÷��̾� �г��� �����ϴ� �Լ�
    private void SetPlayerPanel(GameObject[] panelList)
    {
        // _nickNameText.text = player.NickName; // �г��� �ҷ�����
        //TODO : _characterImage = ""; // ĳ���� �̹��� �ҷ�����
        //TODO : ���� ĳ���Ϳ� ��� ǥ�� �������
        //TODO : �÷��̾ ���� ���¶�� �� �÷��̾� ��ǥ ��ư ��Ȱ��ȭ // button.interatable == false
    }

    // �÷��̾� �г� ���� �Լ�
    private void SpawnPlayerPanel()
    {
        photonView.RPC("SpawnPlayerPanelRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

    [PunRPC]
    public void SpawnPlayerPanelRPC(int index)
    {
        Debug.Log("11111111111111111111111111111");
        _panelList[index].SetActive(true);
        _panelList[index].GetComponent<VoteScenePlayerData>().VoteButton.onClick.AddListener(() => { _voteManager.Vote(index); });
    }


    //��ǥ ���� �� ��ŵ �� ��ŭ �͸� �̹��� ����
    private void SpawnSkipAnonymImage()
    {
        photonView.RPC("SpawnSkipAnonymImageRPC", RpcTarget.All, _voteData.SkipCount);
    }



    [PunRPC]
    public void SpawnSkipAnonymImageRPC(int index)
    {
        for (int i = 0; i < index; i++)
        {
            _SkipAnonymImage[i].SetActive(true);
        }
    }

    //��ǥ ���� �� ��ǥ �� ��ŭ �÷��̾� �гο� �͸� �̹��� ����
    private void SpawnAnonymImage()
    {
        photonView.RPC("SpawnAnonymImageRPC", RpcTarget.All, _voteManager._voteCounts);
    }

    [PunRPC]
    public void SpawnAnonymImageRPC(int index)
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < index; j++)
            {
                //TODO : ��ǥ����ŭ Ȱ��ȭ, 2���� �迭 ���
            }
        }
    }

    // �ð� ���� �Լ�
    private void CountTime()
    {
        foreach (Button button in _voteButtons)
        {
            button.interactable = false;
            _skipButton.interactable = false;
        }


        _voteData.ReportTimeCount -= (float)Time.deltaTime; // Time.deltaTime ���� �ʿ� �� ����
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        if (_voteData.ReportTimeCount <= 0) // ����Ʈ Ÿ�� ���� �� ��ǥ, ��ŵ ��ư Ȱ��ȭ
        {
            _stateText.text = "VOTE!";
            foreach (Button button in _voteButtons)
            {
                button.interactable = true;
                _skipButton.interactable = true;
            }
            _reportTimeCountSlider.gameObject.SetActive(false); // ���� ������ ��
            _voteData.VoteTimeCount -= (float)Time.deltaTime;
            _voteTimeCountSlider.value = _voteData._voteTimeCount;
            if (_voteData.VoteTimeCount <= 0) // ��ǥ �ð� ���� �� ��ǥ, ��ŵ ��ư ��Ȱ��ȭ
            {
                DisableButton();
                //  SpawnAnonymImage();
                SpawnSkipAnonymImage();
                _voteManager.GetVoteResult();
            }
        }
    }

    // ��ǥ ��ư ��Ȱ��ȭ �Լ�
    public void DisableButton()
    {
        foreach (var button in _voteButtons)
        {
            button.enabled = false;
            Debug.Log("��ǥ��ư ��Ȱ��ȭ");
        }
        _skipButton.enabled = false;
    }

    IEnumerator SpawnPlayerPanelRoutine()
    {
        yield return 2f.GetDelay();
        SpawnPlayerPanel();
    }
}

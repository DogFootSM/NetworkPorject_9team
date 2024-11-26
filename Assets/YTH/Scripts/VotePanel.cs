using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    private PlayerType _playerType; // Duck�� ���Ǿ�

    [SerializeField] GameObject[] _panelList; // PlayerActorNumber �ε����� �̸� �����ص� �гε��� ����Ʈ�� ��� ����

    [SerializeField] GameObject[] _SkipanonymImage; // ��ŵ �� ��ŭ �͸� �̹��� ����

    [SerializeField] VoteScenePlayerData[] playerData;

    #region UI Property
    [Header("UI")]
    // [SerializeField] GameObject _characterImage; // ��ǥâ���� �� �÷��̾� ĳ���� �̹���

    //  [SerializeField] Image _voteSignImage; // ��ǥ�� �÷��̾� ǥ�� �̹���

    //  [SerializeField] Image _deadSignImage; // ���� ���� ǥ�� �̹���

    [SerializeField] TMP_Text _nickNameText; // �� �÷��̾� �г��� �ؽ�Ʈ

    [SerializeField] GameObject _votePanel; // ��ǥâ ��ü �г�

    [SerializeField] GameObject _playerPanel; // �� �÷��̾� �г�

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // �Ű��ڸ� ���� �� �ִ� �ð� ī��Ʈ

    [SerializeField] Slider _voteTimeCountSlider; // ��ǥ ���� �ð� ī��Ʈ
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        _voteTimeCountSlider.value = _voteData.VoteTimeCount;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        CountTime();
        // _voteButton.interactable = _voteData.ReportTimeCount <= 0;  ��ġ �����Ұ�
        // _skipButton.interactable = _voteData.ReportTimeCount <= 0;
        //
        // _voteButton.interactable = _voteData.VoteTimeCount <= 0;
        // _skipButton.interactable = _voteData.VoteTimeCount <= 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSkipAnonymImage();
        }
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayerPanel();
        SetPlayerPanel(PhotonNetwork.LocalPlayer); // ��� �÷��̾ ������Ʈ�ϰ� �����ϱ�
        foreach (Player player in PhotonNetwork.PlayerList) // ���� �ʿ�
        {
            GetComponent<VoteScenePlayerData>();
        }

        // ��ǥ �� ���� ������ ��� �÷��̾� _playerData.DidVote == false ���ֱ�
    }

    // �� �÷��̾� �г��� �����ϴ� �Լ�
    public void SetPlayerPanel(Player player)
    {
        // _nickNameText.text = player.NickName; // �г��� �ҷ�����
        //TODO : _characterImage = ""; // ĳ���� �̹��� �ҷ�����
        //TODO : ���� ĳ���Ϳ� ��� ǥ�� �������
        //TODO : �÷��̾ ���� ���¶�� �� �÷��̾� ��ǥ ��ư ��Ȱ��ȭ // button.interatable == false
    }

    // �÷��̾� �г� ���� �Լ�
    public void SpawnPlayerPanel()
    {   //ActorNumber 1 ���� ���� 
        // �ε��� ��ȣ�� �Ű������� �ޱ� ���ؼ� -1
        photonView.RPC("SpawnPlayerPanelRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

    [PunRPC]
    public void SpawnPlayerPanelRPC(int index)
    {
        _panelList[index].SetActive(true);
        _panelList[index].GetComponent<VoteScenePlayerData>().VoteButton.onClick.AddListener(() => { _voteManager.Vote(index); });
    }


    //��ǥ ���� �� ��ŵ �� ��ŭ �͸� �̹��� ����
    public void SpawnSkipAnonymImage()
    {
        photonView.RPC("SpawnSkipAnonymImageRPC", RpcTarget.All, _voteData.SkipCount);
    }

    [PunRPC]
    public void SpawnSkipAnonymImageRPC(int index)
    {
        for (int i = 0; i < index; i++)
        {
            _SkipanonymImage[i].SetActive(true);
        }
    }

    //��ǥ ���� �� ��ǥ �� ��ŭ �÷��̾� �гο� �͸� �̹��� ����
    public void SpawnAnonymImage()
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
                //TODO : ��ǥ����ŭ Ȱ��ȭ
            }
        }
    }

    public void CountTime()
    {
        _voteData.ReportTimeCount -= (float)Time.deltaTime; // Time.deltaTime ���� �ʿ� �� ����
        //Debug.Log(_voteData.ReportTimeCount);
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        if (_voteData.ReportTimeCount <= 0)
        {
            _reportTimeCountSlider.gameObject.SetActive(false); // ���� ������ ��
            _voteData.VoteTimeCount -= (float)Time.deltaTime;
            _voteTimeCountSlider.value = _voteData.VoteTimeCount;
            //Debug.Log(_voteData.VoteTimeCount);

            if (_voteData.VoteTimeCount <= 0)
            {
              //  SpawnAnonymImage();
                SpawnSkipAnonymImage();
                _voteManager.GetVoteResult();
            }
        }
    }
}

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;

    [SerializeField] Player _targetPlayer; // ������ �÷��̾� 

    [SerializeField] Player[] player;

    private PlayerType _playerType; // Duck�� ���Ǿ�

    public GameObject[] panelList;

    #region UI Property
    [Header("UI")]
    // [SerializeField] GameObject _characterImage; // ��ǥâ���� �� �÷��̾� ĳ���� �̹���

    //  [SerializeField] Image _voteSignImage; // ��ǥ�� �÷��̾� ǥ�� �̹���

    //  [SerializeField] Image _deadSignImage; // ���� ���� ǥ�� �̹���

    [SerializeField] GameObject _anonymPlayerImage; // ��ǥ�� �͸��� �÷��̾� �̹���

    [SerializeField] TMP_Text _nickNameText; // �� �÷��̾� �г��� �ؽ�Ʈ

    [SerializeField] GameObject _votePanel; // ��ǥâ ��ü �г�

    [SerializeField] GameObject _playerPanel; // �� �÷��̾� �г�

    [SerializeField] Button _voteButton; // �� �÷��̾� �г��� ���� �� ��ǥ �Ǵ� ��ư

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // �Ű��ڸ� ���� �� �ִ� �ð� ī��Ʈ

    [SerializeField] Slider _voteTimeCountSlider; // ��ǥ ���� �ð� ī��Ʈ
    #endregion

    private void Awake()
    {
        Init();
        PhotonNetwork.PlayerList.InitCustomProperties();
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
        SpawnPanelWithSetParent();
        SetPlayerPanel(PhotonNetwork.LocalPlayer); // ��� �÷��̾ ������Ʈ�ϰ� �����ϱ�
        foreach (Player player in PhotonNetwork.PlayerList) // ���� �ʿ�
        {
            GetComponent<VoteScenePlayerData>();
        }

       

       // _playerData.IsDead = false;
       // _playerData.IsReporter = false;
       // _playerData.DidVote = false;
    }

    // �� �÷��̾� �г��� �����ϴ� �Լ�
    public void SetPlayerPanel(Player player)     
    {
        // _nickNameText.text = player.NickName;
        //TODO : _characterImage = ""; // ĳ���� �̹��� �ҷ�����
        //TODO : �÷��̾ ���� ���¶�� �� �÷��̾� ��ǥ ��ư ��Ȱ��ȭ
        _targetPlayer = player;
    }

    // �÷��̾� �г� ���� �Լ�
    public void SpawnPanelWithSetParent()
    {
        GameObject myPanel = PhotonNetwork.Instantiate("PlayerPanel", Vector2.zero, Quaternion.identity);
        //myPanel.transform.SetParent(_voteData.PlayerPanelParent, false);
        Debug.Log($"{PhotonNetwork.LocalPlayer} ���� �Ϸ�");
        photonView.RPC("SetParentRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber-1);
       
    }

    [PunRPC]
    public void SetParentRPC(int index)
    {
        panelList[index].SetActive(true);
        panelList[index].GetComponent<VoteScenePlayerData>().voteButton.onClick.AddListener(() => { _voteManager.OnClickPlayerPanel(index); });
    }

    public void CountTime()
    {
        _voteData.ReportTimeCount -= (float)Time.deltaTime;
        //Debug.Log(_voteData.ReportTimeCount);

        if (_voteData.ReportTimeCount <= 0)
        {
            _reportTimeCountSlider.gameObject.SetActive(false); // ���� ������ ��
            _voteData.VoteTimeCount -= (float)Time.deltaTime;
            //Debug.Log(_voteData.VoteTimeCount);

            if (_voteData.VoteTimeCount == 0)
            {
                PhotonNetwork.LocalPlayer.GetVoteResult();
            }
        }
    }
}

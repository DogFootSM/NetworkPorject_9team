using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCreateRoomBox : BaseUI
{
    [SerializeField] int _minPlayer = 5;
    [SerializeField] int _maxPlayer = 16;

    private TMP_InputField _createNickNameInput => GetUI<TMP_InputField>("CreateNickNameInput");
    private TMP_Text _createPlayerCountText => GetUI<TMP_Text>("CreatePlayerCountText");
    private Slider _createPlayerCountSlider => GetUI<Slider>("CreatePlayerCountSlider");
    private Slider _createRoomOpenSlider => GetUI<Slider>("CreateRoomOpenSlider");
    private TMP_Text _createRoomOpenText => GetUI<TMP_Text>("CreateRoomOpenText");
    private GameObject _createPrivacyCheck => GetUI("CreatePrivacyCheck");
    private void Awake()
    {
        Bind();
        Init();
    }


    private void Start()
    {
        SubscribesEvent();
    }

    private void OnEnable()
    {
        ClearCreateRoomBox();
    }
    /// <summary>
    /// �� ����
    /// </summary>
    private void CreateRoom()
    {
        string nickName = _createNickNameInput.text;
        if (nickName != string.Empty) // �г��� ���� ��
        {
            nickName.ChangeNickName();
        }

        string roomCode = Util.GetRandomRoomCode(6); // ���� ���ڵ� ȹ��
        int maxPlayer = (int)_createPlayerCountSlider.value; // �ִ� �ο� ȹ��
        bool isVisible = (int)_createRoomOpenSlider.value == 0 ? true : false; // ���� ���� ȹ��

        // �� �ɼ� ����
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.IsVisible = isVisible;
        options.SetPrivacy(_createPrivacyCheck.activeSelf);

        LobbyScene.ActivateLoadingBox(true);
        PhotonNetwork.CreateRoom(roomCode, options);
    }

    /// <summary>
    /// �÷��̾� ī��Ʈ ������Ʈ
    /// </summary>
    private void UpdatePlayerCount(float value)
    {
        _createPlayerCountText.SetText(value.GetText());
    }

    /// <summary>
    /// ������ ������� ������Ʈ
    /// </summary>
    private void UpdateIsVisible(float value)
    {
        if (value == 1f) // �����̴� �� 1�� �����, 0�� ����
        {
            _createRoomOpenText.SetText("�����".GetText());
        }
        else
        {
            _createRoomOpenText.SetText("����".GetText());
        }
    }

    /// <summary>
    /// �����̹��� ���
    /// </summary>
    private void UpdatePrivacyMode()
    {
        if (_createPrivacyCheck.activeSelf == false)
        {
            // �����̹��� ��� Ȱ��ȭ
            _createPrivacyCheck.SetActive(true);
        }
        else
        {
            // ��Ȱ��ȭ
            _createPrivacyCheck.SetActive(false);
        }
    }
    /// <summary>
    /// �� ���� ȭ�� �ʱ�ȭ
    /// </summary>
    private void ClearCreateRoomBox()
    {
        _createNickNameInput.text = string.Empty;
        _createPlayerCountSlider.value = (int)((_createPlayerCountSlider.maxValue + _createPlayerCountSlider.minValue) / 2); // ���� ��ġ��ŭ
        _createRoomOpenSlider.value = 1f; // �⺻ ����� ��
        _createPrivacyCheck.SetActive(false);
    }
    private void Init()
    {        
        // TODO : ���� ���ӸŴ��� ���������� �ִ��ּ� �ο� �����ؼ� �����;��� �ʿ䰡 ����
        _createPlayerCountSlider.minValue = _minPlayer;
        _createPlayerCountSlider.maxValue = _maxPlayer;

    }
    private void SubscribesEvent()
    {
        _createPlayerCountSlider.onValueChanged.AddListener(UpdatePlayerCount);
        _createRoomOpenSlider.onValueChanged.AddListener(UpdateIsVisible);
        GetUI<Button>("CreateBackButton").onClick.AddListener(() => MainPanel.ChangeBox(MainPanel.Box.Join));
        GetUI<Button>("CreateRoomButton").onClick.AddListener(CreateRoom);
        GetUI<Button>("CreatePrivacyButton").onClick.AddListener(UpdatePrivacyMode);
    }
}
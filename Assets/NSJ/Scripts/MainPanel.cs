using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;
    [SerializeField] int _minPlayer;
    [SerializeField] int _maxPlayer;

    #region private �ʵ�

    enum Box { Main, Quick, Join, Create, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];

    // MainBox
    private TMP_Text _mainNameText => GetUI<TMP_Text>("MainNameText");
    private TMP_Text _mainNickNameText => GetUI<TMP_Text>("MainNickNameText");
    //JoinBox
    private TMP_InputField _joinNickNameInput => GetUI<TMP_InputField>("JoinNickNameInput");
    private TMP_InputField _joinRoomInput => GetUI<TMP_InputField>("JoinRoomInput");
    private GameObject _joinInvisibleOnImage => GetUI("JoinInvisibleOnImage");

    // CreateRoomBox
    private TMP_InputField _createNickNameInput => GetUI<TMP_InputField>("CreateNickNameInput");
    private TMP_Text _createPlayerCountText => GetUI<TMP_Text>("CreatePlayerCountText");
    private Slider _createPlayerCountSlider => GetUI<Slider>("CreatePlayerCountSlider");
    private Slider _createRoomOpenSlider => GetUI<Slider>("CreateRoomOpenSlider");
    private TMP_Text _createRoomOpenText => GetUI<TMP_Text>("CreateRoomOpenText");
    private GameObject _createPrivacyCheck => GetUI("CreatePrivacyCheck");

    // QuickBox
    private TMP_InputField _quickNickNameInput => GetUI<TMP_InputField>("QuickNickNameInput");
    private GameObject _quickColorBox => GetUI("QuickColorBox");


    private StringBuilder _sb = new StringBuilder();
    #endregion

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
        ChangeBox(Box.Main);
    }

    #region JoinBox ��ư
    /// <summary>
    /// �κ� ����
    /// </summary>
    private void JoinLobby()
    {
        string nickName = _joinNickNameInput.text; // �г��� ĳ��


        ActivateLoadingBox(true); // �ε�â Ȱ��ȭ

        if (nickName != string.Empty) // �г��� ������ ������ �г��� ����
        {
            ChangeNickName(nickName); // �г��� ����(�����Ʈ��ũ �г��� ����, �����ͺ��̽� �г��� ����      
        }

        ActivateLoadingBox(true);
        PhotonNetwork.JoinLobby(); // �κ� ����
    }
    /// <summary>
    /// �� �ڵ� �ڵ� �빮�� ��ȯ
    /// </summary>
    private void ChangeRoomCodeToUpper(string value)
    {
        _joinRoomInput.text = value.ToUpper();
    }

    /// <summary>
    /// ���ڵ� �����/���̱�
    /// </summary>
    private void ChangeRoomCodeInvisible()
    {
        if(_joinRoomInput.contentType == TMP_InputField.ContentType.Password) 
        {
            //���̱�
            _joinRoomInput.contentType = TMP_InputField.ContentType.Standard;
            _joinInvisibleOnImage.SetActive(false);
        }
        else
        {
            //�����
            _joinRoomInput.contentType = TMP_InputField.ContentType.Password;
            _joinInvisibleOnImage.SetActive(true);
        }
    }
    /// <summary>
    /// �� �ڵ�� ����
    /// </summary>
    private void JoinRoomCode()
    {
        string roomCode = _joinRoomInput.text; //�� �ڵ� ĳ��
        if (roomCode == string.Empty)
            return;

        string nickName = _joinNickNameInput.text; // �г��� ĳ��

        ActivateLoadingBox(true); // �ε�â Ȱ��ȭ

        if (nickName != string.Empty) // �г��� ������ ������ �г��� ����
        {
            ChangeNickName(nickName); // �г��� ����(�����Ʈ��ũ �г��� ����, �����ͺ��̽� �г��� ����      
        }

        PhotonNetwork.JoinRoom(roomCode); // �� �ڵ�� �� ����
    }
    #endregion

    #region �� ����

    /// <summary>
    /// �� ����
    /// </summary>
    private void CreateRoom()
    {
        string nickName = _createNickNameInput.text;
        if (nickName != string.Empty) // �г��� ���� ��
        {
            ChangeNickName(nickName);
        }

        string roomCode = GetRandomRoomCode(6); // ���� ���ڵ� ȹ��
        int maxPlayer = (int)_createPlayerCountSlider.value; // �ִ� �ο� ȹ��
        bool isVisible = (int)_createRoomOpenSlider.value == 0 ? true : false; // ���� ���� ȹ��

        // �� �ɼ� ����
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.IsVisible = isVisible;

        ActivateLoadingBox(true);
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
        if(value == 1f) // �����̴� �� 1�� �����, 0�� ����
        {
            _createRoomOpenText.SetText("�����".GetText());
        }
        else
        {
            _createRoomOpenText.SetText("����".GetText());
        }
    }

    #endregion

    #region ���� ����
    /// <summary>
    /// ���� ����
    /// </summary>
    private void StartRandomMatch()
    {
        string nickName = _quickNickNameInput.text;
        if (nickName != string.Empty) // �г��� ���� ���� ���� �ÿ� �г��� ����
        {
            ChangeNickName(nickName);
        }

        ActivateLoadingBox(true);
        PhotonNetwork.JoinRandomRoom();
    }
    /// <summary>
    /// ���� ���� ��Ī ���� �� ���ο� �� �ڵ� ����
    /// </summary>
    private void CreateRandomRoom(short returnCode, string message)
    {
        string roomCode = GetRandomRoomCode(6); // ���� ���ڵ� ȹ��
        int maxPlayer = 10;
        bool isVisible = true;

        // �� �ɼ� ����
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.IsVisible = isVisible;

        PhotonNetwork.CreateRoom(roomCode, options);
    }

    #endregion

    #region �α׾ƿ�
    /// <summary>
    /// �α׾ƿ�
    /// </summary>
    private void LogOut()
    {
        ActivateLoadingBox(true);
        BackendManager.Auth.SignOut(); // �α׾ƿ�
        PhotonNetwork.Disconnect(); // ���� ���� ����
    }

    #endregion

    #region �г� ����

    /// <summary>
    /// UI �ڽ� ����
    /// </summary>
    private void ChangeBox(Box box)
    {
        ActivateLoadingBox(false);

        for (int i = 0; i < _boxs.Length; i++) 
        {
            if (_boxs[i] == null)
                return;

            if(i == (int)box) // �ٲٰ��� �ϴ� �ڽ��� Ȱ��ȭ
            {
                _boxs[i].SetActive(true);
                ClearBox(box); // �ʱ�ȭ �۾��� ����
            }
            else
            {
                _boxs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// UI �ڽ� �ʱ�ȭ �۾�
    /// </summary>
    /// <param name="box"></param>
    private void ClearBox(Box box)
    {
        switch (box)
        {
            case Box.Main:
                ClearMainBox();
                break;
            case Box.Create:
                ClearCreateRoomBox();
                break;
            case Box.Join:
                ClearJoinBox();
                break;
            case Box.Quick:
                ClearQuickBox();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���� ȭ�� �ʱ�ȭ
    /// </summary>
    private void ClearMainBox()
    {
        if (BackendManager.Instance == null)
            return;
        if (BackendManager.User == null)
            return;
        _mainNameText.SetText($"{BackendManager.User.FirstName} {BackendManager.User.SecondName} ���� �α����߽��ϴ�".GetText());
        _mainNickNameText.SetText($"[{BackendManager.User.NickName}]".GetText());
    }
    /// <summary>
    /// �� ����(���� �ϱ� ��ư) ����ȭ
    /// </summary>
    private void ClearJoinBox()
    {
        _joinNickNameInput.text =string.Empty;
        _joinRoomInput.text =string.Empty;
        _joinRoomInput.contentType = TMP_InputField.ContentType.Standard;
        _joinInvisibleOnImage.SetActive(false);
    }

    /// <summary>
    /// �� ���� ȭ�� �ʱ�ȭ
    /// </summary>
    private void ClearCreateRoomBox()
    {
        _createNickNameInput.text =string.Empty;
        _createPlayerCountSlider.value = (int)((_createPlayerCountSlider.maxValue + _createPlayerCountSlider.minValue) / 2); // ���� ��ġ��ŭ
        _createRoomOpenSlider.value = 1f; // �⺻ ����� ��
        _createPrivacyCheck.SetActive(false);
    }

    private void ClearQuickBox()
    {
        _quickNickNameInput.text = string.Empty;
        _quickColorBox.SetActive(false);
    }


    /// <summary>
    /// �ε� ȭ�� Ȱ��ȭ / ��Ȱ��ȭ
    /// </summary>
    private void ActivateLoadingBox(bool isActive)
    {
        if (isActive)
        {
            _loadingBox.SetActive(true);
        }
        else
        {
            _loadingBox.SetActive(false);
        }
    }


    #endregion


    #region �ʱ� ����
    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void Init()
    {
        #region box �迭 ����

        _boxs[(int)Box.Main] = GetUI("MainBox");
        _boxs[(int)Box.Quick] = GetUI("QuickBox");
        _boxs[(int)Box.Join] = GetUI("JoinBox");
        _boxs[(int)Box.Create] = GetUI("CreateRoomBox");

        #endregion

        #region CreateRoomBox
        // TODO : ���� ���ӸŴ��� ���������� �ִ��ּ� �ο� �����ؼ� �����;��� �ʿ䰡 ����
        _createPlayerCountSlider.minValue = _minPlayer;
        _createPlayerCountSlider.maxValue = _maxPlayer;

        #endregion
    }

    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribesEvent()
    {
        

        #region MainBox

        GetUI<Button>("MainLogOutButton").onClick.AddListener(LogOut);
        GetUI<Button>("MainQuickMatchButton").onClick.AddListener(() => ChangeBox(Box.Quick));
        GetUI<Button>("MainJoinButton").onClick.AddListener(() => ChangeBox(Box.Join));

        #endregion

        #region QuickBox

        GetUI<Button>("QuickBackButton").onClick.AddListener(() => ChangeBox(Box.Main));

        #endregion

        #region JoinBox

        GetUI<Button>("JoinBackButton").onClick.AddListener(() => ChangeBox(Box.Main));
        GetUI<Button>("JoinCreateRoomButton").onClick.AddListener(() => ChangeBox(Box.Create));
        _joinRoomInput.onValueChanged.AddListener(ChangeRoomCodeToUpper);
        GetUI<Button>("JoinInvisibleButton").onClick.AddListener(ChangeRoomCodeInvisible);
        GetUI<Button>("JoinLobbyButton").onClick.AddListener(JoinLobby);
        GetUI<Button>("JoinRoomButton").onClick.AddListener(JoinRoomCode);

        #endregion

        #region CreateRoomBox

        _createPlayerCountSlider.onValueChanged.AddListener(UpdatePlayerCount);
        _createRoomOpenSlider.onValueChanged.AddListener(UpdateIsVisible);
        GetUI<Button>("CreateBackButton").onClick.AddListener(() => ChangeBox(Box.Join));
        GetUI<Button>("CreateRoomButton").onClick.AddListener(CreateRoom);

        #endregion

        #region QuickBox

        LobbyScene.Instance.OnJoinRandomFailedEvent += CreateRandomRoom;
        GetUI<Button>("QuickColorButton").onClick.AddListener(() => { _quickColorBox.SetActive(!_quickColorBox.activeSelf); });
        GetUI<Button>("QuickStartButton").onClick.AddListener(StartRandomMatch);
        #endregion
    }
    #endregion

    /// <summary>
    /// �г��� ����
    /// </summary>
    /// <param name="nickName"></param>
    private void ChangeNickName(string nickName)
    {
        BackendManager.User.NickName = nickName; // ���� ������ �г��� ����
        // �г��� ������ ���̽��� �Ϻ� ���� ����
        BackendManager.SettingDic.Clear();
        BackendManager.SettingDic.Add(UserDate.NICKNAME, nickName);
        BackendManager.Auth.CurrentUser.UserId.GetUserDataRef().UpdateChildrenAsync(BackendManager.SettingDic);
        PhotonNetwork.LocalPlayer.NickName = nickName; // ���� ��Ʈ��ũ �г��� ����
    }

    /// <summary>
    /// ���� �� �ڵ� ���
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private string GetRandomRoomCode(int length)
    {
        _sb.Clear();
        for (int i = 0; i < length; i++)
        {
            int numberOrAlphabet = Random.Range(0, 2);
            if (numberOrAlphabet == 0) // ������
            {
                int numberASKII = Random.Range(48, 58); // �ƽ�Ű�ڵ� 48~57������(0~9)
                _sb.Append((char)numberASKII);
            }
            else // ������
            {
                int alphabetASKII = Random.Range(65, 91); // �ƽ�Ű�ڵ� 65~91������ (A~Z)
                _sb.Append((char)alphabetASKII);
            }
        }
        return _sb.ToString();
    }
}

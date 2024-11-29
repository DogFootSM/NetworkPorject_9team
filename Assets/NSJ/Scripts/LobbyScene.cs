using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public static LobbyScene Instance;
    public static bool IsLoginCancel { get { return  Instance._isLoginCancel; } set { Instance._isLoginCancel = value; } }
    public static bool IsJoinRoomCancel { get { return Instance._isJoinRoomCancel; } set { Instance._isJoinRoomCancel = value; } }

    #region �̺�Ʈ
    public event UnityAction OnConnectedEvent;
    public event UnityAction<DisconnectCause> OnDisconnectedEvent;
    public event UnityAction OnCreateRoomEvent;
    public event UnityAction OnJoinedRoomEvent;
    public event UnityAction<short, string> OnJoinRandomFailedEvent;
    public event UnityAction OnLeftRoomEvent;
    public event UnityAction<Player> OnPlayerEnteredRoomEvent;
    public event UnityAction<Player> OnPlayerLeftRoomEvent;
    public event UnityAction<Player, PhotonHashtable> OnPlayerPropertiesUpdateEvent;
    public event UnityAction OnJoinedLobbyEvent;
    public event UnityAction OnLeftLobbyEvent;
    public event UnityAction<Player> OnMasterClientSwitchedEvent;
    public event UnityAction<List<RoomInfo>> OnRoomListUpdateEvent;
    #endregion 

    #region private �ʵ�
    public enum Panel { Login, Main, Lobby, Room, Loading, Option, Size }

    [System.Serializable]
    struct PanelStruct
    {
        public GameObject BackGroundImage;
        public GameObject LoginPanel;
        public GameObject MainPanel;
        public GameObject LobbyPanel;
        public GameObject RoomPanel;
        public GameObject LoadingPanel;
        public GameObject OptionPanel;
    }
    [SerializeField] private PanelStruct _panelStruct;
    private static GameObject s_backGroundImage { get { return Instance._panelStruct.BackGroundImage; } }
    private static GameObject s_loginPanel { get { return Instance._panelStruct.LoginPanel; } }
    private static GameObject s_mainPanel { get { return Instance._panelStruct.MainPanel; } }
    private static GameObject s_lobbyPanel { get { return Instance._panelStruct.LobbyPanel; } }
    private static GameObject s_roomPanel { get { return Instance._panelStruct.RoomPanel; } }
    private static GameObject s_loadingPanel { get { return Instance._panelStruct.LoadingPanel;} }
    private static GameObject s_optionPanel { get { return Instance._panelStruct.OptionPanel;} }

    private List<GameObject> _panels = new List<GameObject>((int)Panel.Size);
    private GameObject _curPanel;
    private static GameObject s_curPanel { get { return Instance._curPanel; } }

    private bool _isLoginCancel;
    private bool _isJoinRoomCancel;
    #endregion

    private void Awake()
    {
        InitSingleTon(); // �̱���
        Init(); // �ʱ� ����

    }
    private void Start()
    {
        SubscribesEvent();

        if (OptionPanel.Instance != null)
        {
           
            _panelStruct.OptionPanel = OptionPanel.Instance.gameObject;
            _panels.Add(OptionPanel.Instance.gameObject);
        }

        InitPanel();
    }

    #region ���� ��Ʈ��ũ �ݹ� �Լ�
    /// <summary>
    /// ������ ���� ����� �ݹ�
    /// </summary>
    public override void OnConnectedToMaster()
    {
        ChangePanel(Panel.Main);
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// ���� ���� ���� �� �ݹ�
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        ChangePanel(Panel.Login);
        OnDisconnectedEvent?.Invoke(cause);
    }

    /// <summary>
    /// �� ���� �� �ݹ�
    /// </summary>
    public override void OnCreatedRoom()
    {
        OnCreateRoomEvent?.Invoke();
    }

    /// <summary>
    /// �� ���� �� �ݹ�
    /// </summary>
    public override void OnJoinedRoom()
    {
        ChangePanel(Panel.Room);
        OnJoinedRoomEvent?.Invoke();
    }
    /// <summary>
    /// �� ���� ���� �� �ݹ�
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ActivateLoadingBox(false);
    }

    /// <summary>
    /// ������Ī ���� ���� �� �ݹ�
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnJoinRandomFailedEvent?.Invoke(returnCode, message);
    }

    /// <summary>
    /// �� ���� �� �ݹ�
    /// </summary>
    public override void OnLeftRoom()
    {
        ChangePanel(Panel.Main);
        OnLeftRoomEvent?.Invoke();
    }
    /// <summary>
    /// �÷��̾� ���� �� �ݹ�
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
    }
    /// <summary>
    /// �÷��̾� ���� �� �ݹ�
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
    }
    /// <summary>
    /// �÷��̾� ������Ƽ ���� �� �ݹ�
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
    }
    /// <summary>
    /// ������ Ŭ���̾�Ʈ ���� �� �ݹ�
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
    }

    /// <summary>
    /// �κ� ���� �� �ݹ�
    /// </summary>
    public override void OnJoinedLobby()
    {
        ChangePanel(Panel.Lobby);
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// �κ� ���� �� �ݹ�
    /// </summary>
    public override void OnLeftLobby()
    {
        ChangePanel(Panel.Main);
        OnLeftLobbyEvent?.Invoke();
    }
    /// <summary>
    /// �� ����Ʈ ������Ʈ
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StartCoroutine(OnRoomListUpdateRoutine(roomList));
    }
    IEnumerator OnRoomListUpdateRoutine(List<RoomInfo> roomList)
    {
        yield return null;
        OnRoomListUpdateEvent?.Invoke(roomList);
        // �κ��г� ȣ���� �̺�Ʈ �����ð��� ���� 1������ �ʴ� �ڷ�ƾ
    }


    #endregion

    /// <summary>
    /// �ε� ȭ�� Ȱ��ȭ / ��Ȱ��ȭ
    /// </summary>
    public static void ActivateLoadingBox(bool isActive)
    { 
        s_loadingPanel.SetActive(isActive);
    }

    /// <summary>
    /// �ɼ� â Ȱ��ȭ / ��Ȱ��ȭ
    /// </summary>
    /// <param name="isActive"></param>
    public static void ActivateOptionBox(bool isActive)
    {
        s_optionPanel.SetActive(isActive);
    }

    /// <summary>
    /// �г� ��ü
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {
        for (int i = 0; i < _panels.Count; i++)
        {
            if (i == (int)panel) // �Ű������� ��ġ�ϴ� �г��̸� Ȱ��ȭ
            {
                if (_panels[i] == null)
                    return;
                _panels[i].SetActive(true);
                _curPanel = _panels[i];
                if (panel == Panel.Room) // �г��� ���̸� �޹�� ��Ȱ��ȭ
                {
                    s_backGroundImage.SetActive(false);
                }
                else
                {
                    s_backGroundImage.SetActive(true);
                }
            }
            else // �ƴϸ� ��Ȱ��ȭ
            {
                _panels[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// �ε� ĵ�� ����
    /// </summary>
   public static void SetIsLoadingCancel()
    {
        if(s_curPanel == s_loginPanel)
        {
            IsLoginCancel = true;
        }
        else if(s_curPanel == s_mainPanel)
        {
            IsJoinRoomCancel = true;
        }
    }



    #region �ʱ� ����

    /// <summary>
    /// �̱��� ����
    /// </summary>
    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �ʱ� �г� ����
    /// </summary>
    private void InitPanel()
    {
        if (PhotonNetwork.InRoom) // �濡 �����ѻ��¿�����
        {
            ChangePanel(Panel.Room);
        }
        else if (PhotonNetwork.IsConnected) // �ƴϸ� ������ ������ִ� ���¿��� ��
        {
            ChangePanel(Panel.Main);
        }
        else
        {
            ChangePanel(Panel.Login);
        }
    }

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void Init()
    {
        _panels.Add(s_loginPanel);
        _panels.Add(s_mainPanel);
        _panels.Add(s_lobbyPanel);
        _panels.Add(s_roomPanel);
        _panels.Add(s_loadingPanel);
        //_panels[(int)Panel.Option] = s_optionPanel;

        //ActivateLoadingBox(false);
        //ActivateOptionBox(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}

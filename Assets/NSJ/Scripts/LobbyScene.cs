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
    public static GameObject Loading { get { return Instance._popUp.Loading; } }
    public static GameObject Option { get { return Instance._popUp.Option; } }
  
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
    public enum Panel { Login, Main, Lobby, Room, Size }

    [System.Serializable]
    struct PanelStruct
    {
        public GameObject BackGroundImage;
        public GameObject LoginPanel;
        public GameObject MainPanel;
        public GameObject LobbyPanel;
        public GameObject RoomPanel;
    }
    [SerializeField] private PanelStruct _panelStruct;
    private static GameObject s_backGroundImage { get { return Instance._panelStruct.BackGroundImage; } }
    private static GameObject s_loginPanel { get { return Instance._panelStruct.LoginPanel; } }
    private static GameObject s_mainPanel { get { return Instance._panelStruct.MainPanel; } }
    private static GameObject s_lobbyPanel { get { return Instance._panelStruct.LobbyPanel; } }
    private static GameObject s_roomPanel { get { return Instance._panelStruct.RoomPanel; } }

    private GameObject[] _panels = new GameObject[(int)Panel.Size];
    private GameObject _curPanel;
    private static GameObject s_curPanel { get { return Instance._curPanel; } }

    [System.Serializable]
    struct PopUpUI
    {
        public GameObject Loading;
        public GameObject Option;
    }
    [SerializeField] PopUpUI _popUp;

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
        ChangePanel(Panel.Login);
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
        // ���� �� �ڷ�ƾ�̾�. �ƾ� �ڷ�ƾ ���� ��, ���� ��.
    }


    #endregion

    /// <summary>
    /// �ε� ȭ�� Ȱ��ȭ / ��Ȱ��ȭ
    /// </summary>
    public static void ActivateLoadingBox(bool isActive)
    {
        Loading.SetActive(isActive);
    }

    /// <summary>
    /// �ɼ� â Ȱ��ȭ / ��Ȱ��ȭ
    /// </summary>
    /// <param name="isActive"></param>
    public static void ActivateOptionBox(bool isActive)
    {
        Option.SetActive(isActive);
    }

    /// <summary>
    /// �г� ��ü
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {

        for (int i = 0; i < _panels.Length; i++)
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
    /// �ʱ� ����
    /// </summary>
    private void Init()
    {
        _panels[(int)Panel.Login] = s_loginPanel;
        _panels[(int)Panel.Main] = s_mainPanel;
        _panels[(int)Panel.Lobby] = s_lobbyPanel;
        _panels[(int)Panel.Room] = s_roomPanel;

        ActivateLoadingBox(false);
        ActivateOptionBox(false);
    }

    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}

using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public static LobbyScene Instance;

    #region �̺�Ʈ
    public event UnityAction OnConnectedEvent;
    public event UnityAction<DisconnectCause> OnDisconnectedEvent;
    public event UnityAction OnCreateRoomEvent;
    public event UnityAction OnJoinedRoomEvent;
    public event UnityAction OnLeftRoomEvent;
    public event UnityAction<Player> OnPlayerEnteredRoomEvent;
    public event UnityAction<Player> OnPlayerLeftRoomEvent;
    public event UnityAction<Player, PhotonHashtable> OnPlayerPropertiesUpdateEvent;
    public event UnityAction OnJoinedLobbyEvent;
    public event UnityAction OnLeftLobbyEvent;
    public event UnityAction<Player> OnMasterClientSwitchedEvent;
    #endregion

    #region private �ʵ�
    enum Panel { Login, Main, Lobby, Room, Size }

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
    private GameObject _backGroundImage { get { return _panelStruct.BackGroundImage; } }
    private GameObject _loginPanel { get {  return _panelStruct.LoginPanel; } }
    private GameObject _mainPanel{ get { return _panelStruct.MainPanel; } }
    private GameObject _lobbyPanel { get { return _panelStruct.LobbyPanel; } }
    private GameObject _roomPanel { get { return _panelStruct.RoomPanel; } }

    private GameObject[] _panels = new GameObject[(int)Panel.Size];
    #endregion

    private void Awake()
    {
        InitSingleTon(); // �̱���
        Init(); // �ʱ� ����
        SubscribesEvent();
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
    #endregion

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
                _panels[i].SetActive(true);

                if(panel == Panel.Room) // �г��� ���̸� �޹�� ��Ȱ��ȭ
                {
                    _backGroundImage.SetActive(false);
                }
                else
                {
                    _backGroundImage.SetActive(true);
                }
            }
            else // �ƴϸ� ��Ȱ��ȭ
            {
                _panels[i].SetActive(false);
            }
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
        _panels[(int)Panel.Login] = _loginPanel;
        _panels[(int)Panel.Main] = _mainPanel;
        _panels[(int)Panel.Lobby] = _lobbyPanel;
        _panels[(int)Panel.Room] = _roomPanel;
    }

    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}
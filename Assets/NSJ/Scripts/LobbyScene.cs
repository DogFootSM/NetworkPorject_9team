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

    #region 이벤트
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

    #region private 필드
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
        InitSingleTon(); // 싱글톤
        Init(); // 초기 설정

    }
    private void Start()
    {
        SubscribesEvent();
        ChangePanel(Panel.Login);
    }

    #region 포톤 네트워크 콜백 함수
    /// <summary>
    /// 마스터 서버 연결시 콜백
    /// </summary>
    public override void OnConnectedToMaster()
    {
        ChangePanel(Panel.Main);
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// 서버 접속 해제 시 콜백
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        ChangePanel(Panel.Login);
        OnDisconnectedEvent?.Invoke(cause);
    }

    /// <summary>
    /// 방 생성 시 콜백
    /// </summary>
    public override void OnCreatedRoom()
    {
        OnCreateRoomEvent?.Invoke();
    }

    /// <summary>
    /// 방 입장 시 콜백
    /// </summary>
    public override void OnJoinedRoom()
    {
        ChangePanel(Panel.Room);
        OnJoinedRoomEvent?.Invoke();
    }

    /// <summary>
    /// 랜덤매칭 입장 실패 시 콜백
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnJoinRandomFailedEvent?.Invoke(returnCode, message);
    }

    /// <summary>
    /// 방 퇴장 시 콜백
    /// </summary>
    public override void OnLeftRoom()
    {
        ChangePanel(Panel.Main);
        OnLeftRoomEvent?.Invoke();
    }
    /// <summary>
    /// 플레이어 입장 시 콜백
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
    }
    /// <summary>
    /// 플레이어 퇴장 시 콜백
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
    }
    /// <summary>
    /// 플레이어 프로퍼티 변경 시 콜백
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
    }
    /// <summary>
    /// 마스터 클라이언트 변경 시 콜백
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
    }

    /// <summary>
    /// 로비 입장 시 콜백
    /// </summary>
    public override void OnJoinedLobby()
    {
        ChangePanel(Panel.Lobby);
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// 로비 퇴장 시 콜백
    /// </summary>
    public override void OnLeftLobby()
    {
        ChangePanel(Panel.Main);
        OnLeftLobbyEvent?.Invoke();
    }
    /// <summary>
    /// 룸 리스트 업데이트
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
        // 로비패널 호춠시 이벤트 구독시간을 위한 1프레임 늦는 코루틴
        // 젠장 또 코루틴이야. 아아 코루틴 나의 신, 나의 빛.
    }


    #endregion

    /// <summary>
    /// 로딩 화면 활성화 / 비활성화
    /// </summary>
    public static void ActivateLoadingBox(bool isActive)
    {
        Loading.SetActive(isActive);
    }

    /// <summary>
    /// 옵션 창 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive"></param>
    public static void ActivateOptionBox(bool isActive)
    {
        Option.SetActive(isActive);
    }

    /// <summary>
    /// 패널 교체
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {

        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == (int)panel) // 매개변수와 일치하는 패널이면 활성화
            {
                if (_panels[i] == null)
                    return;
                _panels[i].SetActive(true);
                _curPanel = _panels[i];
                if (panel == Panel.Room) // 패널이 룸이면 뒷배경 비활성화
                {
                    s_backGroundImage.SetActive(false);
                }
                else
                {
                    s_backGroundImage.SetActive(true);
                }
            }
            else // 아니면 비활성화
            {
                _panels[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 로딩 캔슬 세팅
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



    #region 초기 설정

    /// <summary>
    /// 싱글톤 지정
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
    /// 초기 설정
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
    /// 이벤트 구독
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}

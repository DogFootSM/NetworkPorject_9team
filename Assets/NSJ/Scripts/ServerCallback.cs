using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class ServerCallback : MonoBehaviourPunCallbacks
{
    public static ServerCallback Instance;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 마스터 서버 연결시 콜백
    /// </summary>
    public override void OnConnectedToMaster()
    {
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// 서버 접속 해제 시 콜백
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
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
        OnJoinedRoomEvent?.Invoke();
    }
    /// <summary>
    /// 방 입장 실패 시 콜백
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {

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
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// 로비 퇴장 시 콜백
    /// </summary>
    public override void OnLeftLobby()
    {
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
    }
}

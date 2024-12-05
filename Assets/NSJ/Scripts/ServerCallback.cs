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
    /// ������ ���� ����� �ݹ�
    /// </summary>
    public override void OnConnectedToMaster()
    {
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// ���� ���� ���� �� �ݹ�
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
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
        OnJoinedRoomEvent?.Invoke();
    }
    /// <summary>
    /// �� ���� ���� �� �ݹ�
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {

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
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// �κ� ���� �� �ݹ�
    /// </summary>
    public override void OnLeftLobby()
    {
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
}

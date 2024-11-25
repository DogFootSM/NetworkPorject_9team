using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;
    [SerializeField] RoomEntry _roomEntryPrefab;

    private RoomInfo _selectingRoomInfo;

    private Dictionary<string, RoomEntry> _roomDic = new Dictionary<string, RoomEntry>();
    #region private �ʵ�
    enum Box { Lobby, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];
    private GameObject _lobbyStartButton => GetUI("LobbyStartButton");
    #endregion

    private void Awake()
    {
        Bind();
        Init();
    }

    private void Start()
    {
        SubscribeEvent();
    }

    private void OnEnable()
    {
        ChangeBox(Box.Lobby);
    }

    /// <summary>
    /// ������ �� ������Ʈ
    /// </summary>
    public void UpdateSelectRoom(RoomInfo roomInfo)
    {
        // ���õ� �� ĳ��
        // ���� ���� �ι� �����ϸ� ĳ�� Ǯ��
        if (_selectingRoomInfo == roomInfo)
        {
            _selectingRoomInfo = null;
            _lobbyStartButton.SetActive(false);
        }
        else
        {
            _selectingRoomInfo = roomInfo;
            _lobbyStartButton.SetActive(true);
        }

        // ��ü �� ����Ʈ ���� üũ
        foreach (RoomEntry roomEntry in _roomDic.Values)
        {
            roomEntry.CheckSelect(_selectingRoomInfo);
        }
    }

    /// <summary>
    /// �븮��Ʈ ������Ʈ
    /// </summary>
    /// <param name="roomList"></param>
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            // ������ų�, ������ų�, �������� �ƴҶ� (������ ��)
            if (room.RemovedFromList || room.IsVisible == false || room.IsOpen == false)
            {
                // �ش���� ��ϵ��� ���� ���̾��� ��
                if (_roomDic.ContainsKey(room.Name) == false)
                    continue;
                Destroy(_roomDic[room.Name].gameObject);
                _roomDic.Remove(room.Name);
            }
            // ���� ���Ӱ� ��������
            else if (_roomDic.ContainsKey(room.Name) == false)
            {
                RoomEntry roomEntry = Instantiate(_roomEntryPrefab, GetUI("LobbyContent").transform);
                _roomDic.Add(room.Name, roomEntry);
                // TODO : �� ��Ʈ�� ����
                roomEntry.SetRoom(room);
                roomEntry.LobbyPanel = this;
            }
            // ���� ���� ���������� �ִ� ���
            else if (_roomDic.ContainsKey(room.Name))
            {
                RoomEntry roomEntry = _roomDic[room.Name];
                // �� ��Ʈ�� ����
                roomEntry.SetRoom(room);
            }
        }
    }
    /// <summary>
    /// �� �����ϱ�
    /// </summary>
    private void JoinRoom()
    {
        ClearRoomEntry();
        ActivateLoadingBox(true);
        PhotonNetwork.JoinRoom(_selectingRoomInfo.Name);
    }

    /// <summary>
    /// �� ����Ʈ ����
    /// </summary>
    private void ClearRoomEntry()
    {
        foreach (RoomEntry roomEntry in _roomDic.Values)
        {
            Destroy(roomEntry.gameObject);
        }
        _roomDic.Clear();
    }


    /// <summary>
    /// �κ� ������
    /// </summary>
    private void LeftLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

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

            if (i == (int)box) // �ٲٰ��� �ϴ� �ڽ��� Ȱ��ȭ
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
            case Box.Lobby:
                ClearLobby(); 
                break;
            default:
                break;
        }
    }

    private void ClearLobby()
    {
        _lobbyStartButton.SetActive(false);
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

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void Init()
    {
        _boxs[(int)Box.Lobby] = GetUI("LobbyBox");
    }
    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribeEvent()
    {
        LobbyScene.Instance.OnRoomListUpdateEvent += UpdateRoomList;
        LobbyScene.Instance.OnLeftLobbyEvent += ClearRoomEntry;
        GetUI<Button>("LobbyBackButton").onClick.AddListener(LeftLobby);
        GetUI<Button>("LobbyStartButton").onClick.AddListener(JoinRoom);
        Debug.Log(GetUI<Button>("LobbyStartButton"));
    }

}

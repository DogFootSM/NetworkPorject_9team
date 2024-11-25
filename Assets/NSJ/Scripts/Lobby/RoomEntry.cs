using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : BaseUI
{
    private RoomInfo _thisRoomInfo;
    public RoomInfo ThisRoomInfo { get { return _thisRoomInfo; } }

    private Button _roomEntry;
    private GameObject _selectRoomEntry => GetUI("SelectRoomEntry");
    private TMP_Text _roomNameText => GetUI<TMP_Text>("RoomNameText");
    private TMP_Text _roomPlayerText => GetUI<TMP_Text>("RoomPlayerText");

    private LobbyPanel _lobbyPanel;
    public LobbyPanel LobbyPanel { get { return _lobbyPanel; } set { _lobbyPanel = value; } }

    private void Awake()
    {
        Bind();
        _roomEntry = GetComponent<Button>();
        _selectRoomEntry.SetActive(false);
    }
    private void Start()
    {
        SubscribesEvent();
    }


    /// <summary>
    /// �� ����
    /// </summary>
    public void SetRoom(RoomInfo roomInfo)
    {
        // TODO: ȣ��Ʈ �̸��� ã�� ��...??
        this._thisRoomInfo = roomInfo;

        _roomNameText.SetText(roomInfo.Name);

        _roomPlayerText.SetText($"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}");
    }

    /// <summary>
    /// ���É���� üũ �� ������Ʈ
    /// </summary>
    /// <param name="roomInfo"></param>
    public void CheckSelect(RoomInfo roomInfo)
    {
        if (ThisRoomInfo == roomInfo)
        {
            _selectRoomEntry.SetActive(true);
        }
        else
        {
            _selectRoomEntry.SetActive(false);
        }
    }

    /// <summary>
    /// ���� �� ȣ��
    /// </summary>
    private void Select()
    {
        LobbyPanel.UpdateSelectRoom(ThisRoomInfo);
    }

    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribesEvent()
    {
        _roomEntry.onClick.AddListener(Select);
    }


}

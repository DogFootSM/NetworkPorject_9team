using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;

    #region private �ʵ�
    private const string SHOWTEXT = "���̱�";
    private const string HIDETEXT = "�����";
    enum Box { Room, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];

    private TMP_InputField _roomCodeText => GetUI<TMP_InputField>("RoomCodeText");
    private TMP_Text _roomTitleText => GetUI<TMP_Text>("RoomTitleText");
    private TMP_Text _roomCodeActiveText => GetUI<TMP_Text>("RoomCodeActiveText");
    private GameObject _roomStartButton => GetUI("RoomStartButton");
    private TMP_Text _roomPlayerCountText => GetUI<TMP_Text>("RoomPlayerCountText");
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
        ChangeBox(Box.Room);
    }

    /// <summary>
    /// �÷��̾� ��ȭ�� ���� �� ������Ʈ
    /// </summary>
    private void UpdateChangeRoom()
    {
        UpdatePlayerCount();
    }
    /// <summary>
    /// �÷��̾� ī��Ʈ ������Ʈ
    /// </summary>
    private void UpdatePlayerCount()
    {
        _roomPlayerCountText.SetText($"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
    }

    /// <summary>
    /// ���ڵ� �����/ ���̱�
    /// </summary>
    private void ToggleActiveRoomCode()
    {
        if (_roomCodeText.contentType == TMP_InputField.ContentType.Password)
        {
            // ���̱�
            _roomCodeText.contentType = TMP_InputField.ContentType.Standard;
            _roomCodeActiveText.SetText(HIDETEXT);
        }
        else
        {
            // �����
            _roomCodeText.contentType = TMP_InputField.ContentType.Password;
            _roomCodeActiveText.SetText(SHOWTEXT);
        }
        _roomCodeText.Select();
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// �� ������
    /// </summary>
    private void LeftRoom()
    {
        ActivateLoadingBox(true);
        PhotonNetwork.LeaveRoom();
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
            case Box.Room:
                ClearRoomBox(); 
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// �� �ʱ�ȭ
    /// </summary>
    private void ClearRoomBox()
    {
        _roomTitleText.SetText($"{PhotonNetwork.LocalPlayer.NickName}�� ��".GetText());

        _roomCodeText.text = $"{PhotonNetwork.CurrentRoom.Name}";
        _roomCodeText.contentType = TMP_InputField.ContentType.Standard;
        _roomCodeActiveText.text = HIDETEXT;

        _roomPlayerCountText.SetText($"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}".GetText());
        _roomStartButton.SetActive(false);
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

    // �ʱ� ����
    private void Init()
    {
        _boxs[(int)Box.Room] = GetUI("RoomBox");
    }

    // �̺�Ʈ ����
    private void SubscribesEvent()
    {
        PlayerNumbering.OnPlayerNumberingChanged += UpdateChangeRoom;

        GetUI<Button>("RoomLeftButton").onClick.AddListener(LeftRoom);
        GetUI<Button>("RoomCodeActiveButton").onClick.AddListener(ToggleActiveRoomCode);
    }

}

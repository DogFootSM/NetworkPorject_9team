using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;

    #region private �ʵ�
    enum Box { Lobby ,Size}
    private GameObject[] _boxs = new GameObject[(int)Box.Size];
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
            default:
                break;
        }
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
        GetUI<Button>("LobbyBackButton").onClick.AddListener(LeftLobby);
    }
}

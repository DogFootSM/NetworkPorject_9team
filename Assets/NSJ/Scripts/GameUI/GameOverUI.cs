using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BaseUI 
{
    private GameObject _gooseWinUI => GetUI("GooseWinGround");
    private GameObject _duckWinUI => GetUI("DuckWinGround");

    private void Awake()
    {
        Bind();
    }
    private void Start()
    {
        SubscribesEvent();

        SetActive(false);
        // ������ Ŭ���̾�Ʈ�� �� ��ư �̿� ����        
    }

    /// <summary>
    /// UI ����(false ����)
    /// </summary>
    public void SetActive(bool active)
    {
        GetUI("GooseWinUI").SetActive(active);
    }

    /// <summary>
    /// UI ��Ÿ����, ���� �¸�, ���� �¸� ��������
    /// </summary>
    /// <param name="active"></param>
    /// <param name="type"></param>
    public void SetActive(bool active,PlayerType type)
    {
        GetUI("GooseWinUI").SetActive(active);

        GetUI("BackButton").SetActive(PhotonNetwork.IsMasterClient == true);
        _gooseWinUI.SetActive(type == PlayerType.Goose);
        _duckWinUI.SetActive(type == PlayerType.Duck);
    }

    private void SubscribesEvent()
    {
        GetUI<Button>("BackButton").onClick.AddListener(() => { GameLoadingScene.BackLobby(); });
    }
}

using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBox : BaseUI
{
    private void Awake()
    {
        Bind();
        Init();
    }
    private void Start()
    {
        SubscribesEvents();
    }

    /// <summary>
    /// �ε� ����
    /// </summary>
    private void Stop()
    {
        // TODO : �������� ��� �����ؾ� �ϴ°�
        LobbyScene.SetIsLoadingCancel();
        gameObject.SetActive(false);
    }

    private void Init()
    {

    }
    private void SubscribesEvents()
    {
        GetUI<Button>("LoadingCancelButton").onClick.AddListener(Stop);
    }


}

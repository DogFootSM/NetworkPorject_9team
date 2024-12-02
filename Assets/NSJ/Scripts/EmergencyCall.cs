using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EmergencyCall : BaseUIPun
{
    private GameObject _emergencyCall => GetUI("EmergencyCall");
    private EmergencyCallButton _button => GetUI<EmergencyCallButton>("Button");
    private GameObject _buttonPush => GetUI("ButtonPush");
    private Animator _animator;

    int _closePopUpHash = Animator.StringToHash("ClosePopup");

    private void Awake()
    {
        Bind();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SubscribeEvents();
    }

    private void Update()
    {
        if (VoteScene.Instance != null)
        {
            _emergencyCall.SetActive(false);
            return;
        }
    }

    /// <summary>
    /// ��ư Ŭ���� ���� �̹��� ������
    /// </summary>
    private void ClickDownButton()
    {
        _buttonPush.SetActive(true);
    } 

    /// <summary>
    /// ��ư Ŭ�� ���� �� ���� �̹��� ��ǥ�� �� ���ȸ��
    /// </summary>
    private void ClickUpButton()
    {
        _buttonPush.SetActive(false);
        // ���콺 ����Ʈ�� ��ư ���� ���� �ÿ� ��޼���

        if (GameManager.Instance.GlobalMissionState == true)
            return;

        if (_button.OnButton)
        {
            // TODO : ���ȸ��
            int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();   
            photonView.RPC(nameof(RPCEmergencyCall),RpcTarget.All, playerNumber);
        }
    }


    [PunRPC]
    private void RPCEmergencyCall(int playerNumber)
    {
        StartCoroutine(EmergencyCallRoutine(playerNumber));
    }

    IEnumerator EmergencyCallRoutine(int playerNumber)
    {
        Color reporterColor = PlayerDataContainer.Instance.GetPlayerData(playerNumber).PlayerColor;
        GameUI.ShowEmergency(reporterColor);
        yield return GameUI.Emergency.Duration.GetDelay();
        if (PhotonNetwork.IsMasterClient == true)
        {
            SceneChanger.LoadScene("VoteScene", LoadSceneMode.Additive);
        }
    }


    private void Close()
    {
        StartCoroutine(CloseRoutine());    
    }

    IEnumerator CloseRoutine()
    {
       _animator.Play(_closePopUpHash);
        yield return 0.2f.GetDelay();
        _emergencyCall.SetActive(false);
    }

    private void SubscribeEvents()
    {
        GetUI<Button>("CloseButton").onClick.AddListener(Close);
        _button.OnClickDown += ClickDownButton;
        _button.OnClickUp += ClickUpButton;
    }
}

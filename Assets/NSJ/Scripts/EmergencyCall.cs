using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EmergencyCall : BaseUI
{
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
        if (_button.OnButton)
        {
            // TODO : ���ȸ��
        }
    }

    [PunRPC]
    private void RPCEmergencyCall()
    {
        StartCoroutine(EmergencyCallRoutine());
    }

    IEnumerator EmergencyCallRoutine()
    {
       // GameUI.ShowEmergency();
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
        gameObject.SetActive(false);
    }

    private void SubscribeEvents()
    {
        GetUI<Button>("CloseButton").onClick.AddListener(Close);
        _button.OnClickDown += ClickDownButton;
        _button.OnClickUp += ClickUpButton;
    }
}

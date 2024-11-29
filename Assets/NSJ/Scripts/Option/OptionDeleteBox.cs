using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionDeleteBox : BaseUI
{
    enum Box { Delete, Confirm, Error, Success, Size}
    private GameObject[] _boxs = new GameObject[(int)Box.Size];

    private TMP_InputField _confirmEmailInput => GetUI<TMP_InputField>("ConfirmEmailInput");
    private TMP_InputField _confirmPasswordInput => GetUI<TMP_InputField>("ConfirmPasswordInput");
    private GameObject _confirmButton => GetUI("ConfirmButton");

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
        ChangeBox(Box.Delete);
    }

    /// <summary>
    /// ���� ���� �� ���� ����
    /// </summary>
    private void ConfirmEmail()
    {
        // �̸��� �н����� ĳ��
        string email = _confirmEmailInput.text;
        string password = _confirmPasswordInput.text;

        FirebaseUser user = BackendManager.Auth.CurrentUser;
        Credential credential = EmailAuthProvider.GetCredential(email, password);

        // ����� �̸��� �� ���� �õ�
        user.ReauthenticateAsync(credential)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ChangeBox(Box.Error);
                    return;
                }
                // �̸��� ���� ���� �� ���� ����
                DeleteUser();
            });
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void DeleteUser()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        // ���� ���� �õ�
        user.DeleteAsync()
            .ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled || task.IsFaulted)
                {
                    ChangeBox(Box.Error); 
                    return;
                }

                ChangeBox(Box.Success);
            });
    }


    /// <summary>
    /// ���� ���� ����
    /// </summary>
    private void DisconnectServer()
    {
        // ���� �� �ڵ����� ���� ���� ����
        LobbyScene.ActivateLoadingBox(true);
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// ���� ��ư Ȱ��ȭ üũ
    /// </summary>
    /// <param name="text"></param>
    private void ActivateConfirmButton(string text)
    {
        _confirmButton.SetActive(false);
        // �̸��ϰ� ��й�ȣ �Ѵ� �Է��ؾ߸� Ȱ��ȭ
        if (_confirmEmailInput.text == string.Empty)
            return;
        if (_confirmPasswordInput.text == string.Empty)
            return;
        _confirmButton.SetActive(true);
    }
    /// <summary>
    /// UI �ڽ� ����
    /// </summary>
    /// <param name="box"></param>
    private void ChangeBox(Box box)
    {
        for (int i = 0; i < _boxs.Length; i++) 
        {
            if(i == (int)box)
            {
                _boxs[i].SetActive(true);
                ClearBox(box);
            }
            else
            {
                _boxs[i].SetActive(false);
            }
        }
    }
    /// <summary>
    /// �ڽ� Ŭ���� ����
    /// </summary>
    /// <param name="box"></param>
    private void ClearBox(Box box)
    {
        switch(box)
        {
            case Box.Confirm:
                ClearConfirmBox();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���� ���� �ڽ� Ŭ����
    /// </summary>
    private void ClearConfirmBox()
    {
        _confirmEmailInput.text =string.Empty;
        _confirmPasswordInput.text = string.Empty;
        _confirmButton.SetActive(false);
    }

    private void Init()
    {
        _boxs[(int)Box.Delete] = GetUI("DeleteBox");
        _boxs[(int)Box.Confirm] = GetUI("ConfirmBox");
        _boxs[(int)Box.Error] = GetUI("ErrorBox");
        _boxs[(int)Box.Success] = GetUI("SuccessBox");
    }
    private void SubscribeEvent()
    {
        GetUI<Button>("DeleteButton").onClick.AddListener(()=> 
        {
            if (LobbyScene.Instance != null)
            {
                ChangeBox(Box.Confirm);
            }
        });
        _confirmEmailInput.onValueChanged.AddListener(ActivateConfirmButton);
        _confirmPasswordInput.onValueChanged.AddListener(ActivateConfirmButton);
        GetUI<Button>("ConfirmButton").onClick.AddListener(ConfirmEmail);
        GetUI<Button>("ErrorBackButton").onClick.AddListener(() => ChangeBox(Box.Delete));
        GetUI<Button>("SuccessButton").onClick.AddListener(DisconnectServer);
    }
}

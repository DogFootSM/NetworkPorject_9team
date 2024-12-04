using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainJoinBox : BaseUI
{
    private TMP_InputField _joinRoomInput => GetUI<TMP_InputField>("JoinRoomInput");
    private TMP_InputField _joinNickNameInput => GetUI<TMP_InputField>("JoinNickNameInput");
    private GameObject _joinInvisibleOnImage => GetUI("JoinInvisibleOnImage");

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
        ClearJoinBox();
    }

    /// <summary>
    /// �κ� ����
    /// </summary>
    private void JoinLobby()
    {
        string nickName = _joinNickNameInput.text; // �г��� ĳ��


        LoadingBox.StartLoading(); // �ε�â Ȱ��ȭ

        if (nickName != string.Empty) // �г��� ������ ������ �г��� ����
        {
            nickName.ChangeNickName(); // �г��� ����(�����Ʈ��ũ �г��� ����, �����ͺ��̽� �г��� ����      
        }

        LoadingBox.StartLoading();
        PhotonNetwork.JoinLobby(); // �κ� ����
    }
    /// <summary>
    /// �� �ڵ� �ڵ� �빮�� ��ȯ
    /// </summary>
    private void ChangeRoomCodeToUpper(string value)
    {
        _joinRoomInput.text = value.ToUpper();
    }

    /// <summary>
    /// ���ڵ� �����/���̱�
    /// </summary>
    private void ChangeRoomCodeInvisible()
    {
        if (_joinRoomInput.contentType == TMP_InputField.ContentType.Password) //�Ⱥ��̴� ����
        {
            _joinRoomInput.contentType = TMP_InputField.ContentType.Standard;
            _joinInvisibleOnImage.SetActive(false);
        }
        else
        {
            _joinRoomInput.contentType = TMP_InputField.ContentType.Password;
            _joinInvisibleOnImage.SetActive(true);
        }
        StartCoroutine(ChangeRoomCodeInvisibleRoutine());
    }

    IEnumerator ChangeRoomCodeInvisibleRoutine()
    {
        string temp = _joinRoomInput.text;
        _joinRoomInput.text = string.Empty;
        yield return null;
        _joinRoomInput.text = temp;
    }


    /// <summary>
    /// �� �ڵ�� ����
    /// </summary>
    private void JoinRoomCode()
    {
        string roomCode = _joinRoomInput.text; //�� �ڵ� ĳ��
        if (roomCode == string.Empty)
            return;

        string nickName = _joinNickNameInput.text; // �г��� ĳ��

        LoadingBox.StartLoading(); // �ε�â Ȱ��ȭ

        if (nickName != string.Empty) // �г��� ������ ������ �г��� ����
        {
            nickName.ChangeNickName(); // �г��� ����(�����Ʈ��ũ �г��� ����, �����ͺ��̽� �г��� ����      
        }

        PhotonNetwork.JoinRoom(roomCode); // �� �ڵ�� �� ����
    }


    /// <summary>
    /// �� ����(���� �ϱ� ��ư) ����ȭ
    /// </summary>
    private void ClearJoinBox()
    {
        _joinNickNameInput.text = string.Empty;
        _joinRoomInput.text = string.Empty;
        _joinInvisibleOnImage.SetActive(false);
    }
    private void Init()
    {

    }
    private void SubscribesEvent()
    {
        GetUI<Button>("JoinBackButton").onClick.AddListener(() => MainPanel.ChangeBox(MainPanel.Box.Main));
        GetUI<Button>("JoinBackButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonOff));

        GetUI<Button>("JoinCreateRoomButton").onClick.AddListener(() => MainPanel.ChangeBox(MainPanel.Box.Create));
        GetUI<Button>("JoinCreateRoomButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));

        _joinRoomInput.onValueChanged.AddListener(ChangeRoomCodeToUpper);
        GetUI<Button>("JoinInvisibleButton").onClick.AddListener(ChangeRoomCodeInvisible);
        GetUI<Button>("JoinInvisibleButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));

        GetUI<Button>("JoinLobbyButton").onClick.AddListener(JoinLobby);
        GetUI<Button>("JoinLobbyButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));

        GetUI<Button>("JoinRoomButton").onClick.AddListener(JoinRoomCode);
        GetUI<Button>("JoinRoomButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));
    }
}

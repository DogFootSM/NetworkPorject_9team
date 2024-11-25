using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionQuitBox : BaseUI
{
    private TMP_Text _quitText => GetUI<TMP_Text>("QuitText");
    enum ButtonType { Quit , MainMenu, Size}
    private GameObject _gameQuitButton => GetUI("GameQuitButton");
    private GameObject _mainMenuButton => GetUI("MainMenuButton");
    private GameObject[] _buttons = new GameObject[(int)ButtonType.Size];
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
        ClearQuitBox();
    }

    /// <summary>
    /// �гο� ���� UI ��ȭ
    /// </summary>
    private void ClearQuitBox()
    {
        // �濡 �������� ���� �޴� ��ư�� ��������
        if (PhotonNetwork.InRoom)
        {
            ChangeButton(ButtonType.MainMenu);
            _quitText.SetText("���ư��ðڽ��ϱ�?".GetText());
        }
        else
        {
            ChangeButton(ButtonType.Quit);
            _quitText.SetText("���ô� �ǰ���?".GetText());
        }
    }

    /// <summary>
    /// ��ư ����
    /// </summary>
    /// <param name="button"></param>
    private void ChangeButton(ButtonType button)
    {
        //���ڰ� button �� �ش��ϴ� ��ư ���� ��� false
        for (int i = 0; i < _buttons.Length; i++) 
        {
            if(i == (int)button) 
            {
                _buttons[i].SetActive(true);
            }
            else
            {
                _buttons[i].SetActive(false);
            }
        }
    }
    /// <summary>
    ///  ���� ����
    /// </summary>
   private void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();        
#endif
    }

    /// <summary>
    /// ���� �޴� �̵�
    /// �涰����
    /// </summary>
    private void ChangeMainmenu()
    {
        // �ɼ� ����
        LobbyScene.ActivateOptionBox(false);

        // �ε�ȭ��
        LobbyScene.ActivateLoadingBox(true);
        // �涰����
        PhotonNetwork.LeaveRoom();     
    }

    private void Init()
    {
        _buttons[(int)ButtonType.Quit] = _gameQuitButton;
        _buttons[(int)ButtonType.MainMenu] = _mainMenuButton;       
    }
    private void SubscribeEvent()
    {
        GetUI<Button>("GameQuitButton").onClick.AddListener(GameQuit);
        GetUI<Button>("MainMenuButton").onClick.AddListener(ChangeMainmenu);
    }
}

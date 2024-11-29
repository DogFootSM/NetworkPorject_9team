using GameUIs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public static ReportUI Report { get { return Instance._reportUI; } }
    public static EmergencyUI Emergency { get { return Instance._emergencyUI; } }  
    public static GameStartUI GameStart { get { return Instance._gameStartUI; } }
    public static PlayerUI Player { get { return Instance._playerUI; } }
    public static VoteResultUI VoteResult { get { return Instance._voteResultUI; } }  

    [SerializeField] ReportUI _reportUI;
    [SerializeField] EmergencyUI _emergencyUI;
    [SerializeField] GameStartUI _gameStartUI;
    [SerializeField] PlayerUI _playerUI;
    [SerializeField] VoteResultUI _voteResultUI;

    private void Awake()
    {
        InitSingleTon();
    }

    /// <summary>
    /// �������� UI �����ϸ鼭 Ȱ��ȭ
    /// </summary>
    public static void ShowGameStart(PlayerType type)
    {
        GameStart.gameObject.SetActive(true);
        GameStart.SetUI(type);
    }

    /// <summary>
    /// �÷��̾� UI Ȱ��ȭ
    /// </summary>
    public static void ShowPlayer(PlayerType type)
    {
        Player.gameObject.SetActive(true);
        Player.SetUI(type);
    }

    /// <summary>
    /// ���� Ȱ��ȭ
    /// </summary>
    public static void ShowReport(Color reporterColor, Color corpseColor)
    {
        Report.SetColor(reporterColor, corpseColor);
        Report.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��� ���� Ȱ��ȭ
    /// </summary>
    /// <param name="playerColor"></param>
    public static void ShowEmergency(Color playerColor)
    {
        Emergency.SetColor(playerColor);
        Emergency.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��ǥ ��� Ȱ��ȭ
    /// </summary>
    public static void ShowVoteResult(Color playerColro, string name, PlayerType type)
    {
        VoteResult.SetUI(playerColro, name, type);
        VoteResult.gameObject.SetActive(true);
    }

    private void InitSingleTon()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

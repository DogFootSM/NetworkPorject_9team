using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    //�׽�Ʈ�� �ڵ�

    public static GameManager Instance { get; private set; }
    [field: SerializeField] public bool MissionDelay { get; set; }

    [SerializeField] public Slider _missionScoreSlider;

    private int _totalMissionScore = 30;
    private int _clearMissionScore = 0;
    
    //�׽�Ʈ��
    public int _myScore = 0;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log($"���� �̼� :{_clearMissionScore}"); ;
    }

    public void AddMissionScore()
    {
        _myScore++;
        photonView.RPC(nameof(MissionTotalScore), RpcTarget.AllViaServer, 1); 
    }

    [PunRPC]
    public void MissionTotalScore(int score)
    {
        _clearMissionScore += score; 
        _missionScoreSlider.value = (float)_clearMissionScore / (float)_totalMissionScore;
    }

}

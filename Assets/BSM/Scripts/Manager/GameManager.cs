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
    
     
    //�۷ι� �̼� �˾�â ���� ����
    public bool _globalMission;


    //�׽�Ʈ��
    public int _myScore = 0;

    //�� �۷ι� �̼ǿ��� Ŭ���� ���� �� ����� ���� ��� �ʿ�

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

    /// <summary>
    /// �� Ŭ���̾�Ʈ���� �̼� Ŭ���� �ø��� ���� ����
    /// </summary>
    public void AddMissionScore()
    {
        _myScore++;
        photonView.RPC(nameof(MissionTotalScore), RpcTarget.AllViaServer, 1); 
    }

    /// <summary>
    /// ���� ����ȭ
    /// </summary>
    /// <param name="score"></param>
    [PunRPC]
    public void MissionTotalScore(int score)
    {
        _clearMissionScore += score; 
        _missionScoreSlider.value = (float)_clearMissionScore / (float)_totalMissionScore;
    }

    /// <summary>
    /// �� Ŭ���̾�Ʈ���� �纸Ÿ�� �ɷ� ����� �̼� Ŭ���� ����
    /// </summary>
    public void CompleteGlobalMission()
    {
        photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
    }

    /// <summary>
    /// �纸Ÿ�� �̼� Ŭ���� ���� ����ȭ
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void GlobalMissionRPC(bool value)
    {
        _globalMission = value;

    }

}

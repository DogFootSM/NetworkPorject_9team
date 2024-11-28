using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    //�׽�Ʈ�� �ڵ�

    public static GameManager Instance { get; private set; } 

    [SerializeField] public Slider _missionScoreSlider;

    private int _totalMissionScore = 30;
    private int _clearMissionScore = 0;
    
     
    //�۷ι� �̼� �˾�â ���� ����
    public bool _globalMissionClear;
    public bool _firstGlobalFire;
    public bool _secondGlobalFire;
    public int _globalFireCount = 2;


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
        Debug.Log($"���� �̼� :{_clearMissionScore}");
        Debug.Log($"�Ҳ��� Ƚ�� :{_globalFireCount} / GlobalMission{_globalMissionClear}");
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
    /// �Ҳ��� �̼� ����
    /// </summary>
    public void GlobalFire()
    {
        photonView.RPC(nameof(FireCountSync), RpcTarget.AllViaServer, 1);
    }

    public void FirstFire()
    {
        photonView.RPC(nameof(FirstFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void FirstFireRPC(bool value)
    {
        _firstGlobalFire = value; 
    }

    public void SecondFire()
    {
        photonView.RPC(nameof(SecondFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void SecondFireRPC(bool value)
    {
        _secondGlobalFire = value;
    }



    /// <summary>
    /// �Ҳ��� �̼� ī��Ʈ ����ȭ
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void FireCountSync(int value)
    {
        _globalFireCount -= value;

        if(_globalFireCount < 1)
        {
            photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
        } 
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
        _globalMissionClear = value;

    }

}

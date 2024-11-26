using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnectionMission : MonoBehaviour
{

    private MissionController _missionController;
    private MissionState _missionState;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "���� ��� �����ϱ�";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 4;
    }

    private void Start()
    {
        
    }

    //���� ���� Ŭ�� �� ���콺 ��ǥ�� ���� �̵�
    //�ٸ� ����� ���� �� ���� �ȵ�


    //IF ���� ����� ����
    //�輱�� ���������� ����?
    //���� ����
    //�̼� ī��Ʈ --
    //���� ���� ���İ� 1
    //���� ����
    //���� ���� ���� ���

    //ELSE
    //���� ���� �ȵ�
    //���� �ٽ� �پ��


    private void Update()
    {
        _missionController.PlayerInput();
        WireConnection();
    }

    private void WireConnection()
    {


    }


    private void IncreaseTotalScore()
    {
        //Player�� Ÿ���� �޾� �� �� ������ ����
        PlayerType type = PlayerType.Goose;

        if (type.Equals(PlayerType.Goose))
        {
            //��ü �̼� ���� ����
            //�̼� ���� ����ȭ �ʿ� > ��� �����ð���
            GameManager.Instance.TEST();
        }
    }

    /// <summary>
    /// �̼� Ŭ���� �� ���� ���
    /// </summary>
    private void MissionClear()
    {
        if (_missionState.ObjectCount < 1)
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            _missionController.MissionCoroutine(0.5f);
            IncreaseTotalScore();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockMission : MonoBehaviour
{

    private MissionState _missionState;
    private MissionController _missionController;

    private void Awake() => Init();


    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
       
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 3;
    }

    private void Start()
    {
        //1,2,3 ����
        //1,2,3 ��ư

    }

    private void Update()
    {
        _missionController.PlayerInput();
    }

    private void PullLever()
    {
        //���� ���� �� �� �� Count ����

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

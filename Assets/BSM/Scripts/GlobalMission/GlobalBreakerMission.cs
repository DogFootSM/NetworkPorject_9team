using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBreakerMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;



    //��ư�� Ŭ������ �� �ִϸ��̼� ���
    //��ư ON/OFF ���� ���
    //�Һ� �ִϸ��̼� ���
    //Ŭ���� �� ���� GlobalButton�� ������ �ִ� Count�� ����
    //Count�� 0�� ���� ��
    //������ on/off �� �� �ְ�

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "���� ��ġ��";
    }

    private void Update()
    {
        _missionController.PlayerInput();

        Interaction();
    }

    private void Interaction()
    {



    }


}

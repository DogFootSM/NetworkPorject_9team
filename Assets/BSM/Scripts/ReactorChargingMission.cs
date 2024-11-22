using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//ó�� �÷� �� �������� ���� > �ð��� ������ G �� ���
//G �� 255 �Ǹ� R �� 60���� ���� ����

public class ReactorChargingMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private Animator _chargeAnim;


    private void Awake() => Init();

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "���ڷ� �߽ɺ� �����ϱ�";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 5;
    }

    
    //Input �޾ƿ���,
    //Ŭ���ϰ� �ִ� ����
        //Animation True ����
        //�������� �ִϸ��̼� ���
    //���콺 �� ����
        //False ����
        //�ö���� �ִϸ��̼� ���

    //������ �� ����������? > ������ �ִ� �ð��� üũ�ؼ� �˻�    

        //���� ���� = True 
        //������ ���

    //������ �� > Color.G ����
        //Color.G > 1 ? 
            //Color.R ����
        

    //Slider Value >= 1 ?
        //mission clear
        

    private void IncreaseTotalScore()
    {
        //Player�� Ÿ���� �޾� �� �� ������ ����
        PlayerType type = PlayerType.Duck;

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

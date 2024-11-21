using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPurifierMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private Vector2 _offset = new Vector2(30, -80);
    private GameObject _cord;

    private Animator _cordAnimator;
    private int _cordHash;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        //�ڵ� �ִϸ��̼����� ���� �ʿ�
        _cordAnimator = _missionController.GetMissionObj<Animator>("Spray");
        _cordHash = Animator.StringToHash("SprayBody");

        _cord = _missionController.GetMissionObj("CordObject");
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "������ �����ϱ�";
    }


    private void OnEnable()
    {
        //�ڵ� ��ġ �ʱ�ȭ
        
        _missionState.ObjectCount = 1;
    }

    


    private void Update()
    {
        _cord.transform.position = _missionState.MousePos + _offset;

    }

    private void DrawLineRenderer()
    {
        //���� ������ ��ġ ������ ��ü > �ڵ����

    }

    private void MoveCord()
    {
        //���콺 ��ġ�� ����ٴϰ�
        //���콺���� ���� ��ġ �ʱ�ȭ

    }





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
            _missionController.MissionCoroutine();
            IncreaseTotalScore();
        }
    }


}

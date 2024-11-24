using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;


    private Animator _animator;
    private int _pillowHash;
    private int _idleHash;

    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _missionController = GetComponent<MissionController>();
        _missionState.MissionName = "������ �ε�� ���";
    }

    private void Start()
    {
        _animator = _missionController.GetMissionObj<Animator>("Pillow");
        _pillowHash = Animator.StringToHash("Pillow");
        _idleHash = Animator.StringToHash("PillowIdle"); 
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 15;
    }

    private void Update()
    {
        _missionController.PlayerInput();
        ShakingPillow();
    }

    /// <summary>
    /// ���� Ŭ�� ���
    /// </summary>
    private void ShakingPillow()
    {
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            _animator.Play(_pillowHash);
            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            _missionState.ObjectCount--;
            MissionClear();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //���� ��� ��� �� ��� ���·�
            _animator.Play(_idleHash);
        }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireConnectionMission : MonoBehaviour
{ 
    private MissionController _missionController;
    private MissionState _missionState;

    private GameObject _startPos;
    private RectTransform _wire;

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

    //���� : ���콺 ��ǥ - Wire ��ǥ > Width
    //ȸ���� : ���콺 ��ǥ�� ���� ȸ��?

    /// <summary>
    /// ���� ���� ��� ����
    /// </summary>
    private void WireConnection()
    { 
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            //���� ���� ��ġ
            _startPos = _missionController._searchObj.transform.parent.GetChild(0).gameObject;
        }
        else if (Input.GetMouseButton(0))
        {
            if (_startPos.transform.childCount == 0 || _missionState.MousePos.x < 670f)
            {
                return;
            }

            //���� ��ġ ������Ʈ�� �ڽ� ������Ʈ > wire �̹��� 
            _wire = _startPos.transform.GetChild(0).GetComponent<RectTransform>();

            //670?
            float distance = Vector2.Distance(_wire.transform.position, _missionState.MousePos);

            _wire.sizeDelta = new Vector2(distance, 20);
             
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _wire.sizeDelta = new Vector2(0, 20);
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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WaterPurifierMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;
     
    private Vector2 _socketLocation = new Vector2(-397, 121);
    
    private Vector2 _startPos;
    private GameObject _cord;
    private LineRenderer _linerenderer;

    private Animator _cordAnimator;
    private int _cordHash;
    private Image _test;

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
        _startPos = _cord.gameObject.transform.position;
        _linerenderer = _missionController.GetMissionObj<LineRenderer>("SocketObject");
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
        SelectCordObj();
        DrawLineRenderer(); 
        MissionClear();
        
        
        Debug.Log($"{_cord.transform.position}");
    }

    private void SelectCordObj()
    { 
        if (Input.GetMouseButton(0))
        {
            _missionController.PlayerInput();
            _cord.transform.position = _missionState.MousePos; 
        }

        else if (Input.GetMouseButtonUp(0))
        {
            LocationOfSocket();

            _cord.transform.position = _startPos; 
        } 
    }

    private void DrawLineRenderer()
    {
        //���� ������ ��ġ ������ ��ü > �ڵ����
        //_linerenderer. 
        //UI Linerender �׷���

    }

    private void LocationOfSocket()
    {
        //���콺 ���� �� �ܼ�Ʈ�� ��ġ�ΰ�?
        //�ִϸ��̼� ���
        //Count--;
        //MissionClear;
        //�ܼ�Ʈ�� ��ġ�� �ƴ϶�� �ڵ��� ��ġ�� ó�� ��ġ��
        


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

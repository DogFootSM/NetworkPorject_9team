using POpusCodec.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WaterPurifierMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState; 
    private LineRenderer _linerenderer; 
    private Animator _cordAnimator;
    private int _cordHash;
    private Image _test;
    

    private Rect _socketLocation;
    private Rect _cordStartPos;
    private RectTransform _cord;
    private Coroutine _plugCo;

    private void Awake()
    {
        Init(); 
    }

    private void Start()
    {
        //�ڵ� �ִϸ��̼����� ���� �ʿ�
        _cordAnimator = _missionController.GetMissionObj<Animator>("Cord");
        _cordHash = Animator.StringToHash("Plugin");
 
        _linerenderer = _missionController.GetMissionObj<LineRenderer>("SocketObject");
        _cord = _missionController.GetMissionObj<RectTransform>("CordObject");
 
        _cordStartPos = new Rect(-437, -321, 90, 120);
        _socketLocation = new Rect(-397, 127, 90, 120);
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "������ �����ϱ�";
        
    }


    private void OnEnable()
    { 
        _missionState.ObjectCount = 1; 
    }

    private void OnDisable()
    {
        //�ڵ� ��ġ �ʱ�ȭ 
        _cord.anchoredPosition = _cordStartPos.position; 
    }


    private void Update()
    {
        SelectCordObj();
        DrawLineRenderer();  
    }
 
    private void SelectCordObj()
    {
 
        if (Input.GetMouseButton(0))
        {
            _missionController.PlayerInput();
            _cord.position = _missionState.MousePos; 
        }

        else if (Input.GetMouseButtonUp(0))
        {
            LocationOfSocket();
             
        } 
    }
     
    private void LocationOfSocket()
    { 
        //��,�� | ��,�� ���� ��ǥ �� 
        float socketX1 = _socketLocation.position.x - 10f;
        float socketY1 = _socketLocation.position.y - 10f;
         
        float socketX2 = _socketLocation.position.x + 10f;
        float socketY2 = _socketLocation.position.y + 10f;

        //�ڵ� ������ ��
        float cordX = (int)_cord.anchoredPosition.x;
        float cordY = (int)_cord.anchoredPosition.y;

        if ((cordX > socketX1 && cordY > socketY1) && (cordX < socketX2 && cordY < socketY2))
        {
            
            _cordAnimator.Play(_cordHash);
            _cord.anchoredPosition = _socketLocation.position;
            _missionState.ObjectCount--;
             
            MissionClear();
        } 
        else
        {
            _cord.anchoredPosition = _cordStartPos.position;
        } 

    }
 
     
    private void DrawLineRenderer()
    {
        //���� ������ ��ġ ������ ��ü > �ڵ����
        //_linerenderer. 
        //UI Linerender �׷���

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
            StartCoroutine(PluginCoroutine()); 
            IncreaseTotalScore();
        }
    } 

    private IEnumerator PluginCoroutine()
    {
        yield return Util.GetDelay(1f);
        _missionState.ClosePopAnim();
        yield return Util.GetDelay(0.5f);
        gameObject.SetActive(false);
    }




}


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
    private GameObject _cable;

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
        _cable = _missionController.GetMissionObj("Cable"); 
        _cordStartPos = new Rect(-437, -321, 90, 120);
        _socketLocation = new Rect(-397, 127, 90, 120);
        
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
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
        if (GameManager.Instance.GlobalMissionState || !_missionState.IsPerform)
        {
            gameObject.SetActive(false);
        }

        SelectCordObj();
        FollowCableToCord(); 
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
     
    /// <summary>
    /// �ܼ�Ʈ ��ġ Ȯ�� ���
    /// </summary>
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
 
    
    /// <summary>
    /// �ڵ带 ���� �̵��ϴ� ���̺� ���
    /// </summary>
    private void FollowCableToCord()
    {
        RectTransform width = _cable.GetComponent<RectTransform>();

        //���̺��� �ڵ��� ����
        Vector3 dir = _cable.transform.position - _cord.position; 

        //���̺��� �ڵ带 �ٶ󺸴� �� 
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        //���� ũ�⿡ ���̺��� Width���� �� �Ÿ� ����
        float correctionWidth = dir.magnitude - width.sizeDelta.x;
        
        //������ ������ ���̺� ���� �缳��
        width.sizeDelta = new Vector2(width.sizeDelta.x + correctionWidth, width.sizeDelta.y);
         
        //���̺��� ����ŭ forward�������� ȸ��
        _cable.transform.rotation = Quaternion.AngleAxis(-angle, -Vector3.forward);
         
    }

    private void IncreaseTotalScore()
    {
        PlayerType type = _missionState.MyPlayerType;

        if (type.Equals(PlayerType.Goose))
        {
            GameManager.Instance.AddMissionScore();
        }
    }

    /// <summary>
    /// �̼� Ŭ���� �� ���� ���
    /// </summary>
    private void MissionClear()
    {
        if (_missionState.ObjectCount < 1)
        {
            _missionState.IsPerform = false;
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


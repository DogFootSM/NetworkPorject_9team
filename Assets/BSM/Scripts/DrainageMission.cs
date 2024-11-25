using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrainageMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;

    private List<GameObject> _goList;
    private bool IsSelect;

    private GameObject _go;

    private RectTransform _featherSpawn;
    private RectTransform _eggShellSpawn;
    private RectTransform _friedEggSpawn;
    private RectTransform _dustSpawn1;
    private RectTransform _dustSpawn2;
    private RectTransform _dustSpawn3;


    private float _curObjPosX;
    private float _curObjPosY;

    //���� ���� �� ������Ʈ�� List�� �߰�
    //OnDisable ���� �� ��Ȱ��ȭ �� ������Ʈ�� Ȱ��ȭ �� ��ġ �ʱ�ȭ



    //ELSE IF ���콺 �� ��ư Up
    //IF �ʱ� ��ġ���� ���� �Ÿ���ŭ �̵� �ƴ���
    //�ִϸ��̼� ��� > �ڷ�ƾ 1�� �� > ������Ʈ Active False
    //���� ���



    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _missionController = GetComponent<MissionController>();
        _missionState.MissionName = "���� ���� ����� �ձ�";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 6;
        _goList = new List<GameObject>(_missionState.ObjectCount);
    }

    private void Start()
    {
        SetObjectPos();
    }


    private void OnDisable()
    {
        foreach (GameObject element in _goList)
        {
            element.SetActive(true);
            ResetObjPos(element.gameObject); 
        } 
    }


    private void Update()
    {
        _missionController.PlayerInput();
        CleaningHole();
    }

    /// <summary>
    /// �� ������Ʈ�� �ʱ�ȭ�� ��ġ
    /// </summary>
    private void SetObjectPos()
    {

        _featherSpawn = _missionController.GetMissionObj<RectTransform>("FeatherSpawn");
        _eggShellSpawn = _missionController.GetMissionObj<RectTransform>("EggShellSpawn");
        _friedEggSpawn = _missionController.GetMissionObj<RectTransform>("FriedEggSpawn");
        _dustSpawn1 = _missionController.GetMissionObj<RectTransform>("Dust1Spawn");
        _dustSpawn2 = _missionController.GetMissionObj<RectTransform>("Dust2Spawn");
        _dustSpawn3 = _missionController.GetMissionObj<RectTransform>("Dust3Spawn");

    }


    /// <summary>
    /// GameObject ��ġ ������
    /// </summary>
    /// <param name="go"></param>
    private void ResetObjPos(GameObject go)
    {
        DraingeObjType type = go.GetComponent<MissionObj>().type;
        RectTransform _goRect = go.GetComponent<RectTransform>();
        
        switch (type)
        {
            case DraingeObjType.Feather:
                _goRect.anchoredPosition = _featherSpawn.anchoredPosition;
                break;

            case DraingeObjType.FriedEgg:
                _goRect.anchoredPosition = _friedEggSpawn.anchoredPosition;
                break;

            case DraingeObjType.EggShell:
                _goRect.anchoredPosition = _eggShellSpawn.anchoredPosition;
                break;

            case DraingeObjType.Dust1:
                _goRect.anchoredPosition = _dustSpawn1.anchoredPosition;
                break;

            case DraingeObjType.Dust2:
                _goRect.anchoredPosition = _dustSpawn2.anchoredPosition;
                break;

            case DraingeObjType.Dust3:
                _goRect.anchoredPosition = _dustSpawn3.anchoredPosition;
                break;
        }

    }



    private void CleaningHole()
    {
        if (!_missionState.IsDetect) return;


        //������Ʈ�� �������� ���� ���¿����� ������Ʈ ����
        if (!IsSelect)
        {
            _go = _missionController._searchObj;

            if (!_goList.Contains(_go))
            {
                _goList.Add(_go);
            }
            _curObjPosX = _go.GetComponent<RectTransform>().anchoredPosition.x;
            _curObjPosY = _go.GetComponent<RectTransform>().anchoredPosition.y;


        }

        if (Input.GetMouseButton(0))
        {
            IsSelect = true;
            _go.transform.position = _missionState.MousePos;
            _go.transform.localScale = new Vector3(0.8f, 0.8f);

        }

        else if (Input.GetMouseButtonUp(0))
        {
            RectTransform _rect = _go.GetComponent<RectTransform>();

            (float, float) _rectPos = (_rect.anchoredPosition.x, _rect.anchoredPosition.y);
            (float, float) _xPos = (_curObjPosX - 60, _curObjPosX + 60);
            (float, float) _yPos = (_curObjPosY - 60, _curObjPosY + 60);


            if (_rectPos.Item1 < _xPos.Item1 || _rectPos.Item1 > _xPos.Item2)
            {
                _go.SetActive(false);
                _missionState.ObjectCount--;
                SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            }
            else if (_rectPos.Item2 < _yPos.Item1 || _rectPos.Item2 > _yPos.Item2)
            {
                _go.SetActive(false);
                _missionState.ObjectCount--;
                SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            }

            IsSelect = false;
            _go.transform.localScale = new Vector3(1, 1);
            MissionClear();
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

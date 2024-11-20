using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathroomMission : MonoBehaviour
{
    private int _clearCount = 5;

    private MissionController _missionController;
    private MissionState _missionState;
    private List<GameObject> _stainList = new List<GameObject>(5);
    private Coroutine _clearRoutine;

    private void Awake() => Init();
    private void OnEnable() => _missionState.ObjectCount = 5;

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>(); 
    }

    /// <summary>
    /// �̼� ���� �� ��� ������Ʈ Ȱ��ȭ
    /// </summary>
    private void OnDisable()
    {
        foreach (GameObject ele in _stainList)
        {
            ele.gameObject.SetActive(true); 
        } 
    }

    private void Update()
    {
        _missionController.PlayerInput();
        RemoveTrain();
        MissionClear();
    }


    /// <summary>
    /// ������ ������Ʈ ���� ���
    /// </summary>
    private void RemoveTrain()
    {
        //������ ������Ʈ�� ���� ��� ����
        if (!_missionState.IsDetect) return;

        GameObject go = _missionController._searchObj.gameObject; 
        Image image = go.GetComponent<Image>();

        if (Input.GetMouseButtonDown(0))
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.35f);

            //Alpha ���� 0�� ���� ��� ��Ȱ��ȭ ó�� �� �̼� ���� ī��Ʈ ����
            if (image.color.a < 0)
            { 
                _missionState.ObjectCount--; 
                _stainList.Add(go);
                go.SetActive(false);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
        } 
    }

    /// <summary>
    /// �̼� Ŭ���� �� ���� ���
    /// </summary>
    private void MissionClear()
    {
        if(_missionState.ObjectCount < 1)
        {
            _clearRoutine = StartCoroutine(ClearCoroutine());
            CoroutineManager.Instance.ManagerStartCoroutine(this, _clearRoutine);
        }
    }

    private IEnumerator ClearCoroutine()
    {
        yield return Util.GetDelay(1f);
        //�̼� Ŭ���� ���� ���
        //�� �̼� ������ ����
        gameObject.SetActive(false);
    }


}

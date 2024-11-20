using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathroomMission : MonoBehaviour
{
     
    private MissionController _missionController;
    private MissionState _missionState;
    private List<GameObject> _stainList = new List<GameObject>(5);
    private Coroutine _clearRoutine; 
    

    private void Awake() => Init();

    private void OnEnable()
    {
        //Ȱ��ȭ ���� �� ������� �ö���� �ִϸ��̼�
        //�������� �߰��� MoveGameObject �߰��ؼ� �ִϸ��̼� �����ϸ� �ɵ�
        _missionState.ObjectCount = 5;
         
    }

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
        //�̼� ������� �� �ϴ����� �������� �ִϸ��̼� ���
        IncreaseTotalScore();

        foreach (GameObject element in _stainList)
        {
            Image image = element.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1); 
            element.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        _missionController.PlayerInput();
        RemoveTrain();
        
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
            SoundManager.Instance.SFXPlay(_missionState._clips[0]);

            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.35f);

            //������ ���� ������Ʈ�� ���� ��� ����Ʈ�� �߰�
            if (!_stainList.Contains(go))
            {
                _stainList.Add(go);
            }
             
            //Alpha ���� 0�� ���� ��� ��Ȱ��ȭ ó�� �� �̼� ���� ī��Ʈ ����
            if (image.color.a < 0)
            {
                _missionState.ObjectCount--; 
                go.SetActive(false);
                MissionClear();
            } 
            
        }
    }

    private void IncreaseTotalScore()
    {
        PlayerType type = PlayerType.Duck;

        if (type.Equals(PlayerType.Goose))
        {
            //���ӸŴ��� ���� ����    
        }
    }

    /// <summary>
    /// �̼� Ŭ���� �� ���� ���
    /// </summary>
    private void MissionClear()
    {
        if (_missionState.ObjectCount < 1)
        {
            _clearRoutine = StartCoroutine(ClearCoroutine());
            CoroutineManager.Instance.ManagerStartCoroutine(this, _clearRoutine);
        }
    }

    private IEnumerator ClearCoroutine()
    {
        yield return Util.GetDelay(0.5f);
        //�� �̼� ������ ���� �߰� �ʿ�
        SoundManager.Instance.SFXPlay(_missionState._clips[1]); 
        gameObject.SetActive(false);
    }


}

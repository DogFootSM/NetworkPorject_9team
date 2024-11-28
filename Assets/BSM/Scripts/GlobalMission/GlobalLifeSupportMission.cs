using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLifeSupportMission : MonoBehaviour
{
    //���� ���� ����Ʈ
    //���� Ĩ ����Ʈ

    //������ ���� ����Ʈ

    //��, �� ��� ���� ����Ʈ
    //������ ���Կ� ���� Ĩ ����Ʈ


    //Ĩ���� � Ÿ������ ������ �ְ�, �� Ÿ���� �̿��ؼ� ������ �����̶� ����Ʈ ���ذ��� �ɵ�

    //OnEnable
    //���� ���Կ� �ִ� Ĩ�� ����Ʈ�� ����ְ�
    //���� ����
    //�׸��� ù ��°���� Position ��ġ

    //�� ���� 1 ~ 9 ��° ĭ ���� Position�� ����� �� ���� ������Ʈ ��ġ

    //������ ����
    //�� Ĩ�� Ŭ�� �� �巡���ϸ� �� ���Կ� ��ġ
    //��ġ�� �� ������ ��� ���� ���԰� 0��°���� 8��° ���� Ÿ�� ��

    //��� ����� Ÿ���� ������ �̼� Clear
    //�ϳ��� �ٸ� ��� Fail
    private MissionController _missionController;
    private MissionState _missionState;

    private Vector2 temp;

    [SerializeField] private List<RectTransform> _leftSlots = new List<RectTransform>(9);
    [SerializeField] private List<GameObject> _temps = new List<GameObject>(9);

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();

        _missionState.MissionName = "���� ���� ��ġ �缳���ϱ�";
    }

    

    private void OnEnable()
    {
        //����Ʈ ���� ����
        LeftSlotShuffle();



    }



    /// <summary>
    /// ���� ���� ���� ���
    /// </summary>
    private void LeftSlotShuffle()
    {
        for (int i = 0; i < _leftSlots.Count; i++)
        {
            int rand = Random.Range(0, 9);

            //���� �ε����� ���� ���� ���� �ε��� ����
            if (i == rand)
            {
                while (i != rand)
                {
                    rand = Random.Range(0, 9);
                }
            }

            //��ġ ��ȯ
            temp = _leftSlots[i].anchoredPosition;
            _leftSlots[i].anchoredPosition = _leftSlots[rand].anchoredPosition;
            _leftSlots[rand].anchoredPosition = temp;

            //����Ʈ ��� ��ȯ
            RectTransform tempRect = _leftSlots[i];
            _leftSlots[i] = _leftSlots[rand];
            _leftSlots[rand] = tempRect; 
        }
    }

}

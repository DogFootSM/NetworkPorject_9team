using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPerform : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _missionList = new List<MonoBehaviour>();
    [SerializeField] private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();
 
    private MissionState _missionState;
    private MonoBehaviour _parentClass;
    private int randIndex = 0;
    private int curIndex = 0;

    private void Awake()
    {
        
    }
     
    private void Start()
    {
        SetMissionList();
    }
     
    public void SetMissionList()
    {
        for (int i = 0; i < 3; i++)
        {
            randIndex = Random.Range(0, 8);

            _missionState = _missionList[randIndex].gameObject.GetComponent<MissionState>();
             
            if (_missionState.IsAssign)
            {
                while (true)
                {
                    randIndex = Random.Range(0, 8);
                    _missionState = _missionList[randIndex].gameObject.GetComponent<MissionState>();

                    if (!_missionState.IsAssign)
                    {
                        break;
                    }
                }
            }

            _textList[i].text = _missionState.MissionName;
            _missionState.IsPerform = true;
            _missionState.IsAssign = true;
            _missionState.TextIndex = i;
        }
    }



    //mission state�� perform�� true�� �ֵ鸸 �̼� ���� �����ϰ�.
    //perform�� True�� �ֵ鸸 Text�� State�� MissionName ����

    //�̼��� Ŭ���� ������ �ش� �̼��� perform�� false�� �����ϰ�
    //�ٸ� �̼��� �� True�� �����Ͽ� Text ����

    //�̼� ����Ʈ Ȯ��/��� �ִϸ��̼� �ʿ�

}

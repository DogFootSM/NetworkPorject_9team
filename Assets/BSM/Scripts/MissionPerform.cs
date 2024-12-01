using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionPerform : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _missionList = new List<MonoBehaviour>();
    [SerializeField] private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();

    private MissionState _missionState;
    private MonoBehaviour _parentClass;
    private int randIndex = 0;


    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            randIndex = Random.Range(0, 8);

            _missionState = _missionList[randIndex].gameObject.GetComponent<MissionState>();
             
            _missionState.IsPerform = true; 
        }
    }

    private void OnEnable()
    {
        
        
    }

    //mission state�� perform�� true�� �ֵ鸸 �̼� ���� �����ϰ�.
    //perform�� True�� �ֵ鸸 Text�� State�� MissionName ����
    
        //�̼��� Ŭ���� ������ �ش� �̼��� perform�� false�� �����ϰ�
        //�ٸ� �̼��� �� True�� �����Ͽ� Text ����
         
}

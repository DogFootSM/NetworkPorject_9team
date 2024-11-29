using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalBreakerMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController; 

    private void Awake()
    {
        Init();
    }
     
    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "���� ��ġ��";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 8;
    } 

    private void Update()
    {
        //Ŭ���̾�Ʈ���� �̼��� Ŭ���� ���� ��� ��� Ŭ���̾�Ʈ �̼� �˾�â ��Ȱ��ȭ
        if (GameManager.Instance._globalMissionClear)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput(); 
        Interaction();
    }
     
    /// <summary>
    /// ����ġ �̹������� ��ȣ�ۿ�
    /// </summary>
    private void Interaction()
    {
        if (!_missionState.IsDetect) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            GlobalButton global = _missionController._searchObj.GetComponent<GlobalButton>();
            global.PlayAnimation();
            global.PowerCount--;
             
            if(global.PowerCount < 0)
            {
                if (!(global.PowerCount == -1 && !global.ButtonCheck))
                {
                    _missionState.ObjectCount += global.ButtonCheck ? -1 : 1;
                } 
            } 
            MissionClear();
        } 
    }

    private void MissionClear()
    {
        if (_missionState.ObjectCount > 0) return;

        SoundManager.Instance.SFXPlay(_missionState._clips[1]);
        GameManager.Instance.CompleteGlobalMission();
        _missionController.MissionCoroutine(0.5f); 
    }


}

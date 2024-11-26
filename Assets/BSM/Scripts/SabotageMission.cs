using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SabotageMission : MonoBehaviour
{

    private MissionState _missionState;
    private MissionController _missionController;

    private Image _arm;

    private TextMeshProUGUI _codeText;
    private int _randCode;

    private void Awake()
    {
        Init();
    }
     
    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _missionController = GetComponent<MissionController>();
        _missionState.MissionName = "���� �̼�";
    }

    private void OnEnable()
    {
        _randCode = Random.Range(1000, 10000);
        Debug.Log(_randCode);
    }

    private void Start()
    {
        _arm = _missionController.GetMissionObj<Image>("Arm");
        _arm.color = MissionState.PlayerColor;

        _codeText = _missionController.GetMissionObj<TextMeshProUGUI>("Code");
        SetCodeText();
    }

    private void SetCodeText()
    {
        while(_randCode > 0)
        {

            _codeText.text += (_randCode % 10).ToString() + " ";
            _randCode /= 10;

        }

    }




    //���� �ڵ� = random.range
    //���ڸ� ���� ©�� �ڵ�Text�� �Է�


    //��ư Ŭ�� ������ = ���� �Լ� ���
    //��ư Ű�е� child Text ���� �Է�?


    //InputText ���� �Լ�

}

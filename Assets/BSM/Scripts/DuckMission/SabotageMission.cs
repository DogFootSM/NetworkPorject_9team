using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SabotageMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;

    private Image _arm;

    private TextMeshProUGUI _inputText;
    private TextMeshProUGUI _codeText;
    private int _randCode;

    public event EventHandler OnChangedPassword;


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
        OnChangedPassword += ComparePassword;
    }

    private void Start()
    {
        _arm = _missionController.GetMissionObj<Image>("Arm");
        _arm.color = MissionState.PlayerColor;

        _inputText = _missionController.GetMissionObj<TextMeshProUGUI>("InputText");

        _codeText = _missionController.GetMissionObj<TextMeshProUGUI>("Code");

        SetCodeText();

    }

    private void OnDisable()
    {
        _inputText.text = "";
        _inputText.color = Color.white;
        _inputText.alignment = TextAlignmentOptions.Left;
        _inputText.fontSize = 55;

        OnChangedPassword -= ComparePassword;
        SetCodeText();
    }


    private void SetCodeText()
    {
        _codeText.text = "";
        _randCode = UnityEngine.Random.Range(1000, 10000);
        _codeText.text = Convert.ToString(_randCode, 10);
    }

    public void ClickKeypad(string value)
    {

        _inputText.text += value;
        SoundManager.Instance.SFXPlay(_missionState._clips[0]);

        if (_inputText.text.Length > 3)
        {
            OnChangedPassword?.Invoke(this, EventArgs.Empty);
        }
    }


    /// <summary>
    /// � �纸Ÿ�� �̼����� ������ ���?
    /// ������ ���� ��ġ�صδϱ�
    /// ���⼭ Type�� �����صΰ� Ŭ���� ���� �� Ÿ�Կ� �´� ������ ����?
    /// Player���� � ���� �Ѱ��������
    /// </summary>


    /// <summary>
    /// ��й�ȣ �� ��� �� Text ����
    /// </summary>
    private void ComparePassword(object sender, EventArgs args)
    { 
        if (_codeText.text.Equals(_inputText.text))
        {
            //���� Player �纸Ÿ�� �ɷ� ȹ��
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            Debug.Log("�纸Ÿ�� �̼� ����");
            _inputText.text = "����!";
            _inputText.color = Color.green;
            _inputText.alignment = TextAlignmentOptions.Center;
            _inputText.fontSize = 70;
        }
        else
        {
            Debug.Log("�纸Ÿ�� �̼� ����");
            _inputText.text = "����";
            _inputText.color = Color.red;
            _inputText.alignment = TextAlignmentOptions.Center;
            _inputText.fontSize = 70;
        }

        _missionController.MissionCoroutine(1f);
    } 
}

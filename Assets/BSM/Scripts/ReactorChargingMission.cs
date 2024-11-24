using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;



//ó�� �÷� �� �������� ���� > �ð��� ������ G �� ���
//G �� 255 �Ǹ� R �� 60���� ���� ����

public class ReactorChargingMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;


    private Animator _chargeAnim;
    private Slider _energySlider;
    private Image _leverColor;

    private bool IsPress;
    private int reverseHash;
    private int pressHash; 
    private float _elapsedTime;
    private float _notPressTime;
    private bool _checkPress;

    private float _r;
    private float _g;
    private float _b;


    private void Awake() => Init();
    private void Start()
    {
        _energySlider = _missionController.GetMissionObj<Slider>("LeverSlider");
        _chargeAnim = _missionController.GetMissionObj<Animator>("Lever");
        _leverColor = _missionController.GetMissionObj<Image>("LeverColor");

        reverseHash = Animator.StringToHash("Reverse");
        pressHash = Animator.StringToHash("Press");

        _r = _leverColor.color.r;
        _g = _leverColor.color.g;
        _b = _leverColor.color.b;
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "���ڷ� �߽ɺ� �����ϱ�";


    }

    private void OnEnable()
    {
        _elapsedTime = 0;
        _missionState.ObjectCount = 1;
        IsPress = false;
    }

    private void OnDisable()
    {
        _energySlider.value = 0;
        _leverColor.color = new Color(_r, _g, _b); 
    }


    private Coroutine _beepCo;
    private void Update()
    {
        _missionController.PlayerInput();
        LeverPress();
        ChargeEnergy();



        if (IsPress)
        {
            if(_beepCo == null)
            {
                _beepCo = StartCoroutine(BeepCoroutine());
            }
           
        }
        else
        {
            if (_beepCo != null)
            {
                StopCoroutine(_beepCo);
                _beepCo = null;
            }
        } 
    }


    /// <summary>
    /// ���� ������ �ִ� ���¿� ���� �ִϸ��̼� ���/�ǰ��� ����
    /// </summary>
    private void LeverPress()
    {
        if (!_missionState.IsDetect) return;

        //������ ������ �ִ� ���� Ȯ��
        //2�� �̻� ������ ���� ���� �����̸� �ִϸ��̼� Rebind
        if (!_checkPress)
        {
            _notPressTime += Time.deltaTime;

            if (_notPressTime > 2f && _energySlider.value == 0f)
            {
                _chargeAnim.Rebind();
                _notPressTime = 0;
            }
        }
        else
        {
            _notPressTime = 0;
        } 

        if (Input.GetMouseButton(0))
        {
            _checkPress = true;
            _chargeAnim.SetFloat(reverseHash, 1);
            _chargeAnim.SetBool(pressHash, true);
             
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > 1.5f)
                IsPress = true;

            if (_energySlider.value >= 1f)
            {
                Debug.Log("�̼� Ŭ����");
                _missionState.ObjectCount--;
                MissionClear();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _chargeAnim.SetFloat(reverseHash, -1f);
            _chargeAnim.SetBool(pressHash, false);
             
            _elapsedTime = 0;
            IsPress = false;
            _checkPress = false;
        } 
    }

    /// <summary>
    /// ������ �ִ� ���¿� ���� �����̴� �� ����
    /// </summary>
    private void ChargeEnergy()
    {
        _energySlider.value += IsPress ? Time.deltaTime * 0.1f : (-Time.deltaTime * 0.4f);
        ChargeColorChange();
    }

    /// <summary>
    /// ������ �ִ� �ð� ����Ͽ� �����̴� �÷� �� ����
    /// </summary>
    private void ChargeColorChange()
    {
        //���� �÷��� R,G ��
        float colorG = _leverColor.color.g;
        float colorR = _leverColor.color.r;

        if (IsPress)
        {
            if (colorG <= 1f)
            {
                colorG += Time.deltaTime * (_energySlider.value * 0.3f);
            }
            else
            {
                if (colorR >= 0.5f)
                {
                    colorR += -(Time.deltaTime * 0.3f);
                }
            }
        }
        else
        {
            if (colorR < 1f)
            {
                colorR += Time.deltaTime * 0.3f;
            }
            else
            {
                if (colorG >= 0.12f)
                {
                    colorG += -(Time.deltaTime * (_energySlider.value * 6.5f));
                }
            }

        }
        _leverColor.color = new Color(colorR, colorG, _leverColor.color.b);

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



    /// <summary>
    /// 1�ʸ��� ���� ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeepCoroutine()
    {

        while (true)
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            yield return Util.GetDelay(1f); 
        } 
    }


}

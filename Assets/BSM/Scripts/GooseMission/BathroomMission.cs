using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathroomMission : MonoBehaviour
{
     
    private MissionController _missionController;
    private MissionState _missionState;
    private List<GameObject> _stainList = new List<GameObject>(5);

    private Vector2 _offset = new Vector2(30, -80);
    private GameObject _spray;
    private Animator _sprayAnim;
    private int _sprayHash;
     
    private void Awake() => Init();

    private void Start()
    {
        _sprayAnim = _missionController.GetMissionObj<Animator>("Spray");
        _sprayHash = Animator.StringToHash("SprayBody");
        _spray = _missionController.GetMissionObj("Spray");
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 5;
    }

    private void Init()
    { 
        _missionController = GetComponent<MissionController>(); 
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "����� û���ϱ�";
    }

    /// <summary>
    /// �̼� ���� �� ��� ������Ʈ Ȱ��ȭ
    /// </summary>
    private void OnDisable()
    {
        foreach (GameObject element in _stainList)
        {
            Image image = element.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1); 
            element.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GlobalMissionState)
        {
            gameObject.SetActive(false);
        }

        _spray.transform.position = _missionState.MousePos + _offset;

        _missionController.PlayerInput();
        RemoveTrain(); 
    }


    /// <summary>
    /// ������ ������Ʈ ���� ���
    /// </summary>
    public void RemoveTrain()
    {
        //������ ������Ʈ�� ���� ��� ����
        if (!_missionState.IsDetect) return;

        GameObject go = _missionController._searchObj.gameObject;
        Image image = go.GetComponent<Image>();

        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            _sprayAnim.Play("SprayBody");

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
        PlayerType type = _missionState.MyPlayerType;

        if (type.Equals(PlayerType.Goose))
        {
            GameManager.Instance.AddMissionScore();
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

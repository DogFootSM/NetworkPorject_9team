using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlobalLifeSupportMission : MonoBehaviour
{

    [SerializeField] private List<Chips> _leftSlots = new List<Chips>(9);
    [SerializeField] private List<Chips> _waitSlots = new List<Chips>(9);
    [SerializeField] private List<GameObject> _rightSlots = new List<GameObject>(9);
    [SerializeField] private AudioClip _failCilp;

    private List<GameObject> _returnSlots = new List<GameObject>();

    private bool[] _emptyArr = new bool[9];

    private MissionController _missionController;
    private MissionState _missionState;

    private Vector2 _tempVector;
    private int _randIndex;
    private bool nullCheck;
    private bool compareCheck;

    private RectTransform _emptyRect;
    private GameObject _waitObj;
    private GameObject _rightObj;
    private GameObject _compareSlotObj;

    public event EventHandler OnCheckedSlot;


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
        for (int i = 0; i < _emptyArr.Length; i++)
        {
            _emptyArr[i] = true;
        }


        OnCheckedSlot += CompareSlot;

        //����Ʈ ���� ����
        LeftSlotShuffle();
        WaitSlotShuffle();
    }

    private void OnDisable()
    {
        OnCheckedSlot -= CompareSlot;
        
        //��� �߰� �ݺ���
        for (int i = 0; i < _rightSlots.Count; i++)
        {
            //������ ���Կ� ��ġ�� ������Ʈ�� �ִٸ�
            if (_rightSlots[i].transform.childCount != 0)
            {
                for (int j = 0; j < _emptyArr.Length; j++)
                {   
                    //��⿭ üũ �迭���� false�� ������
                    if (!_emptyArr[j])
                    {
                        //����ִ� ��ҿ� �߰�
                        _waitSlots[j] = _rightSlots[i].transform.GetChild(0).GetComponent<Chips>();
                        _emptyArr[j] = true;
                        break;
                    }
                }
            }
        }



        for (int i = 0; i < _rightSlots.Count; i++)
        {
            //������ ���Կ� ��ġ�� ������Ʈ�� �ִٸ�
            if (_rightSlots[i].transform.childCount != 0)
            {
                for (int j = 0; j < _returnSlots.Count; j++)
                { 
                    if (_returnSlots[j].transform.childCount != 0) continue;

                    //raycast ���� ����
                    _rightSlots[i].GetComponent<Image>().raycastTarget = true;
                    
                    //���̾��Ű ������Ʈ ��ġ ����
                    _rightSlots[i].transform.GetChild(0).SetParent(_returnSlots[j].transform);

                    //raycast ���� ����
                    _returnSlots[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().raycastTarget = true;
                    
                    //�̵� �� ȭ��� ��ġ �缳��
                    _emptyRect = _returnSlots[j].transform.GetChild(0).GetComponent<RectTransform>();
                    _emptyRect.anchoredPosition = new Vector2(0, 0);
                    break;
                }

            }
        } 
    }


    private void Update()
    {
        _missionController.PlayerInput();
        SelectChip();
    }

    /// <summary>
    /// ���� ���� ���� ���
    /// </summary>
    private void LeftSlotShuffle()
    {
        for (int i = 0; i < _leftSlots.Count; i++)
        {
            _randIndex = UnityEngine.Random.Range(0, 9);

            //���� �ε����� ���� ���� ���� �ε��� ����
            if (i == _randIndex)
            {
                while (i != _randIndex)
                {
                    _randIndex = UnityEngine.Random.Range(0, 9);
                }
            }

            //���� ��ġ�� Rect
            RectTransform prevRect = _leftSlots[i].transform.parent.GetComponent<RectTransform>();

            //���� ��ġ�� Rect
            RectTransform nextRect = _leftSlots[_randIndex].transform.parent.GetComponent<RectTransform>();

            //��ġ ��ȯ
            _tempVector = prevRect.anchoredPosition;
            prevRect.anchoredPosition = nextRect.anchoredPosition;
            nextRect.anchoredPosition = _tempVector;

            //����Ʈ ��� ��ȯ
            Chips tempElement = _leftSlots[i];
            _leftSlots[i] = _leftSlots[_randIndex];
            _leftSlots[_randIndex] = tempElement;
        }


    }

    /// <summary>
    /// ��� ���� ���� ���
    /// </summary>
    private void WaitSlotShuffle()
    {
        for (int i = 0; i < _waitSlots.Count; i++)
        {
            _randIndex = UnityEngine.Random.Range(0, 9);

            if (i == _randIndex)
            {
                while (i != _randIndex) _randIndex = UnityEngine.Random.Range(0, 9);
            }

            RectTransform prevRect = _waitSlots[i].transform.parent.GetComponent<RectTransform>();
            RectTransform nextRect = _waitSlots[_randIndex].transform.parent.GetComponent<RectTransform>();

            //��ġ ��ȯ
            _tempVector = prevRect.anchoredPosition;
            prevRect.anchoredPosition = nextRect.anchoredPosition;
            nextRect.anchoredPosition = _tempVector;

            //����Ʈ ��� ��ȯ
            Chips tempElement = _waitSlots[i];
            _waitSlots[i] = _waitSlots[_randIndex];
            _waitSlots[_randIndex] = tempElement;
        }

    }

    /// <summary>
    /// Ĩ ���� �� �̵� ���
    /// </summary>
    private void SelectChip()
    {
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_missionController._searchObj.CompareTag("WaitSlot"))
            {
                _waitObj = _missionController._searchObj;
                _waitObj.transform.localScale = new Vector2(0.8f, 0.8f);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (_waitObj == null) return;

            _waitObj.transform.position = _missionState.MousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _compareSlotObj = _missionController._searchObj.gameObject;

            //���� ���� ������ ��������
            if (_compareSlotObj.CompareTag("RightSlot"))
            {
                if (_waitObj == null) return;

                //�ƹ��͵� ��ġ�Ǿ� ���� ���� �������� �˻�
                if (_compareSlotObj.transform.childCount == 0)
                {
                    //��⿭�� ���ư� ������Ʈ ��ġ ����
                    _returnSlots.Add(_waitObj.transform.parent.parent.gameObject);

                    //������ �������� �̵�
                    _waitObj.transform.parent.SetParent(_compareSlotObj.transform);

                    RectTransform child = _waitObj.GetComponent<RectTransform>();
                    RectTransform parent = _waitObj.transform.parent.GetComponent<RectTransform>();
                    Chips chip = _waitObj.transform.parent.GetComponent<Chips>();

                    //���� �̵� �� ��ġ �缳��
                    child.anchoredPosition = new Vector2(0, 0);
                    parent.anchoredPosition = new Vector2(0, 0);
                    _waitObj.GetComponent<Image>().raycastTarget = false;
                    _compareSlotObj.GetComponent<Image>().raycastTarget = false;
                     
                    //��⿭�� ���ư� �ε���
                    int index = _waitSlots.IndexOf(chip);
                    
                    //�迭�� �� ��° �ε������� �������� ����
                    _emptyArr[index] = false;
                    _waitSlots[index] = null;
                     
                    SoundManager.Instance.SFXPlay(_missionState._clips[0]);

                }
                else
                {
                    _waitObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                }
            }
            else
            {
                if (_waitObj == null) return;

                _waitObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }

            _waitObj.transform.localScale = new Vector2(1, 1);
            _waitObj = null;

            //�̼� Ŭ���� ���� ���� �˻�
            for (int i = 0; i < _waitSlots.Count; i++)
            { 
                nullCheck = _waitSlots[i] == null ? true : false;

                if (!nullCheck)
                {
                    break;
                }
            }

            if (nullCheck)
            {
                OnCheckedSlot?.Invoke(this, EventArgs.Empty);
            } 
        }
    }


    /// <summary>
    /// ����, ������ ������ ���� �����ϰ� ��ġ�Ǿ����� �˻�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void CompareSlot(object sender, EventArgs args)
    {

        for (int i = 0; i < _leftSlots.Count; i++)
        {
            Chips child = _rightSlots[i].transform.GetChild(0).GetComponent<Chips>();

            if (_leftSlots[i]._chipType == child._chipType)
            {
                compareCheck = true;
            }
            else
            {
                compareCheck = false;
                break;
            }
        }

        if (compareCheck)
        {
            Debug.Log("�̼� Ŭ����");

            MissionClear();
        }
        else
        {
            MissionFail();
        }
    }

    private void MissionClear()
    {
        SoundManager.Instance.SFXPlay(_missionState._clips[1]);
        GameManager.Instance.CompleteGlobalMission();
        _missionController.MissionCoroutine(0.5f);
    }

    private void MissionFail()
    {
        SoundManager.Instance.SFXPlay(_failCilp);
        _missionController.MissionCoroutine(0.5f);


    }

}
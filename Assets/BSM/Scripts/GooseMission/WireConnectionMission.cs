using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WireConnectionMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private List<GameObject> _wireList;
    private List<GameObject> _endPointList;
    private List<Image> _startPointList;

    private Dictionary<GameObject, Color> _colorDict;

    private GameObject _startPos;
    private RectTransform _wire;
    private bool isComplete;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "���� ��� �����ϱ�";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 4;
        _wireList = new List<GameObject>(_missionState.ObjectCount);
        _endPointList = new List<GameObject>(_missionState.ObjectCount);
        _startPointList = new List<Image>(_missionState.ObjectCount);

        _colorDict = new Dictionary<GameObject, Color>();
        isComplete = false;
    }

    private void OnDisable()
    {
        //�̼� �˾� â ���� �� ���� ���� ������ ���󺹱�
        foreach (GameObject ele in _wireList)
        {
            RectTransform rect = ele.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 20);
        }

        //endPoint ���� ���󺹱�
        foreach (GameObject ele in _endPointList)
        {
            Image image = ele.GetComponent<Image>();
            image.color = _colorDict[ele];
            image.raycastTarget = true;
            image.transform.GetChild(0).GetComponent<Image>().color = _colorDict[ele];
        }

        //startPoint ���� ���󺹱�
        foreach (Image img in _startPointList)
        {
            img.raycastTarget = true;
        }
    }

    private void Update()
    {
        _missionController.PlayerInput();
        WireConnection();

        if (_missionController._searchObj != null)
            Debug.Log(_missionController._searchObj.name);
    }

    /// <summary>
    /// ���� ���� ��� ����
    /// </summary>
    private void WireConnection()
    {
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            //���� ���� ��ġ
            _startPos = _missionController._searchObj.transform.parent.GetChild(0).gameObject;

            //������ ������Ʈ�� InnerImage�� �ƴϸ� ���� x
            if (_missionController._searchObj.name.Equals("InnerImage"))
            {
                if (!_startPointList.Contains(_missionController._searchObj.GetComponent<Image>()))
                {
                    _startPointList.Add(_missionController._searchObj.GetComponent<Image>());
                }
            }

        }
        else if (Input.GetMouseButton(0))
        {
            //�ڽ��� ���ų� ���콺 ��ġ�� ������ ���� ��� Return
            if (_startPos.transform.childCount == 0 || _missionState.MousePos.x < 670f)
            {
                return;
            }

            //���� ��ġ ������Ʈ�� �ڽ� ������Ʈ > wire �̹��� 
            _wire = _startPos.transform.GetChild(0).GetComponent<RectTransform>();

            if (!_wireList.Contains(_wire.gameObject))
            {
                _wireList.Add(_wire.gameObject);
            }

            MouseTrackingWire(_wire);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            CompareColor();
            MissionClear();
        }
    }


    /// <summary>
    /// ���� ���콺 ��ġ Ʈ��ŷ ���
    /// </summary>
    /// <param name="wire"></param>
    private void MouseTrackingWire(RectTransform wire)
    {
        //wire��ġ - ���콺 ��ġ
        float wireWidth = Vector2.Distance(_wire.transform.position, _missionState.MousePos);

        //Wire ����
        _wire.sizeDelta = new Vector2(wireWidth, 20);

        //Wire ����
        float x = _missionState.MousePos.x;
        float y = _missionState.MousePos.y;

        //���콺 ��ġ - wire ��ġ
        Vector3 dir = new Vector3(x, y, 0) - _wire.transform.position;

        //Wire ȸ����
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //Wire ȸ��
        _wire.transform.rotation = Quaternion.AngleAxis(-angle, -Vector3.forward);
    }


    /// <summary>
    /// ���� �� ���
    /// </summary>
    private void CompareColor()
    {
        if (_wire == null) return;

        Image endPointImage = _missionController._searchObj.GetComponent<Image>();
        Color endPointColor = endPointImage.color;

        //�������� Wire�� �� ��
        if (endPointColor.CompareRGB(_wire.GetComponent<Image>().color) && _missionState.MousePos.x > 700f)
        {
            float endR = endPointColor.r;
            float endG = endPointColor.g;
            float endB = endPointColor.b;

            endPointImage.color = new Color(endR, endG, endB, 1);
            Image childGlow = endPointImage.transform.GetChild(0).GetComponent<Image>();
            childGlow.color = new Color(endR, endG, endB, 1);

            //����Ʈ�� ������Ʈ�� ���� ��� �߰�
            if (!_endPointList.Contains(endPointImage.gameObject))
            {
                _endPointList.Add(endPointImage.gameObject);

                //�߰��� ������Ʈ�� Ű�� ���� ��� �߰�
                if (!_colorDict.ContainsKey(endPointImage.gameObject))
                {
                    _colorDict[endPointImage.gameObject] = new Color(endR, endG, endB, 0.37f);
                }
            }

            //������ �Ϸ������ wire�� ���� ����
            _wire = null;

            //����� ���� ������ ã�Ƽ� RaycastTarget Off
            foreach (Image img in _startPointList)
            {
                if (img.color.CompareRGB(endPointColor))
                {
                    img.raycastTarget = false;
                    endPointImage.raycastTarget = false;
                }
            }

            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            _missionState.ObjectCount--;

        }
        else
        {
            _wire.sizeDelta = new Vector2(0, 20);
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
        if (_missionState.ObjectCount < 1 && !isComplete)
        {
            isComplete = true;
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            _missionController.MissionCoroutine(0.5f);
            IncreaseTotalScore();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionController : BaseMission
{ 
    //UI 상호작용
    private GraphicRaycaster _graphicRaycaster;
    private Canvas _grCanvas;
    private PointerEventData _ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> _rayResult = new List<RaycastResult>();

    private GameObject _spray; 
    private Vector2 _offset = new Vector2(30, -80);

    //팝업 종료 코루틴
    private Coroutine _closeCo;

    private MissionState _missionState;
    [HideInInspector] public GameObject _searchObj;
 
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 얼룩 청소 미션 오브젝트 초기화
    /// </summary>
    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _grCanvas = transform.parent.GetComponent<Canvas>();
        _graphicRaycaster = _grCanvas.GetComponent<GraphicRaycaster>();
        GetMissionComponent<Button>("MissionCloseButton").onClick.AddListener(CloseMissionPopUp); 
        _spray = GetMissionObject("Spray");
    }
 


    /// <summary>
    /// 미션 진행 중 오브젝트 감지
    /// </summary>
    public void PlayerInput()
    {
        _missionState.MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y); 
        _spray.transform.position = _missionState.MousePos + _offset;

        _ped.position = _missionState.MousePos;

        _rayResult.Clear(); 

        _graphicRaycaster.Raycast(_ped, _rayResult);
  
        if (_rayResult.Count > 0)
        {
            if (_rayResult[0].gameObject.name.Equals("MissionCloseButton"))
                return;

            //감지한 오브젝트를 넘겨줌
            _searchObj = _rayResult[0].gameObject;
            _missionState.IsDetect = true;
        }
        else
        {
            _missionState.IsDetect = false;
        }
    }
     
    /// <summary>
    /// 미션 팝업 종료
    /// </summary>
    private void CloseMissionPopUp()
    {
        SoundManager.Instance.SFXPlay(_missionState._clips[2]);
        MissionCoroutine();
    }

    /// <summary>
    /// 공용으로 사용할 팝업 종료 애니메이션 코루틴
    /// </summary>
    public void MissionCoroutine()
    {
        _closeCo = StartCoroutine(CloseMission());
        CoroutineManager.Instance.ManagerStartCoroutine(this, _closeCo);
    }

    private IEnumerator CloseMission()
    {
        _missionState.ClosePopAnim();
        yield return Util.GetDelay(0.5f);
        gameObject.SetActive(false);
    }

    public T GetMission<T>(string name) where T : Component
    { 
        return GetMissionComponent<T>(name);
    }


}

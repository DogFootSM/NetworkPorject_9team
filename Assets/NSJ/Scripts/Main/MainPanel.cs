using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour 
{
    public static MainPanel Instance;

    [SerializeField] private Button _settingButton;

    public enum Box { Main, Quick, Join, Create, Size }

    [System.Serializable]
    struct BoxStruct
    {
        public GameObject Main;
        public GameObject Quick; 
        public GameObject Join;
        public GameObject Create;
    }
    [SerializeField] BoxStruct _boxStruct;

    private GameObject[] _boxs = new GameObject[(int)Box.Size];
    private static GameObject[] s_boxs { get { return Instance._boxs; } set { Instance._boxs = value; } }

    private StringBuilder _sb = new StringBuilder();


    private void Awake()
    {
        InitSingleTon();
        Init();
    }

    private void Start()
    {
        SubscribesEvent();
    }

    private void OnEnable()
    { 
        if (LobbyScene.Instance != null &&LobbyScene.IsJoinRoomCancel == true) // �ε� ĵ�� �ʱ�ȭ �� UI ���� ����
        {
            LobbyScene.IsJoinRoomCancel = false;
            return;
        }

        ChangeBox(Box.Main);
    }

    private void OnDisable()
    {
        if (LobbyScene.IsJoinRoomCancel == true) // �ε� ĵ�� ��
        {
            CancelJoinRoom();
        }
    }

    /// <summary>
    /// �� ���� ĵ��
    /// </summary>
    private void CancelJoinRoom()
    {
        LoadingBox.StartLoading();
        PhotonNetwork.LeaveRoom();
    }


    /// <summary>
    /// UI �ڽ� ����
    /// </summary>
    public static void ChangeBox(Box box)
    {
        LoadingBox.StopLoading();

        for (int i = 0; i < s_boxs.Length; i++)
        {
            if (s_boxs[i] == null)
                return;

            if (i == (int)box) // �ٲٰ��� �ϴ� �ڽ��� Ȱ��ȭ
            {
                s_boxs[i].SetActive(true);
                //ClearBox(box); // �ʱ�ȭ �۾��� ����
            }
            else
            {
                s_boxs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void Init()
    {
        _boxs[(int)Box.Main] = _boxStruct.Main;
        _boxs[(int)Box.Quick] = _boxStruct.Quick;
        _boxs[(int)Box.Join] = _boxStruct.Join;
        _boxs[(int)Box.Create] = _boxStruct.Create;
    }

    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    private void SubscribesEvent()
    {
        _settingButton.onClick.AddListener(() => OptionPanel.SetActiveOption(true));
        _settingButton.onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));
    }

    /// <summary>
    /// �̱��� ����
    /// </summary>
    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

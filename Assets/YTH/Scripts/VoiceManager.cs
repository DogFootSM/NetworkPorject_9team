using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";
    public static VoiceManager Instance { get; private set; }

    [SerializeField] PlayerController _controller;

    public PunVoiceClient _voiceClient;

    public Photon.Voice.Unity.Recorder _recorder;

    public VoiceConnection _voiceConnection;

    private const byte LIVING_GROUP = 1;
    private const byte DEAD_GROUP = 2;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }


    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Util.GetDelay(3f);
        Debug.Log("�� ���� ��  ` ` ` ");
        PhotonNetwork.JoinRoom(RoomName);
    }
    public override void OnJoinedRoom()
    {
        // �� ���̽�
        if (_voiceClient == null)
        {
            _voiceClient = PunVoiceClient.Instance;
        }

        if (_recorder == null)
        {
            Debug.LogError("Recorder is not assigned!");
            return;
        }
    }


    // �÷��̾� ��Ʈ�ѿ��� ȣ���� ��
    public void SetVoiceChannel(bool isGhost)
    {
        // ������� => �׷� 1 ���
        if (!_controller.isGhost)
        {
            _recorder.InterestGroup = LIVING_GROUP;
            Debug.Log("������� : 1�� ä�� �̿���");
        }
        // ���� �÷��̾� => �׷� 2�� ���
        else
        {
            _recorder.InterestGroup = DEAD_GROUP;
            Debug.Log("����Ͽ� ä�� ��ȯ : 2�� ä��");
        }
    }
}

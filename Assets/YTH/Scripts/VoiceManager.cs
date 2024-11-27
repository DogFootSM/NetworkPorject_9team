using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
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

        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        Debug.Log(PhotonNetwork.NetworkClientState);

        //// ���� ����Ǿ� �뿡 ������ ���̽� Ȱ��ȭ
        //if (PhotonNetwork.InRoom)
        //{
        //_recorder.TransmitEnabled = true;
        //}
        //else
        //{
        //_recorder.TransmitEnabled = false;
        //}
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

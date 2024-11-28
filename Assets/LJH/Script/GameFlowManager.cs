using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviourPunCallbacks   // ���߿� ������
{
    public static GameFlowManager Instance;

    private float Succ = 0f;
    public const string RoomName = "TestRoom";


    //�ʿ��� ������ : �̼� ������ , 

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // ���� �ν��Ͻ��� ������ ���� ������ ��ü�� ����
        }
    }
    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false; // ����� ��

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // �濡 ���� ~   
        StartCoroutine(StartDelayRoutine());
    }
    IEnumerator StartDelayRoutine()
    {
        yield return 1.5f.GetDelay(); // ��Ʈ��ũ �غ�� ���ð� �ʿ� 
        TestGameStart();
    }

    public void TestGameStart()
    {
        // ���� ���� 

        PlayerSpawn();
    }




    public void ReportingOn() 
    {
        // Todo : �Ű�Ǹ� ��ǥ������ �̵�
        Debug.Log("��ǥ ���� !");
    }
    public void MissionTest() 
    {
        Succ += 5f;

        if (Succ > 100f) 
        {
            MissonClearWin();
        }
    }
    public void MissonClearWin() 
    {
        Debug.Log("�̼� �¸� !");
    }
    public void SabotageSucc() 
    {
        Debug.Log("�纸Ÿ�� !");
    }

    //public void DuckWin() { }

    private void PlayerSpawn()
    {
        Vector2 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
        // ���� �ڸ��� �迭�� �޾Ƴ��� �������� ���� �÷��̾�� ��ȯ��Ű�� �ɵ� 

        GameObject obj = PhotonNetwork.Instantiate("LJH_Player", randPos, Quaternion.identity);
        GameObject panel = PhotonNetwork.Instantiate("NamePanel", randPos, Quaternion.identity);
        panel.GetComponent<UiFollowingPlayer>().setTarget(obj);
    }
}

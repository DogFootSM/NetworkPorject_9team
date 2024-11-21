using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "DebugGameScene1111";
    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDealyRoutine());
    }

    IEnumerator StartDealyRoutine()
    {
        yield return new WaitForSeconds(1f); // ��Ʈ��ũ �غ� �ʿ��� �ð� ��¦ �ֱ�
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("���� ����");
        PlayerSpawn();

        if (PhotonNetwork.IsMasterClient == false)
            return;
    }

    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate("YTH2/Player", randomPos, Quaternion.identity);
    }
}

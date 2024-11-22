using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VoteScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] private Transform _playerPanelParent;

    private List<VotePanel> playerPanels = new List<VotePanel>();

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
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
        SpawnPlayerPanel();
    }

    public void SpawnPlayerPanel()
    {
        // 1. �� �÷��̾� �г� ����
        VotePanel myPanel = PhotonNetwork.Instantiate("PlayerPanel", Vector3.zero, Quaternion.identity).GetComponent<VotePanel>();
        playerPanels.Add(myPanel);
        Debug.Log(myPanel);
        Debug.Log(_playerPanelParent);
        myPanel.transform.SetParent(_playerPanelParent);
        myPanel.SetPlayerPanel(PhotonNetwork.LocalPlayer);


        // 2. �ٸ� �÷��̾���� �г� ����
        //foreach (Player player in PhotonNetwork.PlayerList)
        //{
        //   // if (player == PhotonNetwork.LocalPlayer)
        //   //     return;
        //   //
        //    VotePanel otherPanels = PhotonNetwork.Instantiate("PlayerPanelPrefab", Vector2.zero, Quaternion.identity).GetComponent<VotePanel>(); // �ٸ� �÷��̾� �ǳ� ����
        //    playerPanels.Add(otherPanels);
        //    otherPanels.transform.SetParent(_playerPanelParent);
        //    otherPanels.SetPlayerPanel(player);
        //}
    }


}

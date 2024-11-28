using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] PlayerReadyUI _readyUI;
    private PlayerController _myPlayer;

    private void Start()
    {
        SubscribesEvents();
    }

    private void SubscribesEvents()
    {
        LobbyScene.Instance.OnJoinedRoomEvent += SpawnPlayer;
        LobbyScene.Instance.OnPlayerEnteredRoomEvent += SetPlayerToNewPlayer;
        LobbyScene.Instance.OnPlayerPropertiesUpdateEvent += SetPlayerPropertiesUpdate;
        LobbyScene.Instance.OnMasterClientSwitchedEvent += SetMasterClientSwitched;
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    private void SpawnPlayer()
    {
        // ������ġ ����
        Vector2 randomPos = new Vector2(Random.Range(-8, 8), Random.Range(1, 4));

        // �ش� ���� ��ġ�� ����
        GameObject playerInstance = PhotonNetwork.Instantiate("LJH_Player",randomPos, Quaternion.identity);
        // ���� �÷��̾� ĳ��
        _myPlayer = playerInstance.GetComponent<PlayerController>();
        SetPlayerToOrigin();
    }

    /// <summary>
    /// ���� �ִ� �÷��̾�鿡�� ���� �÷��̾� ����ȭ
    /// </summary>
    private void SetPlayerToOrigin()
    {
        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerToOrigin), RpcTarget.All, id, PhotonNetwork.LocalPlayer);
    }

    /// <summary>
    /// ���� ���� �÷��̾�� ���� �÷��̾� ����ȭ
    /// </summary>
    private void SetPlayerToNewPlayer(Player newPlayer)
    {
        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerToNewPlayer), RpcTarget.All, id, PhotonNetwork.LocalPlayer, newPlayer);
    }


    /// <summary>
    /// ���� �ִ� �÷��̾�鿡�� ���� �÷��̾� ����ȭ RPC
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerToOrigin(int id, Player player)
    {
        PhotonView playerView = PhotonView.Find(id);
        SetPlayer(playerView, player);
    }

    /// <summary>
    ///  ���� ���� �÷��̾�� ���� �÷��̾� ����ȭ RPC
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerToNewPlayer(int id, Player player, Player newPlayer)
    {
        // ���ο� �÷��̾ ������ �ƴϸ� �Լ��� ������ ����
        if (newPlayer != PhotonNetwork.LocalPlayer)
            return;
        PhotonView playerView = PhotonView.Find(id);
        SetPlayer(playerView, player);
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    private void SetPlayer(PhotonView photonView, Player player)
    {
        TMP_Text nickNameText = photonView.GetComponentInChildren<TMP_Text>();
        nickNameText.SetText(player.NickName);

        // ���� UI ����
        PlayerReadyUI readyUI = Instantiate(_readyUI, photonView.transform);
        if (player.IsMasterClient == true)
        {
            readyUI.ChangeImage(PlayerReadyUI.Image.Master);
        }
        else
        {
            bool ready = player.GetReady();
            if (ready == true)
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.Ready);
            }
            else
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.UnReady);
            }
        }
    }

    /// <summary>
    ///  �÷��̾� ������Ƽ ����
    /// </summary>
    private void SetPlayerPropertiesUpdate(Player player, PhotonHashTable arg1)
    {
        if (player != PhotonNetwork.LocalPlayer)
            return;

        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerProperty), RpcTarget.All, id, player);
    }

    /// <summary>
    /// ������ Ŭ���̾�Ʈ ����
    /// </summary>
    private void SetMasterClientSwitched(Player newMaster)
    {
        if (newMaster != PhotonNetwork.LocalPlayer)
            return;

        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerProperty), RpcTarget.All, id, newMaster);
    }

    [PunRPC]
    private void RPCSetPlayerProperty(int id, Player player)
    {
        PhotonView playerView = PhotonView.Find(id);

        PlayerReadyUI readyUI = playerView.GetComponentInChildren<PlayerReadyUI>();
        if (player.IsMasterClient == true)
        {
            readyUI.ChangeImage(PlayerReadyUI.Image.Master);
        }
        else
        {
            bool ready = player.GetReady();
            if (ready == true)
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.Ready);
            }
            else
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.UnReady);
            }
        }

    }

}

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using System.Collections;
using UnityEngine;

public class SeparationVoice : MonoBehaviourPun
{
    PlayerDataContainer _playerDataContainer => PlayerDataContainer.Instance;

    private Speaker _speaker;

    IEnumerator Start()
    {
        yield return null;
        Speaker speaker = GetComponentInChildren<Speaker>();
        speaker.transform.SetParent(null);
        _speaker = speaker;
    }

    private void Update()
    {
        if (_playerDataContainer == null || LobbyScene.Instance != null || _speaker == null)
            return;
        SeparateVoice();


        // ������
        if (VoteScene.Instance == null)
        {
            _speaker.gameObject.SetActive(true);
        }
        // ��ǥ��
        else
        {
            _speaker.gameObject.SetActive(false);
        }
    }

    public void SeparateVoice()
    {
        photonView.RPC(nameof(SeparateVoiceRpc), RpcTarget.AllBuffered);
    }

    // �÷��̾� ��� �� ����Ŀ ��ġ �����Ͽ� ���̽� �и�

    [PunRPC]
    public void SeparateVoiceRpc()
    {
        if (_playerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        {
            _speaker.transform.position = new Vector3(0, 0, 30);
        }
        else
        {
            _speaker.transform.position = transform.position;
        }
    }
}


using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;

    [SerializeField] VotePanel _votePanel;

    public int[] voteCounts; // �� �÷��̾��� ��ǥ���� �迭�� ����

    public void OnClickPlayerPanel(int index) // �÷��̾� �г��� ���� ��ǥ
    {
        photonView.RPC("VotePlayerRPC", RpcTarget.All, index);
        //_playerData.DidVote =true;
    }

    [PunRPC]
    public void VotePlayerRPC (int index)
    {
        voteCounts[index]++;
        Debug.Log(index);
    }

    public void OnClickSkip() // ��ŵ ��ư ���� ��
    {
        _voteData.SkipCount++;

      // if (photonView.IsMine == false)
      //     return;
      // _playerData.DidVote = true;
    }



}

using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;

    [SerializeField] VotePanel _votePanel;

    public int[] voteCounts;

    public void OnClickPlayerPanel(int index) // �÷��̾� �г��� ���� ��ǥ
    {
        photonView.RPC("VotePanelClickedRPC", RpcTarget.All, index);
      
    }

    [PunRPC]
    public void VotePanelClickedRPC(int index)
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

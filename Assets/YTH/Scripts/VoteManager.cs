using Photon.Pun;
using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VotePanel _votePanel;

    [SerializeField] VoteScenePlayerData[] _playerData;

    public int[] _voteCounts; // �� �÷��̾���(ActorNumber�� ����� �ε��� ��ȣ)�� ��ǥ���� �迭�� ����


    // IsDead == false �϶��� ��ǥ �����ϰ� ���� �߰�
    public void Vote(int index) // �÷��̾� �г��� ���� ��ǥ
    {
        photonView.RPC("VotePlayerRPC", RpcTarget.All, index);
        foreach (var button in VotePanel._voteButtons)
        {
            button.interactable = false;
        }
    }

    [PunRPC]
    public void VotePlayerRPC(int index)
    {
        _voteCounts[index]++;
        Debug.Log($"{index}�� �÷��̾� ��ǥ�� {_voteCounts[index]} ");
    }

    // IsDead == false �϶��� ��ŵ �����ϰ� ���� �߰�
    public void OnClickSkip() // ��ŵ ��ư ���� ��
    {
        photonView.RPC("OnClickSkipRPC", RpcTarget.AllBuffered);
        foreach (var button in VotePanel._voteButtons)
        {
            button.interactable = false;
        }
    }

    [PunRPC]
    public void OnClickSkipRPC()
    {
        _voteData.SkipCount++;
        Debug.Log($" ��ŵ �� : {_voteData.SkipCount}");
    }

    // ��ǥ ���� �� ���� ���
    public void GetVoteResult()
    {
        // �ִ� ��ǥ�� ã�� ���
        int top = -1;
        int top2 = -1;
        int playerIndex = -1;

        for (int i = 0; i < 12; i++)
        {
            if (_voteCounts[i] > top)
            {
                top = _voteCounts[i];
                playerIndex = i;  
            }
            else if (_voteCounts[i] == top)
            {
                top2 = _voteCounts[i];
                Debug.Log("����ǥ�� ���� ��~");
                return;
            }
        }

        // ��ŵ ī��Ʈ��ŭ �͸� �̹��� ����
        // ��ǥ �� ��ŭ �͸� �̹��� ����

        Debug.Log($"{_voteData.SkipCount}ǥ ���!");
        Debug.Log($"{playerIndex}�� �÷��̾� �缱 {top}ǥ : �߹�˴ϴ�");
        
        //TODO : ��Ʈ�� �Ǵ� ���
        return;
    }
}

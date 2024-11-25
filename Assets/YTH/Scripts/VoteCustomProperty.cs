using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class VoteCustomProperty
{
    // ��� �÷��̾�� ["VotedCount"]�� Ű ������ ������ Ŀ����������Ƽ ����
    public static void InitCustomProperties(this Player[] playerList)
    {
        foreach (Player player in  PhotonNetwork.PlayerList)
        {
            PhotonHashtable properties = new PhotonHashtable();
            properties["VotedCount"] = 0;
            player.SetCustomProperties(properties);
        }
    }

    // �÷��̾� �г��� ������ �� ȣ��Ǵ� Ŀ����������Ƽ �Լ�
    // ["VotedCount"] �� ��ȭ�� ����� targetPlayer�� curVotedNum + 1 (���� ��ǥ �� + 1)
    public static void VotePlayer(this Player targetPlayer)
    {
        int curVotedNum = targetPlayer.CustomProperties.ContainsKey("VotedNum") ? (int)targetPlayer.CustomProperties["VotecdNum"] : 0;

        PhotonHashtable properties = new PhotonHashtable();
        properties["VotedNum"] = curVotedNum + 1;
        targetPlayer.SetCustomProperties(properties);

        Debug.Log($"{targetPlayer.NickName} �� ��ǥ �� : {curVotedNum + 1} ");
    }

    //votepanel �� �̵� �� ��
    // ��ǥ ����� �˷��ִ� �Լ�
    // ��� �÷��̾��� ["VotedCount"] �� ��ȭ ����� �˷���
    public static void GetVoteResult()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int votedResults = player.CustomProperties.ContainsKey("VoteCount") ? (int)player.CustomProperties["VotedNum"] : 0;
            Debug.Log($"{player.NickName} �� �� ��ǥ �� : {votedResults}");
            // votedResults ��ŭ targetPlayer�� �гο� �͸� �̹��� ����
        }

        // ��ǥ���� ���� ���� �÷��̾� �缱
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            //TODO : �ݺ��� ������ ã�Ƴ���
        }
    }

  //  if (properties.ContainsKey(CustomProperty.READY))
  //      {
  //          UpdatePlayers();
} //


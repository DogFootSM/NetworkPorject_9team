using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class VoteCustomProperty
{
   private static VoteScenePlayerData _playerData;

   private const string VOTEDCOUNT = "VotedCount";


   // ��� �÷��̾�� ["VotedCount"]�� Ű ������ ������ Ŀ����������Ƽ ����
   public static void InitCustomProperties(this Player[] playerList)
   {
       foreach (Player player in  PhotonNetwork.PlayerList)
       {
           PhotonHashtable properties = new PhotonHashtable();
           properties[VOTEDCOUNT] = 0;
           player.SetCustomProperties(properties);
       }
   }


   // �÷��̾� �г��� ������ �� ȣ��Ǵ� Ŀ����������Ƽ �Լ�
   // ["VotedCount"] �� ��ȭ�� ����� targetPlayer�� curVotedNum + 1 (���� ��ǥ �� + 1)
   public static void VotePlayer(this Player targetPlayer)
   {
       // Containskey�� true �̸� ���� �Ѱ��ְ�
       // false �̸� 0�� �Ѱ���
       int num = targetPlayer.CustomProperties.ContainsKey(VOTEDCOUNT) ? (int)targetPlayer.CustomProperties[VOTEDCOUNT] : 0;
     
       PhotonHashtable properties = new PhotonHashtable();
       properties[VOTEDCOUNT] = num++;
       targetPlayer.SetCustomProperties(properties);

       Debug.Log($"{targetPlayer.NickName} �� ��ǥ �� : {_playerData.VoteCount + 1} ");
   }


   //votepanel �� �̵� �� ��
   // ��ǥ ����� �˷��ִ� �Լ�
   // ��� �÷��̾��� ["VotedCount"] �� ��ȭ ����� �˷���
   public static void GetVoteResult(this Player player)
   {
       foreach (Player players in PhotonNetwork.PlayerList)
       {
           int votedResults = player.CustomProperties.ContainsKey(VOTEDCOUNT) ? (int)player.CustomProperties[VOTEDCOUNT] : 0;
           Debug.Log($"{player.NickName} �� �� ��ǥ �� : {_playerData.VoteCount}");
           //TODO : votedResults ��ŭ targetPlayer�� �гο� �͸� �̹��� ����
       }

       // ��ǥ���� ���� ���� �÷��̾� �缱 ����
       foreach (Player players in PhotonNetwork.PlayerList)
       {
           //TODO : �ݺ��� ������ ã�Ƴ���
       }

   }
} 


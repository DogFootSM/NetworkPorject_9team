using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataContainer : MonoBehaviourPun
{
    [SerializeField] public PlayerData[] playerDataArray;

    private int MaxPlayers = 15;
    public static PlayerDataContainer Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }

        // �迭  �ʱ�ȭ
        playerDataArray = new PlayerData[MaxPlayers];
        for (int i = 0; i < MaxPlayers; i++)
        {
            playerDataArray[i] = new PlayerData("None", PlayerType.Goose, Color.white, true);
        }
        
    }
    
    public void SetPlayerData(int actorNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost) 
    {
        photonView.RPC("RpcSetPlayerData", RpcTarget.AllBuffered, actorNumber , playerName, type, Rcolor,Gcolor,Bcolor, isGhost);
    }
    public PlayerData GetPlayerData(int actorNumber) 
    {
        return playerDataArray[actorNumber-1];
    }
    public void InitPlayerList()
    {
        
        for (int i = 0; i < 15; i++)
        {
            Debug.Log("�迭 �ʱ�ȭ");
            int actorNumber = i+1;
            string playerName = "None";
            PlayerType type = PlayerType.Goose; // �⺻��
            float Rcolor = 1f;
            float Gcolor = 1f;
            float Bcolor = 1f;
            bool isGhost = false;// �⺻��

            SetPlayerData(actorNumber, playerName, type, Rcolor, Gcolor, Bcolor, isGhost);
        }
        
    }
    public void UpdatePlayerGhostList(int actorNumber) 
    {
        photonView.RPC("RpcUpdatePlayerGhostList", RpcTarget.All, actorNumber);
    }
     


    [PunRPC]
    private void RpcUpdatePlayerGhostList(int actorNumber) 
    {
        playerDataArray[actorNumber].IsGhost = true;
        Debug.Log($"{actorNumber}�� �÷��̾�� ����{playerDataArray[actorNumber-1].IsGhost}");
    }

    [PunRPC]
    private void RpcSetPlayerData(int actorNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {
        Debug.Log("������ ����");
        int index = actorNumber - 1; // �迭 �ε��� ��ȯ
        Color color = new Color(Rcolor, Gcolor, Bcolor,255f);

        // �迭 ��Ұ� null�� ��� �ʱ�ȭ
        if (playerDataArray[index] == null)
        {
            playerDataArray[index] = new PlayerData(playerName, type, color, isGhost);
        }
        else
        {
            // �̹� ���� �ִ� ��� ������Ʈ
            playerDataArray[index].PlayerName = playerName;
            playerDataArray[index].Type = type;
            playerDataArray[index].PlayerColor = color;
            playerDataArray[index].IsGhost = isGhost;
        }
    }

}

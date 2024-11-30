using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingScene : MonoBehaviourPun
{
    // �κ���� �־���� �κ� �ִ� ������ ������ �������� ���� 
    // ���� �����ϸ� ���� ������� 
    // ��ǥ�� ��ȯ
    [SerializeField] Transform[] SpawnPoints;

    private GameObject player;
    public static GameObject MyPlayer { get { return Instance.player; } }
    private Color color;

    private bool isOnGame = false;

    public static GameLoadingScene Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // ���� �ν��Ͻ��� ������ ���� ������ ��ü�� ����
        }
        SpawnPoints = new Transform[6];
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))  // üũ������ ����� �� ������ �� 
        {
            GameOverKill();
            GameOverMission();
        }
    }

    public void GameStart()
    {
        SceneChanger.LoadLevel(1);
        isOnGame = true;
        StartCoroutine(Delaying());
    }



    IEnumerator Delaying()
    {
        yield return 2f.GetDelay();

        RandomSpawner(); // ���� ���� �� ��ȯ 
        yield return null;

        PlayerDataContainer.Instance.RandomSetjob(); // ���� ���� ���� 
        photonView.RPC(nameof(RpcSyncPlayerData), RpcTarget.AllBuffered);
    }


    // ���� ���� ���� 
    // �����¸� : ���� ��� ���
    // �����¸� : ���� ��� ��� , �̼� ������ �޼�
    // => ��ǥ ���� , �÷��̾� ����ø��� ���� Ȯ��    �̼��Ҷ����� ������ Ȯ�� 
    // Ȯ���ؼ� ���� ������ ���� �����ϱ�
    // ���� ���� ������ �Ǹ� ��� ���â �����ְ� �ٽ� ���� �κ��
    // ���� üũ�� ����(PhotonNetwork.MasterClient)�� �ؾ��ϳ�? �ƴ� �� �̵��� rpc�� ������� 
    private int GooseNotDead = 0; // ������ ���� , ����
    private int DuckNotDead = 0;
    public void GameOverKill() // ��ǥ���� �� ���νø��� ȣ�� 
    {   
        
        PlayerDataContainer.Instance.SetPlayerTypeCounts();
        GooseNotDead = PlayerDataContainer.Instance.GooseCount;
        DuckNotDead = PlayerDataContainer.Instance.DuckCount;
        Debug.Log($"���� : ���� {GooseNotDead} ���� {DuckNotDead}");

        if (GooseNotDead < DuckNotDead)// ������ ���� �������� ������ ���� �¸� , ��ǥ���� ���̱�ϱ�   or ���� ������ ������
        {
            // �����¸��� ���� ��� ǥ�� �� �κ�� �̵�
        }
        else if (DuckNotDead == 0)  // ������ �� ������  ���� �¸� 
        {
            // �����¸��� ���� ��� ǥ�� �� �κ�� �̵�
        }
    }
    public void GameOverMission() // �̼ǿϷ�ø��� ȣ�� 
    {
        
        if (GameManager.Instance._missionScoreSlider.value == 1f) 
        {
            // �̼ǿϷ�¸��� ���� ��� ǥ�� �� �κ�� �̵� 
        }
        
    }




    private void RandomSpawner()
    {
        photonView.RPC("RpcRandomSpawner", RpcTarget.All);
    }

    private void spawnPlayer(Vector3 Pos)
    {
        player = PhotonNetwork.Instantiate("LJH_Player", Pos, Quaternion.identity);
        color = PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).PlayerColor;
        GameObject panel = PhotonNetwork.Instantiate("NamePanel", Pos, Quaternion.identity);
        panel.GetComponent<UiFollowingPlayer>().setTarget(player);

    }

    [PunRPC]
    private void RpcRandomSpawner()
    {
        GameObject obj = GameObject.Find("SpawnPoint");

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SpawnPoints[i] = obj.transform.GetChild(i);
        }
        int x = Random.Range(0, obj.transform.childCount);

        spawnPlayer(SpawnPoints[x].position);
    }

    [PunRPC]
    private void RpcSyncPlayerData()
    {
        player.GetComponent<PlayerController>().SettingColor(color.r, color.g, color.b);  // �ϴ� ���� �� ������ �ȵ� 
        player.GetComponent<PlayerController>().SetJobs();
        PlayerDataContainer.Instance.SetPlayerTypeCounts();
    }
}

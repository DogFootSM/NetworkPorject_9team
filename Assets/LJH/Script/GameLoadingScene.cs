using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingScene : MonoBehaviourPun
{
    // �κ���� �־���� �κ� �ִ� ������ ������ �������� ���� 
    // ���� �����ϸ� ���� ������� 
    // ��ǥ�� ��ȯ
    [SerializeField] Transform[] SpawnPoints;


    private bool isOnGame= false;

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
        PlayerDataContainer.Instance.RandomSetjob(); // ���� ���� ���� 
        player.GetComponent<PlayerController>().SettingColor(color.r, color.g, color.b);  // �ϴ� ���� �� ������ �ȵ� 
        player.GetComponent<PlayerController>().SetJobs(); 
    }

    private void RandomSpawner()   
    {
        photonView.RPC("RpcRandomSpawner", RpcTarget.All);
    }
    private GameObject player;
    private Color color;
    private void spawnPlayer(Vector3 Pos) 
    {
        player = PhotonNetwork.Instantiate("LJH_Player", Pos, Quaternion.identity);
        color = PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.ActorNumber).PlayerColor;
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
}

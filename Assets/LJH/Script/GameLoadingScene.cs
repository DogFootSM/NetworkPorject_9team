using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingScene : MonoBehaviour
{
    // �κ���� �־���� �κ� �ִ� ������ ������ �������� ���� 
    // ���� �����ϸ� ���� ������� 
    // ��ǥ�� ��ȯ
    [SerializeField] Transform[] SpawnPoints;
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
    }



    private void RandomSpawner() 
    {
        //GameObject obj =  GameObject.Find("SpawnPoint");
        //Debug.Log(obj.transform.childCount);
        //obj.transform.childCount;
    }
}

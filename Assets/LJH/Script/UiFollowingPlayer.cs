using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiFollowingPlayer : MonoBehaviourPun
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] TMP_Text nameTxt;
    [SerializeField] GameObject MasterIcon;
    [SerializeField] GameObject ReadyIcon;

   
    
    private void Start()
    {   
        name = PhotonNetwork.LocalPlayer.NickName; // �ٲ�� �ҵ�? 

        if (PhotonNetwork.IsMasterClient == true) 
        {
            if(photonView.IsMine == true)
                photonView.RPC("RpciconActive", RpcTarget.AllBuffered, "Master", true);
        }
        if (photonView.IsMine == true)
        {
            photonView.RPC("RpcSetNicknamePanel", RpcTarget.AllBuffered, name);
            gameObject.AddComponent<TestNamePanelHide>();
        }
    }
    private void Update()
    {
        Following();
    }

    public void setTarget(GameObject obj) 
    {
        target = obj.transform;
    }

    private void Following() 
    {
        if (target == null) 
        {
            return;
        }
        
        transform.position = target.position+offset;

        
    }
    public void Ready() 
    {
        photonView.RPC("RpciconActive", RpcTarget.AllBuffered, "Ready",true);

        // ���� �ް� ��ư�� ������ ������ ����� ��� �ؾ��ϳ�?
    }

    //���� ���� ������ �� ������ rpc�� �ؾ� �� 
    PlayerType[] playerTypes;
    private void NameToRed()
    {
        for (int i = 0; i < PlayerDataContainer.Instance.playerDataArray.Length; i++)
        {
            playerTypes[i] = PlayerDataContainer.Instance.GetPlayerJob(i);
        }

        if (PlayerDataContainer.Instance.GetPlayerJob(PhotonNetwork.LocalPlayer.GetPlayerNumber()) == PlayerType.Duck)
        {
            //
        }
    }

    [PunRPC]

    private void RpciconActive(string name , bool isActive) 
    {
        if (name == "Ready")
        {
            ReadyIcon.SetActive(isActive);  
        }
        else if (name == "Master")
        {
            MasterIcon.SetActive(isActive);
        }
        else 
        {
            Debug.Log("�ٸ��� ���� �̸�");
        }
    }

    [PunRPC]

    private void RpcSetNicknamePanel(string name) 
    {
        
            nameTxt.text =name;
    }
}

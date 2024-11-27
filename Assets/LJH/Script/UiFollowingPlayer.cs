using Photon.Pun;
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

    string name;

    private void Start()
    {   
        name = PhotonNetwork.LocalPlayer.NickName;

        if (PhotonNetwork.IsMasterClient == true) 
        {
            if(photonView.IsMine == true)
                photonView.RPC("RpciconActive", RpcTarget.AllBuffered, "Master", true);
        }

        photonView.RPC("RpcSetNicknamePanel", RpcTarget.AllBuffered,name);


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

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviourPunCallbacks, IPunObservable
{
    TMP_Text _tmpText;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       int value = stream.SendAndReceiveStruct(1); // ���� ����ȭ
       GameObject gameObject = stream.SendAndReceiveClass(this.gameObject); // ������Ʈ ����ȭ
       float lack = info.GetLack(); // ���� �ð� ��������
        _tmpText.SetText("�ؽ�Ʈ".GetText()); // �ؽ�Ʈ �޽� ���� ���� 
    }
    IEnumerator TestRoutine()
    {
        yield return 0.751f.GetDelay(); // ������ ������ ��
    }
}

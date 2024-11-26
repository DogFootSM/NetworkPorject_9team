using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.Rendering;

[System.Serializable]
public class VoteSceneData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _skipCount; // ��ŵ�� ��� ��
    public int SkipCount { get { return _skipCount; } set { _skipCount = value; } }


    [SerializeField] private float _reportTimeCount; // �Ű��ڸ� ���� �� �ִ� �ð�
    public float ReportTimeCount { get { return _reportTimeCount; } set { _reportTimeCount = value; } }


    [SerializeField] public float _voteTimeCount; // ��ǥ ���� �ð�
    public float VoteTimeCount { get { return _voteTimeCount; } set { _voteTimeCount = value; } }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(SkipCount);
            stream.SendNext(ReportTimeCount);
            stream.SendNext(VoteTimeCount);
        }
        else if (stream.IsReading)
        {
            SkipCount = (int)stream.ReceiveNext();
            ReportTimeCount = (float)stream.ReceiveNext();
            VoteTimeCount = (float)stream.ReceiveNext();
        }
    }
}

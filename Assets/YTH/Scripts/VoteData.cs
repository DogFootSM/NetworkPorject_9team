using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.Rendering;

[System.Serializable]
public class VoteData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _voteCount; // ��ǥ ��
    public int VoteCount { get { return _voteCount; } set { _voteCount = value; } }


    [SerializeField] private int _skipCount; // ��ŵ�� ��� ��
    public int SkipCount { get { return _skipCount; } set { _skipCount = value; } }


    [SerializeField] private float _reportTimeCount; // �Ű��ڸ� ���� �� �ִ� �ð�
    public float ReportTimeCount { get { return _reportTimeCount; } set { _reportTimeCount = value; } }


    [SerializeField] public float _voteTimeCount; // ��ǥ ���� �ð�
    public float VoteTimeCount { get { return _voteTimeCount; } set { _voteTimeCount = value; } }


    [SerializeField] private bool _didVote; // ��ǥ�ߴ��� ����
    public bool DidVote { get { return _didVote; } set { _didVote = value; } }


    [SerializeField] private bool _isDead; // �׾����� ����
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }


    [SerializeField] private bool _isReporter;
    public bool IaReporter { get { return _isReporter; } set { _isReporter = value; } }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_voteCount);
            stream.SendNext(_skipCount);
            stream.SendNext(_reportTimeCount);
            stream.SendNext(_voteTimeCount);
            stream.SendNext(_didVote);
            stream.SendNext(_isDead);
            stream.SendNext(_isReporter);
        }
        else if (stream.IsReading)
        {
            _voteCount = (int)stream.ReceiveNext();
            _skipCount = (int)stream.ReceiveNext();
            _reportTimeCount = (int)stream.ReceiveNext();
            _voteTimeCount = (int)stream.ReceiveNext();
            _didVote = (bool)stream.ReceiveNext();
            _isDead = (bool)stream.ReceiveNext();
            _isReporter = (bool)stream.ReceiveNext();
        }
    }
}

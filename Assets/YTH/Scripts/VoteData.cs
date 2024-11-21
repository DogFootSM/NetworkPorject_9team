using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoteData
{
    [SerializeField] private int _voteCount; // ��ǥ ��
    public int VoteCount { get { return _voteCount; } set { _voteCount = value; } }


    [SerializeField] private int _skipCount; // ��ŵ�� ��� ��
    public int SkipCount { get { return _skipCount; } set { _skipCount = value; } }


    [SerializeField] private float _reportTimeCount; // �Ű��ڸ� ���� �� �ִ� �ð�
    public float ReportTimeCount { get { return _reportTimeCount; } set { _reportTimeCount = value; } }


    [SerializeField] public float _voteTimeCount; // ��ǥ ���� �ð�
    public float VoteTimeCount { get { return VoteTimeCount; } set { VoteTimeCount = value; } }


    [SerializeField] private bool _didVote; // ��ǥ�ߴ��� ����
    public bool DidVote { get { return _didVote; } set { _didVote = value; } }


    [SerializeField] private bool _isDead; // �׾����� ����
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }


    [SerializeField] private bool _isReporter;
    public bool IaReporter { get { return _isReporter; } set { _isReporter = value; } }



}

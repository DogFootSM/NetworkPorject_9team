using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class VotePanel : MonoBehaviour
{
    [SerializeField] VoteData _voteData;

    [SerializeField] Image _characterImage; // ��ǥâ���� �� �÷��̾� ĳ���� �̹���

    [SerializeField] Image _voteSignImage; // ��ǥ�� �÷��̾� ǥ�� �̹���

    [SerializeField] Image _deadSignImage; // ���� ���� ǥ�� �̹���

    [SerializeField] TMP_Text _nickNameText; // �� �÷��̾� �г���

    [SerializeField] GameObject _votePanel; // ��ǥâ ��ü �г�

    [SerializeField] GameObject _playerPanel; // �� �÷��̾� �г�

    [SerializeField] Button _voteButton; // �� �÷��̾� �г��� ���� �� ��ǥ �Ǵ� ��ư

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // �Ű��ڸ� ���� �� �ִ� �ð� ī��Ʈ

    [SerializeField] Slider _voteTimeCountSlider; // ��ǥ ���� �ð� ī��Ʈ

    [SerializeField] Player _targetPlayer; // ������ �÷��̾� 


    public void SetPlayerPanel(Player target)     // �� �÷��̾� �г��� �ʱ�ȭ�ϴ� �Լ�
    {
        _targetPlayer = target;

        _nickNameText.text = target.NickName;


       // �������� ĳ���ʹ� �� �����÷��̾�Ը� ǥ��
       // if (localPlayer.IsImposter == true)
       // {
       //     _nickNameText.color = Color.red ;
       // }
       //

       //TODO : _characterImage = ""; // ĳ���� �̹��� �ҷ�����

       // �÷��̾ ���� ���¶�� �� �÷��̾� �г��� ������ ȸ�� �г� on

    }
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _voteData.ReportTimeCount = _reportTimeCountSlider.value;
        _voteData.VoteTimeCount = _voteTimeCountSlider.value;
    }

    private void Update()
    {
        _voteData.ReportTimeCount -= (float)PhotonNetwork.Time;

        if (_voteData.ReportTimeCount == 0)
        {
            _voteData.VoteTimeCount -= (float)PhotonNetwork.Time;
        }
    }

    public void Vote(Player targetPlayer) // �÷��̾� �г��� ���� ��ǥ
    {
        //TODO: ���� �÷��̾� ��ǥ�� ++;
        _voteData.DidVote = true;
    }

    public void OnClickSkip() 
    {
        _voteData.SkipCount++;
        _voteData.DidVote = true;
    }


}

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;


    // [SerializeField] GameObject _characterImage; // ��ǥâ���� �� �÷��̾� ĳ���� �̹���

    //  [SerializeField] Image _voteSignImage; // ��ǥ�� �÷��̾� ǥ�� �̹���

    //  [SerializeField] Image _deadSignImage; // ���� ���� ǥ�� �̹���

    [SerializeField] GameObject _anonymPlayerImage; // ��ǥ�� �͸��� �÷��̾� �̹���

    [SerializeField] TMP_Text _nickNameText; // �� �÷��̾� �г���

    [SerializeField] GameObject _votePanel; // ��ǥâ ��ü �г�

    [SerializeField] GameObject _playerPanel; // �� �÷��̾� �г�

    [SerializeField] Button _voteButton; // �� �÷��̾� �г��� ���� �� ��ǥ �Ǵ� ��ư

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // �Ű��ڸ� ���� �� �ִ� �ð� ī��Ʈ

    [SerializeField] Slider _voteTimeCountSlider; // ��ǥ ���� �ð� ī��Ʈ

    [SerializeField] Player _targetPlayer; // ������ �÷��̾� 

    private PlayerType _playerType; // Duck�� ���Ǿ�

    public void SetPlayerPanel(Player player)     // �� �÷��̾� �г��� �ʱ�ȭ�ϴ� �Լ�
    {
         _nickNameText.text = player.NickName;
        //TODO : _characterImage = ""; // ĳ���� �̹��� �ҷ�����
        //TODO : �÷��̾ ���� ���¶�� �� �÷��̾� ��ǥ ��ư ��Ȱ��ȭ
        _targetPlayer = player;
    }

    private void Awake()
    {
        Init();
        PhotonNetwork.PlayerList.InitCustomProperties();
    }

    private void Init()
    {
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        _voteTimeCountSlider.value = _voteData.VoteTimeCount;
    }
   

    private void Update()
    {
        CountTime();
    }

    public void OnClickPlayerPanel() // �÷��̾� �г��� ���� ��ǥ
    {
        PhotonNetwork.LocalPlayer.VotePlayer();
        _playerData.DidVote = true;
    }

    
    public void OnClickSkip() // ��ŵ ��ư ���� ��
    {
        _voteData.SkipCount++;
        _playerData.DidVote = true;
       
    }

    public void CountTime()
    {
        _voteData.ReportTimeCount -= (float)Time.deltaTime;
        Debug.Log(_voteData.ReportTimeCount);

        if (_voteData.ReportTimeCount <= 0)
        {

            _reportTimeCountSlider.gameObject.SetActive(false);
            _voteData.VoteTimeCount -= (float)PhotonNetwork.Time;
            Debug.Log(_voteData.VoteTimeCount);
        }
    }

   
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    [Header("�纸Ÿ�� �ɷ� ��� ���� ����")]
    [SerializeField] private AudioClip _sirenClip;
    [SerializeField] private AudioClip _bgmClip;
    [SerializeField] private Image _sirenPanelImage;
    [SerializeField] private Button _fireBtn;
    [SerializeField] private Button _lifeBtn;
    [SerializeField] private Button _breakerBtn;


    public bool GlobalMissionState;

    public static GameManager Instance { get; private set; }

    [SerializeField] public Slider _missionScoreSlider;

    private int _totalMissionScore = 30;
    private int _clearMissionScore = 0;


    [Header("�۷ι� �̼� �˾�â ���� ���� ����")]
    public bool GlobalMissionClear = true;

    [Header("�� ������ �̼� Ŭ���� ����")]
    public bool FirstGlobalFire;
    public bool SecondGlobalFire;
    public int GlobalFireCount = 2;


    private SabotageType _useAbility;
    public SabotageType UserAbility { get; set; }


    private bool _sabotageFire;
    public bool SabotageFire
    {
        get { return _sabotageFire; }

        set
        {
            _sabotageFire = value;
            _fireBtn.interactable = _sabotageFire;
        }
    }

    private bool _sabotageLife;
    public bool SabotageLife
    {
        get
        {
            return _sabotageLife;
        }
        set
        {
            _sabotageLife = value;
            _lifeBtn.interactable = _sabotageLife;
        }
    }


    private bool _sabotageBreaker;
    public bool SabotageBreaker
    {
        get
        {
            return _sabotageBreaker;
        }
        set
        {
            _sabotageBreaker = value;
            _breakerBtn.interactable = _sabotageBreaker;
        }
    }

    private void Awake()
    {
        SetSingleton();
        _fireBtn.onClick.AddListener(DuckFireAbilityInvoke);
        _lifeBtn.onClick.AddListener(DuckLifeAbilityInvoke);
        _breakerBtn.onClick.AddListener(DuckBreakerAbilityInvoke); 
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log($"���� �ɷ� ���� ���� : fire : {_sabotageFire}, Life:{_sabotageLife}, br{_sabotageBreaker}");
    }

    private void DuckFireAbilityInvoke()
    {
        _useAbility = SabotageType.Fire; 
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true, _useAbility);
    }

    private void DuckLifeAbilityInvoke()
    {
        _useAbility = SabotageType.OxygenBlock;
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true, _useAbility);
    }

    private void DuckBreakerAbilityInvoke()
    {
        _useAbility = SabotageType.BlackOut;
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true, _useAbility);
    }
     
    /// <summary>
    /// Duck ���� �纸Ÿ�� �ɷ� ��� ����ȭ
    /// </summary>
    public void DuckAbilityInvoke()
    {
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void DuckAbilityRPC(bool value, SabotageType type)
    {
        switch (type)
        {
            case SabotageType.Fire:
                SabotageFire = false;
                break;

            case SabotageType.BlackOut:
                SabotageBreaker = false;
                break;

            case SabotageType.OxygenBlock:
                SabotageLife = false;
                break;
        }
        Debug.Log($"�ߵ��� �ɷ� :{type}");

        GlobalMissionClear = false;
        GlobalMissionState = value;
        SoundManager.Instance.BGMPlay(_sirenClip);
        StartCoroutine(SirenCoroutine());
    }

    /// <summary>
    /// ���̷� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator SirenCoroutine()
    {
        float value = 0f;

        while (!GlobalMissionClear)
        {
            if (!_sirenPanelImage.gameObject.activeSelf)
            {
                _sirenPanelImage.gameObject.SetActive(true);
            }

            Color alpha = _sirenPanelImage.color;
            value = alpha.a;

            if (alpha.a > 0f)
            {
                while (true)
                {
                    value -= Time.deltaTime * 0.5f;

                    _sirenPanelImage.color = new Color(1f, 0, 0, value);

                    if (_sirenPanelImage.color.a < 0f) break;

                    yield return null;
                }
            }


            if (alpha.a < 0f)
            {

                while (true)
                {
                    value += Time.deltaTime * 0.5f;

                    _sirenPanelImage.color = new Color(1f, 0, 0, value);

                    if (_sirenPanelImage.color.a > 0.43f) break;

                    yield return null;
                }
            }

        }

        _useAbility = SabotageType.None;
        yield break;
    }

    //------ �������� ������ �̼�

    /// <summary>
    /// �� Ŭ���̾�Ʈ���� �̼� Ŭ���� �ø��� ���� ����
    /// </summary>
    public void AddMissionScore()
    {
        photonView.RPC(nameof(MissionTotalScore), RpcTarget.AllViaServer, 1);
    }

    /// <summary>
    /// ���� ����ȭ
    /// </summary>
    /// <param name="score"></param>
    [PunRPC]
    public void MissionTotalScore(int score)
    {
        _clearMissionScore += score;
        _missionScoreSlider.value = (float)_clearMissionScore / (float)_totalMissionScore;
    }

    /// <summary>
    /// �Ҳ��� �̼� ����
    /// </summary>
    public void GlobalFire()
    {
        photonView.RPC(nameof(FireCountSync), RpcTarget.AllViaServer, 1);
    }

    public void FirstFire()
    {
        photonView.RPC(nameof(FirstFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void FirstFireRPC(bool value)
    {
        FirstGlobalFire = value;
    }

    public void SecondFire()
    {
        photonView.RPC(nameof(SecondFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void SecondFireRPC(bool value)
    {
        SecondGlobalFire = value;
    }

    /// <summary>
    /// �Ҳ��� �̼� ī��Ʈ ����ȭ
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void FireCountSync(int value)
    {
        GlobalFireCount -= value;

        if (GlobalFireCount < 1)
        {
            photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
        }
    }

    /// <summary>
    /// �� Ŭ���̾�Ʈ���� �纸Ÿ�� �ɷ� ����� �̼� Ŭ���� ����
    /// </summary>
    public void CompleteGlobalMission()
    {
        photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
    }

    /// <summary>
    /// �纸Ÿ�� �ɷ� �̼� Ŭ���� ���� ����ȭ
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void GlobalMissionRPC(bool value)
    {
        GlobalMissionClear = value;
        GlobalMissionState = false;
        _sirenPanelImage.gameObject.SetActive(false);
        SoundManager.Instance.BGMPlay(_bgmClip);
    }

}

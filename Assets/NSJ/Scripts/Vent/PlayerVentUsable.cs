using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using UnityEngine;

public class PlayerVentUsable : MonoBehaviourPun
{
    [HideInInspector] public bool InVent;

    private PlayerController _player;
    private Vent _vent;

    bool _isClickButton;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (GameUI.Instance != null)
        {
            GameUI.Player.EnterVentButton.onClick.AddListener(() => { _isClickButton = true; });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine == false) 
            return;

        if (PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost == true)
            return;
        if (_player.playerType == PlayerType.Goose)
            return;



        if(_enterTriggerRoutine == null)
        {
            _enterTriggerRoutine = StartCoroutine(EnterTriggerRoutine(collision));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (photonView.IsMine == false) 
            return;
        if (_player.playerType == PlayerType.Goose) 
           return;


        if (_enterTriggerRoutine != null)
        {
            StopCoroutine(_enterTriggerRoutine);
            _enterTriggerRoutine = null;
        }
    }

    Coroutine _enterTriggerRoutine;
    /// <summary>
    /// Ʈ���� ���� �� ��Ʈ �Է� ���
    /// </summary>
    IEnumerator EnterTriggerRoutine(Collider2D collision)
    {
        while (true)
        {
            // ��Ʈ���� ���� ����Ʈ Ŭ����
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vent vent = collision.GetComponent<Vent>();
                if (vent == null)
                    yield break;
                EnterVent(vent);
            }
            yield return null;  
        }
    }

    /// <summary>
    /// ��Ʈ ����
    /// </summary>
    private void EnterVent(Vent vent)
    {
        InVent =true;

        // ��Ʈ ���
        _vent = vent;
        // ��Ʈ ���� �̺�Ʈ ���
        _vent.OnChangeVentEvent += ChangeVent;

        // ī�޶� ���� 
        // TODO: �ó׸ӽſ� ���� �ڵ� ���� ���ɼ� ����
        Camera.main.transform.SetParent(vent.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // �÷��̾� ��ġ �Ⱥ��̴°��� ��ġ
        Vector2 tempPos = new Vector2(10000, 10000);
        transform.position = tempPos;

        // ��Ʈ ����
        vent.Enter(Vent.ActorType.Enter);

        // �Է� ��� �ڷ�ƾ ����
        if (_enterVentRoutine == null)
            _enterVentRoutine = StartCoroutine(EnterVentRoutine());
    }

    /// <summary>
    /// ��Ʈ �ӿ��� �ڷ�ƾ
    /// </summary>
    Coroutine _enterVentRoutine;
    IEnumerator EnterVentRoutine()
    {
        while (true)
        {
            yield return null;
            // ��Ʈ ������
            if (Input.GetKeyDown(KeyCode.LeftShift) || _isClickButton == true || VoteScene.Instance != null)
            {
                _isClickButton = false;

                ExitVent();
                _enterVentRoutine = null;
                yield break;
            }
        }
    }

    /// <summary>
    /// ��Ʈ ������
    /// </summary>
    private void ExitVent()
    {
        // ī�޶� �ٽ� �÷��̾� ����
        // TODO : �ó׸ӽſ� ���� �ڵ� �����ؾ���
        Camera.main.transform.SetParent(null);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // ��Ʈ �̺�Ʈ ���� ����
        _vent.OnChangeVentEvent -= ChangeVent;

        // �÷��̾� ��ġ ��Ʈ ��ġ�� �̵�
        transform.position = _vent.transform.position;
        // ��Ʈ ����
        _vent.Exit(Vent.ActorType.Enter);

        InVent = false;
    }

    /// <summary>
    /// ��Ʈ ��ü(�̵�)
    /// </summary>
    private void ChangeVent(Vent vent)
    {
        // ���� ��Ʈ ���� ����
        _vent.OnChangeVentEvent -= ChangeVent;

        // ���� ��Ʈ ����
        _vent.Exit(Vent.ActorType.Change);

        // ��Ʈ ��ü
        _vent = vent;
        
        // ���� ��Ʈ �̺�Ʈ ����
        vent.OnChangeVentEvent += ChangeVent;

        // ī�޶� �̵�
        Camera.main.transform.SetParent(_vent.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // ��Ʈ ����
        vent.Enter(Vent.ActorType.Change);
    }
}

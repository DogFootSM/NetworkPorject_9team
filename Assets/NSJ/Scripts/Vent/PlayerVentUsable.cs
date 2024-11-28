using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerVentUsable : MonoBehaviourPun
{
    private PlayerController _player;
    private Vent _vent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine == false) 
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
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
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // ��Ʈ �̺�Ʈ ���� ����
        _vent.OnChangeVentEvent -= ChangeVent;

        // �÷��̾� ��ġ ��Ʈ ��ġ�� �̵�
        transform.position = _vent.transform.position;
        // ��Ʈ ����
        _vent.Exit(Vent.ActorType.Enter);
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

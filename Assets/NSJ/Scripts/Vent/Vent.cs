using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Vent;

public class Vent : MonoBehaviourPun
{
    public enum ActorType { Enter, Change}

    public Vent[] MoveableVents;

    public event UnityAction<Vent> OnChangeVentEvent;

    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _dirArrowPrefab;

    private Dictionary<GameObject, Vent> _arrowDic = new Dictionary<GameObject, Vent>();
    private Animator animator;
    private int animatorHash = Animator.StringToHash("Vent");


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    /// <summary>
    /// ��Ʈ ����
    /// </summary>
    public void Enter(ActorType actorType)
    {
        // �����ִ� ��Ʈ �� ��ŭ ����
        foreach (Vent vent in MoveableVents)
        {
            GameObject arrow = Instantiate(_dirArrowPrefab, transform.position, transform.rotation);

            // ȭ��ǥ �ٸ� ��Ʈ �ٶ󺸱�
            Vector2 newPos = vent.transform.position - arrow.transform.position;
            float rotZ = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, rotZ);
            arrow.transform.SetParent(_canvas);
            arrow.transform.position += arrow.transform.right * 2 ;

            // ��ư �̺�Ʈ ���
            Button arrowButton = arrow.GetComponentInChildren<Button>();
            arrowButton.onClick.AddListener(() => ChangeVent(arrow));

            _arrowDic.Add(arrow, vent);
           
        }

        //RPC
        if (actorType == ActorType.Enter)
        {
            photonView.RPC(nameof(RPCVent), RpcTarget.AllViaServer);
        }
    }

    /// <summary>
    /// ��Ʈ ����
    /// </summary>
    public void Exit(ActorType actorType)
    {
        foreach (GameObject arrow in _arrowDic.Keys)
        {
            Destroy(arrow.gameObject);
        }
        _arrowDic.Clear();

        //RPC
        if (actorType == ActorType.Enter)
        {
            photonView.RPC(nameof(RPCVent), RpcTarget.AllViaServer);
        }
    }

    /// <summary>
    /// ��Ʈ ���� �̺�Ʈ ȣ��
    /// </summary>
    /// <param name="arrow"></param>
    private void ChangeVent(GameObject arrow)
    {
        Vent nextVent= _arrowDic[arrow];
        OnChangeVentEvent?.Invoke(nextVent);
    }

    [PunRPC]
    public void RPCVent()
    {
        animator.Play(animatorHash);
    }
}

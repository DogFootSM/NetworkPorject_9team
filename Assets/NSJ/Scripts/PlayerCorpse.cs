using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerCorpse : MonoBehaviourPun
{
    Coroutine _lifeRoutine;
    private void OnEnable()
    {
        if (photonView.IsMine == false)
            return;

        if (_lifeRoutine == null)
        {
            _lifeRoutine = StartCoroutine(LifeRoutine());
        }
    }
    private void OnDisable()
    {
        if (photonView.IsMine == false)
            return;

        if (_lifeRoutine != null)
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }
    }

    /// <summary>
    /// ��ǥ�� �Ѿ�� ������ ���� ��������
    /// </summary>
    /// <returns></returns>
    IEnumerator LifeRoutine()
    {
        while (true)
        {
            // ��ǥ�� ���� ��
            if (VoteScene.Instance != null)
            {
                DeleteCorpse();         
            }
            yield return 0.1f.GetDelay();
        }
    }

    /// <summary>
    /// ��ü ����
    /// </summary>
    private void DeleteCorpse()
    {
        if (_lifeRoutine != null)
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }

        //ReportingObject reportingObject = GetComponent<ReportingObject>();
        //reportingObject.Reporting();

        //gameObject.SetActive(false);
    }

}

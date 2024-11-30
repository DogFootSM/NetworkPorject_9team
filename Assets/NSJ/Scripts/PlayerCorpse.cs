using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCorpse : MonoBehaviour
{
    Coroutine _lifeRoutine;
    private void OnEnable()
    {
        if( _lifeRoutine == null )
        {
            _lifeRoutine = StartCoroutine(LifeRoutine());
        }
    }
    private void OnDisable()
    {
        if(_lifeRoutine != null)
        {
            StopCoroutine( _lifeRoutine );
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
                yield return 1f.GetDelay();
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

        ReportingObject reportingObject = GetComponent<ReportingObject>();
        reportingObject.Reporting();
    }

}

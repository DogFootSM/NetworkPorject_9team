using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalButton : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _powerClips = new List<AudioClip>();

    private int _powerCount;
    public int PowerCount
    {
        get
        {
            return _powerCount;
        }
        set
        {
            _powerCount = value;

            if(_powerCount < 1)
            {
                //���� ų �� �ְ� Bool ���� ����
            }


        }
    }

    private void OnEnable()
    {
        _powerCount = Random.Range(1, 15);
        Debug.Log($"{gameObject.name} : {_powerCount}");
    }


    //�� ��ư �� Ŭ���ؾ� �� Ƚ�� > Random.Range�� �ο�
    //�� ��ư �� ��ũ��Ʈ�� �Ѱ��ִ°� ������
}

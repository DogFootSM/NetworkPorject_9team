using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }

    private Dictionary<MonoBehaviour, Coroutine> _routineDict = new Dictionary<MonoBehaviour, Coroutine>();

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public Coroutine GetCoroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }


    /// <summary>
    /// ���� �ڷ�ƾ�� �޾ƿͼ� �����ϴ� ���
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public IEnumerator ManagerStartCoroutine(MonoBehaviour key, Coroutine value)
    {
        if (_routineDict.ContainsKey(key))
        {
            //Ű�� ���� �̹� �����Ѵٸ� ������� �ڷ�ƾ ����
            if (_routineDict.TryGetValue(key, out Coroutine routine))
            {
                StopCoroutine(routine);
                _routineDict.Remove(key);
            }
        }
        _routineDict[key] = value;

        IEnumerator enumerator = _routineDict.GetEnumerator();

        //���� ������ ���� ���� �ݺ�
        while (enumerator.MoveNext())
        { 
            //�ش� �ڷ�ƾ�� ��� �ð���ŭ ����
            yield return enumerator.Current; 
        }
    }


    /// <summary>
    /// �ڷ�ƾ ������ �����ϴ� ���
    /// </summary>
    /// <param name="key"></param>
    public void ManagerStopCoroutine(MonoBehaviour key)
    {
        if (!_routineDict.ContainsKey(key))
            return;

        StopCoroutine(_routineDict[key]);
        _routineDict.Remove(key);
    }

}

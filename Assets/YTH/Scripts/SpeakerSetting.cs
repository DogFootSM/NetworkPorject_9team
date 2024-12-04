using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerSetting : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom == false)
        {
            Destroy(gameObject);
        }

        StartCoroutine(SetSpeaker());
    }

    IEnumerator SetSpeaker()
    {
        yield return null;
        // VoteScene ��ȯ �� ����Ŀ SpecialBlend 0(2D)���� ���� 
        if (VoteScene.Instance != null)
        {
            _audioSource.spatialBlend = 0;
        }
        // GameScene ��ȯ �� ����Ŀ SpecialBlend 1(3D)���� ���� 
        else
        {
            _audioSource.spatialBlend = 1;
        }
    }
}

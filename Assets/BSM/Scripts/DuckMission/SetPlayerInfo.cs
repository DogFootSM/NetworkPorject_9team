using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SetPlayerInfo : MonoBehaviour
{
    [Header("Sabotage Ability")]
    [SerializeField] private UnityEngine.UI.Image _armImage;   //���� ���� �� �÷� �� ����
    [SerializeField] private GameObject _abilityPrefab;
    private PlayerType _playerType;
    private Coroutine _setCo;

    private Light2D _light2D;
    private void Start()
    {
        _setCo = StartCoroutine(SetPlayerCoroutine());
        _light2D = Camera.main.transform.GetChild(0).GetComponent<Light2D>();
    }

    private void Update()
    {
        if (_playerType.Equals(PlayerType.Duck))
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _abilityPrefab.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Ability �˾�â ���� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetPlayerCoroutine()
    {
        yield return Util.GetDelay(3f);
        SetPlayer();
    }

    /// <summary>
    /// ���� �÷��̾� Ability �˾�â ���� ����
    /// </summary>
    private void SetPlayer()
    {
        _playerType = PlayerDataContainer.Instance.GetPlayerJob(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        PlayerData data = PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        _armImage.color = data.PlayerColor;
    }

}

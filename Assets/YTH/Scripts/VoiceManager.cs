using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using System.Collections;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    public static VoiceManager Instance { get; private set; }

    PlayerDataContainer PlayerDataContainer => PlayerDataContainer.Instance;

    [SerializeField] Recorder _recorder;

    private bool _initTarget = false;

    [SerializeField] PlayerController _playerController;

    private void Start()
    {
        StartCoroutine(SetTargetPlayerRoutine());
    }

    public void DisableVoice()
    {
        //보이스 off 기능
        // if (PlayerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        // {
        //     _recorder.TransmitEnabled = false;
        // }
    }

    IEnumerator SetTargetPlayerRoutine()
    {

        while (true)
        {
            if (PlayerDataContainer == null)
            {
                yield return null;
            }
            else
            {
                while (true)
                {
                    yield return 1f.GetDelay();
                    if (PlayerDataContainer == null)
                        break;

                    if (_recorder.TargetPlayers == null)
                        continue;

                    // 액터넘버 == 타겟플레이어 배열에이 있는 인덱스
                    // 타겟플레이어 배열에 인덱스 번호로 보이스를 보냄

                    // 플레이어데이터를 코루틴으로 계속 받음
                    // 죽은 사람 있으면 인덱스 번호 삭제

                    // 본인 플레이어 생존 상황
                    if (_playerController.isGhost == false)
                    {
                        // 사망 플레이어 인덱스 삭제
                        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                        {
                            if (PlayerDataContainer.GetPlayerData(i).IsGhost == true)
                            {
                                _recorder.TargetPlayers[i] = 0;
                            }
                        }
                    }

                    // 본인 플레이어 사망 상황
                    if (_playerController.isGhost == true)
                    {
                        // 모든 인덱스 0으로 초기화(1번만 호출) + 사망 플레이어 인덱스 추가
                        if (_initTarget == false)
                        {
                            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                            {
                                _recorder.TargetPlayers[i] = 0;
                            }
                            _initTarget = true;
                        }

                        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                        {
                            if (PlayerDataContainer.GetPlayerData(i).IsGhost == true)
                            {
                                _recorder.TargetPlayers[i] = i + 1;
                            }
                        }
                    }
                }
            }
        }
    }
}

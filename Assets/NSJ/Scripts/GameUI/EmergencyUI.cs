using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class EmergencyUI : BaseUI
    {
        [SerializeField] private float _duration;

        private Image _playerArm => GetUI<Image>("PlayerArmImage");
        private void OnEnable()
        {
            StartCoroutine(DurationRoutine());
        }

        public void SetColor(Color playerColor)
        {
            _playerArm.color = playerColor;
        }

        /// <summary>
        /// ���ӽð����ȸ� ��Ÿ��
        /// </summary>
        IEnumerator DurationRoutine()
        {
            yield return _duration.GetDelay();
            gameObject.SetActive(false);
        }
    }

}

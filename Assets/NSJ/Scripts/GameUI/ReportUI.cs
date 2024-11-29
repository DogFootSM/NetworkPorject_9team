using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class ReportUI :BaseUI
    {
        [SerializeField] private float _duration;


        private Image _reporter => GetUI<Image>("PlayerHeadImage");
        private Image _corpse => GetUI<Image>("PlayerCorpseImage");
        private void OnEnable()
        {
            StartCoroutine(DurationRoutine());
        }

        /// <summary>
        /// �� ����
        /// </summary>
        /// <param name="reporterColor"></param>
        /// <param name="corpseColor"></param>
        public void SetColor(Color reporterColor, Color corpseColor)
        {
            _reporter.color = reporterColor;
            _corpse.color = corpseColor;
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


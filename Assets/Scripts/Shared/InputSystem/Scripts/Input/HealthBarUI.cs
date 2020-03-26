using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Progress.InputSystem
{
    public class HealthBarUI : BarUI
    {
        [SerializeField]
        private Image foregroundImage;
        [SerializeField]
        private float updateTime = 0.2f;

        [SerializeField]
        private HealthController _healthController;

        public override void OnSetupController(BarController controller)
        {
            _healthController = controller as HealthController;

            _healthController.RegisterOnChanged(HandleStatChanged);
        }

        private void HandleStatChanged(Unit unit, float percentage)
        {
            StartCoroutine(ChangeToPercentage(percentage));
        }

        private IEnumerator ChangeToPercentage(float percentage)
        {
            float preChangePercentage = foregroundImage.fillAmount;
            float elapsedTime = 0f;

            while (elapsedTime < updateTime)
            {
                elapsedTime += Time.deltaTime;
                foregroundImage.fillAmount = Mathf.Lerp(preChangePercentage, percentage, elapsedTime / updateTime);
                yield return null;
            }

            //  Set fill to exact value to avoid inaccuracies
            foregroundImage.fillAmount = percentage;
        }
    }
}

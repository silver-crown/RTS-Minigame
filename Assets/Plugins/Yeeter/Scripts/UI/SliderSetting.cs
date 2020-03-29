using UnityEngine;
using UnityEngine.UI;

namespace Yeeter
{
    public class SliderSetting : MonoBehaviour
    {
        [SerializeField] private Text _keyText = null;
        [SerializeField] private Text _valueText = null;
        [SerializeField] private Slider _slider = null;

        /// <summary>
        /// Sets the settings key that the slider will read from/write to.
        /// </summary>
        /// <param name="settingKey">The setting key.</param>
        public void SetKey(string settingKey)
        {
            var value = StreamingAssetsDatabase.GetSetting(settingKey);
            var parts = settingKey.Split(new char[] { '.' });
            _keyText.text = parts[parts.Length - 1];
            _valueText.text = value;
            _slider.value = float.Parse(value);
            _slider.onValueChanged.AddListener(v =>
            {
                _valueText.text = v.ToString();
                StreamingAssetsDatabase.AddSettingToBeChanged(settingKey, v.ToString());
            });
        }
    }
}
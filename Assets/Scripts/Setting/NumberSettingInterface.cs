using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberSetttingInterface : MonoBehaviour
{
    Slider _slider;
    TMP_InputField _inputField;

    private void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _inputField = GetComponentInChildren<TMP_InputField>();
    }
    public void OnValueChanged(bool sliderSource)
    {
        float newValue = 0f;
        if (sliderSource) { UpdateValue(_slider.value); }
        else { SetValueFromString(_inputField.text); }

    }

    public void SetValueFromString(string newValue)
    {
        if (float.TryParse(newValue, out float number))
        {
            UpdateValue(number);
        }
    }

    void UpdateValue(float newValue)
    {
        Settings.Instance.SetMass(newValue);
        _slider.value = newValue;
        _inputField.text = newValue.ToString();
    }
}
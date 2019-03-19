using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class SampleDebugUIView : BaseDebugUIView
    {

        [SerializeField]
        private DebugUIButton _button = null;

        [SerializeField]
        private DebugUIDropdown _dropdown = null;

        [SerializeField]
        private DebugUIInputField _inputField = null;

        [SerializeField]
        private DebugUISlider _slider = null;

        [SerializeField]
        private DebugUIToggle _toggle = null;

        protected override void Bind()
        {
            base.Bind();
            _button.onClick = () =>
            {
                Debug.Log("OnClick button");
            };
            _dropdown.onValueChanged = (i) =>
            {
                Debug.Log($"OnValueChanged dropdown : index = {i} val = {_dropdown.GetOptionData(i).text}");
            };
            _inputField.onEndEdit = (s) =>
            {
                Debug.Log($"OnEndEdit inputfield : text = {s}");
            };
            _slider.onValueChanged = (f) =>
            {
                Debug.Log($"OnValueChanged slider : val = {f}");
            };
            _toggle.onValueChanged = (b) =>
            {
                Debug.Log($"OnValueChanged toggle : isOn = {b}");
            };
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Kosu.UnityLibrary
{
    public class LogPanel : MonoBehaviour
    {

        [SerializeField]
        private Text _logStringPrefab = null;

        [SerializeField]
        private RectTransform _logStringParent = null;

        [SerializeField]
        private int _maxQueueSize = 100;

        [SerializeField]
        private ScrollRect _scrollRect = null;

        [SerializeField]
        private RectTransform _content = null;

        private int _textCount = 0;

        private bool _autoScroll = true;

        [SerializeField]
        private Toggle _autoScrollToggle = null;

        private void Awake()
        {
            _autoScrollToggle.isOn = true;
        }

        private void OnEnable()
        {
            Application.logMessageReceivedThreaded += LogCallbackHandler;
            _autoScrollToggle.onValueChanged.AddListener((isOn) =>
            {
                _autoScroll = isOn;
            });
        }

        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= LogCallbackHandler;
            _autoScrollToggle.onValueChanged.RemoveAllListeners();
        }

        private void Update()
        {
            if (_autoScroll && _textCount != _content.childCount)
            {
                _scrollRect.verticalNormalizedPosition = 0;
                _textCount = _content.childCount;
            }
        }

        private void LogCallbackHandler(string logString, string stackTrace, LogType type)
        {
            string log = "";

            switch (type)
            {
                case LogType.Log:
                    log = "<color=\"#00FF11\">";
                    break;

                case LogType.Warning:
                    log = "<color=\"#FFF000\">";
                    break;

                case LogType.Error:
                case LogType.Exception:
                    log = "<color=\"#FF0500\">";
                    break;
            }

            log += string.Format("[{0}][{1}] {2}</color>",
                                 type.ToString(),
                                 System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                 logString);
            var text = Instantiate(_logStringPrefab);
            text.text = log;
            text.transform.SetParent(_logStringParent, false);

            while (_logStringParent.childCount > _maxQueueSize)
            {
                var child = _logStringParent.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }

    }
}

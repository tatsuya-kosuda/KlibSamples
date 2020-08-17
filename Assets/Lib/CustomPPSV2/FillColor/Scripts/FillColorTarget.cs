using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace klib
{
    [ExecuteInEditMode]
    public class FillColorTarget : MonoBehaviour
    {

        [SerializeField]
        private int _colorIndex;

        public int ColorIndex { get { return _colorIndex; } set { _colorIndex = value; } }

        [SerializeField]
        private Renderer[] _renderer;

        public Renderer[] Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = GetComponentsInChildren<Renderer>();
                }

                return _renderer;
            }
        }

        [SerializeField]
        private MeshFilter[] _meshFilter;

        public MeshFilter[] MeshFilter
        {
            get
            {
                if (_meshFilter == null)
                {
                    _meshFilter = Renderer.Select(_ => _.gameObject.GetComponent<MeshFilter>()).ToArray();
                }

                return _meshFilter;
            }
        }

        [SerializeField]
        private bool _isEraseColor;

        public bool IsEraseColor { get { return _isEraseColor; } set { _isEraseColor = value; } }

        [SerializeField, Range(0.1f, 10)]
        private float _topColorPos = 1;

        public float TopColorPos { get { return _topColorPos; } set { _topColorPos = value; } }

        [SerializeField, Range(0, 10)]
        private float _bottomColorPos = 0;

        public float BottomColorPos { get { return _bottomColorPos; } set { _bottomColorPos = value; } }

        [SerializeField, Range(-1, 1)]
        private float _offset = 0;

        public float Offset { get { return _offset; } set { _offset = value; } }

        [SerializeField, Range(0, 1)]
        private float _fillAmount = 1;

        public float FillAmount { get { return _fillAmount; } set { _fillAmount = value; } }

        public float MaxFillAmount { get; set; }

        [SerializeField]
        private int _bottomColorIndex;

        public int BottomColorIndex { get { return _bottomColorIndex; } set { _bottomColorIndex = value; } }

    }
}

using System;

using System.Reflection;
using TMPro;


namespace UnityEngine.UI.TableUI
{
    [System.Serializable]
    public class TextProperties : MonoBehaviour
    {
        [System.Serializable]
        internal enum GroupSelectionMethod { Header, Body, All, MinMax }
        [SerializeField]
        internal GroupSelectionMethod groupSelectionMethod = GroupSelectionMethod.All;

        [SerializeField]
        internal Vector2Int min = Vector2Int.zero;
        [SerializeField]
        internal Vector2Int max = Vector2Int.zero;

        [SerializeField, HideInInspector]
        private TMP_FontAsset _fontAsset;
        public TMP_FontAsset FontAsset
        {
            get { return _fontAsset; }
            set
            {
                if (_fontAsset == null)
                {
                    _fontAsset = value;
                    return;
                }
                if (_fontAsset == value)
                    return;
                _fontAsset = value;
                ApplyProperty("FontAsset", "font");

            }
        }

        [SerializeField, HideInInspector]
        private int _fontStyle;
        public int FontStyle
        {
            get { return _fontStyle; }
            set
            {

                if (FontStyle == value)
                    return;
                _fontStyle = value;

                ApplyProperty("FontStyle", "fontStyle");
            }
        }

        [SerializeField, HideInInspector]
        private float _fontSize = 15f;

        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value == _fontSize)
                    return;
                _fontSize = value;
                ApplyProperty("FontSize", "fontSize");

            }
        }

        [SerializeField, HideInInspector]
        private bool _autoSize;
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (_autoSize == value) return; _autoSize = value;
                ApplyProperty("AutoSize", "enableAutoSizing",
                    (TextMeshProUGUI tmp) =>
                    {
                        if (!_autoSize)
                        {
                            _fontSizeMin = tmp.fontSizeMin;
                            _fontSizeMax = tmp.fontSizeMax;
                            _characterWidthAdjustment = tmp.characterWidthAdjustment;
                            _lineSpacingAdjustment = tmp.lineSpacingAdjustment;
                        }
                        _fontSize = tmp.fontSize;

                    });

            }
        }

        [SerializeField, HideInInspector]
        private float _fontSizeMin;
        public float FontSizeMin
        {
            get { return _fontSizeMin; }
            set { if (value == _fontSizeMin) return; _fontSizeMin = value; ApplyProperty("FontSizeMin", "fontSizeMin"); }
        }

        [SerializeField, HideInInspector]
        private float _fontSizeMax;
        public float FontSizeMax
        {
            get { return _fontSizeMax; }
            set { if (value == _fontSizeMax) return; _fontSizeMax = value; ApplyProperty("FontSizeMax", "fontSizeMax"); }
        }

        [SerializeField, HideInInspector]
        private float _characterWidthAdjustment;
        public float CharacterWidthAdjustment
        {
            get { return _characterWidthAdjustment; }
            set { if (value == _characterWidthAdjustment) return; _characterWidthAdjustment = value; ApplyProperty("CharacterWidthAdjustment", "characterWidthAdjustment"); }

        }

        [SerializeField, HideInInspector]
        private float _lineSpacingAdjustment;
        public float LineSpacingAdjustment
        {
            get { return _lineSpacingAdjustment; }
            set { if (value == _lineSpacingAdjustment) return; _lineSpacingAdjustment = value; ApplyProperty("LineSpacingAdjustment", "lineSpacingAdjustment"); }

        }

        [SerializeField, HideInInspector]
        private Color _vertexColor = Color.black;
        public Color VertexColor
        {
            get { return _vertexColor; }
            set { if (value == _vertexColor) return; _vertexColor = value; ApplyProperty("VertexColor", "color"); }
        }

        [SerializeField, HideInInspector]
        private TextAlignmentOptions _alignment = TextAlignmentOptions.Center;
        public TextAlignmentOptions Alignment
        {
            get { return _alignment; }
            set { if (_alignment == value) return; _alignment = value; ApplyProperty("Alignment", "alignment"); }
        }


        [SerializeField, HideInInspector]
        private float _wrapMixWC = 0.4f;
        public float WrapMixWC
        {
            get { return _wrapMixWC; }
            set { if (_wrapMixWC == value) return; _wrapMixWC = value; ApplyProperty("WrapMixWC", "wordWrappingRatios"); }
        }

        public delegate void TextPropertiesUndoEvent();
        public TextPropertiesUndoEvent textPropertiesUndoEvent;

        void ApplyProperty(string oriProp, string targetProp, Action<TextMeshProUGUI> extraAction = null)
        {
            TableUI tu = GetComponent<TableUI>();
            UpdateMinMaxValues(tu);
            Type t = GetType();
            PropertyInfo prop = t.GetProperty(oriProp);


            for (int i = min.x; i < max.x; i++)
            {
                for (int j = min.y; j < max.y; j++)
                {
                    try
                    {
                        TextMeshProUGUI tmp = tu.data[j].list[i];
                        typeof(TextMeshProUGUI).GetProperty(targetProp).SetValue(tmp, prop.GetValue(this));

                        Utils.SetDirty(tmp);
                        extraAction?.Invoke(tmp);
                    }
                    catch (System.Exception) { }
                }
            }

        }



        private void UpdateMinMaxValues(TableUI tu)
        {
            if (groupSelectionMethod.Equals(GroupSelectionMethod.MinMax))
                return;

            if (groupSelectionMethod.Equals(GroupSelectionMethod.Header))
            {
                min.x = 0;
                min.y = 0;
                max.x = tu.Header ? tu.Columns : 0;
                max.y = tu.Header ? 1 : 0;
            }
            else if (groupSelectionMethod.Equals(GroupSelectionMethod.Body))
            {
                min.x = 0;
                min.y = tu.Header ? 1 : 0;
                max.x = tu.Columns;
                max.y = tu.Rows;
            }
            else if (groupSelectionMethod.Equals(GroupSelectionMethod.All))
            {
                min.x = 0;
                min.y = 0;
                max.x = tu.Columns;
                max.y = tu.Rows;
            }
        }

        internal void CopyAllValues(TextMeshProUGUI tmp)
        {
            tmp.font = FontAsset;
            tmp.fontStyle = (FontStyles)FontStyle;
            tmp.fontSize = FontSize;
            tmp.enableAutoSizing = AutoSize;
            if (AutoSize)
            {
                tmp.fontSizeMin = FontSizeMin;
                tmp.fontSizeMax = FontSizeMax;
                tmp.characterWidthAdjustment = CharacterWidthAdjustment;
                tmp.lineSpacingAdjustment = LineSpacingAdjustment;
            }
            tmp.color = VertexColor;
            tmp.alignment = Alignment;
            tmp.wordWrappingRatios = WrapMixWC;

            Utils.SetDirty(tmp);
        }

    }
}

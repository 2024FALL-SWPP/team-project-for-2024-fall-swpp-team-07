
using System.Collections.Generic;

namespace UnityEngine.UI.TableUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    [System.Serializable,RequireComponent(typeof(RectTransform))]
    public class UILineRenderer : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
    {
        [SerializeField, Tooltip("Thickness of the line")]
        private float _lineThickness = 2;

        public float LineThickness
        {
            get { return _lineThickness; }
            set
            {
                if (_lineThickness == value)
                    return;
                _lineThickness = value;
                SetAllDirty();
                Utils.SetDirty(this);
            }
        }


        [SerializeField, Tooltip("Points to draw lines between")]
        internal Vector2[] m_points;
        public Vector2[] Points { get { return m_points; } set { if (m_points == value) return; m_points = value; SetAllDirty(); } }

        public virtual float minWidth { get { return 0; } }

        public virtual float preferredWidth { get { return 0; } }

        public virtual float flexibleWidth { get { return -1; } }

        public virtual float minHeight { get { return 0; } }

        public virtual float preferredHeight { get { return 0; } }
        public virtual float flexibleHeight { get { return -1; } }

        public virtual int layoutPriority { get { return 0; } }

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera){return true;}

        public virtual void CalculateLayoutInputHorizontal() { }
        public virtual void CalculateLayoutInputVertical() { }

        private void PopulateMesh(VertexHelper vh, Vector2[] pointsToDraw)
        {
            var segments = new List<UIVertex[]>();

            for (var i = 1; i < pointsToDraw.Length; i += 2)
            {
                Vector2 start = pointsToDraw[i - 1];
                Vector2 end = pointsToDraw[i];

                start = new Vector2(start.x, start.y);
                end = new Vector2(end.x, end.y);

                segments.Add(CreateLineSegment(start, end, segments.Count > 1 ? segments[segments.Count - 2] : null));
            }


            for (var i = 0; i < segments.Count; i++)
            {
                vh.AddUIVertexQuad(segments[i]);
            }

            if (vh.currentVertCount > 64000)
            {
                Debug.LogError("Max Verticies size is 64000, current mesh vertcies count is [" + vh.currentVertCount + "] - Cannot Draw");
                vh.Clear();
                return;
            }
        }


       

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (m_points != null && m_points.Length > 0)
            {
                vh.Clear();
                PopulateMesh(vh, m_points);
            }
        }

        private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, UIVertex[] previousVert = null)
        {
            Vector2 offset = new Vector2((start.y - end.y), end.x - start.x).normalized * LineThickness / 2;

            Vector2 v1 = start - offset;
            Vector2 v2 = start + offset;
            Vector2 v3 = end + offset;
            Vector2 v4 = end - offset;

            return SetVbo(new[] { v1, v2, v3, v4 });

        }
        protected UIVertex[] SetVbo(Vector2[] vertices)
        {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                Vector3 v = vertices[i];
                vert.position = v;
                vbo[i] = vert;
            }
            return vbo;
        }
       

    }
}

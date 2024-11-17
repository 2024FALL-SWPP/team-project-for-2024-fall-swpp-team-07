using System.Collections.Generic;
using TMPro;

namespace UnityEngine.UI.TableUI
{
    [System.Serializable]
    public class SelectionGrid
    {
        [SerializeField, HideInInspector]
        public GUISkin skin;

        [SerializeField, HideInInspector]
        internal List<BoolList> _values;
        public List<BoolList> Values
        {
            get { return _values; }
            private set { }
        }

        [SerializeField, HideInInspector]
        public Vector2Int chosenCell = new Vector2Int(-1, -1);
    }

    [System.Serializable,SelectionBase, RequireComponent(typeof(RectTransform))]
    public class TableUI : MonoBehaviour
    {

        [SerializeField, HideInInspector]
        public SelectionGrid selectionGrid;

        //Maximum values can be changed, but the minimun must be >= 1 
        public static int MAX_ROWS = 20, MIN_ROWS = 1, MAX_COL = 20, MIN_COL = 1;



        [SerializeField,HideInInspector]
        private List<float> _columnsWidth;
        public List<float> ColumnsWidth
        {
            get { return _columnsWidth; }
            set { if (_columnsWidth == value) return; _columnsWidth = value; }
        }

        [SerializeField, HideInInspector]
        private List<float> _rowsHeight;
        public List<float> RowsHeight
        {
            get { return _rowsHeight; }
            private set { }
        }

        [SerializeField, HideInInspector]
        private bool _textAutoScale;

        internal bool TextAutoScale
        {
            get { return _textAutoScale; }
            set
            {
                if (_textAutoScale == value)
                    return;
                _textAutoScale = value;
                UpdateRowHeight();
                
            }
        }

        [SerializeField,HideInInspector]
        internal List<TextMeshList> data;
        
        public TextMeshProUGUI GetCell(int row, int column)
        {
            return data[row].list[column];
        }

        [SerializeField, HideInInspector]
        private int _columns;
        public int Columns
        {
            get => _columns;
            set
            {
                if (_columns == value || value < MIN_COL || value > MAX_COL)
                    return;
                GenerateColumns(value);
                UpdateColumnsWidth();
                UpdateRowsWidth();
                UpdateTableSize();
                GenerateBorders();
            }
        }

        [SerializeField, HideInInspector]
        private int _rows;
        public int Rows
        {
            get => _rows;
            set
            {
                if (_rows == value || value < MIN_ROWS || value > MAX_ROWS)
                    return;
                GenerateRows(value);
                UpdateRowsWidth();
                UpdateTableSize();
                GenerateBorders();
            }
        }

        [SerializeField, HideInInspector]
        public TextProperties headerCellProperties;
        [SerializeField, HideInInspector]
        public TextProperties bodyCellProperties;


        [SerializeField, HideInInspector]
        private bool _striped;
        public bool Striped
        {
            get => _striped;
            set { if (_striped == value) return; _striped = value; UpdateColor(); }
        }

        [SerializeField, HideInInspector]
        private Color _mainColor;
        public Color MainColor { get => _mainColor; set { if (_mainColor == value) return; _mainColor = value; UpdateColor(); } }

        [SerializeField, HideInInspector]
        private Color _secondaryColor;
        public Color SecondaryColor { get => _secondaryColor; set { if (_secondaryColor == value) return; _secondaryColor = value; UpdateColor(); } }

        [SerializeField, HideInInspector]
        private Color _borderColor = Color.black;
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value)
                    return;
                _borderColor = value;
                Transform borders = transform.Find("Borders");
                borders.GetComponent<UILineRenderer>().color = _borderColor;
                borders.GetComponent<UILineRenderer>().SetAllDirty();

                Utils.SetDirty(borders);

            }
        }

        [SerializeField, HideInInspector]
        private Color _headerColor;
        public Color HeaderColor { get => _headerColor; set { if (_headerColor == value) return; _headerColor = value; UpdateHeaderColor(); } }



        [SerializeField, HideInInspector]
        private BorderType _borderType;
        public BorderType BorderType
        {
            get => _borderType;
            set
            {
                if (_borderType.Equals(value))
                    return;
                _borderType = value;
                GenerateBorders();
            }
        }

        [SerializeField, HideInInspector]
        private float _borderThickness;
        public float BorderThickness
        {
            get => _borderThickness;
            set
            {
                if (_borderThickness == value)
                    return;
                _borderThickness = value;

                Transform borders = transform.Find("Borders");
                borders.GetComponent<UILineRenderer>().LineThickness = _borderThickness;
                
            }
        }

        [SerializeField, HideInInspector]
        private bool _header;
        public bool Header { get => _header; 
            set {
                if (value == _header || _rows <= 0) 
                    return; 
                _header = value; 
                if (Header)
                {
                    rows[0].transform.Find("panel").GetComponent<Image>().color = HeaderColor;
                    headerCellProperties.FontStyle = (int)FontStyles.Bold;
                }
                else
                {
                    rows[0].transform.Find("panel").GetComponent<Image>().color = GetRowColor(0);
                    headerCellProperties.FontStyle = (int)FontStyles.Normal;
                }
                Utils.SetDirty(rows[0]);
            } 
        }

        [SerializeField, HideInInspector]
        private List<GameObject> rows;

        [SerializeField, HideInInspector]
        public string undoRedoEvent;

        internal void Init()
        {
            this.selectionGrid = new SelectionGrid();
            this.selectionGrid.skin = Resources.Load<GUISkin>("GUISkin");
            this.rows = new List<GameObject>();

            bodyCellProperties = GetComponents<TextProperties>()[0];
            headerCellProperties = GetComponents<TextProperties>()[1];
            headerCellProperties.textPropertiesUndoEvent += RefreshHeaderTextProperties;
            bodyCellProperties.textPropertiesUndoEvent += RefreshBodyTextProperties;

            Vector2 size = GetRectSize(gameObject.GetComponent<RectTransform>());
            size.x = 560f;
            size.y = 200f;
            GetComponent<RectTransform>().sizeDelta = size;
            this.data = new List<TextMeshList>();
            ColorUtility.TryParseHtmlString("#FF9600", out this._headerColor);
            ColorUtility.TryParseHtmlString("#ECECEC", out this._secondaryColor);
            _mainColor = Color.white;
            BorderColor = Color.black;
            this._columnsWidth = new List<float>();
            this._rowsHeight = new List<float>();
            this.selectionGrid._values = new List<BoolList>();
            BoolList l = new BoolList();
            l.list=new List<bool>();
            l.list.Add(false);
            this.selectionGrid.Values.Add(l);
            this.Rows = 6;
            this.Columns = 4;

            headerCellProperties.groupSelectionMethod = TextProperties.GroupSelectionMethod.Header;
            bodyCellProperties.groupSelectionMethod = TextProperties.GroupSelectionMethod.Body;
            
            BorderType = BorderType.Vertical_And_Header;
            Header = true;
            Striped = true;
            BorderThickness = 1f;            
        }

        public Vector2 GetRectSize(RectTransform rect)
        {
            Vector3[] corners = new Vector3[4];
            rect.GetLocalCorners(corners);
            return new Vector2(Vector3.Distance(corners[0], corners[3]), Vector3.Distance(corners[0], corners[1]));
        }

        private void UpdateColor()
        {
            int k = this.Header ? 1 : 0;
            for (int i = k; i < Rows; i++)
            {
                GameObject row = rows[i];
                row.transform.Find("panel").GetComponent<Image>().color = GetRowColor(i);
                Utils.SetDirty(row);
            }
        }

        private void UpdateHeaderColor()
        {
            if (!Header)
                return;

            GameObject row = rows[0];
            row.transform.Find("panel").GetComponent<Image>().color = HeaderColor;
            Utils.SetDirty(row);
        }

        private void GenerateRows(int rowNumber)
        {
            if (Rows > rowNumber)
            {
                Utils.RegisterFullObjectHierarchyUndo(this, "Table");
                for (int k = Rows-1; k >= rowNumber; k--)
                {
                    
                    GameObject row = rows[k];
                    rows.RemoveAt(k);
                    _rows--;
                    data.RemoveAt(data.Count - 1);
                    this.selectionGrid._values.RemoveAt(this.selectionGrid._values.Count-1);
                    _rowsHeight.RemoveAt(_rowsHeight.Count-1);
                    Utils.DestroyObjectImmediate(row);
                }
            }
            else
            {
                for (int k = Rows; k < rowNumber; k++)
                {
                    BoolList l = new BoolList();
                    l.list = new List<bool>();
                    l.list.Add(false);
                    float height = Rows >= 1 ? sumAllValuesInList(RowsHeight) / Rows : 30f;
                    this.selectionGrid._values.Add(l);
                    _rowsHeight.Add(height);
                    GenerateRow("row" + k, k);
                    _rows++;
                    Utils.RegisterCreatedObjectUndo(rows[rows.Count - 1],"Table");
                }
            }
        }

        private void GenerateRow(string name, int n)
        {
            GameObject row = new GameObject(name);
            row.transform.SetParent(this.transform.Find("Content"));
            Vector2 size = new Vector2(GetRectSize(GetComponent<RectTransform>()).x, _rowsHeight[n]);
            row.AddComponent<RectTransform>().sizeDelta = size;
            GameObject panel = new GameObject("panel");
            panel.transform.SetParent(row.transform);
            Image panelImage = panel.AddComponent<Image>();

            panelImage.color = GetRowColor(n);

            panel.GetComponent<RectTransform>().sizeDelta = size;

            GameObject cells = new GameObject("cells");
            cells.transform.SetParent(row.transform);
            cells.AddComponent<RectTransform>();
            HorizontalLayoutGroup hlg = cells.AddComponent<HorizontalLayoutGroup>();
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = true;
            hlg.childControlHeight = false;
            hlg.childControlWidth = false;
            cells.GetComponent<RectTransform>().sizeDelta = size;
            TextMeshList list = new TextMeshList();
            list.list = new List<TextMeshProUGUI>();
            for (int i = 0; i < Columns; i++)
            {
                list.list.Add(GenerateColumnInRow("column" + i, row,n,i));
                this.selectionGrid.Values[n+1].list.Add(false);
            }
            data.Add(list);
            rows.Add(row);
            UpdateColumnsWidth();

        }

        private Color GetRowColor(int n)
        {
            if (Striped)
            {
                if (n % 2 == 0)
                    return MainColor;
                else
                    return SecondaryColor;
            }
            else
            {
                return MainColor;
            }
        }

        private void GenerateColumns(int columnNumber)
        {
            if (Columns > columnNumber)
            {
                Utils.RegisterFullObjectHierarchyUndo(this, "Table");

                //Needed because the columnsWidth is common for all rows and it only needs to be deleted once, not once per row iteration.
                bool firstIteration = true;
                for (int i = 0; i < rows.Count; i++)
                {
                    GameObject cells = rows[i].transform.Find("cells").gameObject;
                    
                    while (cells.transform.childCount > columnNumber)
                    {
                        GameObject column = cells.transform.GetChild(cells.transform.childCount - 1).gameObject;
                        data[i].list.RemoveAt(data[i].list.Count - 1);
                        if (firstIteration)
                        {
                            _columnsWidth.RemoveAt(_columnsWidth.Count - 1);
                            _columns--;
                        }
                        this.selectionGrid.Values[i+1].list.RemoveAt(this.selectionGrid.Values[i+1].list.Count-1);
                        Utils.DestroyObjectImmediate(column);
                    }
                    firstIteration = false;
                }
            }
            else
            {
                for (int k = Columns; k < columnNumber; k++)
                {
                    float width = Columns >= 1 ? sumAllValuesInList(ColumnsWidth) / Columns : 200f;
                    _columnsWidth.Add(width);
                    GenerateColumnInAllRows("column" + k, k);                   
                    _columns++;
                }
            }
        }

        private void GenerateColumnInAllRows(string name, int columnN)
        {
            this.selectionGrid.Values[0].list.Add(false);
            for (int i = 0; i < rows.Count; i++)
            {
                data[i].list.Add(GenerateColumnInRow(name, rows[i], i, columnN));
                this.selectionGrid.Values[i+1].list.Add(false);
            }

        }

        private TextMeshProUGUI GenerateColumnInRow(string name,GameObject row, int rowN,int columnN)
        {
            GameObject cells = row.transform.Find("cells").gameObject;

            GameObject text = new GameObject("text");
            text.transform.SetParent(cells.transform);
            TextMeshProUGUI tmp = text.AddComponent<TextMeshProUGUI>();
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(ColumnsWidth[columnN], RowsHeight[rowN]);

            if (rowN == 0)
            {
                headerCellProperties.CopyAllValues(tmp);
            }
            else
            {
                bodyCellProperties.CopyAllValues(tmp);
            }
            tmp.text = name;
            Utils.RegisterCreatedObjectUndo(text,"Table");
            return tmp;
        }

        private void UpdateColumnsWidth()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                GameObject cells = rows[i].transform.Find("cells").gameObject;

                for (int j = 0; j < cells.transform.childCount; j++)
                {
                    RectTransform rectTransform = cells.transform.GetChild(j).GetComponent<RectTransform>();
                    Vector2 size = rectTransform.sizeDelta;
                    size.x = ColumnsWidth[j];
                    rectTransform.sizeDelta = size;

                }
            }
        }

        public void UpdateColumnWidth(float value, int col)
        {
            if (ColumnsWidth[col] == value)
                return;

            ColumnsWidth[col] = value;
            UpdateColumnsWidth();
            UpdateRowsWidth();
            UpdateTableSize();
            GenerateBorders();
        }

        public void UpdateRowHeight(float value, int row)
        {
            if (RowsHeight[row] == value)
                return;
            RowsHeight[row] = value;
            UpdateRowHeight();
            UpdateTableSize();
            GenerateBorders();
        }

        public void ResizeTable(Vector2 newSize)
        {

            float sumWidth = sumAllValuesInList(ColumnsWidth);
            bool doUpdate = false;
            if (!Mathf.Approximately(sumWidth, newSize.x))
            {
                for (int i = 0; i < ColumnsWidth.Count; i++)
                {
                    float percentage = (ColumnsWidth[i]) / sumWidth;
                    float value = percentage * newSize.x;
                    //UpdateColumnWidth(value, i);
                    _columnsWidth[i] = value;
                }
                doUpdate = true;
            }

            float sumHeight = sumAllValuesInList(RowsHeight);
            if (!Mathf.Approximately(newSize.y , sumHeight))
            {               
                for (int i = 0; i < RowsHeight.Count; i++)
                {
                    float percentage = (RowsHeight[i]) / sumHeight;
                    float value = percentage * newSize.y;
                    _rowsHeight[i] = value;
                    //UpdateRowHeight(value, i);
                }
                doUpdate = true;
            }

            if (doUpdate)
            {
                UpdateColumnsWidth();
                UpdateRowsWidth();
                UpdateRowHeight();
                UpdateTableSize(newSize);
                GenerateBorders();
            }
        }

        private void UpdateRowsWidth()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                GameObject row = this.rows[i];
                Vector2 size = row.GetComponent<RectTransform>().sizeDelta;
                size.x = sumAllValuesInList(ColumnsWidth);
                row.GetComponent<RectTransform>().sizeDelta = size;
                row.transform.Find("panel").GetComponent<RectTransform>().sizeDelta = size;
                row.transform.Find("cells").GetComponent<RectTransform>().sizeDelta = size;
            }
        }


        private void UpdateRowHeight()
        {
            Transform content = transform.Find("Content");
            for (int i = 0; i < content.childCount; i++)
            {
                GameObject row = content.GetChild(i).gameObject;
                GameObject panel = row.transform.Find("panel").gameObject;
                GameObject cells = row.transform.Find("cells").gameObject;

                Vector2 size = row.GetComponent<RectTransform>().sizeDelta;
                size.y = RowsHeight[i];
                row.GetComponent<RectTransform>().sizeDelta = size;
                panel.GetComponent<RectTransform>().sizeDelta = size;
                cells.GetComponent<RectTransform>().sizeDelta = size;

                for (int j = 0; j < cells.transform.childCount; j++)
                {
                    if (TextAutoScale)
                        cells.transform.GetChild(j).GetComponent<TextMeshProUGUI>().fontSize = RowsHeight[i] * 0.4f;
                    Vector2 s = cells.transform.GetChild(j).GetComponent<RectTransform>().sizeDelta;
                    s.y = RowsHeight[i];
                    cells.transform.GetChild(j).GetComponent<RectTransform>().sizeDelta = s;
                }
            }
        }

        private void UpdateTableSize()
        {
            float width = sumAllValuesInList(ColumnsWidth);
            float height = sumAllValuesInList(RowsHeight);

            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

            Vector2 size = new Vector2(width, height);

            this.transform.Find("Content").GetComponent<RectTransform>().sizeDelta = size;
            this.transform.Find("Borders").GetComponent<RectTransform>().sizeDelta = size;
        }

        //In this case the table rect is not updated because it is changed directly from the editor.
        private void UpdateTableSize(Vector2 size)
        {
            this.transform.Find("Content").GetComponent<RectTransform>().sizeDelta = size;
            this.transform.Find("Borders").GetComponent<RectTransform>().sizeDelta = size;

        }

        private void GenerateBorders()
        {
            GameObject borders = transform.Find("Borders").gameObject;
            borders.GetComponent<UILineRenderer>().Points = null;
            if (rows.Count <= 0)
            {
                borders.SetActive(false);
                return;
            }
            if (!borders.activeInHierarchy)
            {
                borders.SetActive(true);
            }

            switch (BorderType)
            {
                case BorderType.All:
                    GenerateHorizontalBorders(false,true);
                    GenerateVerticalBorders(true);
                    break;
                case BorderType.Horizontal:
                    GenerateHorizontalBorders(false,false);
                    break;
                case BorderType.Vertical:
                    GenerateVerticalBorders(false);
                    break;
                case BorderType.Outline:
                    GenerateOuterBorder();
                    break;
                case BorderType.Vertical_And_Header:
                    if (Header)
                        GenerateHeaderBorder(false);
                    GenerateVerticalBorders(true);
                    break;
                default:
                    borders.SetActive(false);
                    break;
            }
            
        }

        private void GenerateVerticalBorders(bool concatenate)
        {
            if (rows.Count <= 0)
                return;
            Transform borders = transform.Find("Borders");
            Transform content = transform.Find("Content");

            UILineRenderer lineRenderer = borders.GetComponent<UILineRenderer>();
            Vector2[] points = new Vector2[(Columns + 1) * 2];
            Vector3[] cornersV3 = new Vector3[4];
            content.GetComponent<RectTransform>().GetWorldCorners(cornersV3);
            Vector2[] corners = applyFuncionOnVector3Array(cornersV3, (Vector3 v) =>
            {
                return content.transform.InverseTransformPoint(v);
            });
            points[0] = corners[1];
            points[1] = corners[1];
            points[1].y -= sumAllValuesInList(RowsHeight);

            float columnWidth = content.GetComponent<RectTransform>().sizeDelta.x / Columns;
            float deltaX = 0f;
            for (int i = 0, k = 2; i < Columns; i++, k += 2)
            {
                Vector2 p1 = points[0];
                Vector2 p2 = points[1];
                p1.x += deltaX+ ColumnsWidth[i];
                p2.x += deltaX + ColumnsWidth[i];
                deltaX = deltaX + ColumnsWidth[i];
                points[k] = p1;
                points[k + 1] = p2;
            }
            if (concatenate && lineRenderer.Points != null)
            {
                Vector2[] globalPoints = new Vector2[lineRenderer.Points.Length + points.Length];
                lineRenderer.Points.CopyTo(globalPoints, 0);
                points.CopyTo(globalPoints, lineRenderer.Points.Length);
                lineRenderer.Points = globalPoints;
            }
            else
            {
                lineRenderer.Points = points;
            }
            lineRenderer.SetAllDirty();

            Utils.SetDirty(borders);
        }

        private void GenerateHorizontalBorders(bool concatenate,bool extended)
        {
            if (rows.Count <= 0)
                return;
            Transform borders = transform.Find("Borders");
            Transform content = transform.Find("Content");

            UILineRenderer lineRenderer = borders.GetComponent<UILineRenderer>();
            Vector2[] points = new Vector2[(rows.Count + 1) * 2];
            Vector3[] cornersV3 = new Vector3[4];

            content.GetComponent<RectTransform>().GetWorldCorners(cornersV3);
            Vector2[] corners = applyFuncionOnVector3Array(cornersV3, (Vector3 v) =>
            {
                return content.transform.InverseTransformPoint(v);
            });

            float sum = sumAllValuesInList(ColumnsWidth);

            points[0] = corners[1];
            points[1] = points[1];           
            Vector2 p = points[0];
            p.x += sum;
            points[1] = p;

            if (extended)
            {
                points[0].x -= BorderThickness / 2;
                points[1].x += BorderThickness / 2;
            }
            for (int i = 0, k = 2; i < rows.Count; i++, k += 2)
            {
                Vector2 p1 = points[k-2];
                Vector2 p2 = points[k-1];
                p1.y -= RowsHeight[i];
                p2.y -= RowsHeight[i];
                points[k] = p1;
                points[k + 1] = p2;
            }
            if (concatenate && lineRenderer.Points != null)
            {
                Vector2[] globalPoints = new Vector2[lineRenderer.Points.Length + points.Length];
                lineRenderer.Points.CopyTo(globalPoints, 0);
                points.CopyTo(globalPoints, lineRenderer.Points.Length);
                lineRenderer.Points = globalPoints;
            }
            else
            {
                lineRenderer.Points = points;
            }
            lineRenderer.SetAllDirty();
            Utils.SetDirty(borders);
        }

        private float sumAllValuesInList(List<float> list)
        {
            float sum = 0f;
            foreach(float val in list)
            {
                sum += val;
            }
            return sum;
        }

        private void GenerateHeaderBorder(bool concatenate)
        {
            if (rows.Count <= 0)
                return;

            Transform borders = transform.Find("Borders");
            Transform content = transform.Find("Content");

            UILineRenderer lineRenderer = borders.GetComponent<UILineRenderer>();
            Vector2[] points = new Vector2[2];
            Vector3[] cornersV3 = new Vector3[4];

            content.GetComponent<RectTransform>().GetWorldCorners(cornersV3);
            Vector2[] corners = applyFuncionOnVector3Array(cornersV3, (Vector3 v) =>
            {
                return content.transform.InverseTransformPoint(v);
            });


            points[0] = corners[1];
            points[1] = corners[2];

            points[0].y -= RowsHeight[0];
            points[1].y -= RowsHeight[0];

            if (concatenate && lineRenderer.Points != null)
            {
                Vector2[] globalPoints = new Vector2[lineRenderer.Points.Length + points.Length];
                lineRenderer.Points.CopyTo(globalPoints, 0);
                points.CopyTo(globalPoints, lineRenderer.Points.Length);
                lineRenderer.Points = globalPoints;
            }
            else
            {
                lineRenderer.Points = points;
            }
            lineRenderer.SetAllDirty();

            Utils.SetDirty(borders);
        }

        private void GenerateOuterBorder()
        {
            Transform borders = transform.Find("Borders");

            UILineRenderer lineRenderer = borders.GetComponent<UILineRenderer>();
            Vector3[] cornersV3 = new Vector3[4];
            borders.GetComponent<RectTransform>().GetWorldCorners(cornersV3);
            Vector2[] corners = applyFuncionOnVector3Array(cornersV3, (Vector3 v) =>
            {
                return borders.transform.InverseTransformPoint(v);
            });

            Vector2[] points = new Vector2[8];

            for (int i = 1, k = 1; i < points.Length; i += 2, k++)
            {
                int a = mod(k, 4);
                points[i] = corners[a];
                points[i - 1] = corners[mod(a - 1, 4)];
            }
            float offset = BorderThickness / 2;
            points[0].y -= offset;
            points[1].y += offset;
            points[4].y += offset;
            points[5].y -= offset;
            lineRenderer.Points = points;
            lineRenderer.SetAllDirty();

            Utils.SetDirty(borders);
        }

        private delegate Vector2 v3Fun(Vector3 v);
        private Vector2[] applyFuncionOnVector3Array(Vector3[] v3, v3Fun fun)
        {
            Vector2[] v2 = new Vector2[v3.Length];
            for (int i = 0; i < v3.Length; i++)
            {
                v2[i] = fun(v3[i]);
            }
            return v2;
        }

        static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }


        internal void Refresh()
        {
            UpdateColumnsWidth();
            UpdateRowsWidth();
            UpdateRowHeight();
            UpdateTableSize();
            GenerateBorders();
        }

        internal void RefreshBorder() {
            Transform borders = transform.Find("Borders");
            borders.GetComponent<UILineRenderer>().LineThickness = _borderThickness;
            borders.GetComponent<UILineRenderer>().color = _borderColor;
            GenerateBorders();
        }

        internal void RefreshColor()
        {
            UpdateHeaderColor();
            UpdateColor();
        }

        public void RefreshHeaderTextProperties()
        {
            Utils.RecordObject(headerCellProperties, "Header");
            Utils.RecordObjects(data[0].list.ToArray(), "Header");
        }

        public void RefreshBodyTextProperties()
        {
            Utils.RecordObject(bodyCellProperties, "Body");
            for(int i = 1; i < Rows; i++)
            {
                Utils.RecordObjects(data[i].list.ToArray(), "Body");
            }

        }

        public void OnUndoRedoEvent()
        {

            if (undoRedoEvent.Equals("Border"))
            {
                RefreshBorder();
            }
            else if (undoRedoEvent.Equals("Color"))
            {
                RefreshColor();
            }
            else
            {
                Refresh();
            }

            
        }

        public void MakeAllRowsTheSameHeight()
        {
            Utils.RegisterFullObjectHierarchyUndo(this, "Table");
            float height = sumAllValuesInList(RowsHeight) / Rows;
            for(int i = 0; i < Rows; i++)
            {
                _rowsHeight[i] = height;
            }
            Refresh();
        }

        public void MakeAllColumnsTheSameWidth()
        {
            Utils.RegisterFullObjectHierarchyUndo(this, "Table");
            float width = sumAllValuesInList(ColumnsWidth) / Columns;
            for (int i = 0; i < Columns; i++)
            {
                _columnsWidth[i] = width;
            }
            Refresh();
        }

        [System.Serializable]
        public class TextMeshList : ListWrapper<TextMeshProUGUI> { }




    }

    [System.Serializable]
    public class ListWrapper<T>
    {
        [SerializeField]
        public List<T> list;
    }

    [System.Serializable]
    public class BoolList : ListWrapper<bool> { }
    public enum BorderType { None, Outline, Horizontal, Vertical, Vertical_And_Header, All }
}

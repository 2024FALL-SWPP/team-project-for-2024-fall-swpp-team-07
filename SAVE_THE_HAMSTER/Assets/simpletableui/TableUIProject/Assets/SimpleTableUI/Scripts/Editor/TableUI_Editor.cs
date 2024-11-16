using UnityEditor;
using TMPro;

namespace UnityEngine.UI.TableUI
{
    [CustomEditor(typeof(TableUI))]
    public class TableUI_Editor : Editor
    {
        TableUI tu;
        public TextMeshPro transf;

        bool headerFoldout,bodyFoldout;

        Editor bodyCellPropertiesEditor,headerCellPropertiesEditor;

        Vector2 previousSize;
        Vector2 scrollPos;

        GUIStyle box;
        GUIStyle labelStyle,labelStyle2;

        static readonly GUIContent rowLabel = new GUIContent("Row Number", "The number of rows of the table.");
        static readonly GUIContent columnLabel = new GUIContent("Column Number", "The number of columns of the table.");
        static readonly GUIContent showHeaderLabel = new GUIContent("Show Header", "If checked, the first row will be a header.");
        static readonly GUIContent borderTypeLabel = new GUIContent("Border Type", "Style of the borders of the table.");
        static readonly GUIContent borderThicknessLabel = new GUIContent("Border Thickness", "How thick is the border of the table.");
        static readonly GUIContent borderColorLabel = new GUIContent("Border Color", "Color of the border of the table.");
        static readonly GUIContent headerFoldoutLabel = new GUIContent("Header Style");
        static readonly GUIContent bodyFoldoutLabel = new GUIContent("Body Style");
        static readonly GUIContent stripedLabel = new GUIContent("Striped", "Set two colors for the background rows in an striped pattern (Header color not included).");
        static readonly GUIContent headerColorLabel = new GUIContent("Header Color", "The background color of the header row.");
        static readonly GUIContent mainColorLabel = new GUIContent("Main Color", "The background color of non header rows.");
        static readonly GUIContent secondaryColorLabel= new GUIContent("Secondary Color", "The secondary background color of non header rows if Stripped is checked.");
        private void OnEnable()
        {
            if(tu==null)
             tu = target as TableUI;

            Undo.undoRedoPerformed += tu.OnUndoRedoEvent;

            if (tu.bodyCellProperties.textPropertiesUndoEvent == null)
                tu.bodyCellProperties.textPropertiesUndoEvent += tu.RefreshBodyTextProperties;

            if (tu.headerCellProperties.textPropertiesUndoEvent == null)
                tu.headerCellProperties.textPropertiesUndoEvent += tu.RefreshHeaderTextProperties;
            
            if(bodyCellPropertiesEditor==null)
                bodyCellPropertiesEditor = CreateEditor(tu.bodyCellProperties);

            if(headerCellPropertiesEditor==null)
                headerCellPropertiesEditor = CreateEditor(tu.headerCellProperties);

            if(labelStyle==null)
                labelStyle= new GUIStyle() { fontStyle = FontStyle.Bold, fontSize = 15 };

            if(labelStyle2==null)
                labelStyle2= new GUIStyle() { fontStyle = FontStyle.Bold};

            if (box == null)
                box = new GUIStyle("box");
        }      

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= tu.OnUndoRedoEvent;
        }
        public override void OnInspectorGUI()
        {
            tu.undoRedoEvent = Undo.GetCurrentGroupName();


            base.OnInspectorGUI();
            EditorGUILayout.LabelField("SIMPLE TABLE UI [FREE]", labelStyle);
            EditorGUILayout.LabelField("---------------------------------", labelStyle);
            Texture2D LogoTex = Resources.Load<Texture2D>("pro_image");
            GUILayout.Box(LogoTex);
            if (GUILayout.Button("If you like this plugin and would like to support me please consider dowloading the pro version.\n\n It contains more features and has priority on new functionalities and updates\n\n CLICK HERE TO DOWNLOAD!!", GUILayout.Height(100)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/gui/tableui-175586");
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Table Properties", labelStyle);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(box);

            EditorGUI.BeginChangeCheck();
            int rows = EditorGUILayout.IntSlider(rowLabel, tu.Rows, TableUI.MIN_ROWS, TableUI.MAX_ROWS);
            int columns = EditorGUILayout.IntSlider(columnLabel, tu.Columns, TableUI.MIN_COL, TableUI.MAX_COL);
            bool header = EditorGUILayout.Toggle(showHeaderLabel, tu.Header);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Table");
                tu.Rows = rows;
                tu.Columns = columns;
                tu.Header = header;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Border Properties", labelStyle);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(new GUIStyle("box"));

            EditorGUI.BeginChangeCheck();
            BorderType borderType = (BorderType)EditorGUILayout.EnumPopup(borderTypeLabel, tu.BorderType);
            float borderThickness = EditorGUILayout.FloatField(borderThicknessLabel, tu.BorderThickness);
            Color borderColor = EditorGUILayout.ColorField(borderColorLabel, tu.BorderColor);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Border");
                tu.BorderType = borderType;
                tu.BorderThickness = borderThickness;
                tu.BorderColor = borderColor;
            }

            
            

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Text Properties", labelStyle);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(box);

            


            EditorGUI.indentLevel += 1;

            if (tu.Header)
            {
                headerFoldout = EditorGUILayout.Foldout(headerFoldout,headerFoldoutLabel , true, TMPro.EditorUtilities.TMP_UIStyleManager.boldFoldout);
                if (headerFoldout)
                {
                    headerCellPropertiesEditor.OnInspectorGUI();
                }
            }

            bodyFoldout = EditorGUILayout.Foldout(bodyFoldout,bodyFoldoutLabel, true, TMPro.EditorUtilities.TMP_UIStyleManager.boldFoldout);
            if (bodyFoldout)
            {
                bodyCellPropertiesEditor.OnInspectorGUI();

            }

            EditorGUI.indentLevel -= 1;
            
            

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Table Colors", labelStyle);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(box);
            EditorGUI.BeginChangeCheck();
            bool striped = EditorGUILayout.Toggle(stripedLabel, tu.Striped);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Color");
                tu.Striped = striped;
            }
            
            
            if (tu.Header)
            {
                EditorGUI.BeginChangeCheck();
                Color headerColor = EditorGUILayout.ColorField(headerColorLabel, tu.HeaderColor);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Color");
                    tu.HeaderColor = headerColor;
                }
            }

            EditorGUI.BeginChangeCheck();
            Color mainColor = EditorGUILayout.ColorField(mainColorLabel, tu.MainColor);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Color");
                tu.MainColor = mainColor;

            }

            if (tu.Striped)
            {
                EditorGUI.BeginChangeCheck();
                Color secondaryColor = EditorGUILayout.ColorField(secondaryColorLabel, tu.SecondaryColor);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Color");
                    tu.SecondaryColor = secondaryColor;
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            
            Rect r= EditorGUILayout.BeginHorizontal(box);
            if(GUILayout.Button("Make all rows the same height", EditorStyles.miniButtonLeft, GUILayout.Height(50f), GUILayout.MinWidth(r.width / 2)))
            {
                tu.MakeAllRowsTheSameHeight();
            }
            if (GUILayout.Button("Make all columns the same width", EditorStyles.miniButtonRight, GUILayout.Height(50f), GUILayout.MinWidth(r.width / 2)))
            {
                tu.MakeAllColumnsTheSameWidth();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            DrawGrid();
            
        }

        private void OnSceneGUI()
        {
            Vector2 size = tu.GetRectSize(tu.gameObject.GetComponent<RectTransform>());
            
            if (!previousSize.Equals(size))
            {
                previousSize = size;
                Undo.RecordObject(target, "Resize");
                tu.ResizeTable(size);
            }          
        }

        int markRow = -1;
        int markColumn = -1;
        private void DrawGrid()
        {

            GUIStyle columnStyle = new GUIStyle();
            
            columnStyle.fixedWidth = 32f;

            if (tu.selectionGrid.chosenCell.y == -1 && tu.selectionGrid.chosenCell.x == -1)
            {
                GUILayout.Space(18f);
            }
            EditorGUILayout.BeginHorizontal();

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontStyle = FontStyle.Bold;

            
                if (tu.selectionGrid.chosenCell.y != -1)
                {
                    EditorGUILayout.LabelField("Row", labelStyle, GUILayout.Width(25f));
                    EditorGUILayout.LabelField(tu.selectionGrid.chosenCell.y.ToString(), GUILayout.Width(30f));
                }
                if (tu.selectionGrid.chosenCell.x != -1)
                {
                    EditorGUILayout.LabelField("Column", labelStyle, GUILayout.Width(47f));
                    EditorGUILayout.LabelField(tu.selectionGrid.chosenCell.x.ToString());
                }
            
            EditorGUILayout.EndHorizontal();

            float h1 = 40f * (tu.Rows + 1);
            float h = h1 > 400 ? 400 : h1;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(h));
            EditorGUILayout.BeginHorizontal(box);
            GUILayout.FlexibleSpace();
            for (int x = -1; x< tu.Columns; x++)
            {
                EditorGUILayout.BeginVertical(columnStyle);
                for (int y =-1; y < tu.Rows; y++)
                {
                    if (x == -1 && y == -1)
                    {
                        GUILayout.Space(34f);
                    }
                    else if (x == -1)
                    {
                        EditorGUILayout.BeginHorizontal();
                        bool initial = tu.selectionGrid.Values[y + 1].list[x + 1];
                        tu.selectionGrid.Values[y + 1].list[x + 1] = EditorGUILayout.Toggle(tu.selectionGrid.Values[y + 1].list[x + 1] , tu.selectionGrid.skin.customStyles[1], GUILayout.Width(30), GUILayout.Height(30));

                        if (initial != tu.selectionGrid.Values[y + 1].list[x + 1])
                        {
                            markRow = y;
                            if(tu.selectionGrid.Values[y + 1].list[x + 1])
                            {
                                SetAll(false);
                                tu.selectionGrid.Values[y + 1].list[x + 1] = true;
                                tu.selectionGrid.chosenCell.x = x;
                                tu.selectionGrid.chosenCell.y = y;
                            }
                            else
                            {
                                tu.selectionGrid.chosenCell.x = -1;
                                tu.selectionGrid.chosenCell.y = -1;
                            }
                        }
                            

                        EditorGUILayout.EndHorizontal();
                    }
                    else if (y == -1)
                    {
                        EditorGUILayout.BeginVertical();
                        bool initial = tu.selectionGrid.Values[y + 1].list[x + 1];
                        tu.selectionGrid.Values[y + 1].list[x + 1] = EditorGUILayout.Toggle(tu.selectionGrid.Values[y + 1].list[x + 1], tu.selectionGrid.skin.customStyles[2], GUILayout.Width(30), GUILayout.Height(30));
                        if (initial != tu.selectionGrid.Values[y + 1].list[x + 1])
                        {
                            markColumn = x;
                            if (tu.selectionGrid.Values[y + 1].list[x + 1])
                            {
                                SetAll(false);
                                tu.selectionGrid.Values[y + 1].list[x + 1] = true;
                                tu.selectionGrid.chosenCell.x = x;
                                tu.selectionGrid.chosenCell.y = y;
                            }
                            else
                            {
                                tu.selectionGrid.chosenCell.x = -1;
                                tu.selectionGrid.chosenCell.y = -1;
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        GUIStyle toggleStyle = new GUIStyle("toggle");

                        if(markRow==y || markColumn == x)
                        {
                            tu.selectionGrid.Values[y + 1].list[x + 1] = tu.selectionGrid.Values[y + 1].list[0] || tu.selectionGrid.Values[0].list[x + 1];
                        }
                        else
                        {
                            bool initial = tu.selectionGrid.Values[y + 1].list[x + 1];
                            tu.selectionGrid.Values[y + 1].list[x + 1]=EditorGUILayout.Toggle(tu.selectionGrid.Values[y + 1].list[x + 1], tu.selectionGrid.skin.customStyles[0], GUILayout.Width(30), GUILayout.Height(30));
                            if (initial != tu.selectionGrid.Values[y + 1].list[x + 1])
                            {
                                SetAll(false);
                                tu.selectionGrid.Values[y + 1].list[x + 1] = !initial;
                                if (!initial)
                                {
                                    tu.selectionGrid.chosenCell.x = x;
                                    tu.selectionGrid.chosenCell.y = y;
                                }
                                else
                                {
                                    tu.selectionGrid.chosenCell.x = -1;
                                    tu.selectionGrid.chosenCell.y = -1;
                                }
                            }                
                        }
                    }
                 }
                EditorGUILayout.EndVertical();
            }
            markRow = -1;
            markColumn = -1;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            try
            {
                Vector2Int index = GetFirstValue();
                EditorGUILayout.Space();
                if (index.y == 0)
                {
                    tu.UpdateColumnWidth(EditorGUILayout.FloatField("Column Width", tu.ColumnsWidth[(int)index.x-1]),(int) index.x-1);
                }else if (index.x == 0)
                {
                    tu.UpdateRowHeight(EditorGUILayout.FloatField("Row Height", tu.RowsHeight[(int)index.y - 1]), (int)index.y - 1);
                }
                else if (!index.Equals(Vector2.zero))
                {
                   TextMeshProUGUI tmp = tu.GetCell(index.y-1,index.x-1);                
                   CreateEditor(tmp).OnInspectorGUI();
                }
            }
            catch(System.Exception)
            {
            }
        }

        Vector2Int GetFirstValue()
        {
            for(int x =0; x < tu.Columns+1; x++)
            {
                for(int y =0; y< tu.Rows+1; y++)
                {
                    if (tu.selectionGrid.Values[y].list[x])
                        return new Vector2Int(x, y);
                }
            }
            throw new System.Exception();
        }

        void SetAll(bool value)
        {
            for (int x = 0; x < tu.Columns + 1; x++)
            {
                for (int y = 0; y < tu.Rows + 1; y++)
                {
                    tu.selectionGrid.Values[y].list[x] = value;
                }
            }
        }


    }
}
 
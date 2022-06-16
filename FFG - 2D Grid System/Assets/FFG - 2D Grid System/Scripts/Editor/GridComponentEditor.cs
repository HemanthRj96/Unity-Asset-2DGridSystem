using FFG;
using UnityEditor;
using UnityEngine;


namespace FFG_Editors
{
    [CustomEditor(typeof(GridComponent))]
    public class GridComponentEditor : EditorUtils<GridComponent>
    {
        private static bool _enableDebugSettings = false;
        private static bool _enableCellValidator = false;
        private static Vector2 _mousePosition;
        private static KeyCode _validCellKey = KeyCode.F;
        private static KeyCode _invalidCellKey = KeyCode.G;

        SerializedProperty _gcData;
        SerializedProperty _gridDimesion;
        SerializedProperty _cellDimesion;
        SerializedProperty _gridOffset;
        SerializedProperty _invalidCellArray;

        SerializedProperty _offsetType;
        SerializedProperty _presetType;
        SerializedProperty _shouldDrawGizmos;
        SerializedProperty _gridLineColor;
        SerializedProperty _crossLineColor;



        public override void CustomOnGUI()
        {
            bool shouldDisable = EditorApplication.isPlaying || EditorApplication.isPaused;

            if(shouldDisable)
                Info("Exit playmode to edit fields", MessageType.Warning);

            EditorGUI.BeginDisabledGroup(shouldDisable);

            _gcData = GetProperty("_gcData");
            _gridDimesion = _gcData.FindPropertyRelative("_gridDimension");
            _cellDimesion = _gcData.FindPropertyRelative("_cellDimension");
            _gridOffset = _gcData.FindPropertyRelative("_gridOffset");
            _invalidCellArray = _gcData.FindPropertyRelative("_invalidCellArray");
            _offsetType = _gcData.FindPropertyRelative("_offsetType");
            _presetType = _gcData.FindPropertyRelative("_presetType");
            _shouldDrawGizmos = _gcData.FindPropertyRelative("_shouldDrawGizmos");
            _gridLineColor = _gcData.FindPropertyRelative("_gridLineColor");
            _crossLineColor = _gcData.FindPropertyRelative("_crossLineColor");

            Heading("Grid Settings");
            Space(10);

            _enableDebugSettings = EditorGUILayout.Toggle("Show debug settings : ", _enableDebugSettings);
            _enableCellValidator = EditorGUILayout.Toggle("Enable cell editing : ", _enableCellValidator);

            if (_enableCellValidator)
            {
                Space(10);
                Info($"If enabled then by pressing {_validCellKey} you can make a cell in grid valid and by pressing {_invalidCellKey} you can make a cell in grid invalid");
            }

            if (_shouldDrawGizmos.boolValue == false)
            {
                if (Button("Enable Grid View"))
                    _shouldDrawGizmos.boolValue = true;
            }
            else
            {
                if (Button("Disable Grid View"))
                    _shouldDrawGizmos.boolValue = false;
            }

            Space(20);

            PropertyField(_gridDimesion, "Grid Dimensions : ", "Width and height of the grid");
            PropertyField(_cellDimesion, "Cell Dimensions : ", "The size of a single cell");
            PropertyField(_offsetType, "Pivot Type : ", "");


            int h = _gridDimesion.vector2IntValue.x;
            int v = _gridDimesion.vector2IntValue.y;
            Vector2 cd = _cellDimesion.vector2Value;

            // Grid origin
            switch (_offsetType.enumValueIndex)
            {
                case 0:
                    PropertyField(_presetType, "Select Preset Pivot : ", "");
                    switch (_presetType.enumValueIndex)
                    {
                        case 0:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x, -v * cd.y);
                            break;
                        case 1:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, -v * cd.y);
                            break;
                        case 2:
                            _gridOffset.vector2Value = new Vector2(0, -v * cd.y);
                            break;
                        case 3:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x, -v * cd.y / 2);
                            break;
                        case 4:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, -v * cd.y / 2);
                            break;
                        case 5:
                            _gridOffset.vector2Value = new Vector2(0, -v * cd.y / 2);
                            break;
                        case 6:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x, 0);
                            break;
                        case 7:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, 0);
                            break;
                        case 8:
                            _gridOffset.vector2Value = new Vector2(0, 0);
                            break;
                    }
                    break;
                case 1:
                    PropertyField(_gridOffset, "Pivot Point : ", "");
                    break;
            }

            // Debug settings
            if (_enableDebugSettings)
            {
                Space(15);
                Heading("Debug Settings");

                Space(10);
                _gridLineColor.colorValue = EditorGUILayout.ColorField("Grid line color : ", _gridLineColor.colorValue);
                _crossLineColor.colorValue = EditorGUILayout.ColorField("Cross line color : ", _crossLineColor.colorValue);
                Space(10);
                _validCellKey = (KeyCode)EditorGUILayout.EnumPopup("Key for setting valid cells", _validCellKey);
                _invalidCellKey = (KeyCode)EditorGUILayout.EnumPopup("Key for setting invalid cells", _invalidCellKey);
            }

            EditorGUI.EndDisabledGroup();
        }

        private void OnSceneGUI()
        {
            if (_enableCellValidator)
            {
                Event e = Event.current;
                SceneView scene = SceneView.currentDrawingSceneView;

                if (e.isKey)
                {
                    var ppp = EditorGUIUtility.pixelsPerPoint;
                    _mousePosition = e.mousePosition;

                    _mousePosition.y = scene.camera.pixelHeight - _mousePosition.y * ppp;
                    _mousePosition.x = _mousePosition.x * ppp;
                    _mousePosition = scene.camera.ScreenToWorldPoint(_mousePosition);

                    // Initializing cell list                    
                    var gridOrigin = Root.GridOrigin;
                    var cellDimension = Root.CellDimension;
                    var gridDimension = Root.GridDimension;
                    int x = Mathf.FloorToInt((_mousePosition - gridOrigin).x / cellDimension.x);
                    int y = Mathf.FloorToInt((_mousePosition - gridOrigin).y / cellDimension.y);

                    // Check if the mouse position is in a valid grid cell
                    if (x >= 0 && x < gridDimension.x && y >= 0 && y < gridDimension.y)
                    {
                        Vector2Int targetIndex = new Vector2Int(x, y);
                        int index = -1;
                        int length = _invalidCellArray.arraySize;

                        for (int i = 0; i < length; i++)
                        {
                            if (_invalidCellArray.GetArrayElementAtIndex(i).vector2IntValue == targetIndex)
                            {
                                index = i;
                                break;
                            }
                        }

                        if (e.keyCode == _invalidCellKey && index == -1)
                        {
                            _invalidCellArray.InsertArrayElementAtIndex(length);
                            _invalidCellArray.GetArrayElementAtIndex(length).vector2IntValue = targetIndex;
                        }

                        if (e.keyCode == _validCellKey && index != -1)
                            _invalidCellArray.GetArrayElementAtIndex(index).DeleteCommand();
                    }
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps {
    [CustomGridBrush(false, true, false, "GridObject Brush")]
    [CreateAssetMenu(fileName = "New GridObject Brush", menuName = "Brushes/GridObject Brush")]
    public class GridObjectBrush : GridBrush {

        public GridObject m_GridObjectPrefab;
        public Vector3 m_WorldOffset = new Vector3(0f, 0f, 0f);
        public Vector3 m_Rotation = Vector3.zero;
        protected GameObject prev_brushTarget;
        protected Vector3Int prev_position;
        public static Transform GridObjectsHolder;
        public static Transform GridTilesHolder;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if (position == prev_position) {
                return;
            }
            prev_position = position;
            if (brushTarget) {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            // Update the holders
            GetHolders(brushTarget);

            // Do not allow to place the GridObject if there is no tile at the target position
            GridTile tileAtPos = GetGridTileInCell(grid, GridTilesHolder, position);
            if (tileAtPos == null) {
                Debug.Log("There is no GridTile in that position, make sure to place one at this GridObject's position");
            }

            GridObject gridObjectAtPos = GetGridObjectInCell(grid, GridObjectsHolder, position);
            if (gridObjectAtPos != null) {
                Debug.Log("There is another GridObject at the target position");
                return;
            }

            GridObject prefab = m_GridObjectPrefab;
            GridObject instance = (GridObject)PrefabUtility.InstantiatePrefab(prefab);
            if (instance != null) {
                Undo.MoveGameObjectToScene(instance.gameObject, brushTarget.scene, "Paint GridObjectPrefab");
                Undo.RegisterCreatedObjectUndo((Object)instance, "Paint GridObjectPrefab");
                instance.transform.SetParent(GridObjectsHolder);
                var cellCenterOffset = Vector3.Scale(grid.GetLayoutCellCenter(), grid.cellSize.ToVector3XZY());
                var cellCenterOffsetWorld = grid.LocalToWorld(cellCenterOffset);
                instance.transform.position = grid.CellToWorld(position) +  m_WorldOffset + cellCenterOffsetWorld;
                instance.transform.rotation = Quaternion.Euler(m_Rotation);
                instance.m_GridPosition = position.ToVector2IntXY();

            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if (brushTarget) {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            // Update holders
            GetHolders(brushTarget);

            // Check if there is a tile at the target position and erase it
            GridObject erased = GetGridObjectInCell(grid, GridObjectsHolder, position);
            if (erased != null) {
                Undo.DestroyObjectImmediate(erased.gameObject);
            }
        }

        public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
            foreach (var vector3Int in position.allPositionsWithin) {
                Paint(gridLayout, brushTarget, vector3Int);
            }
        }

        public override void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
            foreach (var vector3Int in position.allPositionsWithin) {
                Erase(gridLayout, brushTarget, vector3Int);
            }
        }

        protected virtual void GetHolders(GameObject brushTarget) {
            if (GridObjectsHolder == null) {
                GridObjectsHolder = brushTarget.transform.Find("GridObjects");
                if (GridObjectsHolder == null) {
                    GridObjectsHolder = new GameObject("GridObjects").transform;
                    GridObjectsHolder.SetParent(brushTarget.transform);
                    GridObjectsHolder.localPosition = Vector3.zero;
                }
            }

            if (GridTilesHolder == null) {
                GridTilesHolder = brushTarget.transform.Find("GridTiles");
                if (GridTilesHolder == null) {
                    GridTilesHolder = new GameObject("GridTiles").transform;
                    GridTilesHolder.SetParent(brushTarget.transform);
                    GridTilesHolder.localPosition = Vector3.zero;
                }
            }
        }

        private static GridTile GetGridTileInCell(GridLayout grid, Transform parent, Vector3Int position) {
            int childCount = parent.childCount;

            for (int i = 0; i < childCount; i++) {
                Transform child = parent.GetChild(i);
                var gridTileComp = child.GetComponent<GridTile>();
                if (gridTileComp) {
                    if (position.ToVector2IntXY() == gridTileComp.m_GridPosition) {
                        return gridTileComp;
                    }
                }
            }

            return null;
        }

        private static GridObject GetGridObjectInCell(GridLayout grid, Transform parent, Vector3Int position) {
            int childCount = parent.childCount;         

            for (int i = 0; i < childCount; i++) {
                Transform child = parent.GetChild(i);
                var gridObjectComp = child.GetComponent<GridObject>();
                if (gridObjectComp) {
                    if (position.ToVector2IntXY() == gridObjectComp.m_GridPosition) {
                        return gridObjectComp;
                    }
                }
            }

            return null;
        }
    }

    [CustomEditor(typeof(GridObjectBrush))]
    public class GridObjectBrushEditor : GridBrushEditor {
        private GridObjectBrush gridObjectBrush { get { return target as GridObjectBrush; } }

        private SerializedProperty m_GridObjectPrefab;
        private SerializedObject m_SerializedObject;
        //
        protected override void OnEnable() {
            base.OnEnable();
            m_SerializedObject = new SerializedObject(target);
            m_GridObjectPrefab = m_SerializedObject.FindProperty("m_GridObjectPrefab");
        }

        public override void OnPaintInspectorGUI()
        {
            m_SerializedObject.UpdateIfRequiredOrScript();
            gridObjectBrush.m_Rotation = EditorGUILayout.Vector3Field("Rotation", gridObjectBrush.m_Rotation);
            EditorGUILayout.PropertyField(m_GridObjectPrefab, true);
            m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        // Draw the current Coordinates
        public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing) {
            base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);

            var labelText = "Pos: " + position.position;
            if (position.size.x > 1 || position.size.y > 1) {
                labelText += " Size: " + position.size;
            }

            Handles.Label(grid.CellToWorld(position.position), labelText);
        }
    }
}


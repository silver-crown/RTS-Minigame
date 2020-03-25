using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps {
    [CustomGridBrush(false, true, false, "GridTile Brush")]
    [CreateAssetMenu(fileName = "New GridTile Brush", menuName = "Brushes/GridTile Brush")]
    public class GridTileBrush : GridBrush {
        public GridTile m_GridTilePrefab;
        public int m_HeightInGrid = 0;
        //public float m_ZOffSet = 0f;
        //public Vector3 m_Anchor = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 m_WorldOffset = new Vector3(0f, 0f, 0f);
        public Vector3 m_Rotation = Vector3.zero;

        protected GameObject prev_brushTarget;
        protected Vector3Int prev_position;
        public static Transform GridTilesHolder;
        public static Transform GridObjectsHolder;

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

            if (GridTilesHolder == null) {
                GridTilesHolder = brushTarget.transform.Find("GridTiles");
                if (GridTilesHolder == null) {
                    GridTilesHolder = new GameObject("GridTiles").transform;
                    GridTilesHolder.SetParent(brushTarget.transform);
                    GridTilesHolder.localPosition = Vector3.zero;
                }
            }

            // Update the holders
            GetHolders(brushTarget);

            // Do not allow to place the GridTile if there is another tile at the target position
            GridTile tileAtPos = GetGridTileInCell(grid, GridTilesHolder, position);
            if (tileAtPos != null) {
                Debug.Log("There is another GridTile at the target position");
                return;
            }

            GridTile prefab = m_GridTilePrefab;
            GridTile instance = (GridTile)PrefabUtility.InstantiatePrefab(prefab);
            if (instance != null) {
                Undo.MoveGameObjectToScene(instance.gameObject, brushTarget.scene, "Paint GridTilePrefab");
                Undo.RegisterCreatedObjectUndo((Object)instance, "Paint GridTilePrefab");
                instance.transform.SetParent(GridTilesHolder);
                var cellCenterOffset = Vector3.Scale(grid.GetLayoutCellCenter(), grid.cellSize.ToVector3XZY());
                var cellCenterOffsetWorld = grid.LocalToWorld(cellCenterOffset);
                instance.transform.position = grid.CellToWorld(position) +  m_WorldOffset + cellCenterOffsetWorld;
                instance.transform.rotation = Quaternion.Euler(m_Rotation);
                instance.m_GridPosition = position.ToVector2IntXY();
                instance.m_TileHeight = m_HeightInGrid;

                // If there was a GridObject at the target position we initialize it to this tile
                GridObject gridObjectAtPos = GetGridObjectInCell(grid, GridObjectsHolder, position);
                if (gridObjectAtPos != null) {
                    gridObjectAtPos.SetCurrentGridTile(instance);
                    gridObjectAtPos.AddToTile(instance);
                }
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

            // Update the holders
            GetHolders(brushTarget);

            // Check if there is a tile at the target position and erase it
            GridTile erased = GetGridTileInCell(grid, GridTilesHolder, position);
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

    }

    [CustomEditor(typeof(GridTileBrush))]
    public class GridTileBrushEditor : GridBrushEditor {
        protected GridTileBrush gridTileBrush { get { return target as GridTileBrush; } }
        protected SerializedProperty m_GridTilePrefab;
        protected SerializedObject m_SerializedObject;

        protected override void OnEnable() {
            base.OnEnable();
            m_SerializedObject = new SerializedObject(target);
            m_GridTilePrefab = m_SerializedObject.FindProperty("m_GridTilePrefab");
        }   

        public override void OnPaintInspectorGUI() {
            m_SerializedObject.UpdateIfRequiredOrScript();
            gridTileBrush.m_HeightInGrid = EditorGUILayout.IntField("Height In Grid", gridTileBrush.m_HeightInGrid);
            //gridTileBrush.m_ZOffSet = EditorGUILayout.FloatField("World Z OffSet", gridTileBrush.m_ZOffSet);
            gridTileBrush.m_WorldOffset = EditorGUILayout.Vector3Field("World Z OffSet", gridTileBrush.m_WorldOffset);
            gridTileBrush.m_Rotation = EditorGUILayout.Vector3Field("Rotation", gridTileBrush.m_Rotation);
            EditorGUILayout.PropertyField(m_GridTilePrefab, true);
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
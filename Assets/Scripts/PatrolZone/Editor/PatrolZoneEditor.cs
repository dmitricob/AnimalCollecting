using UnityEditor;
using UnityEngine;

namespace PatrolZone.Editor
{
    [CustomEditor(typeof(PatrolZone))]
    public class PatrolZoneEditor : UnityEditor.Editor
    {
        private SerializedProperty _patrolZonesProperty;

        private void OnEnable()
        {
            _patrolZonesProperty = serializedObject.FindProperty("_patrolZones");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_patrolZonesProperty, true);

            serializedObject.ApplyModifiedProperties();
        }
        private void OnSceneGUI()
        {
            PatrolZone patrolZone = (PatrolZone)target;

            for (int i = 0; i < _patrolZonesProperty.arraySize; i++)
            {
                SerializedProperty rectProperty = _patrolZonesProperty.GetArrayElementAtIndex(i);
                
                Rect rect = rectProperty.rectValue;
                
                // Rect rect = patrolZone._patrolZones[i];

                EditorGUI.BeginChangeCheck();
                
                Vector2 newTopLeft = Handles.FreeMoveHandle(rect.position, 0.5f, Vector2.one * 0.5f, Handles.CircleHandleCap);
                Vector2 newBottomRight = Handles.FreeMoveHandle(rect.position + rect.size, 0.5f, Vector2.one * 0.5f, Handles.CircleHandleCap);
                
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(patrolZone, "Move Rect Corner");
                
                    rectProperty.rectValue = new Rect(newTopLeft, newBottomRight - newTopLeft);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
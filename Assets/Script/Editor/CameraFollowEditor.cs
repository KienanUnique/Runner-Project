using Script.Camera;
using UnityEditor;

namespace Script.Editor
{
    [CustomEditor(typeof(CameraFollow))]
    public class CameraFollowEditor : UnityEditor.Editor
    {
        public enum DisplayCategory
        {
            MiddleOfCell, BetweenCells
        }

        public DisplayCategory centerType;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Default center", EditorStyles.boldLabel);
            centerType = (DisplayCategory) EditorGUILayout.EnumPopup("Type of centering", centerType);

            switch (centerType)
            {
                case DisplayCategory.BetweenCells:
                    DisplayBetweenCellsCentering();
                    break;

                case DisplayCategory.MiddleOfCell:
                    DisplaySingleCellCentering();
                    break;

            }
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lerpTime"));
            
            serializedObject.ApplyModifiedProperties();
        }

        void DisplayBetweenCellsCentering()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultLeftCellX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultRightCellX"));
            serializedObject.FindProperty("centerBetween").boolValue = true;
        }
        
        void DisplaySingleCellCentering()
        {
            serializedObject.FindProperty("centerBetween").boolValue = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultCenterCellX"));
        }
    }
}
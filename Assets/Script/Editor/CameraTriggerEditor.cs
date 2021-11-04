using Script.Triggers;
using UnityEditor;

namespace Script.Editor
{
    [CustomEditor(typeof(CameraTrigger))]
    public class CameraTriggerEditor : UnityEditor.Editor
    {
        public enum DisplayCategory
        {
            MiddleOfCell, BetweenCells
        }

        public DisplayCategory centerType;
        public override void OnInspectorGUI()
        {
            centerType = (DisplayCategory) EditorGUILayout.EnumPopup("Type of centering", centerType);
            EditorGUILayout.Space();
            
            switch (centerType)
            {
                case DisplayCategory.BetweenCells:
                    DisplayBetweenCellsCentering();
                    break;

                case DisplayCategory.MiddleOfCell:
                    DisplaySingleCellCentering();
                    break;

            }
            
            serializedObject.ApplyModifiedProperties();
        }

        void DisplayBetweenCellsCentering()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("leftCellX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rightCellX"));
            serializedObject.FindProperty("centerBetween").boolValue = true;
        }
        
        void DisplaySingleCellCentering()
        {
            serializedObject.FindProperty("centerBetween").boolValue = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("centerCellX"));
        }
    }
}

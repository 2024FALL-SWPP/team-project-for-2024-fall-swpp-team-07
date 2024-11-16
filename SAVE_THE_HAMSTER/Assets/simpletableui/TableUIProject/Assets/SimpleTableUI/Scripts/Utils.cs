
namespace UnityEngine.UI.TableUI
{
    public class Utils
    {
        public static void SetDirty(Object target)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(target);
#endif
        }

        public static void RegisterFullObjectHierarchyUndo(Object target,string name)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(target, name);
#endif
        }

        public static void DestroyObjectImmediate(Object target)
        {
            bool inEditor = false;
#if UNITY_EDITOR
            UnityEditor.Undo.DestroyObjectImmediate(target);
            inEditor = true;
#endif
            if (!inEditor) {
                Object.DestroyImmediate(target);
            }
        }

        public static void RegisterCreatedObjectUndo(Object target, string name)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(target, name);
#endif
        }

        public static void RecordObject(Object target,string name)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(target, name);
#endif
        }

        public static void RecordObjects(Object[] targets, string name)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObjects(targets, name);
#endif
        }
    }
}

using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

//TODO: Add build-time warnings

/// <summary>
/// Field will glow red if null
/// </summary>
public class RequireReference : PropertyAttribute
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer( typeof(RequireReference) )]
    public class ThisPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI ( Rect position , SerializedProperty property , GUIContent label )
        {
            EditorGUI.BeginProperty( position , label , property );
            {
                //test property:
                bool isNull;
                if( property.propertyType==SerializedPropertyType.ObjectReference )
                {
                    isNull = property.objectReferenceValue==null;
                } else if( property.propertyType==SerializedPropertyType.ExposedReference )
                {
                    isNull = property.exposedReferenceValue==null;
                } else
                {
                    label.text += string.Format( " ([{0}] is not compatible with {1} type fields)" , typeof(RequireReference) , property.propertyType );
                    isNull = false;
                }

                //draw
                if( isNull )
                {
                    //draw animated rect:
                    Color color = Color.red;
                    color.a = 0.75f+Mathf.Sin( (float)( ( EditorApplication.timeSinceStartup*10d )%float.MaxValue ) )*0.25f;
                    EditorGUI.DrawRect( position , color );
                }

                //draw PropertyField:
                EditorGUI.PropertyField( position , property , label );
                
            }
            EditorGUI.EndProperty();
        }
    }
    #endif
}
//SOURCE: https://gist.github.com/andrew-raphael-lukasik/60e6dc072dc170ab535a7aff961c3ba9
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

//TODO: Add build-time warnings

/// <summary>
/// Field will glow red when null (or not of spefified Component type)
/// </summary>
public class RequireReference : PropertyAttribute
{
    #region FIELDS

    public System.Type requiredType;

    #endregion
    #region CONSTRUCTORS

    /// <summary>
    /// Require reference
    /// </summary>
    public RequireReference ()
    {
        
    }

    /// <summary>
    /// Require reference to a object with specific Component attached,
    /// </summary>
    public RequireReference ( System.Type componentType )
    {
        requiredType = componentType;
    }

    #endregion
    #region NESTED_TYPES

    #if UNITY_EDITOR
    [CustomPropertyDrawer( typeof(RequireReference) )]
    public class ThisPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI ( Rect position , SerializedProperty property , GUIContent label )
        {
            EditorGUI.BeginProperty( position , label , property );
            {
                RequireReference getRequireReference = attribute as RequireReference;

                //test property:
                System.Object getObject;
                if( property.propertyType==SerializedPropertyType.ObjectReference )
                {
                    getObject = property.objectReferenceValue;
                } else if( property.propertyType==SerializedPropertyType.ExposedReference )
                {
                    getObject = property.exposedReferenceValue;
                } else
                {
                    var sb = new System.Text.StringBuilder();
                    {
                        sb.AppendFormat(
                            " ([{0}] is not compatible with {1} type fields)" ,
                            typeof(RequireReference) ,
                            property.propertyType
                        );
                    }
                    label.text += sb.ToString();
                    getObject = null;
                }

                bool isOfRequiredType = false;
                if( ( getRequireReference.requiredType!=null )&&( getObject!=null ) )
                {
                    if( getObject is GameObject )
                    {
                        isOfRequiredType = ( (GameObject)getObject ).GetComponent( getRequireReference.requiredType )!=null;
                    } else if( getObject is Component )
                    {
                        var component = (Component)getObject;
                        if( component!=null )
                        {
                            isOfRequiredType = component.GetComponent( getRequireReference.requiredType )!=null;
                        }
                    }
                }

                //draw red error overlay:
                if( ( getObject==null )||( isOfRequiredType==false ) )
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

    #endregion
}
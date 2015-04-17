using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class IAInitializer : MonoBehaviour
{
	public string name;

	//FuzzyLogic.Engine moteur;
	/*
	 * On récupère le modèle de moteur d'intelligence (propre à chaque type d'IA)
	 * On récupère les sensors et les sorties d'effectors au format dll avec des noms génériques :
	 * 		On récupère un même sensor pour toutes les instances d'un type d'IA
	 * 		l'Effector est identique pour tous les types d'IA
	 * 		On récupère la sortie effector, qui n'est pas nécessairement partagé entre plusieurs instances d'un même type.
	 * On branche les sensors.
	 * On ajoute les fonctions listeners à la sortie de l'effector
	 */
	void Start ()
	{
		//Assembly a = Assembly.LoadFile(@"C:\Users\Unity\Documents\MarshmallowDLL\MarshmallowDLL\bin\Debug\Marsh");
        /*object o = a.CreateInstance("IALibrary.IA");
        Type type = a.GetType("IALibrary.IA");
        Debug.Log("type = " + type);
        MemberInfo[] memberinfos = type.GetMembers();
        Debug.Log("test member info : " + memberinfos +" of size "+ memberinfos.Length);
        foreach (MemberInfo mi in memberinfos)
        {
            Debug.Log("mi -> " + mi);
        }

        PropertyInfo[] propertyinfos = type.GetProperties();
        Debug.Log("test property info : " + propertyinfos +" of size "+ propertyinfos.Length);
        foreach (PropertyInfo pi in propertyinfos)
        {
            Debug.Log("pi -> " + pi);
        }

        MethodInfo[] methodinfos = type.GetMethods();
        Debug.Log("test methodinfos : " + methodinfos +" of size "+ methodinfos.Length);
        foreach (MethodInfo pi in methodinfos)
        {
            Debug.Log("method info -> " + pi);
        }

        FieldInfo[] fieldinfo = type.GetFields();
        Debug.Log("test fieldinfo : " + fieldinfo +" of size "+ fieldinfo.Length);
        foreach (FieldInfo pi in fieldinfo)
        {
            Debug.Log("field info -> " + pi);
        }

        FieldInfo testinfo = type.GetField("test");
        Debug.Log(testinfo.GetValue(o));

        GameObject gobj = GameObject.FindGameObjectWithTag("MainCamera");
        gobj.AddComponent(type);
*/
//        Debug.Log("test = " + o.GetType().GetMember("test")[0]);

           
	}


}

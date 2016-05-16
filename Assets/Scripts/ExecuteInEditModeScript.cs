using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class ExecuteInEditModeScript : MonoBehaviour
{
	void Update ()
    {
        GameObject[] g = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in g)
        {
            try
            {
                string s1 = go.name; 
                string s2 = s1.Substring(s1.Length - 3);
                if (s2 == "(1)")
                {
                    GameObject[] selectObject = Selection.gameObjects;
                    for (int i = 0; i < Selection.objects.Length; i++)
                    {
                        selectObject[i].name = s1.Remove(s1.Length - 4);
                    }
                }
                s2 = s1.Substring(s1.Length - 7);
                if (s2 == "(Clone)")
                {
                    go.name = s1.Remove(s1.Length - 7);
                }
            }
            catch
            {

            }
        }
	}
}

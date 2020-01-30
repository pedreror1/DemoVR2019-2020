using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Threading.Tasks;

public class PoolCreator 
{
    [MenuItem("GameObject/Create PoolSystem", false, 0)]
    static void Init()
    {
        
        
        string poolName=Selection.activeGameObject.name;
        if (poolName.Contains("Pool"))
        {
            string objectName = poolName.Replace("Pool", "Object");
            
            string classDef = "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\npublic class " +
                               poolName + " : poolSystem<"+objectName+">\n{\n}";
            
            string path = Application.dataPath + "/Scripts/Systems/GenericPoolSystem/GeneratedPools/" + poolName;

            if(!System.IO.Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "/" + poolName + ".cs";

            if (!File.Exists(path))
            {
                StreamWriter outfile = File.CreateText(path);
                outfile.Write(classDef);
                outfile.Dispose();
              
            }

            string objectClass = "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\n\npublic class " +
                                  objectName + " : MonoBehaviour, IPooledObject\n{\n\nprivate void OnEnable()\n{\nborn();\n}\npublic void born()\n{\n" +
                                  "throw new System.NotImplementedException();\n}\npublic void Interaction()\n{\nthrow new System.NotImplementedException();\n}\n" +
                                  "public void Dead()\n{\n" +
                                  poolName + ".Instance.returnToPool(this);\n}\n}";
            string subpath = path.Replace(poolName + ".cs", objectName + ".cs");

            if (!File.Exists(subpath))
            {
                StreamWriter outfile = File.CreateText(subpath);
                outfile.Write(objectClass);
                outfile.Dispose();
            }
            AssetDatabase.Refresh();

            
            
        }
        else
        {
            Debug.Log("Make sure the Selected Object Name Includes Pool :) ");
        } 

    }
   
    
}

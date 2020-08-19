using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HW.Collections;

namespace HW.Editor
{

    public class AssetBuilder : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [MenuItem("Assets/Create/HW/Item")]
        public static void CreateItem()
        {
            Item asset = ScriptableObject.CreateInstance<Item>();

            string name = "item.asset";
            string folder = "Assets/Resources/Items/";

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

       
    }

}

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

        [MenuItem("Assets/Create/HW/Quest")]
        public static void CreateQuestDetail()
        {
            QuestDetail asset = ScriptableObject.CreateInstance<QuestDetail>();

            string name = "quest.asset";
            string folder = "Assets/Resources/" + QuestDetail.ResourceFolder +"/";

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/HW/MessageCollection")]
        public static void CreateMessageCollection()
        {
            MessageCollection asset = ScriptableObject.CreateInstance<MessageCollection>();

            string name = "messageCollection.asset";
            string folder = "Assets/Resources/MessageCollection/";

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/HW/Dialog")]
        public static void CreateDialog()
        {
            Dialog asset = ScriptableObject.CreateInstance<Dialog>();

            string name = "dialog.asset";
            string folder = "Assets/Resources/Dialog/";

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/HW/Tutorial")]
        public static void CreateTutorial()
        {
            Tutorial asset = ScriptableObject.CreateInstance<Tutorial>();

            string name = "tutorial.asset";
            string folder = "Assets/Resources/" + Tutorial.ResourceFolder + "/";

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/HW/UI/MessageCollection")]
        public static void CreateUIMessageCollection()
        {
            MessageCollection asset = ScriptableObject.CreateInstance<MessageCollection>();

            string name = "messageCollection.asset";
            string folder = "Assets/Resources/" + Constants.ResourceFolderMessageCollectionUI +"/";

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            AssetDatabase.CreateAsset(asset, folder + name);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }

}

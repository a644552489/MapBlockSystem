using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using UnityEditor;
using System;
namespace OuY
{
    [CreateAssetMenu(menuName = "ScriptableObject/MapMatrixData")]
    [Serializable]
    public class MapBlockDefine : SerializedScriptableObject
    {
        private static MapBlockDefine _Inst =null;
        public static MapBlockDefine Instance 
        {
            get {   return _Inst; }
        }
        


        [ShowInInspector]
        [TableMatrix(HorizontalTitle = "地板编号")]
        public  int[,] test = new int[ CampMapBlock.blockSizeX, CampMapBlock.blockSizeX];

        [ShowInInspector]
        public Color Owner = new Color();
        [ShowInInspector]
        public Color OwnerEdge = new Color();

        [ShowInInspector]
        public Color Enermy = new Color();
        [ShowInInspector]
        public Color EnermyEdge = new Color();
     



    }

    public class Window_Show : OdinMenuEditorWindow
    {
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            tree.AddAssetAtPath("地板区域", "MapBlock/Resources/BlackGroundBlock.asset");
            return tree;
        }

        [MenuItem("Tools/设置窗口")]
        private static void Open()
        {
            var window = GetWindow<Window_Show>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

    }
   

    /// <summary>
    /// 是否显示或者无效
    /// </summary>
    public class Camp
    {
        public int campId;
    
    }
    public static class CampUtil
    {
        public static Camp NullCamp = new Camp() { campId = -1 };
        public static bool Equal(Camp a, Camp b)
        {
            return a.campId == b.campId;
        }
    }
    public class ColorData
    {
        public Color color1;
        public Color color2;

    }
}
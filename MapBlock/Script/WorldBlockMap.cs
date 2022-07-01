using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OuY
{
    public class WorldBlockMap : MonoBehaviour
    {
        
        private class ShaderID
        {
            public static int _DataMap_Color_ID = Shader.PropertyToID("_DataMap_Color");
            public static int _DataMap_Color2_ID = Shader.PropertyToID("_DataMap_Color2");
            public static int _DataMap_Edge_ID = Shader.PropertyToID("_DataMap_Edge");
            public static int _DataMap_Angle_ID = Shader.PropertyToID("_DataMap_Angle");
            public static int _DataMap_Color_ST_ID = Shader.PropertyToID("_DataMap_Color_ST");

        }



        public  void Init(int serverindex)
        {
            CampMapBlock.Instance.Init(serverindex);
        }
        public  void InitData(int serverindex)
        {
            CampMapBlock.Instance.UpdateDatas(serverindex);
        }
        public void InitMiniMap(int serverIndex)
        {
            Material mat = GetComponent<Renderer>().material;
            mat.SetTexture(ShaderID._DataMap_Color_ID, CampMapBlock.Tex_Color);
            mat.SetTexture(ShaderID._DataMap_Color2_ID, CampMapBlock.Tex_Color2);
            mat.SetTexture(ShaderID._DataMap_Edge_ID, CampMapBlock.Tex_Edge);
            mat.SetTexture(ShaderID._DataMap_Angle_ID, CampMapBlock.Tex_Angle);
            mat.SetVector(ShaderID._DataMap_Color_ST_ID, new Vector4(1.0f / serverIndex, 1, (serverIndex -1) * 1.0f / serverIndex, 0));

        }
        private void Start()
        {

            Init(1);
            InitData(1);
            InitMiniMap(1);
        }



    }
}
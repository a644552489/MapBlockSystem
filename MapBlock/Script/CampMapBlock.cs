using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OuY
{
    public class CampMapBlock
    {
        public const int blockSizeX = 17;
        public const int blockSizeY = 17;


        private const int Index_L = 0;
        private const int Index_R = 1;
        private const int Index_B = 2;
        private const int Index_T = 3;
        private const int Index_LB = 4;
        private const int Index_RB = 5;
        private const int Index_LT = 6;
        private const int Index_RT = 7;

        private static CampMapBlock instance;
        public static CampMapBlock Instance
        {
            get { if (instance == null) instance = new CampMapBlock(); return instance; }
        }

        public MapBlockDefine BlockDefine;

        private class CampBlock
        {
            private static CampBlock nullBlock;
            public static CampBlock NullBlock
            {
                get
                {
                    if (nullBlock == null) nullBlock = new CampBlock() { x = -1, y = -1 };
                    return nullBlock;
                }
            }

            public int serverIndex;
            public int colorIndex;
            public int x, y, index;
            public Camp camp = CampUtil.NullCamp;

            public CampBlock[] aroundBlocks;
            private Color data_Color = Color.clear;
            private Color data_Color2 = Color.clear;
            private Color data_Edge = Color.clear;
            private Color data_Angle = Color.clear;


            public void LogicData()
            {
                if (this == NullBlock) return;

                if (camp.campId > 0 && camp.campId < 3)
                {
                    LogicData_Color();
                    LogicData_Edge(Index_L, 0);
                    LogicData_Edge(Index_R, 1);
                    LogicData_Edge(Index_B, 2);
                    LogicData_Edge(Index_T, 3);
                    LogicData_Angle(Index_L, Index_B, Index_LB, 0);
                    LogicData_Angle(Index_R, Index_B, Index_RB, 1);
                    LogicData_Angle(Index_L, Index_T, Index_LT, 2);
                    LogicData_Angle(Index_R, Index_T, Index_RT, 3);

                }
                else 
                {
                    data_Color = Color.clear;
                    data_Color2 = Color.clear;
                    data_Edge = Color.clear;
                    data_Angle = Color.clear;
                }

                Colors_Color[index] = data_Color;
                Colors_Color2[index] = data_Color2;
                Colors_Edge[index] = data_Edge;
                Colors_Angle[index] = data_Angle;


            }

            public void LogicData_Color()
            {
                //区块颜色
                data_Color = camp.campId == 1 ? CampMapBlock.Instance.BlockDefine.Owner : CampMapBlock.Instance.BlockDefine.Enermy;
                //区块边缘颜色
                data_Color2 = camp.campId== 1 ? CampMapBlock.Instance.BlockDefine.OwnerEdge : CampMapBlock.Instance.BlockDefine.EnermyEdge ;
            }
            public void LogicData_Edge(int index ,int colorIndex)
            {
                Camp camp_a = aroundBlocks[index].camp;
                data_Edge[colorIndex] = CampUtil.Equal(camp, camp_a) ? 0 : 1;
               
            }
            public void LogicData_Angle(int index_A, int index_B, int index_AB, int colorIndex)
            {
                Camp camp_a = aroundBlocks[index_A].camp;
                Camp camp_b = aroundBlocks[index_B].camp;
                Camp camp_ab = aroundBlocks[index_AB].camp;

                data_Angle[colorIndex] = !CampUtil.Equal(camp, camp_ab) && CampUtil.Equal(camp, camp_a) && CampUtil.Equal(camp, camp_b) ? 1 : 0;
            }

            public void LogicData_AroundBlock_L()
            {
                aroundBlocks[Index_L].LogicData();
                aroundBlocks[Index_LB].LogicData();
                aroundBlocks[Index_LT].LogicData();
            }
            public void LogicData_AroundBlock_R()
            {
                aroundBlocks[Index_R].LogicData();
                aroundBlocks[Index_RB].LogicData();
                aroundBlocks[Index_RT].LogicData();
            }




        }



        public static ColorData[][] ColorCofigs;

        public static Texture2D Tex_Color;
        public static Texture2D Tex_Color2;
        public static Texture2D Tex_Edge;
        public static Texture2D Tex_Angle;



        private static Color[] Colors_Color;
        private static Color[] Colors_Color2;
        private static Color[] Colors_Edge;
        private static Color[] Colors_Angle;

        private CampBlock[,] m_map;
        private int m_width, m_height;


        public void Init(int serverNum)
        {
            BlockDefine = Resources.Load<MapBlockDefine>("BlackGroundBlock");
            m_width = serverNum * blockSizeX;
            m_height = blockSizeY;

            m_map = new CampBlock[m_width, m_height];
            for (int j = 0; j < m_height; j++)
            {
                for (int i = 0; i < m_width; i++)
                {
                    m_map[i, j] = new CampBlock()
                    {
                        serverIndex = i / blockSizeX,
                        colorIndex = (i / blockSizeX) % 2,
                        x = i,
                        y = j,
                        index = j * m_width + i

                    };
                }
            }

            for (int j = 0; j < m_height; j++)
            {
                for (int i = 0; i < m_width; i++)
                {
                    m_map[i, j].aroundBlocks = GetAroundBlockXY(i, j);
                }
            }

            Tex_Color = GenTex(m_width, m_height);
            Tex_Color2 = GenTex(m_width, m_height);
            Tex_Edge = GenTex(m_width, m_height);
            Tex_Angle = GenTex(m_width, m_height);

            int size = m_width * m_height;
            Colors_Color = new Color[size];
            Colors_Color2 = new Color[size];
            Colors_Edge = new Color[size];
            Colors_Angle = new Color[size];
        }
        public void UpdateDatas(int serverIndex)
        {

           //先给所有的区块设置camp值，然后比较，防止未被赋值的区块= -1 ，最后不相等
            for (int j = 0; j < blockSizeY; j++)
            {
                for (int i = 0; i < blockSizeX; i++)
                {
                    int x = (serverIndex-1) * blockSizeX + i;
                    int y = j;
                    Camp c = new Camp() { campId = BlockDefine.test[i, j] };
          
                    m_map[x,y].camp = c;
                    
                }
            }

            for (int j = 0; j < blockSizeY; j++)
            {
                for (int i = 0; i < blockSizeX; i++)
                {
                    int x = (serverIndex - 1) * blockSizeX + i;
                    int y = j;
                 

                    m_map[x, y].LogicData();
                    if (i == 0)
                        m_map[x, y].LogicData_AroundBlock_L();
                    else if (i == blockSizeX - 1)
                        m_map[x, y].LogicData_AroundBlock_R();
                }
            }

            ApplyTex();
        }




        private CampBlock[] GetAroundBlockXY(int x, int y)
        {
            CampBlock[] aroundBlock = new CampBlock[8];
            aroundBlock[Index_L] = GetBlockXY(x - 1, y);
            aroundBlock[Index_R] = GetBlockXY(x + 1, y);
            aroundBlock[Index_B] = GetBlockXY(x, y - 1);
            aroundBlock[Index_T] = GetBlockXY(x, y + 1);
            aroundBlock[Index_LB] = GetBlockXY(x - 1, y - 1);
            aroundBlock[Index_RB] = GetBlockXY(x + 1, y - 1);
            aroundBlock[Index_LT] = GetBlockXY(x - 1, y + 1);
            aroundBlock[Index_RT] = GetBlockXY(x + 1, y + 1);
            return aroundBlock;

        }
        private CampBlock GetBlockXY(int x, int y)
        {
            if (y < 0 || y >= m_height) return CampBlock.NullBlock;

            if (x == -1)
            {
                x = m_width - 1;
            }
            else if (x == m_width) x = 0;

            return m_map[x, y];
        }

        private Texture2D GenTex(int width, int heigh)
        {
            Texture2D tex = new Texture2D(width, heigh, TextureFormat.RGBA32, false, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Point;
            return tex;

        }

        private void ApplyTex()
        {
            Tex_Color.SetPixels(Colors_Color);
            Tex_Color2.SetPixels(Colors_Color2);
            Tex_Edge.SetPixels(Colors_Edge);
            Tex_Angle.SetPixels(Colors_Angle);

            Tex_Color.Apply();
            Tex_Color2.Apply();
            Tex_Edge.Apply();
            Tex_Angle.Apply();
        }
    }
}
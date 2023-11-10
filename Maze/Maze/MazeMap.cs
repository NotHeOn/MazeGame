using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    //迷宫地图类 用于生成数字地图
    class MazeMap
    {
        int MapWidth=12;  //迷宫地图宽度
        int MapHeight=9;  //迷宫地图长度
        Random rand = new Random(); //随机数
        int[,] NumMap;  //数字迷宫
        int[,] NumPath;  //迷宫的数字解
        int[,] Path;

        //初始化地图，将所有位置赋值为-1（-1表示未遍历过）
        //初始化数字解，将所有位置赋值为0（0表示该位点不是正确的路径点）
        public MazeMap(int width,int height)
        {
            MapWidth = width;
            MapHeight = height;
            NumMap = new int[MapHeight, MapWidth];
            
            for (int x = 0; x < MapHeight * MapWidth; x++)
            {
                NumMap[(int)(x / MapWidth),(int)( x % MapWidth)] = -1;
            }
            NumPath = new int[MapHeight, MapWidth];
            
            for (int x = 0; x < MapHeight * MapWidth; x++)
            {
                NumPath[(int)(x / MapWidth), (int)(x % MapWidth)] = 0;

            }

            Path = new int[MapHeight, MapWidth];
            for (int x = 0; x < MapHeight * MapWidth; x++)
            {
                Path[(int)(x / MapWidth), (int)(x % MapWidth)] = 0;
            }
        }



        //获取有起点和终点的数字地图
        public int[,] GetNumMap()
        {
            CreateNumMap();
            //创建起点和终点
            NumMap[0, 0] = NumMap[0, 0] ^ 8;//打开上面，作为入口
            NumMap[MapHeight - 1, MapWidth - 1] = NumMap[MapHeight - 1, MapWidth - 1] ^ 2;//打开下面，作为出口
            return NumMap;
        }

        //获取数字解
        public int[,] GetNumPath()
        {
            FindNumPath();
            return NumPath;
        }

        //创建数字地图
        private void CreateNumMap(int RowPos=0,int ColPos=0,int PosState=0)
        {
            List<int> Directions = new List<int> { 0, 1, 2, 3 }; //储存方向，0-上；1-右；2-下；3-左
            int State = 0; //假设每个点位初始状态均为4面封闭
            switch(PosState) //根据上一个点位的状态更新当前点位的状态
            {
                case 1: State = 4;break;
                case 2: State = 8;break;
                case 4: State = 1;break;
                case 8: State = 2;break;
            }
            NumMap[RowPos, ColPos] = State;
            while(Directions.Count>0) //随机选择一个方向前进，并产生一个附加状态
            {
                int X = RowPos;
                int Y = ColPos;
                int DirectNum = rand.Next(Directions.Count);
                int AddState = 0;
                switch(Directions[DirectNum])
                {
                    case 0:
                        X--;AddState = 8;break;
                    case 1:
                        Y++;AddState = 4;break;
                    case 2:
                        X++;AddState = 2;break;
                    case 3:
                        Y--;AddState = 1;break;

                }
                Directions.RemoveAt(DirectNum); //抛弃已用方向
                if(X<MapHeight && Y<MapWidth && X>=0 && Y>=0 && NumMap[X,Y] == -1) //判断遍历是否合法，合法则更新当前点位状态
                {
                    State = State ^ AddState;
                    NumMap[RowPos, ColPos] = State;
                    CreateNumMap(X, Y, AddState);
                }
            }




        }

        //寻找数字解
        private void FindNumPath(int X=0,int Y=0)
        {       
            if (X == MapHeight - 1 && Y == MapWidth - 1)
            {
                for (int x = 0; x < MapHeight * MapWidth; x++)
                {
                    if(Path[(int)(x / MapWidth), (int)(x % MapWidth)] == 1)
                    {
                        NumPath[(int)(x / MapWidth), (int)(x % MapWidth)] = 1;
                    }
                }
                NumPath[MapHeight - 1, MapWidth - 1] = 1;
            }
            else
            {
                for (int i = 0; i <= 3; i++)
                {
                    int _X = X;
                    int _Y = Y;
                    int StepState = 0;
                    switch (i)
                    {
                        case 0:
                            _X--;StepState = 8;break;
                        case 1:
                            _Y++;StepState = 4;break;
                        case 2:
                            _X++;StepState = 2;break;
                        case 3:
                            _Y--;StepState = 1;break;
                    }
                    if (_X < MapHeight && _Y < MapWidth && _X >= 0 && _Y >= 0 && (NumMap[X, Y] & StepState) != 0 && Path[_X, _Y] == 0)
                    {
                        Path[X, Y] = 1;
                        FindNumPath(_X, _Y);
                        Path[X, Y] = 0;
                    }
                   
                }
            }
        }



    }
}

原理：[AStar寻路](https://github.com/shishouheng/Unity-learning/blob/main/note/AStar%E5%AF%BB%E8%B7%AF.pdf)

代码：
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GridType { Normal, Start, End, Obstacel }


public class MyGrid : MonoBehaviour,IComparable<MyGrid>
{
    private GridType gridType;
    public GridType MyGridType
    {
        get { return gridType; }
        set
        {
            gridType = value;
            Color tempColor = Color.black;
            switch(gridType)
            {
                case GridType.Start:
                    tempColor = Color.green;
                    break;
                case GridType.End:
                    tempColor = Color.red;
                    break;
                case GridType.Obstacel:
                    tempColor = Color.blue;
                    break;
            }
            SetColor(tempColor);
        }
    }
    public int x, y;//一维坐标
    public int F, G, H;
    public MyGrid parent;//父方格


    private MeshRenderer mr;
    private void Awake()
    {
        mr=GetComponent<MeshRenderer>();
    }
    public void SetColor(Color c)
    {
        mr.material.color = c;
    }

    public int CompareTo(MyGrid other)
    {
        if (F < other.F)
            return -1;
        else if (F > other.F)
            return 1;
        return 0;
    }
}
```

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AStarManager : MonoBehaviour
{
    //存储所有格子的数组
    MyGrid[,] allGrids;
    //起点终点位置信息
    public int startPosX, startPosY;
    public int endPosX, endPosY;

    //障碍物比率
    [Range(0, 100)]
    public int obstacleRate;

    public GameObject girdPrefab;

    private List<MyGrid> openList;
    private List<MyGrid> closeList;

    //结果，最终路径经过的格子
    private Stack<MyGrid> result;
    private void Awake()
    {
        openList = new List<MyGrid>();
        closeList = new List<MyGrid>();
        result = new Stack<MyGrid>();
        allGrids = new MyGrid[20, 20];
    }

    private void Start()
    {
        OnStart();
    }
    void OnStart()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                MyGrid currentGrid = Instantiate(girdPrefab).GetComponent<MyGrid>();
                Vector2 offset = new Vector2(-5, -5);
                currentGrid.transform.position = new Vector3(offset.x + i * 0.5f, 0, offset.y + j * 0.5f);

                currentGrid.x = i;
                currentGrid.y = j;

                allGrids[i, j] = currentGrid;
                int r = Random.Range(1, 101);
                if (r <= obstacleRate)
                    currentGrid.MyGridType = GridType.Obstacel;
            }
        }
        allGrids[startPosX, startPosY].MyGridType = GridType.Start;
        allGrids[endPosX, endPosY].MyGridType = GridType.End;

        AStarCount();
    }
    void AStarCount()
    {
        //先将起点放入开启列表中
        openList.Add(allGrids[startPosX, startPosY]);
        //拿到中心格子
        MyGrid currentGrid = openList[0];
        //遍历与currentGrid相邻的格子
        while (openList.Count > 0 && currentGrid.MyGridType != GridType.End)
        {
            //对这些格子进行排序并获取F值最低的格子（最靠谱的格子）
            openList.Sort();
            //将最靠谱的格子更新为中心并遍历周围的格子
            //经过排序后最靠谱的格则就是第一个格子
            currentGrid = openList[0];

            if (currentGrid.MyGridType == GridType.End)
            {
                //结束并生成最终行走路径
                GetParent(currentGrid);
                return;
            }

            //不是终点：  遍历周围的格子
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        //确定这些相邻格子的下标
                        int x = currentGrid.x + i;
                        int y = currentGrid.y + j;
                        //如果不是障碍物也不在关闭列表中则计算G和H
                        if (allGrids[x, y].MyGridType != GridType.Obstacel && !closeList.Contains(allGrids[x, y]))
                        {
                            //计算G和H（即靠谱程度）
                            //确保这些格子是在范围内的
                            if (x > 0 && y > 0 && x < allGrids.GetLength(0) && y < allGrids.GetLength(1))
                            {
                                //新的G值
                                int g = (int)(currentGrid.G + Mathf.Sqrt(Mathf.Abs(i) + Mathf.Abs(j)) * 10);
                                //当前相邻的格子是否被检查过
                                if (allGrids[x, y].G == 0 || g < allGrids[x, y].G)
                                {
                                    //更新G值
                                    allGrids[x, y].G = g;
                                    allGrids[x, y].parent = currentGrid;
                                }
                                //h计算
                                allGrids[x, y].H = (Mathf.Abs(x - endPosX) + Mathf.Abs(y - endPosY)) * 10;
                                //f计算
                                allGrids[x, y].F = allGrids[x, y].G + allGrids[x, y].H;
                                //将这些相邻的格子放入开启列表中等待检查
                                if (!openList.Contains(allGrids[x, y]))
                                    openList.Add(allGrids[x, y]);
                            }
                        }
                    }
                }
            }

            //从开启列表移除并加入关闭列表中
            openList.Remove(currentGrid);
            closeList.Add(currentGrid);
            if (openList.Count == 0)
                Debug.Log("Cannot Arrave!!!");
        }
    }
    void GetParent(MyGrid current)
    {
        result.Push(current);
        if (current.parent != null)
            GetParent(current.parent);
        else
            StartCoroutine(ShowResult());
    }
    IEnumerator ShowResult()
    {
        int resultCount = result.Count;
        while(result.Count>0)
        {
            yield return new WaitForSeconds(0.1f);
            //获取栈顶元素
            MyGrid currenResultGrid = result.Pop();//获取+移除
            Color currentColor = Color.Lerp(Color.green, Color.red, (resultCount - result.Count) / (float)resultCount);
            currenResultGrid.SetColor(currentColor);   
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("AStarTest");
    }
}
```
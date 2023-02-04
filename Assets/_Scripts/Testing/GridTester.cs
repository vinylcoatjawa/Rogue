using UnityEngine;
using UnityEngine.UIElements;

public class GridTester : MonoBehaviour
{

    int _width = 25;
    int _height = 25;

    Grid<int> gridArray;


    void Start()
    {


    }
    public void DrawGrid()
    {
        //gridArray = new Grid<int>(_width, _height, 5, new Vector3(-500,0,-500), () => 0, true);
    }

    public void SetValue()
    {
        gridArray.SetGridObject(2, 2, 5);
    }

    public void GetPMap()
    {
        PerlinMap.GenerateNoiseMap(_width, _height, 10, 5f, 3, 0.5f, 0.5f, 123, new Vector2(2, 5), true);
        Debug.Log("PM");


    }

}

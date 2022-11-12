using UnityEngine;

public class GridTester : MonoBehaviour
{

    int _width = 6;
    int _height = 6;

    Grid<int> gridArray;


    void Start()
    {


    }
    public void DrawGrid()
    {
        gridArray = new Grid<int>(_width, _height, 10f, Vector3.zero, () => 0);
    }

    public void SetValue()
    {
        gridArray.SetGridObject(2, 2, 5);
    }


}

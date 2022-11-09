using UnityEngine;
using Grid = Mapgrid.Grid;

public class GridTester : MonoBehaviour
{

    int width = 6;
    int height = 6;

    Grid gridArray = new(6, 6, 10f, Vector3.zero, );
    TextMesh[,] debugTestArray;

    void Start()
    {


    }

    public void printCoords()
    {
        GameObject debugGrid = new GameObject("debug grid");
        debugTestArray = new TextMesh[width, height];
        for (int x = 0; x < gridArray.GetWitdth(); x++)
        {
            for (int y = 0; y < gridArray.GetHeight(); y++)
            {
                Debug.Log(debugTestArray);
                Debug.Log(gridArray);

                debugTestArray[x, y] = CreateWorldText("a", null, gridArray.GetWorldPosition(x, y) + new Vector3(gridArray.GetCellsize(), gridArray.GetCellsize()) * 0.5f, 20, Color.black, TextAnchor.MiddleCenter);
                debugTestArray[x, y].gameObject.transform.SetParent(debugGrid.transform);

                Debug.DrawLine(gridArray.GetWorldPosition(x, y), gridArray.GetWorldPosition(x, y + 1), Color.black, 100f);
                Debug.DrawLine(gridArray.GetWorldPosition(x, y), gridArray.GetWorldPosition(x + 1, y), Color.black, 100f);


            }
        }
        Debug.DrawLine(gridArray.GetWorldPosition(0, height), gridArray.GetWorldPosition(width, height), Color.black, 100f);
        Debug.DrawLine(gridArray.GetWorldPosition(width, 0), gridArray.GetWorldPosition(width, height), Color.black, 100f);
    }


    private void SetDebugText(int x, int y ,TextMesh value)
    {
        debugTestArray[x, y] = value;
    }

    #region UTILS


    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment);
    }
    #endregion
}

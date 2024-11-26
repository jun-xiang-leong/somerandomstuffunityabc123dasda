using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TileLoader: MonoBehaviour
{
    public Vector2 longlat = new Vector2(103.8284f, 1.3752f);
    public List<GameObject> mapDisplays = new List<GameObject>(9);
    public float speed = 0.0001f;

    public int gridPixelSize = 256;
    public GameObject go;

    int zoom_level = 13;
    double longLatIncrement = 360;
    Vector2Int currentGrid = new Vector2Int(0,0);

    float curr_X_offset = 0.0f;
    float curr_Y_offset = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //longLatIncrement = 360.0 / Math.Pow(2.0, 13.0);    
        //currentGrid = LatLonToTile(longlat.x, longlat.y,zoom_level);
    }

    // Update is called once per frame
    void Update()
    {
        RecalculateGrid();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            longlat.y += speed * Time.deltaTime;
            Debug.Log("up");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            longlat.y -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            longlat.x += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            longlat.x -= speed * Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            if(zoom_level == 13)
            {
                zoom_level = 12;
            }
            else if(zoom_level == 12)
            {
                zoom_level = 13;
            }
            RecalculateGrid();
            Debug.Log(zoom_level);
        }

        go.transform.localPosition = new Vector2((curr_X_offset - 0.5f)  * gridPixelSize  , (0.5f-curr_Y_offset)  * gridPixelSize );
    }
    void RecalculateGrid()
    {
        Vector2Int currGrid = LatLonToTile(longlat.y, longlat.x, zoom_level);

        if (currGrid != currentGrid)
        {
            currentGrid = currGrid;
            //upwards + lat 
            //left - long
            mapDisplays[0].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x - 1, currGrid.y + 1);
            mapDisplays[1].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x, currGrid.y + 1);
            mapDisplays[2].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x + 1, currGrid.y + 1);

            mapDisplays[3].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x - 1, currGrid.y);
            mapDisplays[4].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x, currGrid.y);
            mapDisplays[5].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x + 1, currGrid.y);

            mapDisplays[6].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x - 1, currGrid.y - 1);
            mapDisplays[7].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x, currGrid.y - 1);
            mapDisplays[8].GetComponent<SpriteRenderer>().sprite = LoadTile(zoom_level, currGrid.x + 1, currGrid.y - 1);

        }
    }
    public Vector2Int LatLonToTile(double lat , double lon, int zoom)
    {
        float X_offset = (float)((lon + 180) / 360 * Math.Pow(2, zoom));
        float Y_offset = (float)((1 - Math.Log(Math.Tan(lat * Math.PI / 180) + 1 / Math.Cos(lat * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, zoom));
        int tileX = ((int)Math.Floor(X_offset));
        int tileY = (int)(Math.Floor(Y_offset));
        
        curr_X_offset = X_offset - tileX;
        curr_Y_offset = Y_offset - tileY;


        return new Vector2Int(tileX, tileY); 
    }

    public Sprite LoadTile(int zoom,  int tileX, int tileY)
    {
        string path =  $"{zoom}/{tileX}/{tileY}";

        Sprite s = Resources.Load<Sprite>(path);
        if(s == null)
            Debug.Log(path);
        return s;    
    }
}

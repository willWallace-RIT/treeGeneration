using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgs : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject skyPrefab;
    public GameObject groundPrefab;
    public Vector2 min;
    public Vector2 max;
    public int layerCount;
    public List<BoxCollider2D> boxes; 
    // Start is called before the first frame update
    void Start()
    {
        boxes.Add(GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize;
        float width = (height * cam.aspect);
        List<Vector2> dims = GetDim();
        if (dims[1].y < height || dims[0].y > -height || dims[1].x < width || dims[0].x > -width)
        {
            List<BoxCollider2D> corners = new List<BoxCollider2D>();
            List<Vector2> dirs = new List<Vector2>() { new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
            Vector2 extents = (dims[1] - dims[0])*0.5f;
            for (int i = 0; i < dirs.Count; i++)
            {

                float x = ((dirs[i].x) == -1) ? dims[0].x : dims[1].x;
                GameObject gO;
                if (dirs[i].y == -1)
                {
                    float y = dims[0].y;
                    gO = Instantiate(groundPrefab,new Vector2(0.0f,0.0f), Quaternion.identity);
                    gO.transform.position = (new Vector2(x, y)) + (gO.GetComponent<BoxCollider2D>().bounds.extents * dirs[i]);
                    gO.transform.name = "c" + i;
                }
                else
                {
                    float y = dims[1].y;
                    gO = Instantiate(skyPrefab, new Vector2(0.0f,0.0f), Quaternion.identity);
                    gO.transform.position = (new Vector2(x, y)) + (gO.GetComponent<BoxCollider2D>().bounds.extents * dirs[i]);
                    gO.transform.name = "c" + i;

                }
                corners.Add(gO.GetComponent<BoxCollider2D>());
            }
            float vertfloat = corners[1].bounds.min.y - corners[2].bounds.max.y;
            float horizfloat = corners[1].bounds.min.x - corners[0].bounds.max.x;
            int vertCount = Mathf.Abs(Mathf.RoundToInt(((Mathf.Abs(vertfloat) - (boxes[0].size.y)) / corners[0].GetComponent<BoxCollider2D>().bounds.size.y)))+1;
            int horizCount = Mathf.Abs(Mathf.RoundToInt((Mathf.Abs(horizfloat)) / corners[0].GetComponent<BoxCollider2D>().bounds.size.x));
              List<BoxCollider2D> edges = new List<BoxCollider2D>();
            for (int i = 0; i < horizCount; i++)
            {
                GameObject gO = Instantiate(skyPrefab, new Vector2(corners[0].transform.position.x, corners[0].transform.position.y) + (new Vector2(corners[0].bounds.size.x, 0.0f) * (i + 1)), Quaternion.identity);
                edges.Add(gO.GetComponent<BoxCollider2D>());
            }
            for (int i = 0; i < horizCount; i++)
            {
                GameObject gO = Instantiate(groundPrefab, new Vector2(corners[3].transform.position.x, corners[3].transform.position.y) + (new Vector2(corners[0].GetComponent<BoxCollider2D>().bounds.size.x, 0.0f) * (i + 1)), Quaternion.identity);
                edges.Add(gO.GetComponent<BoxCollider2D>());

            }
            int midCount = (int)((float)vertCount / 2.0f);
            for (int i = 0; i < vertCount; i++)
            {
                Vector2 midResize = (i+1 >= midCount) ? -new Vector2(0.0f, corners[0].GetComponent<BoxCollider2D>().bounds.size.y)*0.5f + new Vector2(0.0f, boxes[0].size.y)*0.5f : new Vector2(0.0f,0.0f);
                
                GameObject select = (i == midCount) ? linePrefab : (i < midCount) ? groundPrefab : skyPrefab;
                GameObject gO = Instantiate(select, new Vector2(corners[3].transform.position.x, corners[3].transform.position.y) + (new Vector2(0.0f, corners[0].GetComponent<BoxCollider2D>().bounds.size.y) * (i + 1))+midResize, Quaternion.identity);
                edges.Add(gO.GetComponent<BoxCollider2D>());
            }
            for (int i = 0; i < vertCount; i++)
            {
                Vector2 midResize = (i+1 >= midCount) ? -new Vector2(0.0f, corners[0].GetComponent<BoxCollider2D>().bounds.size.y) * 0.5f + new Vector2(0.0f, boxes[0].size.y) * 0.5f : new Vector2(0.0f, 0.0f);
                GameObject select = (i == midCount) ? linePrefab : (i < midCount) ? groundPrefab : skyPrefab;
                GameObject gO = Instantiate(select, new Vector2(corners[2].transform.position.x, corners[2].transform.position.y) + (new Vector2(0.0f, corners[0].GetComponent<BoxCollider2D>().bounds.size.y) * (i + 1))+midResize, Quaternion.identity);
                edges.Add(gO.GetComponent<BoxCollider2D>());
            }
            boxes.AddRange(corners.ToArray());
            boxes.AddRange(edges.ToArray());  
        }
    }
    List<Vector2> GetDim()
    {
        min = new Vector2(boxes[0].bounds.min.x, boxes[0].bounds.min.y);
        max = new Vector2(boxes[0].bounds.max.x, boxes[0].bounds.max.y);
        for(int i = 0; i < boxes.Count; i++)
        {
            if (min.x > boxes[i].bounds.min.x) min.x = boxes[i].bounds.min.x;
            if (min.y > boxes[i].bounds.min.y) min.y = boxes[i].bounds.min.y;
            if (max.x < boxes[i].bounds.max.x) max.x = boxes[i].bounds.max.x;
            if (max.y < boxes[i].bounds.max.y) max.y = boxes[i].bounds.max.y;
        }
        return new List<Vector2> { min, max };
    }
}

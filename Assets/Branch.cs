using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch: MonoBehaviour
{
    public float t;
    public float startThickness;
    public float maxThickness;
    public float thick = 0.0f;
   
    public float growthRate;
    public float branchLength;
    public float branchStartLength;
    public float branchThreshLength;
    public float branchMaxLength;
    public Vector2 branchVector;
    public Vector2 normalizedLocation;
    public float locationT;
    public float startLocationT;
    public float maxLocationT;
    public bool isGrowing = false;
    public float desiredT = 0;
    public GameObject startCircle;
    public GameObject endCircle;
    public GameObject box;
    public float prevDesiredT=0.0f;
    public bool maxHit = false;
    public SpriteRenderer eSR;
    // Start is called before the first frame update
    void Start()
    {
        //initialize(0, 0, 0, 0, 0, 0, 0, new Vector2(1, 0), new Vector2(0, 0));
    }

    public void initialize(float startThick,float maxThick,float branchStartLength,float branchThreshLength,float branchMaxLength,float locationT,float maxLocationT,Vector2 normalizedLocation,Vector2 branchVector)
    {
        startThickness = startThick;
        maxThickness = maxThick;
        branchLength = branchStartLength;
        this.branchStartLength = branchStartLength;
        this.branchThreshLength = branchThreshLength;
        this.branchMaxLength = branchMaxLength;
        this.locationT = locationT;
        this.maxLocationT = maxLocationT;
        this.startLocationT = locationT;
        this.normalizedLocation = normalizedLocation;
        this.branchVector = branchVector;
        t = 0;
        thick = startThickness;
        startCircle = transform.Find("startCircle").gameObject;
        endCircle = transform.Find("endCircle").gameObject;
        eSR = endCircle.GetComponent<SpriteRenderer>();
        box = transform.Find("box").gameObject;
        startCircle.transform.localPosition = new Vector2(0, 0);
        startCircle.transform.localScale = new Vector2(startThick, startThick);
        endCircle.transform.localScale = new Vector2(startThick, startThick);
        endCircle.transform.localPosition = branchVector * branchStartLength;
        float dist = endCircle.transform.localPosition.magnitude;
        box.transform.localPosition = branchVector * branchStartLength * 0.5f;
        box.transform.localScale = new Vector2(dist, dist);
        box.transform.localRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(branchVector.x, branchVector.y, 0.0f));
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrowing)
        {
           t+= Time.deltaTime;
            float thickness = Mathf.Min(Mathf.Lerp(startThickness, maxThickness, t), maxThickness);
            float length = Mathf.Min(Mathf.Lerp(branchStartLength, branchMaxLength, t), branchMaxLength);
            branchLength = length;
            //transform.localPosition = normalizedLocation * Mathf.Min(Mathf.Lerp(startLocationT, maxLocationT, t));
            thick = thickness;
            startCircle.transform.localScale = new Vector2(thickness, thickness);
            endCircle.transform.localScale = new Vector2(thickness, thickness);
            endCircle.transform.localPosition = branchVector * length;
            box.transform.localPosition = branchVector * length * 0.5f;
            float dist = endCircle.transform.localPosition.magnitude;
            box.transform.localScale = new Vector2(thickness, dist);
            box.transform.localRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(branchVector.x, branchVector.y, 0.0f));
            if (t > desiredT)
            {
                isGrowing = false;
            }

        }
        
    }
    public void SetBranchCols(Color color1)
    {
        endCircle.GetComponent<SpriteRenderer>().color = color1;
        startCircle.GetComponent<SpriteRenderer>().color = color1;
        box.GetComponent<SpriteRenderer>().color = color1;
    }
    public void Grow(float inc)
    {
        prevDesiredT = desiredT;
        desiredT += inc;
        isGrowing = true;
    }
    public List<Vector2> getDim(){
      Vector2 min = new Vector2(startCircle.transform.position.x,startCircle.transform.position.y);
      Vector2 max = new Vector2(startCircle.transform.position.x,startCircle.transform.position.y);
      if(min.x>endCircle.transform.position.x)min.x=endCircle.transform.position.x;
      else{
        max.x =endCircle.transform.position.x;
      }

      if(min.y>endCircle.transform.position.y)min.y=endCircle.transform.position.y;
      else{
        max.y =endCircle.transform.position.y;
      }
        float thickn = startCircle.GetComponent<SpriteRenderer>().bounds.extents.x;
      min.x-=thickn;
      min.y-=thickn;
      max.x+=thickn;
      max.y+=thickn;
      return new List<Vector2>(){min,max};
    } 
        
  
}

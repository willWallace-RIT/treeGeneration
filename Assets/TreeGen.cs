using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen : MonoBehaviour
{
    public GameObject BranchPrefab;
    public Node<Branch> rootTree;

    public float startMaxLength = 5;
    public float startMaxThickness = 1;
    public float startLengthThresh = 1;
    public float inc = 0.1f;
    const float conv = Mathf.PI/180.0f;
    const float spawnChance = 0.1f;
    const float addChance = 0.1f;
    const float branchAngle = 45.0f;
    const float endBranchAngle= 15.0f;
    float t=0.0f; 
    // Start is called before the first frame update
    void Start()
    {
        GameObject branchGO = Instantiate(BranchPrefab, new Vector2(0, 0), Quaternion.identity);
        Branch branch = branchGO.GetComponent<Branch>();
        branch.initialize(0, startMaxThickness, 0, startLengthThresh, startMaxLength, 0, 0, new Vector2(0, 0), new Vector2(0, 1));
        rootTree = new Node<Branch>(branch);
    }

    void Grow()
    {
        rootTree.rRun((Dictionary<string, object> p, Node<Branch> node) => {
            float inc = (float)p["inc"];
            node.Get().Grow(inc);
            return new Dictionary<string, object>(); 
        }, new Dictionary<string, object>() { { "inc", inc } });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Grow();
        }  
        rootTree.rRun((Dictionary<string,object> p,Node<Branch> node)=>{

            Debug.Log("here");
            if(node.Get().isGrowing && node.Get().branchLength>node.Get().branchThreshLength){
              Vector2 location = node.Get().endCircle.transform.position;
              float chance = Random.Range(0.0f,1.0f);
              int parentCount = node.getParentCount(0);
              t+=Time.deltaTime;
              Debug.Log("here");
              if(t>.001f&&chance>0.2f&&node.GetChildren().Count <3  && parentCount<=5 ){
                t=0.0f;
                float angle = Random.Range(-branchAngle,branchAngle);
                GameObject branchGO = Instantiate(BranchPrefab,location,Quaternion.identity);
                Vector2 locVector = node.Get().endCircle.transform.localPosition-node.Get().startCircle.transform.localPosition;
                Branch branch =branchGO.GetComponent<Branch>();
                branchGO.transform.parent=node.Get().gameObject.transform;
                branchGO.transform.localRotation=Quaternion.AngleAxis(angle,Vector3.forward);
                branch.initialize(0.0f,startMaxThickness*Mathf.Pow(0.95f,(float)parentCount-1.0f),0.0f,startLengthThresh*Mathf.Pow(0.7f,(float)parentCount-1.0f),startMaxLength*Mathf.Pow(0.95f,(float)parentCount-1.0f),locVector.magnitude,locVector.magnitude*2.0f,locVector.normalized,Vector2.up);
                branch.desiredT+=node.Get().desiredT-node.Get().t;
                branch.isGrowing=true;
                node.Add(branch);
                }else if(t>.001f){
                 t=0.0f;
                }else if(node.Get().branchLength>=node.Get().branchMaxLength &&parentCount < 7 && !node.Get().maxHit){
                   node.Get().maxHit=false;
                float angle = Random.Range(-endBranchAngle,endBranchAngle);
                GameObject branchGO = Instantiate(BranchPrefab,location,Quaternion.identity);
                Vector2 locVector = node.Get().endCircle.transform.localPosition-node.Get().startCircle.transform.localPosition;
                Branch branch =branchGO.GetComponent<Branch>();
                branchGO.transform.parent=node.Get().gameObject.transform;
                branchGO.transform.localRotation=Quaternion.AngleAxis(angle,Vector3.forward);
                branch.initialize(0.0f,startMaxThickness*Mathf.Pow(0.7f,(float)parentCount-1.0f),0.0f,startLengthThresh*Mathf.Pow(0.7f,(float)parentCount-1.0f),startMaxLength*Mathf.Pow(0.7f,(float)parentCount-1.0f),locVector.magnitude,locVector.magnitude*2.0f,locVector.normalized,Vector2.up);
                branch.desiredT+=node.Get().desiredT-node.Get().t;
                branch.isGrowing=true;
                node.Add(branch);
                 
                 }
              if(!node.Get().maxHit){
                node.Get().eSR.color = (Random.Range(0,2)==0)?Color.green:new Color(0.0f,100.0f,0.0f);
              }else{
                node.Get().eSR.color = Color.white;
              }


            }
            return new Dictionary<string, object>();

            },new Dictionary<string,object>());
        List<Node<Branch>> endNodes = rootTree.getEndNodes();
        endNodes.Add(rootTree);
        List<Vector2> dims = endNodes[0].Get().getDim();
        Vector2 min = dims[0];
        Vector2 max = dims[1];

        for(int i = 1;i<endNodes.Count;i++ ){
          dims = endNodes[i].Get().getDim();
          if(min.x>dims[0].x)min.x=dims[0].x;
          if(min.y>dims[0].y)min.y=dims[0].y;
          if(max.x<dims[1].x)max.x=dims[1].x;
          if(max.y<dims[1].y)max.y=dims[1].y;
        }
        Vector3 dim = (max-min);
        Debug.Log(dim.y+"|"+Camera.main.orthographicSize);
        if(dim.y>Camera.main.orthographicSize){
          Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,dim.y,Time.deltaTime*500);

        }
    }

}

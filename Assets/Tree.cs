using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject BranchPrefab;
    public Node<Branch> rootTree;

    public float startMaxLength = 5;
    public float startMaxThickness = 1;
    public float startLengthThresh = 3;
    public float inc = 0.25f;

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
    }

}

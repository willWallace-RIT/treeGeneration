using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node<T>
{
    private List<Node<T>> branches;
    private Node<T> parent;
    private int id=0;
    public delegate Dictionary<string, object> functionEval(Dictionary<string, object> args, Node<T> node);
    private static int idOffset = 0;
    private T content;
    public Node(T content){
        this.content = content;
        branches = new List<Node<T>>();
        id = idOffset;
        idOffset++;
    }
    public T Get(){
        return content;
    }
    public Node<T> getParent()
    {
        return parent;
    }
    public List<Node<T>> GetChildren(){
        return branches;
    }
    public void Add(T content)
    {
        branches.Add(new Node<T>(content));
        branches[branches.Count-1].parent = this;
    }
    public Dictionary<string, object> rRun(functionEval runFunc,Dictionary<string,object> args){
        Dictionary<string, object> returnVal = runFunc(args,this);
        for(int i =0;i<branches.Count;i++){
            Dictionary<string, object> rValbranches = branches[i].rRun(runFunc, args);
            foreach (var item in rValbranches)
            {
                returnVal.Add(id + "_" + item.Key, item.Value);
            }
        }
        return returnVal;
    }
    public bool IsEndNode()
    {
        if(branches.Count == 0)
        {
            return true;
        }
        return false;
    }
    public List<Node<T>> getAllNodes()
    {
        List<Node<T>> nodes = new List<Node<T>>();
        nodes.Add(this);

        for (int i = 0; i < branches.Count; i++)
        {
            List<Node<T>> branchNodes = branches[i].getAllNodes();
            for (int k = 0; k < branchNodes.Count; k++)
            {
                nodes.Add(branchNodes[k]);
            }
        }
        return nodes;
    }
    public List<Node<T>> getEndNodes()
    {
        List<Node<T>> nodes = new List<Node<T>>();
        if (IsEndNode())
        {
            nodes.Add(this);
            return nodes;
        }
        for(int i = 0; i < branches.Count; i++)
        {
            List<Node<T>> branchNodes = branches[i].getEndNodes();
            for(int k = 0; k < branchNodes.Count; k++)
            {
                nodes.Add(branchNodes[k]);
            }
        }
        return nodes;
    }
    public Node<T> getNode(int dId)
    {
        if (dId == id)
        {
            return this;
        }
        for(int i = 0;i<branches.Count;i++)
        {
            Node<T> branch = branches[i].getNode(dId);
            if(branch != null)
            {
                return branch;
            }  
        }
        return null;
    }
    public int getParentCount(int current)
    {
        if (parent != null)
        {
            return parent.getParentCount(current+1);
        }
        return current;
    }
}

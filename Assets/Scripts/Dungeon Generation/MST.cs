using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Geometry;

public static class MST
{
    static int vertices;

    public struct STEdge
    {
        public float weight;
        public Edge edge;
    }

    class Set
    {
        public int rank;
        public int parent;
    }

    static int Find(int vertex, List<Set> Sets)
    {
        if(Sets[vertex].parent != vertex)
        {
            Sets[vertex].parent = Find(Sets[vertex].parent, Sets);
        }
        return Sets[vertex].parent;
    }

    static void Union(int x, int y, List<Set> Sets)
    {
        if (Sets[x].rank < Sets[y].rank)
        {
            Sets[x].parent = y;
        }

        else if (Sets[x].rank > Sets[y].rank)
        {
            Sets[y].parent = x;
        }

        else
        {
            Sets[y].parent = x;
            Sets[x].rank++;
        }
    }

    static public List<STEdge> FormTree(TriangleNet.Mesh mesh, int v, bool AddExtraEdges = false)
    {
        //Initialized here because static
        List<STEdge> result = new List<STEdge>();
        List<STEdge> STEdges = SetWeights(mesh, v);
        List<Set> Sets = new List<Set>();

        //# of sets are equal to number of vertices in graph
        for (int i = 0; i < vertices; i++)
        {
            Set set = new Set();
            set.parent = i;
            set.rank = 0;
            Sets.Add(set);
        }

        int e = 0;
        int w = 0;
        int cycles = 0;

        //for MST edges = v - 1
        while (e < vertices - 1)
        {

            //Edge to evaluate
            STEdge currentEdge = STEdges[w];

            // Increment the index for next iteration  
            w++;
            //Finds the parent of each node
            int x = Find(currentEdge.edge.P0, Sets);
            int y = Find(currentEdge.edge.P1, Sets);

            //If parents are equal including causes cycles so nothing happpens
            if (x == y)
            {
                cycles++;
            }

            //If parents are not equal including does not cause a cycle
            if (x != y)
            {
                e++;
                result.Add(currentEdge);
                Union(x, y, Sets);
            }

        }

        //This section is still in development. Produces inconsistent results of good to terrible.
        //This is mainly used to prevent the spanning tree from becoming too linear
        if (AddExtraEdges == true)
        {
            float extra = cycles * 0.15f;
            if (extra <= 1) extra = 1;

            for (int z = 0; z < extra; z++)
            {
                STEdge currentEdge = STEdges[Random.Range(0, STEdges.Count)];
                for (int i = 0; i < STEdges.Count; i++)
                {
                    if (result[i].edge == currentEdge.edge)
                    {
                        currentEdge = STEdges[Random.Range(0, STEdges.Count)];
                        continue;
                    }
                    else
                    {
                        result.Add(currentEdge);
                        break;
                    }
                }
            }

            
        }


        Debug.Log("Spanning Tree Complete!");
        

        for (int i = 0; i < result.Count; i++)
        {
            int x = result[i].edge.P0;
            int y = result[i].edge.P1;

            //Debug.Log(x + " " + y);
        }
        return result;
    }

    //Sorts edges by adding weight to them
    static List<STEdge> SetWeights(TriangleNet.Mesh mesh, int v)
    {
        List<STEdge> STEdges = new List<STEdge>();

        foreach (var edge in mesh.Edges)
        {
            Vertex v0 = mesh.vertices[edge.P0];
            Vertex v1 = mesh.vertices[edge.P1];

            //Weight is the distance between the 2 points of an edge
            float weight = Vector3.Distance(new Vector3( (float)v0.x, (float)v0.y, 0), new Vector3((float)v1.x, (float)v1.y, 0));

            STEdge stEdge = new STEdge();
            stEdge.weight = weight;
            stEdge.edge = edge;

            STEdges.Add(stEdge);
        }

        //Ascending order
        STEdges.Sort((a, b) => a.weight.CompareTo(b.weight));

        vertices = v;

        return STEdges;
    }
}

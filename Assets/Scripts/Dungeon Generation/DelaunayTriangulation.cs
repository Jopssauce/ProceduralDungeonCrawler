using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Geometry;

public static class DelaunayTriangulation 
{
    public static TriangleNet.Mesh TriangulatePoints(List<Vector3> centerTiles)
    {
        TriangleNet.Mesh mesh;
        Polygon polygon = new Polygon();

        for (int i = 0; i < centerTiles.Count; i++)
        {
            polygon.Add(new Vertex(centerTiles[i].x, centerTiles[i].y));
        }

        TriangleNet.Meshing.ConstraintOptions options = new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = false };
        mesh = (TriangleNet.Mesh)polygon.Triangulate(options);

        Debug.Log(centerTiles.Count + " Points Triangulated and " + mesh.NumberOfEdges + " Edges Created");
        return mesh;
    }

    public static void Clear()
    {

    }
}

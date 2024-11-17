using System.Collections.Generic;
using System.Linq;
using PCG.Data_Structures;
using UnityEngine;

public class DelaunayTriangulation
{
    public void DisplayDelaunayEdges(HashSet<Edge> delaunayEdges, Transform parentTransform) 
    {
        var Width = 100;
        var Height = 100;
        var cellSize = 2;
        var xOffset = 40;
        var zOffset = 179;
        var screenTopLeft = new Vector3(-Width * cellSize / 2, Height * cellSize / 2, 0);
        
        if (delaunayEdges == null || !parentTransform) return;

        // Create a default material
        Material defaultMaterial = new Material(Shader.Find("Unlit/Color")) { color = Color.green };

        foreach (var edge in delaunayEdges)
        {
            // Calculate world positions for the vertices
            Vector3 startPosition = screenTopLeft +
                                    new Vector3(xOffset + edge.A.X * cellSize,
                                        -edge.A.Y * cellSize,
                                        zOffset);
            Vector3 endPosition = screenTopLeft +
                                  new Vector3(xOffset + edge.B.X * cellSize,
                                      -edge.B.Y * cellSize,
                                      zOffset);

            // Create a new GameObject for the line
            GameObject lineObject = new GameObject($"Line_{edge.A}_{edge.B}");
            lineObject.transform.parent = parentTransform;

            // Add LineRenderer component
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            // Configure LineRenderer properties
            lineRenderer.material = defaultMaterial;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;

            // Set the positions of the line
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }

    
    public HashSet<Edge> GenerateDelaunayTriangulation(List<Room> rooms)
    {
        var midPoints = new List<RoomCell>();
        foreach (var room in rooms)
            midPoints.Add(room.CenterCell);
        if (midPoints.Count < 3) return null;

        var maxX = midPoints[0].X;
        var minX = midPoints[0].X;
        var maxY = midPoints[0].Y;
        var minY = midPoints[0].Y;
        foreach (var midPoint in midPoints)
        {
            if (midPoint.X > maxX) maxX = midPoint.X;
            if (midPoint.X < minX) minX = midPoint.X;
            if (midPoint.Y > maxY) maxY = midPoint.Y;
            if (midPoint.Y < minY) minY = midPoint.Y;
        }

        var width = maxX - minX;
        var height = maxY - minY;

        var superTriangle = new Triangle(
            new Vertex(minX - width, minY - height),
            new Vertex(maxX + width, minY - height),
            new Vertex((minX + maxX) / 2.0f, maxY + height));
        var triangles = new List<Triangle> { superTriangle };
        
        var trianglesToRemove = new List<Triangle>();

        var iteration = 0;
        foreach (var midPoint in midPoints)
        {
            Debug.Log($"Processing point {iteration}: ({midPoint.X}, {midPoint.Y})");

            var edges = new List<Edge>();
            
            trianglesToRemove.Clear();
            foreach (var triangle in triangles)
                if (triangle.IsPointInsideCircumcircle(midPoint))
                {
                    trianglesToRemove.Add(triangle);
                    foreach (var edge in triangle.Edges)
                        edges.Add(edge);
                }

            Debug.Log($"Iteration {iteration}: Removing {trianglesToRemove.Count} triangles");
            foreach (var triangle in trianglesToRemove)
                triangles.Remove(triangle);
            
            var uniqueEdges = new List<Edge>();
            foreach (var edge in edges)
            {
                var reversedEdge = new Edge(edge.B, edge.A);
                if (uniqueEdges.Contains(edge))
                    uniqueEdges.Remove(edge);
                else if (uniqueEdges.Contains(reversedEdge))
                    uniqueEdges.Remove(reversedEdge);
                else
                    uniqueEdges.Add(edge);
            }

            Debug.Log($"Iteration {iteration}: Processing {uniqueEdges.Count} unique edges");
            foreach (var edge in uniqueEdges)
                triangles.Add(new Triangle(edge.A, edge.B, new Vertex(midPoint.X, midPoint.Y)));
            
            Debug.Log($"Iteration {iteration}: Current triangle count: {triangles.Count}");
            iteration++;
        }
        
        // remove triangles that contain super triangle vertices
        trianglesToRemove.Clear();
        foreach (var triangle in triangles)
            foreach (var vertex in superTriangle.Vertices)
                if (triangle.Vertices.Contains(vertex))
                    trianglesToRemove.Add(triangle);
        foreach (var triangle in trianglesToRemove)
            triangles.Remove(triangle);
        Debug.Log($"Triangles: {triangles.Count}");

        var delaunayEdges = new HashSet<Edge>();
        foreach (var triangle in triangles)
            foreach (var edge in triangle.Edges)
            {
                delaunayEdges.Add(edge);
                // Debug.Log($"Edge: {edge.A} - {edge.B}");
            }
        
        return delaunayEdges;        
    }
}
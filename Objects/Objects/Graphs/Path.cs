using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Other;
using Objects.Primitive;
using Speckle.Core.Kits;
using Speckle.Core.Logging;
using Speckle.Core.Models;

namespace Objects.Graphs;
public class Path
{
  /// <summary>
  /// Nodes of the path 
  /// </summary>
  public List<Node> Nodes { get; set; }
  /// <summary>
  /// Edges between the nodes 
  /// </summary>
  public List<Edge> Edges { get; set; }

  /// <summary>
  /// Constructor
  /// </summary>
  public Path()
  {
    Nodes = new List<Node>();
    Edges = new List<Edge>();
  }


  /// <summary>
  /// Add an edge to the path
  /// </summary>
  /// <param name="edge"> The graph edge to be added</param>
  public void AddEdge(Edge edge)
  {
    this.Edges.Add(edge);
    this.Nodes.Add(edge.Start);
    this.Nodes.Add(edge.End);
  }
  /// <summary>
  /// Add a node to the path
  /// </summary>
  /// <param name="n"> The graph node to be added</param>
  internal void AddNode(Node n)
  {
    if (Nodes == null)
    {
      Nodes = new List<Node>();
    }
    Nodes.Add(n);
  }
  /// <summary>
  /// Gets the edges of the path
  /// </summary>
  public void GetEdges()
  {
    if (Edges == null) Edges = new List<Edge>();
    for (int i = 0; i < Nodes.Count - 1; i++)
    {
      Node node = Nodes[i];
      Node next = Nodes[i + 1];
      Edge edge = new Edge(node, next);
      Edges.Add(edge);
    }
  }
}

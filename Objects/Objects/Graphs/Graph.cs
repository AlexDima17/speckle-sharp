using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Other;
using Objects.Primitive;
using Speckle.Core.Kits;
using Speckle.Core.Logging;
using Speckle.Core.Models;
using sg = Objects.Geometry;


namespace Objects.Graphs;
public  class Graph { 
  public HashSet<Node> Nodes
  {
    get
    {
      if (nodes == null)
        nodes = new HashSet<Node>();
      return nodes;
    }
  }
  public Dictionary<long, Edge> Edges
  {
    get
    {
      if (edges == null)
        edges = new Dictionary<long, Edge>();
      return edges;
    }
  }


  private HashSet<Node> nodes;
  private Dictionary<long, Edge> edges;

  public Graph() { }

  /// <summary>
  ///  Clears the graph nodes and Edges
  /// </summary>
  public void Reset()
  {
    if (edges != null && edges.Count > 0)
      edges.Clear();
    if (nodes != null && nodes.Count > 0)
      nodes.Clear();
  }

  public void InserNode(Node n)
  {
    if (nodes == null)
      nodes = new HashSet<Node>();
    int index = nodes.Count;
    if (nodes.Add(n))
    {
      n.SetId(index);
    }
  }

  public void DeleteNode(Node n)
  {
    if (nodes != null && nodes.Contains(n))
    {
      nodes.Remove(n);
      List<Node> _neighbours = nodes.Where(x => x.Neighbours.Contains(n)).ToList();
      for (int i = 0; i < _neighbours.Count; i++)
      {
        _neighbours[i].Neighbours.Remove(n);
        Edge edge = new Edge(n, _neighbours[i]);
        long id = edge.Id;
        long idRev = edge.IdRev;
        Edges.Remove(id);
        Edges.Remove(idRev);
      }

      UpdateIds();

    }
  }
  //for this use case graph will be undirected

  /// <summary>
  /// Adds an edge to the graph, with node already inserted  
  /// </summary>
  /// <param name="n1">start node</param>
  /// <param name="n2">end node</param>
  /// <param name="w">weight of the edge</param>
  public void AddEdge(Node n1, Node n2, double w = 0, bool biDirectional = true)
  {
    if (edges == null)
      edges = new Dictionary<long, Edge>();
    n1.AddNeighbour(n2, biDirectional);
    Edge edge = new Edge(n1, n2, w);
    //this is for undirected graph-1 edge between 2 nodes, opposite one is the same
   
      if (!edges.ContainsKey(edge.Id) && !edges.ContainsKey(edge.IdRev))
      {
        edges.Add(edge.Id, edge);
        if (edge.Id != edge.IdRev)
          edges.Add(edge.IdRev, new Edge(n2, n1, w));
      }
      else if (!edges.ContainsKey(edge.Id))
      {
        edges.Add(edge.Id, edge);
      }
      else if (!edges.ContainsKey(edge.IdRev))
      {
        edges.Add(edge.IdRev, new Edge(n2, n1, w));
      }
      else
      {
        GetEdgeByIds(edge.Id, edge.IdRev).Weight = w;
      }
    
   
  }

 
  public Edge GetEdgeByIds(long id, long idRev)
  {
    Edge result = null;
    if (!edges.TryGetValue(id, out result))
      edges.TryGetValue(idRev, out result);

    return result;
  }  

  /// <summary>
  /// Updates the  indexes of the nodes
  /// </summary>
  public void UpdateIds()
  {
    for (int i = 0; i < nodes.Count; i++)
    {
      nodes.ElementAt(i).SetId(i);
    }
  }

  /// <summary>
  /// Finds path between two nodes using A* algorithm
  /// </summary>
  
  public void AStarPathfinding(Node start, Node end, bool extraScoreSystem, out bool found, out Path path)
  {
    //for every node we need to keep track of its previous
    Node[] previous = new Node[this.nodes.Count];
    double[] fScores = new double[this.nodes.Count];
    double[] gScores = new double[this.nodes.Count];

    List<Node> openSet = new List<Node>();
    List<Node> closedSet = new List<Node>();
    path = new Path();

    openSet.Add(start);

    while (openSet.Count > 0)
    {
      int win = 0;
      for (int i = 0; i < openSet.Count; i++)
      {
        if (fScores[i] < fScores[win])
        {
          win = i;
        }
      }
      Node current = openSet[win];
      //if the checked node is the target
      if (current.Id == end.Id)
      {
        //path ends
        Node temp = current;
        path.AddNode(temp);

        while (previous[temp.Id] != null)
        {
          path.AddNode(previous[temp.Id]);
          temp = previous[temp.Id];
        }
        break;
      }
      openSet.Remove(current);
      closedSet.Add(current);
      List<Node> neighboursList = current.Neighbours.Select(n => (Node)n).ToList();
      HashSet<Node>neighbours = new HashSet<Node>(neighboursList);

      for (int i = 0; i < neighbours.Count; i++)
      {
        Node neighbour = neighbours.ElementAt(i);
        if (!closedSet.Contains(neighbour))
        {
          double tempG = gScores[current.Id];
          tempG = GetScore(current, neighbour, tempG);


          bool newPath = false;
          if (openSet.Contains(neighbour))
          {
            if (tempG < gScores[neighbour.Id])
            {
              gScores[neighbour.Id] = tempG;
              newPath = true;
            }
          }
          else
          {
            gScores[neighbour.Id] = tempG;
            openSet.Add(neighbour);
            newPath = true;
          }
          if (newPath)
          {
            fScores[neighbour.Id] = gScores[neighbour.Id] + neighbour.Heuristic(end);
            previous[neighbour.Id] = current;
          }
        }
      }
    }
    found = path.Nodes.Count > 0;
  }

  private double GetScore(Node current, Node neighbour, double score)
  {
    score += current.Pos.DistanceTo(neighbour.Pos);
    return score;
  }

}



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Objects.Other;
using Objects.Primitive;
using Speckle.Core.Kits;
using Speckle.Core.Logging;
using Speckle.Core.Models;
using sg=Objects.Geometry;

namespace Objects.Graphs;
public  class Node
{
 
  public sg.Point Pos;
  public int Id { get; private set; }
  public HashSet<Edge> Edges;
  public HashSet<Node> Neighbours
  {
    get
    {
      if (neighbours == null)
        neighbours = new HashSet<Node>();
      return neighbours;
    }
  }

  private HashSet<Node> neighbours;
  public Node(sg.Point pos)
  {
    this.Pos = pos;
  }
  public void SetId(int id)
  {
    this.Id = id;
  }

  public void AddNeighbour(Node neighbour, bool addToNeighbour = true)
  {
    if (neighbours == null)
      neighbours = new HashSet<Node>();

    neighbours.Add(neighbour);
    if (addToNeighbour)
      neighbour.AddNeighbour(this, false);
  }

  public void RemoveNeighbour(Node neighbour, bool removeFromNeighbour = true)
  {
    if (neighbours == null)
      neighbours = new HashSet<Node>();
    neighbours.Remove(neighbour);
    if (removeFromNeighbour)
      neighbour.RemoveNeighbour(this, false);
  }

  public double Heuristic(Node trg)
  {
    double eucl= this.Pos.DistanceTo(trg.Pos);
    return eucl;
  }

}



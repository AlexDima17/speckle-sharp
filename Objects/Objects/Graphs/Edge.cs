using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Other;
using Objects.Primitive;
using Speckle.Core.Kits;
using Speckle.Core.Logging;
using Speckle.Core.Models;

namespace Objects.Graphs;
public class Edge
{
  public Node Start { get; set; }
  public Node End { get; set; }
  public long Id { get; set; }
  public long IdRev { get; set; }
  public double Weight { get; set; }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="sn"> Start node</param>
  /// <param name="en"> End node</param>
  /// <param name="w"> Weight, default=1</param>
  public Edge(Node sn, Node en, double w = 1)
  {
    Start = sn;
    End = en;
    Id = (long)(0.5 * (sn.Id + en.Id) * (sn.Id + en.Id + 1) + en.Id);
    IdRev = (long)(0.5 * (sn.Id + en.Id) * (sn.Id + en.Id + 1) + sn.Id);
    Weight = w;
  }

  /// <summary>
  /// Calculates id of the edge from start and end
  /// </summary>
  /// <param name="a"> id of start node</param>
  /// <param name="b"> id of end node</param>
  /// <returns> long</returns>
  public static long GetId(int a, int b)
  {
    return (long)(0.5 * (a + b) * (a + b + 1) + b);
  }

  /// <summary>
  /// Calculates id of the reverse edge from start and end
  /// </summary>
  /// <param name="a"> id of start node</param>
  /// <param name="b"> id of end node</param>
  /// <returns> long</returns>
  public static long GetIdRev(int a, int b)
  {
    return (long)(0.5 * (a + b) * (a + b + 1) + a);
  }
}

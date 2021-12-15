
namespace Utils
{
    public class Graph<TNode, TEdge> where TNode : notnull
    {
        public static Graph<TNode, object?> AsUnweightedDirected(IEnumerable<(TNode origin, TNode destination)> input) => BuildUnweighted(input, true);

        public static Graph<TNode, object?> AsUnweightedUnidirectional(IEnumerable<(TNode origin, TNode destination)> input) => BuildUnweighted(input, false);

        public Graph()
        {
        }

        public void AddEdge(TNode src, TNode dest, TEdge edge, Predicate<TEdge> shouldReplaceEdgePredicate)
        {
            if (!_edges.ContainsKey(src))
            {
                _edges.Add(src, new() { { dest, edge } });
            }
            else
            {
                var srcNode = _edges[src];
                if (srcNode.TryGetValue(dest, out var curEdge))
                {
                    if (shouldReplaceEdgePredicate(curEdge))
                    {
                        _edges[src][dest] = edge;
                    }
                    return;
                }
                _edges[src][dest] = edge;
            }
        }

        public IEnumerable<(TNode origin, TNode destination, TEdge edge)> Edges =>
                _edges.SelectMany(o => o.Value.Select(d => (d.Key, d.Value)).Select(t => (o.Key, t.Key, t.Value)));

        public IEnumerable<TNode> Nodes => _edges.Keys;

        public IEnumerable<TPath> DFS<TPath, TUser>(
            TNode start,
            TNode end,
            Predicate<(TNode currentItem, TNode possibleAdjacentItem, TEdge edge)> shouldWalkPredicate,
            Predicate<(IPath<TNode, TUser> currentPath, TNode currentItem, TNode possibleAdjacentItem, TEdge edge)> shouldReWalkPredicate) where TPath : class, IPath<TNode, TUser>, new()
             => DFSInternal(start, end, shouldWalkPredicate, shouldReWalkPredicate, new TPath());

        private IEnumerable<TPath> DFSInternal<TPath, TUser>(
            TNode start,
            TNode end,
            Predicate<(TNode currentItem, TNode possibleAdjacentItem, TEdge edge)> shouldWalkPredicate,
            Predicate<(IPath<TNode, TUser> currentPath, TNode currentItem, TNode possibleAdjacentItem, TEdge edge)> shouldReWalkPredicate,
            TPath visitedPath) where TPath : class, IPath<TNode, TUser>, new()
        {
            if (Equals(start, end))
            {
                var startPath = new TPath();
                startPath.Add(start);
                return new[] { startPath };
            }

            var nextDestinations = _edges[start].Where(x => shouldWalkPredicate((start, x.Key, x.Value)));
            List<TPath> retPaths = new();

            foreach (var next in nextDestinations)
            {
                var curPath = ClonePath<TPath, TUser>(visitedPath);
                curPath.Add(start);

                if (visitedPath.Contains(next.Key) && !shouldReWalkPredicate((curPath, start, next.Key, next.Value)))
                {
                    continue;
                }
                var results = DFSInternal(next.Key, end, shouldWalkPredicate, shouldReWalkPredicate, curPath).ToArray();

                retPaths.AddRange(results.Select(r => { var tmp = ClonePath<TPath, TUser>(curPath); tmp.Add(r); return tmp; }));
            }
            return retPaths;
        }

        public Dictionary<TNode, long> Dijkstra(TNode originNode, Func<TEdge, long> edgeLengthFunc)
        {
            var dist = new Dictionary<TNode, long>
            {
                [originNode] = 0
            };
            var prev = new Dictionary<TNode, (bool defined, TNode node)>();

            var queue = new PriorityQueue<TNode, long>();

            foreach (var curNode in Nodes)
            {
                if (!Equals(originNode, curNode))
                {
                    dist[curNode] = long.MaxValue;
                    prev[curNode] = (false, curNode);
                }
                queue.Enqueue(curNode, dist[curNode]);
            }

            while (queue.Count > 0)
            {
                var nextNode = queue.Dequeue();

                foreach (var neighbor in _edges[nextNode])
                {
                    var edgeLenght = edgeLengthFunc(neighbor.Value);
                    var alt = (edgeLenght == long.MaxValue) ? long.MaxValue : (dist[nextNode] + edgeLenght);
                    if (alt < dist[neighbor.Key])
                    {
                        dist[neighbor.Key] = alt;
                        prev[neighbor.Key] = (true, nextNode);
                        queue.Enqueue(neighbor.Key, alt);
                    }
                }
            }

            return dist;
        }

        private static TPath ClonePath<TPath, TUser>(TPath path) where TPath : class, IPath<TNode, TUser>, new()
        {
            if (path.Clone() is not TPath clone)
            {
                throw new Exception($"{nameof(TPath)}.Clone should return a {nameof(TPath)} object");
            }
            return clone;
        }

        private static Graph<TNode, object?> BuildUnweighted(IEnumerable<(TNode origin, TNode destination)> input, bool isDirected)
        {
            var edges = new Dictionary<TNode, Dictionary<TNode, object?>>();

            foreach (var (origin, destination) in input)
            {
                if (edges.ContainsKey(origin))
                {
                    edges[origin].Add(destination, null);
                }
                else
                {
                    edges.Add(origin, new() { { destination, null } });
                }
                if (isDirected)
                {
                    if (edges.ContainsKey(destination))
                    {
                        edges[destination].Add(origin, null);
                    }
                    else
                    {
                        edges.Add(destination, new() { { origin, null } });
                    }
                }
            }
            return new(edges);
        }

        private Graph(Dictionary<TNode, Dictionary<TNode, TEdge>> edges)
        {
            _edges = edges ?? throw new ArgumentNullException(nameof(edges));
        }

        private readonly Dictionary<TNode, Dictionary<TNode, TEdge>> _edges = new();
    }
}

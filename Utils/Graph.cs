namespace Utils
{
    public class Graph<TNode> where TNode : notnull
    {
        public static Graph<TNode> AsDirected(IEnumerable<(TNode origin, TNode destination)> input) => Build(input, true);

        public static Graph<TNode> AsUnidirectional(IEnumerable<(TNode origin, TNode destination)> input) => Build(input, false);

        public IEnumerable<TPath> DFS<TPath, TUser>(
            TNode start,
            TNode end,
            Predicate<(TNode currentItem, TNode possibleAdjacentItem)> shouldWalkPredicate,
            Predicate<(IPath<TNode, TUser> currentPath, TNode currentItem, TNode possibleAdjacentItem)> shouldReWalkPredicate) where TPath : class, IPath<TNode, TUser>, new()
             => DFSInternal(start, end, shouldWalkPredicate, shouldReWalkPredicate, new TPath());

        private IEnumerable<TPath> DFSInternal<TPath, TUser>(
            TNode start,
            TNode end,
            Predicate<(TNode currentItem, TNode possibleAdjacentItem)> shouldWalkPredicate,
            Predicate<(IPath<TNode, TUser> currentPath, TNode currentItem, TNode possibleAdjacentItem)> shouldReWalkPredicate,
            TPath visitedPath) where TPath : class, IPath<TNode, TUser>, new()
        {
            if (Equals(start, end))
            {
                var startPath = new TPath();
                startPath.Add(start);
                return new[] { startPath };
            }

            var nextDestinations = _edges[start].Where(x => shouldWalkPredicate((start, x)));
            List<TPath> retPaths = new();

            foreach (var next in nextDestinations)
            {
                var curPath = ClonePath<TPath, TUser>(visitedPath);
                curPath.Add(start);

                if (visitedPath.Contains(next) && !shouldReWalkPredicate((curPath, start, next)))
                {
                    continue;
                }
                var results = DFSInternal(next, end, shouldWalkPredicate, shouldReWalkPredicate, curPath).ToArray();

                retPaths.AddRange(results.Select(r => { var tmp = ClonePath<TPath, TUser>(curPath); tmp.Add(r); return tmp; }));
            }
            return retPaths;
        }

        private static TPath ClonePath<TPath, TUser>(TPath path) where TPath : class, IPath<TNode, TUser>, new()
        {
            if (path.Clone() is not TPath clone)
            {
                throw new Exception($"{nameof(TPath)}.Clone should return a {nameof(TPath)} object");
            }
            return clone;
        }

        private static Graph<TNode> Build(IEnumerable<(TNode origin, TNode destination)> input, bool isDirected)
        {
            var edges = new Dictionary<TNode, HashSet<TNode>>();

            foreach (var (origin, destination) in input)
            {
                if (edges.ContainsKey(origin))
                {
                    edges[origin].Add(destination);
                }
                else
                {
                    edges.Add(origin, new() { destination });
                }
                if (isDirected)
                {
                    if (edges.ContainsKey(destination))
                    {
                        edges[destination].Add(origin);
                    }
                    else
                    {
                        edges.Add(destination, new() { origin });
                    }
                }
            }
            return new(edges);
        }

        private Graph(Dictionary<TNode, HashSet<TNode>> edges)
        {
            _edges = edges ?? throw new ArgumentNullException(nameof(edges));
        }

        private Dictionary<TNode, HashSet<TNode>> _edges;
    }
}

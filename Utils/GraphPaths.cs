namespace Utils
{
    public interface IReadOnlyPath<TNode, TUser> where TNode : notnull
    {
        bool Contains(TNode val);

        TUser? UserContext { get; set; }

        IReadOnlyCollection<TNode> Items { get; }
    }

    public interface IPath<TNode, TUser> : IReadOnlyPath<TNode, TUser> where TNode : notnull
    {
        public void Add(TNode node);

        public void AddRange(IEnumerable<TNode> nodes);

        public void Add(IPath<TNode, TUser> path);

        public IPath<TNode, TUser> Clone();
    }

    public class OrderedPath<TNode, TUser> : IPath<TNode, TUser> where TNode : notnull
    {
        public OrderedPath()
        {
        }

        public OrderedPath(TNode singleNode)
        {
            _path = new() { singleNode };
        }

        public TUser? UserContext { get; set; }

        public IReadOnlyCollection<TNode> Items => _path;

        public bool Contains(TNode val) => _path.Any(x => Equals(x, val));

        public IPath<TNode, TUser> Clone() => new OrderedPath<TNode, TUser>(this);

        public void Add(TNode node)
        {
            _path.Add(node);
        }

        public void AddRange(IEnumerable<TNode> nodes)
        {
            _path.AddRange(nodes);
        }

        public void Add(IPath<TNode, TUser> path)
        {
            AddRange(path.Items);
        }

        private OrderedPath(OrderedPath<TNode, TUser> rhs)
        {
            _path = rhs._path.ToList();
            UserContext = rhs.UserContext;
        }

        private readonly List<TNode> _path = new();
    }


    public class UnOrderedPath<TNode, TUser> : IPath<TNode, TUser> where TNode : notnull
    {
        public UnOrderedPath()
        {

        }

        public TUser? UserContext { get; set; }

        public IReadOnlyCollection<TNode> Items => _nodes;

        public void Add(TNode node)
        {
            _nodes.Add(node);
        }

        public void Add(IPath<TNode, TUser> path) => AddRange(path.Items);

        public void AddRange(IEnumerable<TNode> nodes)
        {
            foreach (var node in nodes)
            {
                Add(node);
            }
        }

        public IPath<TNode, TUser> Clone() => new UnOrderedPath<TNode, TUser>(this);

        public bool Contains(TNode val) => _nodes.Contains(val);

        private UnOrderedPath(UnOrderedPath<TNode, TUser> rhs)
        {
            _nodes = rhs._nodes.ToHashSet();
            UserContext = rhs.UserContext;
        }

        private readonly HashSet<TNode> _nodes = new();
    }

}

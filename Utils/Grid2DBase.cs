namespace Utils
{
    public struct Point2D
    {
        public Point2D(int x, int y) { X = x; Y = y; }
        public int X { get; init; }
        public int Y { get; init; }
    }

    public abstract class Grid2DBase<T>
    {
        protected Grid2DBase()
        {
        }

        public IEnumerable<T> Items => Enumerate().Select(t => t.value);

        public abstract IEnumerable<(int x, int y, T value)> Enumerate();

        public abstract T At(Point2D location);

        public abstract void SetAt(T val, Point2D location);

        public abstract T At(int x, int y);

        public abstract void SetAt(T val, int x, int y);

        public abstract Grid2DBase<T> Clone();

        public IEnumerable<Point2D> GetAdjacentOrthogonalLocations(Point2D location) => Grid2D<T>
                .GenerateAdjacentOrthogonalLocations(location)
                .Where(IsCoordinateValid);

        public IEnumerable<Point2D> GetAllAdjacentLocations(Point2D location) => Grid2D<T>
                .GenerateAllAdjacentLocations(location)
                .Where(IsCoordinateValid);

        public static IEnumerable<Point2D> GenerateAdjacentOrthogonalLocations(Point2D location)
        {
            yield return new(location.X, location.Y - 1);
            yield return new(location.X, location.Y + 1);
            yield return new(location.X - 1, location.Y);
            yield return new(location.X + 1, location.Y);
        }

        public static IEnumerable<Point2D> GenerateAllAdjacentLocations(Point2D location)
        {
            yield return new(location.X, location.Y - 1);
            yield return new(location.X, location.Y + 1);
            yield return new(location.X - 1, location.Y);
            yield return new(location.X + 1, location.Y);
            yield return new(location.X - 1, location.Y - 1);
            yield return new(location.X + 1, location.Y - 1);
            yield return new(location.X - 1, location.Y + 1);
            yield return new(location.X + 1, location.Y + 1);
        }

        public IEnumerable<(int x, int y)> GetAdjacentOrthogonalLocations(int locationX, int locationY) =>
                GetAdjacentOrthogonalLocations(new(locationX, locationY)).Select(t => (t.X, t.Y));

        public IEnumerable<(int x, int y)> GetAllAdjacentLocations(int locationX, int locationY) =>
            GetAllAdjacentLocations(new(locationX, locationY)).Select(t => (t.X, t.Y));

        public static IEnumerable<(int x, int y)> GenerateAdjacentOrthogonalLocations(int locationX, int locationY) =>
            GenerateAdjacentOrthogonalLocations(new(locationX, locationY)).Select(t => (t.X, t.Y));

        public static IEnumerable<(int x, int y)> GenerateAllAdjacentLocations(int locationX, int locationY) =>
            GenerateAllAdjacentLocations(new(locationX, locationY)).Select(t => (t.X, t.Y));

        public Grid2DBase<T> BFS(
            Point2D initialPosition,
            Predicate<((T item, Point2D location) current, (T item, Point2D location) possibleAdjacent)> shouldWalkPredicate,
            Func<T, T> markVisitedFunc,
            bool useOnlyOrthogonalWalking,
            bool allowReWalk = false)
        {
            void RunStep(IEnumerable<Point2D> nextSteps, Action<IEnumerable<Point2D>> pickNextStepsFunc)
            {
                foreach (var cur in nextSteps)
                {
                    var curItem = At(cur.X, cur.Y);
                    pickNextStepsFunc(
                        (useOnlyOrthogonalWalking ? GetAdjacentOrthogonalLocations(cur) : GetAllAdjacentLocations(cur))
                            .Where(t => shouldWalkPredicate.Invoke(((curItem, cur), (At(t.X, t.Y), t))))
                        );
                    SetAt(markVisitedFunc(At(cur)), cur);
                }
            };
            if (!allowReWalk)
            {
                HashSet<Point2D> nextSteps = [initialPosition];
                while (nextSteps.Count > 0)
                {
                    var newPositions = new HashSet<Point2D>(2 * nextSteps.Count);
                    RunStep(nextSteps, t => newPositions.UnionWith(t));
                    nextSteps = newPositions;
                }
            }
            else
            {
                List<Point2D> nextSteps = [initialPosition];
                while (nextSteps.Count > 0)
                {
                    var newPositions = new List<Point2D>(2 * nextSteps.Count);
                    RunStep(nextSteps, t => newPositions.AddRange(t));
                    nextSteps = newPositions;
                }
            }
            return this;
        }

        public Grid2DBase<T> BFS(
            (int x, int y) initialPosition,
            Predicate<((T item, int x, int y) current, (T item, int x, int y) possibleAdjacent)> shouldWalkPredicate,
            Func<T, T> markVisitedFunc,
            bool useOnlyOrthogonalWalking,
            bool allowReWalk = false) =>

            BFS(
                new(initialPosition.x, initialPosition.y),
                (t) => shouldWalkPredicate(((t.current.item, t.current.location.X, t.current.location.Y), (t.possibleAdjacent.item, t.possibleAdjacent.location.X, t.possibleAdjacent.location.Y))),
                markVisitedFunc, useOnlyOrthogonalWalking, allowReWalk
                );

        public Grid2D<long> ComputeWalkCost(
            Point2D initialPosition,
            Predicate<((T item, Point2D location) current, (T item, Point2D location) possibleAdjacent)> shouldWalkPredicate,
            Action<((T item, Point2D location) current, (T item, Point2D location) next)> onWalkNext,
            Func<T, T, long> walkCostFunc,
            bool useOnlyOrthogonalWalking)
        {
            var gridDimensions = GetGridDimensions();
            Grid2D<long> costMap = new(Enumerable.Range(0, gridDimensions.Height).Select(_ => Enumerable.Range(0, gridDimensions.Width).Select(__ => long.MaxValue)));
            costMap.SetAt(0, initialPosition.X, initialPosition.Y);

            BFS(initialPosition, shouldWalkPredicate: t =>
            {
                if (!shouldWalkPredicate(t))
                {
                    return false;
                }
                var possibleNewCost = costMap.At(t.current.location.X, t.current.location.Y) + walkCostFunc(t.current.item, t.possibleAdjacent.item);
                var currentCost = costMap.At(t.possibleAdjacent.location.X, t.possibleAdjacent.location.Y);
                if (possibleNewCost < currentCost)
                {
                    costMap.SetAt(possibleNewCost, t.possibleAdjacent.location.X, t.possibleAdjacent.location.Y);

                    onWalkNext(t);

                    return true;
                }
                return false;
            },
            markVisitedFunc: t => t,
            useOnlyOrthogonalWalking,
            allowReWalk: false);
            return costMap;
        }

        public Grid2D<long> ComputeWalkCost(
            (int x, int y) initialPosition,
            Predicate<((T item, int x, int y) current, (T item, int x, int y) possibleAdjacent)> shouldWalkPredicate,
            Action<((T item, int x, int y) current, (T item, int x, int y) next)> onWalkNext,
            Func<T, T, long> walkCostFunc,
            bool useOnlyOrthogonalWalking) =>

            ComputeWalkCost(
                new(initialPosition.x, initialPosition.y),
                (t) => shouldWalkPredicate(((t.current.item, t.current.location.X, t.current.location.Y), (t.possibleAdjacent.item, t.possibleAdjacent.location.X, t.possibleAdjacent.location.Y))),
                (t) => { onWalkNext(((t.current.item, t.current.location.X, t.current.location.Y), (t.next.item, t.next.location.X, t.next.location.Y))); },
                walkCostFunc, useOnlyOrthogonalWalking
                );

        public static Direction GetDirectionForAdjacentLocation(Point2D origin, Point2D adjacentPosition)
        {
            if (((adjacentPosition.X - origin.X) * (adjacentPosition.X - origin.X) + (adjacentPosition.Y - origin.Y) * (adjacentPosition.Y - origin.Y)) != 1)
            {
                throw new ArgumentException("The two points must be adjacent!");
            }

            if (origin.X == adjacentPosition.X)
            {
                return origin.Y > adjacentPosition.Y ? Direction.North : Direction.South;
            }
            else
            {
                return origin.X > adjacentPosition.X ? Direction.West : Direction.East;
            }
        }

        public enum Direction
        {
            North,
            East,
            South,
            West,
        }

        protected abstract bool IsCoordinateValid(Point2D coordinate);

        protected abstract (int Width, int Height) GetGridDimensions();
    }
}

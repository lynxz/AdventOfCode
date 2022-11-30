// See https://aka.ms/new-console-template for more information
using Tools;

Day23 day = new("23");
day.OutputSecondStar();

public class Day23 : DayBase
{
    public Day23(string day) : base(day)
    {
    }

    private int hallwaySize = 0;

    public override string FirstStar()
    {
        var input = GetRawData();
        var initialWorld = input.Where(c => c == '.' || (c >= 'A' && c <= 'D')).ToArray();
        hallwaySize = initialWorld.Where(c => c == '.').Count();

        return Organize(new GameState(initialWorld, 0)).ToString();
    }

    public override string SecondStar()
    {
        var input = GetRawData();
        var initialWorld = input.Where(c => c == '.' || (c >= 'A' && c <= 'D')).ToArray();
        hallwaySize = initialWorld.Where(c => c == '.').Count();

        var extras = "  #D#C#B#A#\n  #D#B#A#C#";

        var extraChars = extras.Where(c => c >= 'A' && c <= 'D').ToArray();
        var deepWorld = initialWorld.Take(hallwaySize + 4).Concat(extraChars).Concat(initialWorld.Skip(hallwaySize + 4)).ToArray();

        return Organize(new GameState(deepWorld, 0)).ToString();
    }

    record struct GameState(char[] World, int Energy) { }

    bool IsRoomOrganized(char[] world, int depth, int r)
    {
        for (int i = depth - 1; i >= 0; i--)
        {
            var c = world[hallwaySize + i * 4 + r];
            if (c == '.')
            {
                return true;
            }
            if (c != 'A' + r)
            {
                return false;
            }
        }
        return true;
    }

    int RoomCount(char[] world, int depth, int r)
    {
        int count = 0;
        for (int i = depth - 1; i >= 0; i--)
        {
            var c = world[hallwaySize + i * 4 + r];
            if (c == '.')
            {
                return count;
            }
            count++;
        }
        return count;
    }

    void PushRoom(char[] world, int depth, int r, char c)
    {
        for (int i = depth - 1; i >= 0; i--)
        {
            var index = hallwaySize + i * 4 + r;
            if (world[index] == '.')
            {
                world[index] = c;
                return;
            }
        }
        throw new Exception("Cannot push into full room");
    }

    void PopRoom(char[] world, int depth, int r)
    {
        for (int i = 0; i < depth; i++)
        {
            var index = hallwaySize + i * 4 + r;
            if (world[index] != '.')
            {
                world[index] = '.';
                return;
            }
        }
        throw new Exception("Cannot pop from empty room");
    }

    char PeekRoom(char[] world, int depth, int r)
    {
        for (int i = 0; i < depth; i++)
        {
            var index = hallwaySize + i * 4 + r;
            if (world[index] != '.')
            {
                return world[index];
            }
        }
        throw new Exception("Cannot peek at empty room");
    }

    int[] cost = new[] { 1, 10, 100, 1000 };
    int[] directions = new[] { -1, 1 };

    List<GameState> GetNeighbors(int depth, GameState state)
    {
        var neighbors = new List<GameState>();

        // Option 1: Move an amphipod from a hallway to their target room
        for (int i = 0; i < hallwaySize; i++)
        {
            if (state.World[i] == '.')
            {
                continue;
            }

            var picked = state.World[i];
            var pickedIndex = picked - 'A';

            // Is the room empty or ready?
            bool canMoveToRoom = IsRoomOrganized(state.World, depth, pickedIndex);
            if (!canMoveToRoom)
            {
                continue;
            }

            var targetPosition = 2 + 2 * pickedIndex;

            // Is the path empty?
            var direction = targetPosition > i ? 1 : -1;
            for (int j = direction; Math.Abs(j) <= Math.Abs(targetPosition - i); j += direction)
            {
                if (state.World[i + j] != '.')
                {
                    canMoveToRoom = false;
                    break;
                }
            }

            if (!canMoveToRoom)
            {
                continue;
            }

            var newWorld = new char[state.World.Length];
            state.World.CopyTo(newWorld, 0);

            newWorld[i] = '.';
            PushRoom(newWorld, depth, pickedIndex, picked);

            var newEnergy = state.Energy + (Math.Abs(targetPosition - i) + (depth - RoomCount(state.World, depth, pickedIndex))) * cost[pickedIndex];
            neighbors.Add(new GameState(newWorld, newEnergy));
        }

        // Greedy, don't try to remove amphipods from rooms if the rooms are ready to be filled
        if (neighbors.Count > 0)
        {
            return neighbors;
        }

        // Option 2: Remove one amphipod from a room
        for (int r = 0; r < 4; r++)
        {
            if (IsRoomOrganized(state.World, depth, r))
            {
                // Can't take from this one
                continue;
            }

            var picked = PeekRoom(state.World, depth, r);
            var pickedIndex = picked - 'A';

            // Possible targets are empty spaces on hallway, not in front of any room, with no blockers
            var energy = state.Energy + (depth - RoomCount(state.World, depth, r) + 1) * (cost[pickedIndex]);
            var roomPosition = 2 + 2 * r;
            foreach (var direction in directions)
            {
                var distance = direction;
                while (roomPosition + distance >= 0 && roomPosition + distance < hallwaySize && state.World[roomPosition + distance] == '.')
                {
                    if (roomPosition + distance == 2 || roomPosition + distance == 4 || roomPosition + distance == 6 || roomPosition + distance == 8)
                    {
                        // In front of a room, skip
                        distance += direction;
                        continue;
                    }

                    var newWorld = new char[state.World.Length];
                    state.World.CopyTo(newWorld, 0);

                    newWorld[roomPosition + distance] = picked;
                    PopRoom(newWorld, depth, r);

                    neighbors.Add(new GameState(newWorld, energy + Math.Abs(distance) * cost[pickedIndex]));

                    distance += direction;
                }
            }
        }

        return neighbors;
    }

    bool IsTarget(GameState state, int depth)
    {
        for (int i = 0; i < hallwaySize; i++)
        {
            if (state.World[i] != '.')
            {
                return false;
            }
        }

        for (int r = 0; r < 4; r++)
        {
            if (!IsRoomOrganized(state.World, depth, r))
            {
                return false;
            }
        }

        return true;
    }

    int Organize(GameState initialState)
    {
        // Dijkstra on the graph
        var depth = (initialState.World.Length - hallwaySize) / 4;
        var frontier = new PriorityQueue<GameState, int>();
        frontier.Enqueue(initialState, 0);
        var visited = new HashSet<string>();
        while (frontier.Count > 0)
        {
            var node = frontier.Dequeue();
            var worldString = new String(node.World);
            if (visited.Contains(worldString))
            {
                continue;
            }
            if (IsTarget(node, depth))
            {
                return node.Energy;
            }
            visited.Add(worldString);
            frontier.EnqueueRange(GetNeighbors(depth, node).Select(n => (n, n.Energy)));
        }

        throw new Exception("Impossible to organize");
    }
}
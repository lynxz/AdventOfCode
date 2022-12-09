using Tools;

Day07 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day07 : DayBase
{
    public Day07() : base("7")
    {
    }
    public override string FirstStar()
    {
        Node<int> root = BuildDirectory();
        return SumSmallDir(root, 0).ToString();
    }

    public override string SecondStar()
    {
        var root = BuildDirectory();
        var limit = 30000000 - (70000000 - root.Data);
        return FindDir(root, limit, root).Data.ToString();
    }

    private Node<int> BuildDirectory()
    {
        var data = GetRowData() ?? Array.Empty<string>();
        var root = new Node<int>("/", 0, null, new List<Node<int>>());
        var current = root;

        foreach (var l in data.Skip(1))
        {
            if (l == "$ cd ..")
            {
                current = current?.Parent;
            }
            else if (l.StartsWith("$ cd"))
            {
                current = current?.Children?.FirstOrDefault(n => n.Name == l[5..]);
            }
            else if (l.StartsWith("dir"))
            {
                var dir = l[4..];
                Node<int>? node = current?.Children?.FirstOrDefault(n => n.Name == dir);
                if (node == null)
                {
                    node = new Node<int>(dir, 0, current, new List<Node<int>>());
                    current?.Children?.Add(node);
                }
            }
            else if (char.IsNumber(l[0]))
            {
                var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var node = new Node<int>(parts[1], int.Parse(parts[0]), current, null);
                current?.Children?.Add(node);
            }
        }

        CalculateDirSize(root);

        return root;
    }

    private void CalculateDirSize(Node<int>? node)
    {
        if (node?.Children == null)
            return;
        var size = 0;
        foreach (var child in node.Children)
        {
            if (child.Children != null)
            {
                CalculateDirSize(child);
            }
            size += child.Data;
        }
        node.Data = size;
    }

    private int SumSmallDir(Node<int> node, int current)
    {
        if (node?.Children == null)
            return current;

        foreach (var child in node.Children)
            current = SumSmallDir(child, current);

        return node.Data <= 100000 ? node.Data + current : current;
    }

    private Node<int> FindDir(Node<int> node, int limit, Node<int> contender)
    {
        if (node.Children == null)
            return contender;
        if (node.Data >= limit && node.Data < contender.Data)
            contender = node;

        foreach (var child in node.Children)
            contender = FindDir(child, limit, contender);
        
        return contender;
    }
}
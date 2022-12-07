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
        Node root = BuildDirectory();
        return SumSmallDir(root, 0).ToString();
    }

    public override string SecondStar()
    {
        var root = BuildDirectory();
        var limit = 30000000 - (70000000 - root.Size);
        return FindDir(root, limit, root).Size.ToString();
    }

    private Node BuildDirectory()
    {
        var data = GetRowData() ?? Array.Empty<string>();
        var root = new Node("/", 0, null, new List<Node>());
        var current = root;

        foreach (var l in data.Skip(1))
        {
            if (l == "$ cd ..")
            {
                current = current?.Parent;
            }
            else if (l.StartsWith("$ cd"))
            {
                var dir = l.Substring(5);
                current = current?.Children?.FirstOrDefault(n => n.Name == dir);
            }
            else if (l.StartsWith("dir"))
            {
                var dir = l.Substring(4);
                Node? node = current?.Children?.FirstOrDefault(n => n.Name == dir);
                if (node == null)
                {
                    node = new Node(dir, 0, current, new List<Node>());
                    current?.Children?.Add(node);
                }
            }
            else if (char.IsNumber(l[0]))
            {
                var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var node = new Node(parts[1], int.Parse(parts[0]), current, null);
                current?.Children?.Add(node);
            }
        }

        CalculateDirSize(root);

        return root;
    }

    private void CalculateDirSize(Node? node)
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
            size += child.Size;
        }
        node.Size = size;
    }

    private int SumSmallDir(Node node, int current)
    {
        if (node?.Children == null)
            return current;

        foreach (var child in node.Children)
            current = SumSmallDir(child, current);

        return node.Size <= 100000 ? node.Size + current : current;
    }


    private Node FindDir(Node node, int limit, Node contender)
    {
        if (node.Children == null)
        {
            return contender;
        }
        if (node.Size >= limit && node.Size < contender.Size)
            contender = node;
        foreach (var child in node.Children)
        {
            contender = FindDir(child, limit, contender);
        }
        return contender;
    }
}

public class Node
{
    public Node(string name, int size, Node? parent, List<Node>? children)
    {
        Name = name;
        Size = size;
        Parent = parent;
        Children = children;
    }

    public string Name { get; set; }
    public int Size { get; set; }
    public Node? Parent { get; set; }
    public List<Node>? Children { get; set; }
}
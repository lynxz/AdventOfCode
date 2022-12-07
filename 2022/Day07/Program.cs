using Tools;

Day07 day = new();
day.PostSecondStar();

public class Day07 : DayBase
{
    public Day07() : base("7")
    {
    }
    public override string FirstStar()
    {
        var data = GetRowData();
        var root = new Node("/", 0, null, new List<Node>());
        var current = root;
        var c = 0;

        foreach (var l in data.Skip(1))
        {
            if (l == "$ cd ..")
            {
                current = current.Parent;
            }
            else if (l.StartsWith("$ cd"))
            {
                var dir = l.Substring(5);
                Node node = current.Children.FirstOrDefault(n => n.Name == dir);
                if (node == null)
                {
                    node = new Node(dir, 0, current, new List<Node>());
                    current.Children.Add(node);
                }
                current = node;
            }
            else if (l.StartsWith("dir"))
            {
                var dir = l.Substring(4);
                Node node = current.Children.FirstOrDefault(n => n.Name == dir);
                if (node == null)
                {
                    node = new Node(dir, 0, current, new List<Node>());
                    current.Children.Add(node);
                }
            }
            else if (char.IsNumber(l[0]))
            {
                var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var node = new Node(parts[1], int.Parse(parts[0]), current, null);
                current.Children.Add(node);
            }
            c++;
        }

        int result = 0;
        CalculateDirSize(root, ref result);

        return result.ToString();
    }


    private void CalculateDirSize(Node node, ref int current)
    {
        if (node.Size > 0)
            return;
        var size = 0;
        foreach (var child in node.Children)
        {
            if (child.Children != null)
            {
                CalculateDirSize(child, ref current);
            }
            size += child.Size;
        }
        node.Size = size;
        if (size <= 100000) 
            current += size;
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var root = new Node("/", 0, null, new List<Node>());
        var current = root;
        var c = 0;

        foreach (var l in data.Skip(1))
        {
            if (l == "$ cd ..")
            {
                current = current.Parent;
            }
            else if (l.StartsWith("$ cd"))
            {
                var dir = l.Substring(5);
                Node node = current.Children.FirstOrDefault(n => n.Name == dir);
                if (node == null)
                {
                    node = new Node(dir, 0, current, new List<Node>());
                    current.Children.Add(node);
                }
                current = node;
            }
            else if (l.StartsWith("dir"))
            {
                var dir = l.Substring(4);
                Node node = current.Children.FirstOrDefault(n => n.Name == dir);
                if (node == null)
                {
                    node = new Node(dir, 0, current, new List<Node>());
                    current.Children.Add(node);
                }
            }
            else if (char.IsNumber(l[0]))
            {
                var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var node = new Node(parts[1], int.Parse(parts[0]), current, null);
                current.Children.Add(node);
            }
            c++;
        }

        CalculateDirSize2(root);
        var limit = 30000000 - (70000000 - root.Size);
        var minNode = FindDir(root, limit, root);
        return minNode.Size.ToString();
    }

    private void CalculateDirSize2(Node node)
    {
        if (node.Size > 0)
            return;
        var size = 0;
        foreach (var child in node.Children)
        {
            if (child.Children != null)
            {
                CalculateDirSize2(child);
            }
            size += child.Size;
        }
        node.Size = size;
    }

    private Node FindDir(Node node, int limit, Node contender) {
        if (node.Children == null) {
            return contender;
        }
        if (node.Size >= limit && node.Size < contender.Size)
            contender = node;
        foreach(var child in node.Children) {
            contender = FindDir(child, limit, contender);
        }
        return contender;
    }
}

public class Node
{
    public Node(string name, int size, Node parent, List<Node> children)
    {
        Name = name;
        Size = size;
        Parent = parent;
        Children = children;
    }

    public string Name { get; set; }
    public int Size { get; set; }
    public Node Parent { get; set; }
    public List<Node> Children { get; set; }
}
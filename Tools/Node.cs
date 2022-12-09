namespace Tools;

public class Node<T>
{
    public Node(string name = default, T data = default, Node<T> parent = default, IList<Node<T>> children = default)
    {
        Name = name;
        Data = data;
        Parent = parent;
        Children = children;
    }

    public string Name { get; set; }

    public T Data { get; set; }

    public Node<T> Parent { get; set; }

    public IList<Node<T>> Children { get; set; }

}
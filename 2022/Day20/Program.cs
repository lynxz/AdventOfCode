using System.Diagnostics;
using Tools;

Day20 day = new();
day.OutputSecondStar();


public class Day20 : DayBase
{

    public Day20() : base("20")
    {

    }

    public override string FirstStar()
    {
        var data = GetIntRowData();
        var nodes = new List<LinkNode>();
        LinkNode lastLinkNode = new();

        for (int i = 0; i < data.Length; i++)
        {
            var node = new LinkNode { Value = data[i], Left = lastLinkNode };
            if (lastLinkNode != null)
                lastLinkNode.Right = node;
            nodes.Add(node);
            lastLinkNode = node;
        }

        var first = nodes[0];
        first.Left = lastLinkNode;
        lastLinkNode.Right = first;

        for (int p = 0; p < nodes.Count; p++)
        {
            var node = nodes[p];
            Debug.Assert(data[p] == node.Value);

            if (Math.Abs(data[p]) % data.Length != 0)
            {
                for (int i = 0; i < Math.Abs(data[p]) % (data.Length - 1); i++)
                {
                    node = data[p] > 0 ? node.Right : node.Left;
                }
                var ogNode = nodes[p];
                var left = ogNode.Left;
                var right = ogNode.Right;
                left.Right = right;
                right.Left = left;
                if (data[p] > 0)
                {
                    var temp = node.Right;
                    node.Right = ogNode;
                    ogNode.Left = node;
                    ogNode.Right = temp;
                    temp.Left = ogNode;
                }
                else
                {
                    var temp = node.Left;
                    node.Left = ogNode;
                    ogNode.Right = node;
                    ogNode.Left = temp;
                    temp.Right = ogNode;
                }
            }
        }


        var start = nodes.Single(n => n.Value == 0);
        var sum = 0L;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 1000; j++)
                start = start.Right;

            sum += start.Value;
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        const long key = 811589153;
        var data = GetIntRowData().Select(r => r * key).ToArray();
        var nodes = new List<LinkNode>();
        LinkNode lastLinkNode = new();


        for (int i = 0; i < data.Length; i++)
        {
            var node = new LinkNode { Value = data[i], Left = lastLinkNode };
            if (lastLinkNode != null)
                lastLinkNode.Right = node;
            nodes.Add(node);
            lastLinkNode = node;
        }

        var first = nodes[0];
        first.Left = lastLinkNode;
        lastLinkNode.Right = first;

        for (int s = 0; s < 10; s++)
        {
            for (int p = 0; p < nodes.Count; p++)
            {
                var node = nodes[p];
                Debug.Assert(data[p] == node.Value);

                if (Math.Abs(data[p]) % data.Length != 0)
                {
                    for (int i = 0; i < Math.Abs(data[p]) % (data.Length - 1); i++)
                    {
                        node = data[p] > 0 ? node.Right : node.Left;
                    }
                    var ogNode = nodes[p];
                    var left = ogNode.Left;
                    var right = ogNode.Right;
                    left.Right = right;
                    right.Left = left;
                    if (data[p] > 0)
                    {
                        // for (int j = 0; j < data[p] / data.Length; j++)
                        //     node = node.Right;
                        var temp = node.Right;
                        node.Right = ogNode;
                        ogNode.Left = node;
                        ogNode.Right = temp;
                        temp.Left = ogNode;
                    }
                    else
                    {
                        // for (int j = 0; j < -data[p] / data.Length; j++)
                        //     node = node.Left;
                        var temp = node.Left;
                        node.Left = ogNode;
                        ogNode.Right = node;
                        ogNode.Left = temp;
                        temp.Right = ogNode;
                    }
                }
            }
        }

        var start = nodes.Single(n => n.Value == 0);
        var sum = 0L;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 1000; j++)
                start = start.Right;

            sum += start.Value;
        }

        return sum.ToString();
    }
}

public class LinkNode
{

    public long Value { get; set; }

    public LinkNode Left { get; set; }

    public LinkNode Right { get; set; }

}
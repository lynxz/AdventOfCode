using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        Dictionary<string, List<(int nom, int denom, string name)>> _tree;
        Dictionary<string, int> _wasteList;

        void FirstStar()
        {
            GenerateTree();
            var minQuantity = GetMinimumOre();
            System.Console.WriteLine(minQuantity);
        }

        int GetMinimumOre()
        {
            var itemList = new List<(string node, int quantity)> { ("FUEL", 1) };

            while (itemList.Any(n => n.node != "ORE"))
            {
                var nodeName = string.Empty;
                foreach (var node in itemList)
                {
                    if (node.node == "ORE")
                    {
                        continue;
                    }
                    if (!HasOtherDependencies(node.node, itemList.Where(n => n != node)))
                    {
                        nodeName = node.node;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(nodeName))
                {
                    Expand(nodeName, itemList);
                    itemList = Consolidate(itemList);
                }
            }

            return itemList.First().quantity;
        }

        void SecondStar()
        {
            GenerateTree();
            var minQuantity = GetMinimumOre();
            var fuel = 1000000000000 / minQuantity;
            var waste = _wasteList.ToDictionary(k => k.Key, k => _wasteList[k.Key] * fuel);
            waste.Add("ORE", 1000000000000 - fuel * minQuantity);
            long oldValue = 0;
            while (oldValue != fuel)
            {
                oldValue = fuel;
                MergeDown("FUEL", waste);
                var mult = waste["ORE"] / minQuantity;
                foreach (var w in _wasteList)
                {
                    waste[w.Key] += w.Value * mult;
                }
                waste["ORE"] -= mult * minQuantity;
                fuel += mult;
            }
            System.Console.WriteLine(fuel);
            while(PullUp("FUEL", 1, waste)) {
                fuel++;
            }
            System.Console.WriteLine(fuel);
        }

        bool PullUp(string currentNode, long quantity, Dictionary<string, long> waste) {
            if (waste[currentNode] > quantity) {
                waste[currentNode] -= quantity;
                return true;
            };
            if (currentNode == "ORE") {
                return false;
            }
            var children = _tree[currentNode];
            var denom = children.First().denom;
            var required = quantity - waste[currentNode];
            var mult = required / denom;
            if (required % denom != 0) {
                mult++;
            }
            waste[currentNode] = mult*denom - required;
            return children.All(child => PullUp(child.name, child.nom * mult, waste));
        }

        void MergeDown(string currentNode, Dictionary<string, long> waste)
        {
            if (currentNode == "ORE")
            {
                return;
            }
            var children = _tree[currentNode];
            var denom = children.First().denom;
            var multiplier = waste[currentNode] / denom;
            waste[currentNode] -= multiplier * denom;
            foreach (var child in children)
            {
                waste[child.name] += multiplier * child.nom;
            }
            for (int i = children.Count - 1; i > -1; i-- )
            {
                MergeDown(children[i].name, waste);
            }
        }

        private void GenerateTree()
        {
            var regEx = new Regex(@"(\d+)\s([A-Z]+)");
            _tree = new Dictionary<string, List<(int nom, int denom, string name)>>();
            foreach (var line in GetData())
            {
                var parts = line.Split("=>");
                var match = regEx.Match(parts[1]);
                var denom = int.Parse(match.Groups[1].Value);
                var key = match.Groups[2].Value;
                var value = regEx.Matches(parts[0]).Select(m => (nom: int.Parse(m.Groups[1].Value), denom: denom, name: m.Groups[2].Value)).ToList();
                _tree.Add(key, value);
            }
            _wasteList = _tree.Keys.ToDictionary(k => k, k => 0);
        }

        bool HasOtherDependencies(string nodeName, IEnumerable<(string node, int quantity)> subNodes)
        {
            if (subNodes.Any(s => s.node == nodeName))
            {
                return true;
            }
            return subNodes
                .Where(s => _tree.ContainsKey(s.node))
                .Any(s => HasOtherDependencies(nodeName, _tree[s.node].Select(n => (n.name, n.nom))));
        }

        void Expand(string nodeName, List<(string node, int quantity)> itemList)
        {
            var node = itemList.First(n => n.node == nodeName);
            itemList.Remove(node);
            var treeNode = _tree[nodeName];
            var denom = treeNode.First().denom;
            var multiplier = node.quantity / denom;
            if (node.quantity % denom != 0)
            {
                multiplier++;
            }
            _wasteList[nodeName] += multiplier * denom - node.quantity;
            itemList.AddRange(treeNode.Select(n => (n.name, n.nom * multiplier)));
        }

        List<(string node, int quantity)> Consolidate(List<(string node, int quantity)> itemList) =>
             itemList.GroupBy(n => n.node).Select(g => (node: g.Key, quantity: g.Sum(n => n.quantity))).ToList();


        string[] GetData() =>
            File.ReadAllLines("input.txt");

    }
}
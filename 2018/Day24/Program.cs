using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        public void FirstStar()
        {
            var groups = new List<Group>();
            groups.AddRange(GetGroups(File.ReadAllLines("immune"), GroupType.Immune));
            groups.AddRange(GetGroups(File.ReadAllLines("infection"), GroupType.Infection));
            Battle(groups);
        }

        public void SecondStar()
        {
            for (int boost = 46; boost < 10000; boost++)
            {
                System.Console.Write("Boost " + boost + ":");
                var groups = new List<Group>();
                groups.AddRange(GetGroups(File.ReadAllLines("immune"), GroupType.Immune));
                groups.AddRange(GetGroups(File.ReadAllLines("infection"), GroupType.Infection));
                foreach (var group in groups.Where(g => g.GroupType == GroupType.Immune))
                {
                    group.AttackStrength += boost;
                }
                if (Battle(groups))
                {
                    System.Console.WriteLine("Done");
                    return;
                }
            }
        }

        private bool Battle(List<Group> groups)
        {
            while (true)
            {
                foreach (var deadGroup in groups.Where(g => g.Units <= 0).ToList())
                {
                    groups.Remove(deadGroup);
                }
                var immune = groups.Where(g => g.GroupType == GroupType.Immune).ToList();
                var infection = groups.Where(g => g.GroupType == GroupType.Infection).ToList();
                if (immune.Count == 0)
                {
                    System.Console.WriteLine(infection.Sum(s => s.Units));
                    return false;
                }
                if (infection.Count == 0)
                {
                    System.Console.WriteLine(immune.Sum(s => s.Units));
                    return true;
                }
                var battleList = new Dictionary<Group, Group>();
                foreach (var group in groups.OrderByDescending(g => g.Units * g.AttackStrength).ThenByDescending(g => g.Initiativ))
                {
                    var opposition = group.GroupType == GroupType.Immune ? infection : immune;
                    Group selectedTarget = null;
                    var maxDamage = 0;
                    foreach (var op in opposition.OrderByDescending(g => g.Units * g.AttackStrength).ThenByDescending(g => g.Initiativ))
                    {
                        var damage = CalculateDamage(group, op);
                        if (damage > maxDamage)
                        {
                            maxDamage = damage;
                            selectedTarget = op;
                        }
                    }
                    if (selectedTarget != null)
                    {
                        opposition.Remove(selectedTarget);
                        battleList.Add(group, selectedTarget);
                    }
                }
                foreach (var battle in battleList.OrderByDescending(kvp => kvp.Key.Initiativ))
                {
                    if (battle.Key.Units > 0)
                    {
                        var damage = CalculateDamage(battle.Key, battle.Value);
                        var kills = damage / battle.Value.HP;
                        battle.Value.Units -= kills;
                    }
                }
            }
        }

        int CalculateDamage(Group attacker, Group defender)
        {
            if (defender.Immune.Any(i => i == attacker.AttackType))
            {
                return 0;
            }
            if (defender.Weakness.Any(w => w == attacker.AttackType))
            {
                return 2 * attacker.AttackStrength * attacker.Units;
            }
            return attacker.AttackStrength * attacker.Units;
        }

        List<Group> GetGroups(string[] data, GroupType groupType)
        {
            var result = new List<Group>();
            foreach (var line in data)
            {
                var group = new Group { GroupType = groupType };
                group.Units = int.Parse(Regex.Match(line, @"(\d+) units").Groups[1].Value);
                group.HP = int.Parse(Regex.Match(line, @"(\d+) hit points").Groups[1].Value);
                group.Initiativ = int.Parse(Regex.Match(line, @"initiative (\d+)").Groups[1].Value);
                var m = Regex.Match(line, @"does (\d+) (fire|cold|radiation|slashing|bludgeoning)");
                group.AttackStrength = int.Parse(m.Groups[1].Value);
                group.AttackType = (Attack)Enum.Parse(typeof(Attack), m.Groups[2].Value);
                m = Regex.Match(line, @"immune to (?:(fire|cold|radiation|slashing|bludgeoning)(?:, )?)+(?:;|\))");
                group.Immune = m.Success ? m.Groups[1].Captures.Select(c => (Attack)Enum.Parse(typeof(Attack), c.Value)).ToArray() : new Attack[0];
                m = Regex.Match(line, @"weak to (?:(fire|cold|radiation|slashing|bludgeoning)(?:, )?)+(?:;|\))");
                group.Weakness = m.Success ? m.Groups[1].Captures.Select(c => (Attack)Enum.Parse(typeof(Attack), c.Value)).ToArray() : new Attack[0];
                result.Add(group);
            }
            return result;
        }

    }

    public enum Attack
    {
        slashing,
        bludgeoning,
        fire,
        cold,
        radiation
    }

    public enum GroupType
    {
        Immune,
        Infection
    }

    public class Group
    {

        public GroupType GroupType { get; set; }

        public int Units { get; set; }

        public int HP { get; set; }

        public int Initiativ { get; set; }

        public int AttackStrength { get; set; }

        public Attack AttackType { get; set; }

        public Attack[] Immune { get; set; }

        public Attack[] Weakness { get; set; }

    }


}

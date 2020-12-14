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
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        public void FirstStar()
        {
            var data = GetData();
            var mask = string.Empty;
            var mem = new Dictionary<int, long>();
            foreach (var line in data)
            {

                if (line.StartsWith("mask"))
                {
                    mask = line.Split('=')[1].Trim();
                }
                else
                {
                    var values = Regex.Matches(line, @"(\d+)").Select(m => long.Parse(m.Value)).ToArray();
                    AddValue(mem, Convert.ToInt32(values[0]), values[1], mask);
                }
            }

            System.Console.WriteLine(mem.Sum(kvp => kvp.Value));
        }

        void AddValue(Dictionary<int, long> mem, int address, long value, string mask)
        {
            mask = new string(mask.Reverse().ToArray());
            var newValue = 0L;
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                {
                    var tmp = (value >> i) & 1L;
                    newValue += tmp << i;
                }
                else if (mask[i] == '1')
                {
                    newValue += 1L << i;
                }
            }
            
            mem[address] = newValue;
        }

        public void SecondStar()
        {
            var data = GetData();
            var mask = string.Empty;
            var mem = new Dictionary<long, long>();
            foreach (var line in data)
            {

                if (line.StartsWith("mask"))
                {
                    mask = new string(line.Split('=')[1].Trim().Reverse().ToArray());
                }
                else
                {
                    var values = Regex.Matches(line, @"(\d+)").Select(m => long.Parse(m.Value)).ToArray();
                    var addresses = GenerateAddresses(mask, 0L, values[0], 0);
                    foreach (var address in addresses) {
                        mem[address] = values[1];
                    }
                }
            }
            System.Console.WriteLine(mem.Sum(kvp => kvp.Value));
        }

        IEnumerable<long> GenerateAddresses(string mask, long address, long value, int index) {
            if (index == 36) {
                return Enumerable.Repeat(address, 1);
            }

            if (mask[index] == 'X')
                {
                    var addr1 = GenerateAddresses(mask, address, value, index + 1);
                    address += 1L << index;
                    var addr2 = GenerateAddresses(mask, address, value, index + 1);
                    return addr1.Concat(addr2);
                }
                var tmp = (value >> index) & 1L;
                tmp = tmp | long.Parse(mask[index].ToString());
                address += tmp << index;
                return GenerateAddresses(mask, address, value, index +1);
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();

    }
}

// See https://aka.ms/new-console-template for more information
using System.Text;
using Tools;

var day = new Day4();
day.PostSecondStar();

public class Day4 : DayBase
{
    public Day4() : base("4")
    {
    }

    public override string FirstStar()
    {
        var input = "bgvyzdsv";

        for (int i = 0; i < int.MaxValue; i++) {
            var data = $"{input}{i}";
            var hash = CreateMD5(data);
            if (hash.Take(5).All(c => c == '0')) 
                return i.ToString();
        }
        return "-1";
    }

    public override string SecondStar()
    {
       var input = "bgvyzdsv";

        for (int i = 0; i < int.MaxValue; i++) {
            var data = $"{input}{i}";
            var hash = CreateMD5(data);
            if (hash.Take(6).All(c => c == '0')) 
                return i.ToString();
        }
        return "-1";
    }

    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
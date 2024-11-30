using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mocza_szilvia_rbvvps
{
    internal class Program
    {
        static string[,] UresTabla()
        {
            string[,] tabla = new string[13, 13];
            tabla[0, 0] = ""; 
            string[] honapok = { "január", "február", "március", "április", "május", "június", "július", "augusztus", "szeptember", "október", "november", "december" };
  
            for (int i = 0; i < 12; i++)
                tabla[0, i + 1] = honapok[i];
      
            string[] lakasok = { "Fsz1", "Fsz2", "Fsz3", "1.1", "1.2", "1.3", "2.1", "2.2", "2.3", "3.1", "3.2", "3.3" };
            for (int i = 0; i < lakasok.Length; i++)
                tabla[i + 1, 0] = lakasok[i];

            for (int i = 1; i < tabla.GetLength(0); i++)
                for (int j = 1; j < tabla.GetLength(1); j++)
                    tabla[i, j] = "0";

            return tabla;
        }

        static void KiIr(string[,] tabla)
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    Console.Write($"{tabla[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        static bool AdatBeir(ref string[,] matrix, string lakas, string honap)
        {
            int lakasIndex = -1;
            int honapIndex = -1;

            for (int i = 1; i < matrix.GetLength(0); i++)
                if (matrix[i, 0] == lakas)
                {
                    lakasIndex = i;
                    break;
                }

            for (int j = 1; j < matrix.GetLength(1); j++)
                if (matrix[0, j] == honap)
                {
                    honapIndex = j;
                    break;
                }

            if (lakasIndex == -1 || honapIndex == -1)
                return false;

            if (matrix[lakasIndex, honapIndex] != "0")
            {
                Console.WriteLine("Már fizetve.");
                return false;
            }

            matrix[lakasIndex, honapIndex] = "4500";
            return true;

        }


        static void Mentes(string[,] matrix, string fajlNev)
        {
            using (StreamWriter writer = new StreamWriter(fajlNev))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    string[] sor = new string[matrix.GetLength(1)];
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        sor[j] = matrix[i, j];
                    }
                    writer.WriteLine(string.Join(";", sor));
                }
            }
        }

        static string[,] Megnyitas(string fajlNev)
        {
            string[] sorok = File.ReadAllLines(fajlNev);
            string[,] matrix = new string[sorok.Length, sorok[0].Split(';').Length];

            for (int i = 0; i < sorok.Length; i++)
            {
                string[] adat = sorok[i].Split(';'); 
                for (int j = 0; j < adat.Length; j++)
                {
                    matrix[i, j] = adat[j];
                }
            }
            return matrix;
        }

        static string[] Hatralek(string[,] matrix, string honap)
        {
            int honapIndex = -1;
            List<string> hatraleKosLakasok = new List<string>();

            for (int j = 1; j < matrix.GetLength(1); j++)
            {
                if (matrix[0, j] == honap)
                {
                    honapIndex = j;
                    break;
                }
            }

            if (honapIndex == -1)
            {
                Console.WriteLine("Hibás hónapnév!");
                return new string[0];
            }

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j <= honapIndex; j++)
                {
                    if (matrix[i, j] == "0")
                    {
                        hatraleKosLakasok.Add(matrix[i, 0]);
                        break;
                    }
                }
            }

            return hatraleKosLakasok.ToArray();
        }

        static void Main(string[] args)
        {
            string[,] tabla = UresTabla();

            Console.WriteLine("Üres táblázat:");
            KiIr(tabla);

            Console.WriteLine("Adat beírása: Fsz1 január");
            AdatBeir(ref tabla, "Fsz2", "január");
            Console.WriteLine("Adat beírása: 1.1 július");
            AdatBeir(ref tabla, "1.1", "július");

            while (true)
            {
                Console.WriteLine("Szeretne új adatot beadni? (igen/nem)");
                string valasz = Console.ReadLine().ToLower();

                if (valasz == "nem")
                {
                    Console.WriteLine("Nem adott meg adatot!");
                    break;
                }
                else if (valasz == "igen")
                {
                    string lakasNev = "";
                    string honapNev = "";

                    while (true)
                    {

                        Console.WriteLine("Adja meg a lakás nevét (pl. Fsz1):");
                        lakasNev = Console.ReadLine();
                        break;
                    }

                    while (true)
                    {
                        Console.WriteLine("Adja meg a hónap nevét (pl. január):");
                        honapNev = Console.ReadLine();                   
                            break;
                    }

                    if (AdatBeir(ref tabla, lakasNev, honapNev))
                        Console.WriteLine($"Befizetve: {lakasNev} - {honapNev}");
                }
                else
                {
                    Console.WriteLine("Hibás válasz, kérem írjon 'igen' vagy 'nem'-et.");
                }
            }

            Console.WriteLine("\nAdatok beírása után:");
            KiIr(tabla);

            Mentes(tabla, "koltseg.csv");

            string[,] ujTabla = Megnyitas("koltseg.csv");
            Console.WriteLine("\nFájl visszaolvasása:");
            KiIr(ujTabla);

            Console.WriteLine("Kérem a hónap nevét, ameddig a hátralékosokat keressük:");
            string honap = Console.ReadLine();
            string[] hatraleKosok = Hatralek(tabla, honap);

            Console.WriteLine($"Hátralékos lakások {honap}-ig:");
            foreach (string lakas in hatraleKosok)
            {
                Console.WriteLine(lakas);
            }
            Console.WriteLine("A program véget ért. Nyomjon meg egy gombot a kilépéshez...");
            Console.ReadKey();
        }
    }
}
 
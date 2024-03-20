using System;
using System.Collections.Generic;


class SzinhaziJegyErtekesitoProgram
{
    static char[,] nezoTere = new char[16, 15];
    static Dictionary<string, List<int>> foglalasok = new Dictionary<string, List<int>>();

    static void Main()
    {
        RandomFoglalasokGeneralo(10);

        while (true)
        {
            Console.Clear();
            KiirMenu();
            string valasztas = Console.ReadLine();

            switch (valasztas)
            {
                case "1":
                    NezetMegjelenites();
                    break;
                case "2":
                    SzabadHelyekKijelzese();
                    break;
                case "3":
                    Foglalas();
                    break;
                case "4":
                    FoglalasModositasa();
                    break;
                case "5":
                    FoglalasTorlese();
                    break;
                case "6":
                    Mentese();
                    break;
                case "7":
                    Betoltes();
                    break;
                case "8":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Kérem, válasszon újra!");
                    break;
            }

            Console.WriteLine("\nNyomjon meg egy gombot a folytatáshoz...");
            Console.ReadKey();
        }
    }

    static void KiirMenu()
    {
        Console.WriteLine("***** SZÍNHÁZI JEGY ÉRTÉKESÍTŐ PROGRAM *****");
        Console.WriteLine("1. Nézőtér megjelenítése");
        Console.WriteLine("2. Szabad helyek kijelzése");
        Console.WriteLine("3. Foglalás");
        Console.WriteLine("4. Foglalás módosítása");
        Console.WriteLine("5. Foglalás törlése");
        Console.WriteLine("6. Mentés");
        Console.WriteLine("7. Betöltés");
        Console.WriteLine("8. Kilépés");
        Console.Write("Kérem, válasszon egy menüpontot (1-8): ");
    }

    static void NezetMegjelenites()
    {
        Console.WriteLine("***** NÉZŐTÉR MEGJELENÍTÉSE *****");
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                Console.Write(nezoTere[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void SzabadHelyekKijelzese()
    {
        Console.WriteLine("***** SZABAD HELYEK KIJELZÉSE *****");
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (nezoTere[i, j] == 'S')
                {
                    Console.WriteLine($"Szabad hely: Sor {i + 1}, Szék {j + 1}");
                }
            }
        }
    }


    static void Foglalas()
    {
        Console.WriteLine("***** FOGLALÁS *****");
        Console.Write("Adja meg a nevét: ");
        string nev = Console.ReadLine();

        if (foglalasok.ContainsKey(nev))
        {
            Console.WriteLine("Már van foglalás ezen a néven. Kérem, adjon meg másik nevet.");
            return; // Kilépés a foglalásból, ha már van foglalás ezen a néven
        }

        Console.Write("Hány jegyet szeretne foglalni? ");
        int db = int.Parse(Console.ReadLine());

        List<int> foglaltHelyek = new List<int>();
        Console.WriteLine("Válassza ki a szabad helyeket:");

        for (int i = 0; i < db; i++)
        {
            Console.Write($"Hely {i + 1}: ");
            int hely = int.Parse(Console.ReadLine());

            if (nezoTere[(hely - 1) / 15, (hely - 1) % 15] == 'S')
            {
                nezoTere[(hely - 1) / 15, (hely - 1) % 15] = 'F';
                foglaltHelyek.Add(hely);
            }
            else
            {
                Console.WriteLine("A választott hely foglalt vagy érvénytelen. Kérem, válasszon újat.");
                i--;
            }
        }

        foglalasok.Add(nev, foglaltHelyek);
        Console.WriteLine("Foglalás sikeres!");
    }


    static void FoglalasModositasa()
    {
        Console.WriteLine("***** FOGLALÁS MÓDOSÍTÁSA *****");
        Console.Write("Adja meg a nevét: ");
        string nev = Console.ReadLine();

        if (foglalasok.ContainsKey(nev))
        {
            Console.WriteLine("Jelenlegi foglalásai:");

            foreach (int hely in foglalasok[nev])
            {
                Console.WriteLine($"- Hely {hely}");
            }

            Console.Write("Melyik foglalást szeretné módosítani? ");
            int modositandoHely = int.Parse(Console.ReadLine());

            if (foglalasok[nev].Contains(modositandoHely))
            {
                nezoTere[(modositandoHely - 1) / 15, (modositandoHely - 1) % 15] = 'S';
                foglalasok[nev].Remove(modositandoHely);

                Console.WriteLine("Módosítás sikeres!");
            }
            else
            {
                Console.WriteLine("Érvénytelen foglalás. Kérem, válasszon újat.");
            }
        }
        else
        {
            Console.WriteLine("Nincs ilyen névvel rendelkező foglalás.");
        }
    }

    static void FoglalasTorlese()
    {
        Console.WriteLine("***** FOGLALÁS TÖRLÉSE *****");
        Console.Write("Adja meg a nevét: ");
        string nev = Console.ReadLine();

        if (foglalasok.ContainsKey(nev))
        {
            Console.WriteLine("Jelenlegi foglalásai:");

            foreach (int hely in foglalasok[nev])
            {
                Console.WriteLine($"- Hely {hely}");
                nezoTere[(hely - 1) / 15, (hely - 1) % 15] = 'S';
            }

            foglalasok.Remove(nev);
            Console.WriteLine("Törlés sikeres!");
        }
        else
        {
            Console.WriteLine("Nincs ilyen névvel rendelkező foglalás.");
        }
    }

    static string mentettFajl = @"C:\Users\kismu\OneDrive\foglalasok.txt";

    static void Mentese()
    {
        using (StreamWriter writer = new StreamWriter(mentettFajl))
        {
            foreach (var kvp in foglalasok)
            {
                writer.WriteLine($"{kvp.Key},{string.Join(",", kvp.Value)}");
            }
        }
        Console.WriteLine("Foglalások mentve.");
    }



    static void Betoltes()
    {
        foglalasok.Clear(); 

        if (File.Exists(mentettFajl))
        {
            using (StreamReader reader = new StreamReader(mentettFajl))
            {
                string sor;
                while ((sor = reader.ReadLine()) != null)
                {
                    string[] adatok = sor.Split(',');
                    string nev = adatok[0];
                    List<int> helyek = new List<int>();

                    for (int i = 1; i < adatok.Length; i++)
                    {
                        helyek.Add(int.Parse(adatok[i]));
                    }

                    foglalasok.Add(nev, helyek);
                }
            }
            Console.WriteLine("Foglalások betöltve.");
        }
        else
        {
            Console.WriteLine("Nem található mentett fájl.");
        }
    }


    static void RandomFoglalasokGeneralo(int szazalek)
    {
        Random random = new Random();

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (random.Next(100) < szazalek)
                {
                    nezoTere[i, j] = 'F';
                }
                else
                {
                    nezoTere[i, j] = 'S';
                }
            }
        }
    }
}


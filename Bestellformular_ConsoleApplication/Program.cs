using System;
using System.Collections.Generic;
using static System.Console;
using static System.ConsoleColor;

namespace Bestellformular_ConsoleApplication
{
    class Program
    {
        struct Bestellzeile
        {
            public string bezeichnung;
            public double einzelpreis;
            public int bestellmenge;
            public double zeilenpreis;
        };
        static string hinweis = "Alle Preise netto, zzgl. USt.";    // muss static deklariert werden
        public static object gesamtpreis { get; private set; }
        static double mwst; // hier wird die mwst angegeben
        // zugriff Rückgabetyp name (parameter)
        static void Main(string[] args)
        {
            Dictionary<string, double> saetze =
            new Dictionary<string, double>();
            saetze.Add("D7", 0.07);
            saetze.Add("D19", 0.19);
            saetze.Add("GB", 0.15);
            saetze.Add("SW", 0.25);

            Bestellzeile[] bestellung = new Bestellzeile[]
                { new Bestellzeile {bezeichnung ="Kaffee " ,einzelpreis=13.00,bestellmenge=0,zeilenpreis=0.0 },
                  new Bestellzeile {bezeichnung ="Tee " ,einzelpreis=3.20,bestellmenge=0,zeilenpreis=0.0 },
                  new Bestellzeile {bezeichnung ="Phone " ,einzelpreis= 87.99,bestellmenge=0,zeilenpreis=0.0 },
                  new Bestellzeile {bezeichnung ="Printer",einzelpreis=236.39,bestellmenge=0,zeilenpreis=0.0 },
                  new Bestellzeile {bezeichnung ="Desktop",einzelpreis=986.99,bestellmenge=0,zeilenpreis=0.0 }
                };
            int oldCursorTop = 9;
            ConsoleKeyInfo meinKey;
            do
            {
                int startLeft = 4; int startTop = 8;
                int breiteBez = 7; int breiteEp = 7;
                SetCursorPosition(startLeft, startTop);
                Write("Bezeichnung\tE.Preis\tBestellmenge\tPreis");
                ForegroundColor = DarkCyan;
                    double gesamtpreis = 0;
                foreach (Bestellzeile zeile in bestellung)  // nimmt zeilenweise Daten
                {
                    SetCursorPosition(4, ++CursorTop); ;
                    Write("{0,-7}\t{1,7:F2}\t{2,6:D}\t{3,13:F2}", zeile.bezeichnung, zeile.einzelpreis, zeile.bestellmenge, zeile.zeilenpreis);
                        // -7 von links, 7 breit
                    gesamtpreis += zeile.zeilenpreis; 
                }
                hinweiseAusgeben(22, 22, Red); // Aufruf der Funktion
                CursorSize = 10;
                SetCursorPosition(startLeft, 18);
                Write("Gesamtpreis:\t{0:F2}", gesamtpreis);
                steuerBerechnen(gesamtpreis, 0.19);
                // steuerBerechnen hier ausgeben:
                SetCursorPosition(startLeft, CursorTop + 1);
                Write("Ust beträgt:\t{0:F2}", mwst);
                SetCursorPosition(startLeft, CursorTop + 3);
                steuern(saetze);

                SetCursorPosition(breiteBez + 8 + breiteEp + 8, oldCursorTop);
                meinKey = Console.ReadKey(true);
                if (!Char.IsNumber(meinKey.KeyChar))
                {
                    switch (meinKey.Key.ToString())
                    {
                        case "UpArrow": SetCursorPosition(CursorLeft, (CursorTop > startTop + 1) ? --CursorTop : CursorTop); break;
                        case "DownArrow": SetCursorPosition(CursorLeft, (CursorTop < (startTop + 5)) ? ++CursorTop : CursorTop); break;
                        case "RightArrow":
                            bestellung[CursorTop - 9].bestellmenge++;
                            bestellung[CursorTop - 9].zeilenpreis += bestellung[CursorTop - 9].einzelpreis; break;
                        case "LeftArrow":
                            if (bestellung[CursorTop - 9].bestellmenge > 0)
                            {
                                bestellung[CursorTop - 9].bestellmenge--;
                                bestellung[CursorTop - 9].zeilenpreis -= bestellung[CursorTop - 9].einzelpreis;
                            }
                            break;
                        default: break;
                    }
                    oldCursorTop = CursorTop;
                }
                else
                {
                    int num = int.Parse(meinKey.KeyChar.ToString());
                    bestellung[CursorTop - 9].bestellmenge = num;
                    bestellung[CursorTop - 9].zeilenpreis = num * bestellung[CursorTop - 9].einzelpreis;
                    Write(num);
                }
            } while (meinKey.Key != ConsoleKey.Escape);
            ReadLine();
        }   // end of main
        // funktion hier
        static void hinweiseAusgeben()
        {
            Write("\tHinweis: ");
        }
        static void hinweiseAusgeben(int links, int oben)
        {   // Überladung
            SetCursorPosition(links, oben);
            Write("Hinweis: ");
        }
        static void hinweiseAusgeben(int links, int oben, ConsoleColor farbe)
        {   // Überladung für Farbe und hinzufügen vom hinweis
            ConsoleColor hilf = ForegroundColor;    // vorherige Farbe
            ForegroundColor = farbe;
            SetCursorPosition(links, oben);
            Write("Hinweis: {0}", hinweis);
            ForegroundColor = hilf;     // Aufruf vorheriger Farbe
        }
        static double steuerBerechnen(double netto, double ustSatz)
        {
            return mwst = netto * ustSatz;
        }
        static double steuerBerechnen(double netto)
        {   // 19% Steuersatz
            return steuerBerechnen(netto, 19.0);
        }

        static double steuern(Dictionary<string,double> land)
        {
            double value;
            foreach(KeyValuePair<string, double> satz in land)
            {
                Write("\n\tDas Land {0} hat einen Umsatzsteuersatz von {1}",
                    satz.Key, satz.Value);
            }
            Write("\n\tBitte wählen Sie ihr Zielland aus: ");
            string country = ReadLine();
            if (land.TryGetValue(country, out value))
            {
                return value;
            }
            else
            {
                WriteLine("Land nicht gefunden");
            }
            return 19;
        }
    } // end of Class Program
}
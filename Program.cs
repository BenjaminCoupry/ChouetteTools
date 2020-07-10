using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChouetteTools
{
    class Program
    {
        static void Main(string[] args)
        {
            ConstruireCorrespondances("H:/Mes Fichiers/Ressources Programmation/lexique/dictionary.txt", new List<string>() {"rouge","orange","jaune","vert","bleu","indigo","violet" }, "D:/lab/chouette");
            
        }
        public static bool motConstructible(string mot_, List<string> BaseMots)
        {
            string mot = SansAccents(mot_);
            if(mot.Length != BaseMots.Count)
            {
                return false;
            }
            else
            {
                bool ok = true;
                for(int i=0;i<mot.Length;i++)
                {
                    if(!BaseMots[i].Contains(mot[i]))
                    {
                        ok = false;
                        break;
                    }
                }
                return ok;
            }
        }
        public static List<string> motsConstructibles(List<string>Mots, List<string>BaseMots)
        {
            List<string> retour = new List<string>();
            int n = 0;
            foreach(string mot in Mots)
            {
                if(motConstructible(mot,BaseMots))
                {
                    n++;
                    if(n%1000==0)
                    {
                        Console.WriteLine(n + "/" + Mots.Count);
                    }
                    retour.Add(mot);
                }
            }
            return retour;
        }
        public static void ConstruireCorrespondances(string PathDict, List<string> motsDeBase, string Path)
        {
            List<string> Mots = System.IO.File.ReadAllLines(PathDict, Encoding.GetEncoding("iso-8859-1")).ToList<string>();
            List<List<string>> ListeBases = construireBases(motsDeBase);
            Dictionary<string, int> ndict = new Dictionary<string, int>();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path + ".txt"))
            {
                int n = 0;
                foreach (List<string> Base in ListeBases)
                {
                    n++;
                    Console.WriteLine("Base " + n + "/" + ListeBases.Count);
                    file.WriteLine("Base : ");
                    foreach (string mb in Base)
                    {
                        file.Write(mb + "; ");
                    }
                    file.WriteLine();
                    List<string> mc = motsConstructibles(Mots, Base);
                    foreach (string m in mc)
                    {
                        file.WriteLine(m);
                        if (!ndict.ContainsKey(m))
                        {
                            ndict[m] = 1;
                        }
                        else
                        {
                            ndict[m]++;
                        }
                    }
                }
            }
            using (System.IO.StreamWriter dct = new System.IO.StreamWriter(Path + "_motsSeuls.txt"))
            {
                foreach(KeyValuePair<string,int> kv in ndict)
                {
                    dct.WriteLine(kv.Key + "(" + kv.Value + ")");
                }
                dct.WriteLine(ndict.Count);
            }
        }
        public static string SansAccents(string input)
        {
            string accentedStr = input;
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }

        private static List<string> listeSansN(List<string> mots, int N)
        {
            List<string> retour = new List<string>();
            for(int i=0;i<mots.Count;i++)
            {
                if(i!=N)
                {
                    retour.Add(mots[i]);
                }
            }
            return retour;
        }
        public static List<List<string>> construireBases(List<string> motsRestants)
        {
            List<List<string>> retour = new List<List<string>>();
            if (motsRestants.Count == 1)
            {
                retour.Add(new List<string> { motsRestants.ElementAt(0) });
            }
            else
            {
                for (int i = 0; i < motsRestants.Count; i++)
                {
                    List<List<string>> sub = construireBases(listeSansN(motsRestants, i));
                    foreach (List<string> b in sub)
                    {
                        b.Add(motsRestants[i]);
                        retour.Add(b);
                    }
                }
            }
            return retour;
        }
    }
}

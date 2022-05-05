using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Diagnostics;

namespace OS_2_Somonov_A_V_BBBO_10_20
{
    class Program
    {
        static List<string> hashes = new List<string>();
        static object removeLock = new object();
        static Stopwatch timer = new Stopwatch();
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Введите кол-во потоков: ");
            var threadCount = int.Parse(Console.ReadLine());
            hashes.AddRange(File.ReadAllLines("C:\\Users\\User\\Desktop\\Учеба, 2 курс, 4 сем\\Операционные системы\\Практики\\ОС2\\OS_2_Somonov_A_V_BBBO_10_20\\hashes.txt"));

            foreach (var hash in hashes)
                Console.WriteLine(hash);

            Console.WriteLine("Брутфорс запущен");
            timer.Start();

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(BruteForce);
                threads.Add(thread);
                thread.Start();
            }

            while (threads.Where(a => a.IsAlive).Count() > 0)
                Thread.Sleep(500);

            Console.WriteLine("Брутфорс окончен");
            Console.ReadKey();
        }

        static void BruteForce()
        {
            for (char c1 = 'a'; c1 <= 'z'; c1++)
            {
                for (char c2 = 'a'; c2 <= 'z'; c2++)
                {
                    for (char c3 = 'a'; c3 <= 'z'; c3++)
                    {
                        for (char c4 = 'a'; c4 <= 'z'; c4++)
                        {
                            for (char c5 = 'a'; c5 <= 'z'; c5++)
                            {
                                var pass = "" + c1 + c2 + c3 + c4 + c5;
                                var passHash = sha256(pass);

                                List<string> localHashes = new List<string>(hashes);

                                for (int k = 0; k < localHashes.Count; k++)
                                {
                                    var hash = localHashes[k];
                                    if (passHash == hash)
                                    {
                                        try
                                        {
                                            lock (removeLock)
                                            {
                                                hashes.Remove(hash);
                                            }

                                            Console.WriteLine(pass);
                                            Console.WriteLine(timer.Elapsed.ToString());
                                        }
                                        catch (Exception) { }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}

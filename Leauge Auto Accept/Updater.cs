using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leauge_Auto_Accept
{
    internal class Updater
    {
        public static string appVersion;

        public static void initialize()
        {
            Version appVersionTmp = Assembly.GetExecutingAssembly().GetName().Version;
            appVersion = appVersionTmp.Major + "." + appVersionTmp.Minor;
            if (!Settings.disableUpdateCheck)
            {
                versionCheck();
            }
        }

        private static void versionCheck()
        {
            Console.Clear();
            Print.printCentered("Güncellemeler kontrol ediliyor...", SizeHandler.HeightCenter);
            string[] latestRelease = webRequest("https://api.github.com/repos/IcarusExE/LeagueAutoAccept/releases/latest");
            if (latestRelease[0] != "200")
            {
                // Network error
                Console.Clear();
                Print.printCentered("Güncellemeler kontrol edilirken hata ile karşılaştım.", SizeHandler.HeightCenter - 1);
                Print.printCentered("Bu özelliği ayarlardan kapatabilirsin.");
                Print.printCentered("Uygulama 5 saniye içerisinde başlatılıyor.");
                Thread.Sleep(5000);
            }
            else
            {
                try
                {
                    string latestTag = latestRelease[1].Split("tag_name\": \"")[1].Split("\"")[0];
                    if ('v' + appVersion == latestTag)
                    {
                        // Running latest version, no update found/needed
                        Console.Clear();
                        Print.printCentered("Güncelleme bulunamadı. Zaten en güncel sürümü kullanıyorsun.", SizeHandler.HeightCenter);
                        Thread.Sleep(178);
                        return;
                    }
                    else
                    {
                        // Running an different version than the latest release, suggest an update
                        Console.Clear();
                        Print.printCentered("Bir güncelleme bulundu, yükleniyor.", SizeHandler.HeightCenter - 3);
                        Print.printCentered("Şu anki sürüm v" + appVersion + ", En güncel sürüm ise " + latestTag);

                        Print.printCentered("Yapımcı:", SizeHandler.HeightCenter);
                        Print.printCentered("Icarus");

                        Print.printCentered("Bu özelliği ayarlardan kapatabilirsin.", SizeHandler.HeightCenter + 3);
                        Print.printCentered("Uygulama 5 saniye içerisinde başlatılıyor.");

                        Thread.Sleep(5000);
                    }
                }
                catch
                {
                    // Default case, in case github changes the json format or something idk
                    Console.Clear();
                    Print.printCentered("Güncellemeler kontrol edilirken hata ile karşılaştı.", SizeHandler.HeightCenter - 1);
                    Print.printCentered("Bu özelliği ayarlardan kapatabilirsin.");
                    Print.printCentered("Uygulama 5 saniye içerisinde başlatılıyor.");
                    Thread.Sleep(5000);
                }
            }
        }

        public static string[] webRequest(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set URL, User-Agent
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

                    // Get the response
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        string content = response.Content.ReadAsStringAsync().Result;

                        return new[] { ((int)response.StatusCode).ToString(), content };
                    }
                }
            }
            catch
            {
                // If the URL is invalid
                string[] output = { "999", "" };
                return output;
            }
        }
    }
}

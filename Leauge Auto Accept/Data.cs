﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leauge_Auto_Accept
{
    public class itemList
    {
        public string name { get; set; }
        public string id { get; set; }
        public bool free { get; set; }
    }

    internal class Data
    {
        public static List<itemList> champsSorterd = new List<itemList>();
        public static List<itemList> spellsSorted = new List<itemList>();

        public static string currentSummonerId = "";
        public static string currentChatId = "";

        public static void loadSummonerId()
        {
            if (currentSummonerId == "")
            {
                Print.printCentered("Sihirdar kimliğini alınıyor...", 15);
                string[] currentSummoner = LCU.clientRequestUntilSuccess("GET", "lol-summoner/v1/current-summoner");
                Console.Clear();
                currentSummonerId = currentSummoner[1].Split("summonerId\":")[1].Split(',')[0];
            }
        }

        public static void loadPlayerChatId()
        {
            string[] myChatProfile = LCU.clientRequest("GET", "lol-chat/v1/me");
            currentChatId = myChatProfile[1].Split("\"id\":\"")[1].Split("\",")[0];
            currentSummonerId = myChatProfile[1].Split("\"summonerId\":")[1].Split(",\"")[0];
        }

        public static void loadChampionsList()
        {
            Console.Clear();

            if (!champsSorterd.Any())
            {
                loadSummonerId();

                List<itemList> champs = new List<itemList>();

                Print.printCentered("Sahip olduğunuz şampiyonların listesi alınıyor...", 15);
                string[] ownedChamps = LCU.clientRequestUntilSuccess("GET", "lol-champions/v1/inventories/" + currentSummonerId + "/champions-minimal");
                Console.Clear();
                string[] champsSplit = ownedChamps[1].Split("},{");

                foreach (var champ in champsSplit)
                {
                    string champName = champ.Split("name\":\"")[1].Split('"')[0];
                    string champId = champ.Split("id\":")[1].Split(',')[0];
                    string champOwned = champ.Split("owned\":")[1].Split(',')[0];
                    string champFreeXboxPass = champ.Split("xboxGPReward\":")[1].Split('}')[0];
                    string champFree = champ.Split("freeToPlay\":")[1].Split(',')[0];

                    // For some reason Riot provides a "None" champion
                    if (champName == "None")
                    {
                        continue;
                    }

                    // Fuck the yeti
                    if (champName == "Nunu & Willump")
                    {
                        champName = "Nunu";
                    }

                    // Check if the champ can be picked
                    bool isAvailable;
                    if (champOwned == "true" || champFree == "true" || champFreeXboxPass == "true")
                    {
                        isAvailable = true;
                    }
                    else
                    {
                        isAvailable = false;
                    }
                    champs.Add(new itemList() { name = champName, id = champId, free = isAvailable });
                }

                // Sort alphabetically
                champsSorterd = champs.OrderBy(o => o.name).ToList();
            }

            SizeHandler.resizeBasedOnChampsCount();
            Console.Clear();
        }

        public static void loadSpellsList()
        {
            Console.Clear();
            if (!spellsSorted.Any())
            {
                loadSummonerId();

                List<string> enabledSpells = new List<string>();

                Print.printCentered("Mevcut sihirdar büyülerinin listesi alınıyor...", 15);
                string[] availableSpells = LCU.clientRequestUntilSuccess("GET", "lol-collections/v1/inventories/" + currentSummonerId + "/spells");
                Console.Clear();
                string[] spellsSplit = availableSpells[1].Split('[')[1].Split(']')[0].Split(',');

                Print.printCentered("Mevcut oyun modlarının listesi alınıyor...", 15);
                string[] platformConfig = LCU.clientRequestUntilSuccess("GET", "lol-platform-config/v1/namespaces");
                Console.Clear();
                string[] enabledGameModes = platformConfig[1].Split("EnabledModes\":[")[1].Split(']')[0].Split(',');
                string[] inactiveSpellsPerGameMode = platformConfig[1].Split("gameModeToInactiveSpellIds\":{")[1].Split('}')[0].Split("],");

                Console.Clear();
                foreach (var gameMode in enabledGameModes)
                {
                    foreach (var gameMode2 in inactiveSpellsPerGameMode)
                    {
                        string gameMode2tmp = gameMode2 + "]".Replace("]]", "]");
                        string gameMode2Name = gameMode2tmp.Split(':')[0];
                        if (gameMode == gameMode2Name)
                        {
                            string[] inactiveSpells = gameMode2tmp.Split('[')[1].Split(']')[0].Split(',');
                            foreach (var spell in spellsSplit)
                            {
                                bool isActive = true;
                                foreach (var spellInactive in inactiveSpells)
                                {
                                    if (spell + ".0" == spellInactive)
                                    {
                                        isActive = false;
                                        break;
                                    }
                                }
                                if (isActive)
                                {
                                    enabledSpells.Add(spell);
                                }
                            }
                        }
                    }
                }

                // Remove dupes
                enabledSpells = enabledSpells.Distinct().ToList();

                // Get sepll names
                Print.printCentered("Sihirdar büyülerinin adı alınıyor...", 15);
                string[] spellsJson = LCU.clientRequest("GET", "lol-game-data/assets/v1/summoner-spells.json");
                Console.Clear();
                string[] spellsJsonSplit = spellsJson[1].Split('{');

                // Add to list with names
                foreach (var spell in enabledSpells)
                {
                    string spellName = "";
                    foreach (var spellSingle in spellsJsonSplit)
                    {
                        if (spellSingle == "[" || spellSingle == "]")
                        {
                            continue;
                        }
                        string spellId = spellSingle.Split("id\":")[1].Split(',')[0];
                        if (spell == spellId)
                        {
                            spellName = spellSingle.Split("name\":\"")[1].Split('"')[0];
                        }
                    }
                    spellsSorted.Add(new itemList() { name = spellName, id = spell });
                }

                // Sort alphabetically
                spellsSorted = spellsSorted.OrderBy(o => o.name).ToList();
            }
        }
    }
}

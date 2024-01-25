using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace Leauge_Auto_Accept
{
    internal class UI
    {
        public static string currentWindow = "";
        public static string previousWindow = "";

        public static int currentChampPicker = 0;
        public static int currentSpellSlot = 0;

        public static int totalChamps = 0;
        public static int totalSpells = 0;

        // normal/+grid/pages/nocursor/messageEdit
        public static string windowType = "";
        public static int messageIndex = 0; //index for the message currently being edit

        public static int totalRows = SizeHandler.WindowHeight - 2;
        public static int columnSize = 20;
        public static int topPad = 0;
        public static int leftPad = 0;
        public static int maxPos = 0;
        public static int currentPage = 0;
        public static int totalPages = 0;

        private static int[] cursorPositionValue = { 0, 0 };
        public static int[] cursorPosition
        {
            get { return cursorPositionValue; }
            set
            {
                cursorPositionValue = value;
                Console.SetCursorPosition(value[0], value[1]);
            }
        }

        private static bool showCursorValue = false;
        public static bool showCursor
        {
            get { return showCursorValue; }
            set
            {
                if (showCursorValue != value)
                {
                    Console.CursorVisible = value;
                }
                showCursorValue = value;
            }
        }

        public static void initializingWindow()
        {
            Print.canMovePos = false;
            currentWindow = "initializing";
            windowType = "nocursor";
            showCursor = false;

            Print.printCentered("Başlatılıyor...", SizeHandler.HeightCenter);
        }

        public static void leagueClientIsClosedMessage()
        {
            Print.canMovePos = false;
            currentWindow = "leagueClientIsClosedMessage";
            windowType = "nocursor";
            showCursor = false;

            Console.Clear();

            Print.printCentered("LoL istemcisi bulunamadı.", SizeHandler.HeightCenter);
        }

        public static void consoleTooSmallMessage(string direction)
        {
            // Remember what was previously open
            if (currentWindow != "consoleTooSmallMessage")
            {
                previousWindow = currentWindow;
            }

            currentWindow = "consoleTooSmallMessage";
            windowType = "nocursor";
            showCursor = false;

            Console.Clear();

            if (direction == "width")
            {
                Print.printCentered("Konsol genişliği çok küçük. Lütfen genişliği değiştir.", SizeHandler.HeightCenter);
                Print.printCentered("Minimum genişlik:" + SizeHandler.minWidth + " | Şu anki genişlik:" + SizeHandler.WindowWidth);
            }
            else
            {
                Print.printCentered("Konsol uzunluğu çok küçük. Lütfen uzunluğu değiştir.", SizeHandler.HeightCenter);
                Print.printCentered("Minimum uzunluk:" + SizeHandler.minHeight + " | Şu anki uzunluk:" + SizeHandler.WindowHeight);
            }
        }

        public static void reloadWindow(string windowToReload = "current")
        {
            if (windowToReload == "current")
            {
                windowToReload = currentWindow;
            }
            else
            {
                windowToReload = previousWindow;
            }
            switch (windowToReload)
            {
                case "mainScreen":
                    mainScreen();
                    break;
                case "settingsMenu":
                    settingsMenu();
                    break;
                case "delayMenu":
                    delayMenu();
                    break;
                case "leagueClientIsClosedMessage":
                    leagueClientIsClosedMessage();
                    break;
                case "infoMenu":
                    infoMenu();
                    break;
                case "exitMenu":
                    exitMenu();
                    break;
                case "champSelector":
                    champSelector();
                    break;
                case "spellSelector":
                    spellSelector();
                    break;
                case "chatMessagesWindow":
                    chatMessagesWindow();
                    break;
            }
        }

        public static void mainScreen()
        {
            //chatMessagesWindow();
            //return;
            Print.canMovePos = false;
            Navigation.currentPos = Navigation.lastPosMainNav;
            Navigation.consolePosLast = Navigation.lastPosMainNav;

            currentWindow = "mainScreen";
            windowType = "normal";
            showCursor = false;
            topPad = SizeHandler.HeightCenter - 1;
            leftPad = SizeHandler.WidthCenter - 25;
            maxPos = 8;

            Console.Clear();

            // Define logo
            string[] logo =
            {
                @"  _                                                 _                                     _   ",
                @" | |                                     /\        | |            /\                     | |  ",
                @" | |     ___  __ _  __ _ _   _  ___     /  \  _   _| |_ ___      /  \   ___ ___ ___ _ __ | |_ ",
                @" | |    / _ \/ _` |/ _` | | | |/ _ \   / /\ \| | | | __/ _ \    / /\ \ / __/ __/ _ \ '_ \| __|",
                @" | |___|  __/ (_| | (_| | |_| |  __/  / ____ \ |_| | || (_) |  / ____ \ (_| (_|  __/ |_) | |_ ",
                @" |______\___|\__,_|\__, |\__,_|\___| /_/    \_\__,_|\__\___/  /_/    \_\___\___\___| .__/ \__|",
                @"                    __/ |                                                          | |        ",
                @"                   |___/                                                           |_|        "
            };

            // Print logo
            for (int i = 0; i < logo.Length; i++)
            {
                Print.printCentered(logo[i], SizeHandler.HeightCenter - 12 + i);
            }

            // Define options
            string[] optionName = {
                "Bir şampiyon seç",
                "Banlayacağın şampiyonu seç",
                "Sihirdar büyüsü seç 1",
                "Sihirdar büyüsü seç 2",
                "Anında chat mesajı",
                "Auto Accept'i aktifleştir"
            };
            string[] optionValue = {
                Settings.currentChamp[0],
                Settings.currentBan[0],
                Settings.currentSpell1[0],
                Settings.currentSpell2[0],
                Settings.chatMessagesEnabled ? "Devrede, " + Settings.chatMessages.Count : "Devredışı",
                MainLogic.isAutoAcceptOn ? "Devrede" : "Devredışı"
            };

            // Print options
            for (int i = 0; i < optionName.Length; i++)
            {
                Print.printCentered(addDotsInBetween(optionName[i], optionValue[i]), topPad + i);
            }

            // Print the two bottom buttons that are not actaul settings
            Print.printWhenPossible("Bilgi", SizeHandler.HeightCenter + 6, leftPad + 43);
            Print.printWhenPossible("Ayarlar", SizeHandler.HeightCenter + 6, leftPad + 3);


            Print.printWhenPossible("v" + Updater.appVersion, SizeHandler.WindowHeight - 1, 0, false);

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;
        }

        public static void toggleAutoAcceptSettingUI()
        {
            Print.printWhenPossible(MainLogic.isAutoAcceptOn ? "Devrede   " : "Devredışı", topPad + 5, leftPad + 38);
        }

        public static void settingsMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "settingsMenu";
            windowType = "normal";
            showCursor = false;
            topPad = SizeHandler.HeightCenter - 3;
            leftPad = SizeHandler.WidthCenter - 25;
            maxPos = 9;

            Console.Clear();

            // Define options
            string[] optionName = {
                "Ayarları kaydet/config",
                "Verileri önyükle",
                "Instalock seçim",
                "Instalock yasaklama",
                "Güncelleme kontrolünü devre dışı bırak",
                "Otomatik olarak takas yap",
                "Seçimi anında vurgula",
                "Sırayı otomatik başlat",
                "Ayarları geciktir"
            };

            //  Settings.lockDelay.ToString(),

            string[] optionValue = {
                Settings.saveSettings ? "Yes" : "No",
                Settings.preloadData ? "Yes" : "No",
                Settings.instaLock ? "Yes" : "No",
                Settings.instaBan ? "Yes" : "No",
                Settings.disableUpdateCheck ? "Yes" : "No",
                Settings.autoPickOrderTrade ? "Yes" : "No",
                Settings.instantHover ? "Yes" : "No",
                Settings.autoRestartQueue ? "Yes" : "No",
                ""
            };

            // Print options
            for (int i = 0; i < optionName.Length; i++)
            {
                Print.printCentered(addDotsInBetween(optionName[i], optionValue[i]), topPad + i);
            }

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;

            settingsMenuDesc(0);
        }

        public static void settingsMenuDesc(int item)
        {
            // settings descrptions
            switch (item)
            {
                case 0:
                    Print.printCentered("Bir sonraki başlatma için ayarları kaydet.", topPad + maxPos + 2);
                    Print.printCentered("Bu %AppData% klasörünün içinde ayarlarınızı kaydedecek.");
                    break;
                case 1:
                    Print.printCentered("Uygulama başlatıldığında tüm verileri önceden yükler.", topPad + maxPos + 2);
                    Print.printCentered("Şampiyon listesini içerir, sihirdar büyüleri listesi ve dahası.");
                    break;
                case 2:
                    Print.printCentered("Sıra sana geldiğinde anında kilitler.", topPad + maxPos + 2);
                    Print.printCentered("Ayarları geciktir özelliği devre dışı kalır.");
                    break;
                case 3:
                    Print.printCentered("Ban sırası size geldiğinde anında banlar.", topPad + maxPos + 2);
                    Print.printCentered("Ayarları geciktir özelliği devre dışı kalır.");
                    break;
                case 4:
                    Print.printCentered("Başlangıçta güncelleme kontrolünü devre dışı bırakır.", topPad + maxPos + 2);
                    Print.printCentered("");
                    break;
                case 5:
                    Print.printCentered("Birisi talep ettiğinde şampiyon seçimini otomatik olarak takas eder.", topPad + maxPos + 2);
                    Print.printCentered("");
                    break;
                case 6:
                    Print.printCentered("Şampiyon seçim sırası sizde olduğunda anında şampiyonun üzerine gelir.", topPad + maxPos + 2);
                    Print.printCentered("Seçim sırasında, şampiyonu kitlemez fakat vurgular.");
                    break;
                case 7:
                    Print.printCentered("Birkaç dakika içerisinde otomatik olarak sırayı başlatır.", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan süre 5 dakika, Gecikme ayarlarından ayarlanabilir.");
                    break;
                case 8:
                    Print.printCentered("Gecikmeleri ayarlar.", topPad + maxPos + 2);
                    Print.printCentered("");
                    break;
            }
        }

        public static void settingsMenuUpdateUI(int item)
        {
            // Select item to toggle from settings

            //4 => (" " + Settings.lockDelayString).PadLeft(9, '.'),

            string outputText = item switch
            {
                0 => Settings.saveSettings ? " Yes" : ". No",
                1 => Settings.preloadData ? " Yes" : ". No",
                2 => Settings.instaLock ? " Yes" : ". No",
                3 => Settings.instaBan ? " Yes" : ". No",
                4 => Settings.disableUpdateCheck ? " Yes" : ". No",
                5 => Settings.autoPickOrderTrade ? " Yes" : ". No",
                6 => Settings.instantHover ? " Yes" : ". No",
                7=> Settings.autoRestartQueue ? " Yes" : ". No",
                _ => ""
            };
            Print.printWhenPossible(outputText, item + topPad, SizeHandler.WidthCenter + 22 - outputText.Length);
        }



        public static void delayMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "delayMenu";
            windowType = "normal";
            showCursor = false;
            topPad = SizeHandler.HeightCenter - 3;
            leftPad = SizeHandler.WidthCenter - 25;
            maxPos = 7;

            Console.Clear();

            // Define options
            string[] optionName = {
                "Pick hover delay upon phase start",
                "Pick lock delay upon phase start",
                "Pick lock delay before phase end",
                "Ban hover delay upon phase start",
                "Ban lock delay upon phase start",
                "Ban lock delay before phase end",
                "Max queue time before restart"
            };
            string[] optionValue = {
                Settings.pickStartHoverDelay.ToString(),
                Settings.pickStartlockDelay.ToString(),
                Settings.pickEndlockDelay.ToString(),
                Settings.banStartHoverDelay.ToString(),
                Settings.banStartlockDelay.ToString(),
                Settings.banEndlockDelay.ToString(),
                Settings.queueMaxTime.ToString(),
            };

            // Print options
            for (int i = 0; i < optionName.Length; i++)
            {
                Print.printCentered(addDotsInBetween(optionName[i], optionValue[i]), topPad + i);
            }

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;

            delayMenuDesc(0);
        }

        public static void delayMenuDesc(int item)
        {
            // settings descrptions
            switch (item)
            {
                case 0:
                    Print.printCentered("Şampiyon seçimini vurgulama gecikmesi.", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan değer 10000.");
                    break;
                case 1:
                    Print.printCentered("Şampiyon seçimi için gecikme.", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan değer 999999999.");
                    break;
                case 2:
                    Print.printCentered("Süre bitmeden önceki şampiyon seçimini kilitleme gecikmesi.", topPad + maxPos + 2);
                    Print.printCentered("Bu değerden düşük yapmayın (<300), aski taktirde maçın bozulmasına yol açar. Varsayılan değer 1000.");
                    break;
                case 3:
                    Print.printCentered("Şampiyon banlama gecikmesi.", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan değer 1500.");
                    break;
                case 4:
                    Print.printCentered("Şampiyon seçimi için gecikme.", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan değer 999999999.");
                    break;
                case 5:
                    Print.printCentered("Süre bitmeden önceki kilitleme gecikmesi.", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan değer 1000.");
                    break;
                case 6:
                    Print.printCentered("Karşılaşma aramayı iptal edip yeniden başlatmadan önce bekleme süresi ne kadar uzun olmalıdır?", topPad + maxPos + 2);
                    Print.printCentered("Varsayılan değer 300000.");
                    break;
            }
        }

        public static void delayMenuUpdateUI(int item)
        {
            // Select item to toggle from settings

            string outputText = item switch
            {
                0 => (" " + Settings.pickStartHoverDelay).PadLeft(10, '.'),
                1 => (" " + Settings.pickStartlockDelay).PadLeft(10, '.'),
                2 => (" " + Settings.pickEndlockDelay).PadLeft(10, '.'),
                3 => (" " + Settings.banStartHoverDelay).PadLeft(10, '.'),
                4 => (" " + Settings.banStartlockDelay).PadLeft(10, '.'),
                5 => (" " + Settings.banEndlockDelay).PadLeft(10, '.'),
                6 => (" " + Settings.queueMaxTime).PadLeft(10, '.'),
                _ => ""
            };
            Print.printWhenPossible(outputText, item + topPad, SizeHandler.WidthCenter + 22 - outputText.Length);
        }

        public static void infoMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "infoMenu";
            windowType = "nocursor";
            showCursor = false;

            Console.Clear();

            Print.printCentered(addDotsInBetween("Made by", "Icarus"), SizeHandler.HeightCenter - 2);
            Print.printCentered(addDotsInBetween("Sürüm", Updater.appVersion), SizeHandler.HeightCenter - 1);

            Print.printCentered("İyi eğlenceler dilerim.", SizeHandler.HeightCenter + 1);
            Print.printCentered(" Kötü amaçlar için kullanmayınız!", SizeHandler.HeightCenter + 2);
        }

        public static void exitMenu()
        {
            Print.canMovePos = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "exitMenu";
            windowType = "sideways";
            showCursor = false;
            topPad = SizeHandler.HeightCenter + 1;
            leftPad = SizeHandler.WidthCenter - 19;
            maxPos = 2;

            Console.Clear();

            Print.printCentered("Uygulamayı kapatmak istediğinden emin misin?", topPad - 2);
            Print.printWhenPossible((" No").PadLeft(32, ' '), topPad, leftPad + 3, false);
            Print.printWhenPossible("Yes ", topPad, leftPad + 3, false);

            Navigation.handlePointerMovementPrint();

            Print.canMovePos = true;
        }

        public static void champSelector()
        {
            Print.canMovePos = false;

            totalRows = SizeHandler.WindowHeight - 2;

            currentWindow = "champSelector";
            windowType = "grid";
            showCursor = false;

            Navigation.currentInput = "";

            Console.Clear();

            Data.loadChampionsList();

            displayChamps();
            updateCurrentFilter();
        }

        private static void displayChamps()
        {
            Console.CursorVisible = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            topPad = 0;
            leftPad = 0;
            maxPos = totalChamps;

            Console.SetCursorPosition(0, 0);

            List<itemList> champsFiltered = new List<itemList>();
            if ("unselected".Contains(Navigation.currentInput.ToLower()))
            {
                champsFiltered.Add(new itemList() { name = "Seçilmedi", id = "0" });
            }
            if (currentChampPicker == 1)
            {
                if ("none".Contains(Navigation.currentInput.ToLower()))
                {
                    champsFiltered.Add(new itemList() { name = "None", id = "-1" });
                }
            }
            foreach (var champ in Data.champsSorterd)
            {
                if (champ.name.ToLower().Contains(Navigation.currentInput.ToLower()))
                {
                    // Make sure the champ is free or if it's for a ban before adding it to the list
                    if (champ.free || currentChampPicker == 1)
                    {
                        champsFiltered.Add(new itemList() { name = champ.name, id = champ.id });
                    }
                }
            }

            totalChamps = champsFiltered.Count;

            int currentRow = 0;
            string[] champsOutput = new string[totalRows];

            foreach (var champ in champsFiltered)
            {
                string line = "   " + champ.name;
                line = line.PadRight(columnSize, ' ');

                champsOutput[currentRow] += line;

                currentRow++;
                if (currentRow >= totalRows)
                {
                    currentRow = 0;
                }
            }

            foreach (var line in champsOutput)
            {
                string lineNew;
                if (line != null)
                {
                    lineNew = line.Remove(line.Length - 1);
                    lineNew = lineNew.PadRight(119, ' ');
                }
                else
                {
                    lineNew = "".PadRight(119, ' ');
                }
                Print.printWhenPossible(lineNew);
            }
            Navigation.handlePointerMovementPrint();
            Print.canMovePos = true;
            displayCursorIfNeeded();
        }

        public static void spellSelector()
        {
            Print.canMovePos = false;

            totalRows = SizeHandler.WindowHeight - 2;

            currentWindow = "spellSelector";
            windowType = "grid";
            showCursor = false;

            Navigation.currentInput = "";

            Console.Clear();

            Data.loadSpellsList();

            displaySpells();
            updateCurrentFilter();
        }

        private static void displaySpells()
        {
            Console.CursorVisible = false;
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            topPad = 0;
            leftPad = 0;
            maxPos = totalSpells;

            Console.SetCursorPosition(0, 0);

            List<itemList> spellsFiltered = new List<itemList>();
            if ("unselected".Contains(Navigation.currentInput.ToLower()))
            {
                spellsFiltered.Add(new itemList() { name = "Seçilmedi", id = "0" });
            }
            foreach (var spell in Data.spellsSorted)
            {
                if (spell.name.ToLower().Contains(Navigation.currentInput.ToLower()))
                {
                    spellsFiltered.Add(new itemList() { name = spell.name, id = spell.id });
                }
            }

            totalSpells = spellsFiltered.Count;

            int currentRow = 0;
            string[] spelloutput = new string[totalRows];

            foreach (var spell in spellsFiltered)
            {
                string line = "   " + spell.name;
                line = line.PadRight(columnSize, ' ');

                spelloutput[currentRow] += line;

                currentRow++;
                if (currentRow >= totalRows)
                {
                    currentRow = 0;
                }
            }

            foreach (var line in spelloutput)
            {
                string lineNew;
                if (line != null)
                {
                    lineNew = line.Remove(line.Length - 1);
                    lineNew = lineNew.PadRight(119, ' ');
                }
                else
                {
                    lineNew = "".PadRight(119, ' ');
                }
                Print.printWhenPossible(lineNew);
            }
            Navigation.handlePointerMovementPrint();
            Print.canMovePos = true;
            displayCursorIfNeeded();
        }

        public static void updateCurrentFilter()
        {
            showCursor = true;
            if (currentWindow == "champSelector")
            {
                displayChamps();
            }
            else if (currentWindow == "spellSelector")
            {
                displaySpells();
            }

            Navigation.currentPos = 0;
            Console.CursorVisible = false;
            string consoleLine = "Ara: " + Navigation.currentInput;
            Print.printCentered(consoleLine, Console.WindowHeight - 1, false, true);

            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
            updateCursorPosition();
        }

        public static void printHeart()
        {
            int[] position = { 54, 10 };

            string[] lines = {
                "  oooo   oooo",
                " o    o o    o ",
                "o      o      o",
                " o           o",
                "  o         o",
                "   o       o",
                "    o     o",
                "     o   o",
                "      o o",
                "       o"
            };

            foreach (string line in lines)
            {
                Console.SetCursorPosition(position[0], position[1]++);
                Console.WriteLine(line);
            }

            updateCursorPosition();
        }

        private static string addDotsInBetween(string firstString, string secondString, int totalLength = 44)
        {
            int firstStringLength = firstString.Length + 1;
            int secondStringLength = secondString.Length + 1;
            int dotsCount = totalLength - firstStringLength - secondStringLength;

            return firstString + " " + new string('.', dotsCount) + " " + secondString;
        }

        public static void chatMessagesWindow(int pageToLoad = 0)
        {
            Print.canMovePos = false;
            Console.Clear();
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "chatMessagesWindow";
            windowType = "pages";
            showCursor = false;
            topPad = 1;
            leftPad = 2;
            maxPos = Settings.chatMessages.Count + 1; // +1 for "new message" row
            int messageWidth = SizeHandler.WindowWidth - (leftPad * 2) - 6; // calclate the amount of characters to display of each messages before cropping it
            totalRows = SizeHandler.WindowHeight - 4; // calculate rows per page
            if (maxPos > totalRows)
            {
                maxPos = totalRows;
            }

            {
                double totalPagesTmp = ((double)Settings.chatMessages.Count + 1) / (double)totalRows;
                int totalPagesTmp2 = (int)Math.Ceiling(totalPagesTmp);
                totalPages = totalPagesTmp2;
            }

            int currentConsoleRow = topPad;
            int currentMessagePrint = 0;
            int startingIndex = pageToLoad * totalRows;

            // Print all messages
            foreach (var message in Settings.chatMessages)
            {
                if (startingIndex > 0)
                {
                    startingIndex--;
                    continue;
                }

                if (currentMessagePrint + 1 > totalRows)
                {
                    break;
                }

                // Limit messages to console width, crop and add an ellipsis at the end if the message is too long
                string messageOutput = message.Length > messageWidth ? message.Substring(0, messageWidth - 3) + "..." : message;
                Print.printWhenPossible(messageOutput, currentConsoleRow++, leftPad + 3, false);
                currentMessagePrint++;
            }

            // Add a button to create a new message
            if (!(currentMessagePrint + 1 > totalRows)) // +1 for "new message" row
            {
                Print.printWhenPossible("[Yeni İleti]", currentConsoleRow++, leftPad + 3, false);
            }

            // Print pages count, if needed
            if (totalPages > 1)
            {
                string pagesPrint = Print.centerString("Mevcut sayfa: " + (pageToLoad + 1) + " / " + totalPages)[0];
                pagesPrint = Print.replaceAt(pagesPrint, "<- önceki sayfa", leftPad + 3);
                pagesPrint = Print.replaceAt(pagesPrint, "sonraki sayfa ->", SizeHandler.WindowWidth - 17);
                Print.printWhenPossible(pagesPrint, SizeHandler.WindowHeight - 2, 0, false);
            }

            Print.canMovePos = true;
            Navigation.handlePointerMovementPrint();
        }

        public static void chatMessagesEdit()
        {
            Print.canMovePos = false;
            Console.Clear();
            Navigation.currentPos = 0;
            Navigation.consolePosLast = 0;

            currentWindow = "chatMessagesEdit";
            windowType = "messageEdit";
            showCursor = true;
            topPad = SizeHandler.HeightCenter - 2;
            leftPad = SizeHandler.WidthCenter;
            maxPos = 3;

            if (Settings.chatMessages.Count > messageIndex)
            {
                Navigation.currentInput = Settings.chatMessages[messageIndex];
            }
            else
            {
                Navigation.currentInput = "";
            }

            updateMessageEdit();

            Print.printCentered("Kaydet         Sil            İptal", topPad + 3, false);


            Print.canMovePos = true;
            Navigation.handlePointerMovementPrint();
            updateCursorPosition();
        }

        public static void updateMessageEdit()
        {
            string message = Navigation.currentInput;
            int chunkLength = 100; // Length of each chunk

            List<string> chunks = new List<string>();

            // Extract chunks from the input message
            for (int i = 0; i < message.Length; i += chunkLength)
            {
                int length = Math.Min(chunkLength, message.Length - i);
                chunks.Add(message.Substring(i, length));
            }

            string chunk1 = chunks.Count > 0 ? chunks[0] : "";
            string chunk2 = chunks.Count > 1 ? chunks[1] : "";

            Console.CursorVisible = false;

            // make sure the second line is wiped if it has something
            if (chunk2.Length == 0)
            {
                Print.printCentered(chunk2, topPad + 1, false, true);
            }

            // print first line
            Print.printCentered(chunk1, topPad, false, true);

            // only print second line if needed
            if (chunk2.Length > 0)
            {
                Print.printCentered(chunk2, topPad + 1, false, true);
            }
            displayCursorIfNeeded();
        }

        public static void updateCursorPosition()
        {
            Console.SetCursorPosition(cursorPosition[0], cursorPosition[1]);
        }

        public static void displayCursorIfNeeded()
        {
            if (showCursor)
            {
                Console.CursorVisible = true;
            }
        }
    }
}

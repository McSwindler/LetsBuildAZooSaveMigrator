using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

namespace ContainerReader
{
  public static class MapFileReader
  {
    public static void ReadFile(string filename)
    {
        string strDllPath = Path.GetFullPath("SEngine.dll");
        if (File.Exists(strDllPath))
        {
            // Execute the method from the requested .dll using reflection (System.Reflection).
            Assembly DLL = Assembly.LoadFrom(strDllPath);
            SESaveReadWriter.SetIgnoreKeysForSave(DLL, true);
            Type classType = DLL.GetType(String.Format("{0}.{1}", "SEngine", "Reader"));
            if (classType != null)
            {
                // Create class instance.
                dynamic reader = Activator.CreateInstance(classType, new object[]{filename, false});

                Console.WriteLine("Header Version: " + ReadInt(reader));
                ReadZoos(reader);
                Console.WriteLine();
                ReadPlayerData(reader);
                
            }
        }
    }
    
    private static void ReadZoos(dynamic reader)
    {
        int zoos = ReadInt(reader);
        Console.WriteLine("Number of Zoos: " + zoos);
        for(int i = 0; i < zoos; i++) {
            bool isZooActive = ReadBool(reader);
            Console.WriteLine("Zoo " + i + ": " + isZooActive);
            if(!isZooActive)
                continue;

            Console.WriteLine("\tCash Held: " + ReadInt(reader));
            Console.WriteLine("\tLast Save: " + reader.ReadDateTime("h"));
            Console.WriteLine("\tZooKeeper: " + ReadInt(reader));
            int saves = ReadInt(reader);
            Console.WriteLine("\tSaves: " + saves);
            for(int j = 0; j < saves; j++) {
                bool isSaveActive = ReadBool(reader);
                Console.WriteLine("\tSave " + j + ": " + isSaveActive);
                if(!isSaveActive)
                    continue;
                Console.WriteLine("\t\tMorality Rating: " + ReadString(reader));
                Console.WriteLine("\t\tDays Played: " + ReadInt(reader));
                Console.WriteLine("\t\tCash Held: " + ReadInt(reader));
                Console.WriteLine("\t\tLand Unlocked: " + ReadInt(reader));
                Console.WriteLine("\t\tResearch Discovered: " + ReadInt(reader));
                Console.WriteLine("\t\tLast Save: " + reader.ReadDateTime("h"));
                Console.WriteLine("\t\tPercent Completed: " + ReadFloat(reader));
                Console.WriteLine("\t\tZoo Name: " + ReadString(reader));
            }
            Console.WriteLine("\tInvesting Money: " + ReadInt(reader));
            Console.WriteLine("\tIs Cheat?: " + ReadBool(reader));
        }
    }

    private static void ReadPlayerData(dynamic reader)
    {
        Console.WriteLine("Resolution: " + ReadInt(reader));
        Console.WriteLine("Music Volume: " + ReadFloat(reader));
        Console.WriteLine("SFX Volume: " + ReadFloat(reader));
        Console.WriteLine("Days of Last Check In: " + new DateTime(ReadLong(reader)));
        Console.WriteLine("Total Unique Days App Launched On: " + ReadUInt(reader));
        Console.WriteLine("Time Played: " + new TimeSpan(ReadLong(reader)));
        Console.WriteLine("Total Times App Launched: " + ReadUInt(reader));
        Console.WriteLine("UX Multiplier: " + ReadFloat(reader));
        Console.WriteLine("UX Multiplier PC: " + ReadFloat(reader));
        Console.WriteLine("UX Multiplier Console: " + ReadFloat(reader));
        ReadIntList(reader, "Key Bindings");
        ReadIntList(reader, "Research Completed");
        ReadBoolList(reader, "Bus Types Researched");
        ReadIntList(reader, "Cell Blocks Researched");
        ReadVariants(reader);
        ReadBoolList(reader, "Unlocked Routes");
        ReadIntList(reader, "Unlocked Things");
        ReadIntList(reader, "People Employed");
        Console.WriteLine("Sorting Disabled: " + ReadBool(reader));
        Console.WriteLine("Flashing Disabled: " + ReadBool(reader));
        Console.WriteLine("Tile Remaking Disabled: " + ReadBool(reader));
        Console.WriteLine("Icons Disabled: " + ReadBool(reader));
        Console.WriteLine("Unlock Framerate: " + ReadBool(reader));
        Console.WriteLine("Stock Capacity: " + ReadInt(reader));
        Console.WriteLine("Use Big Fonts: " + ReadBool(reader));
        Console.WriteLine("Language: " + ReadInt(reader));
        Console.WriteLine("Use Native Mouse: " + ReadBool(reader));
    }

    private static void ReadVariants(dynamic reader)
    {
        int keys = ReadInt(reader);
        Console.WriteLine("Variants: " + keys);
        for(int i = 0; i < keys; i++){
            int variants = ReadInt(reader);
            Console.WriteLine("\tVariant " + i + ": " + variants);
            for(int j = 0; j < variants; j++) {
                Console.WriteLine("\t\tDiscovered " + ReadInt(reader) + " on Day " + ReadInt(reader));
            }
        }
    }

    private static void ReadIntList(dynamic reader, String desc)
    {
        int keys = ReadInt(reader);
        Console.WriteLine(desc + ": " + keys);
        for(int i = 0; i < keys; i++){
            Console.WriteLine("\t" + desc + " " + i + ": " + ReadInt(reader));
        }
    }

    private static void ReadBoolList(dynamic reader, String desc)
    {
        int keys = ReadInt(reader);
        Console.WriteLine(desc + ": " + keys);
        for(int i = 0; i < keys; i++){
            Console.WriteLine("\t" + desc + " " + i + ": " + ReadBool(reader));
        }
    }

    private static int ReadInt(dynamic reader)
    {
        int _out = 0;
        reader.ReadInt("h", ref _out);

        return _out;
    }

    private static bool ReadBool(dynamic reader)
    {
        bool _out = false;
        reader.ReadBool("h", ref _out);

        return _out;
    }

    private static string ReadString(dynamic reader)
    {
        string _out = "";
        reader.ReadString("h", ref _out);

        return _out;
    }

    private static float ReadFloat(dynamic reader)
    {
        float _out = 0;
        reader.ReadFloat("h", ref _out);

        return _out;
    }

    private static long ReadLong(dynamic reader)
    {
        long _out = 0;
        reader.ReadLong("l", ref _out);

        return _out;
    }

    private static uint ReadUInt(dynamic reader)
    {
        uint _out = 0;
        reader.ReadUInt("l", ref _out);

        return _out;
    }
  }
}
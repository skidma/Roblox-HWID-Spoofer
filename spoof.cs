using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace Valex.Assets.Classes
{
    public static class Spoofer
    {
        public static string GenerateRandomId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        public static string GenerateRandomMac()
        {
            const string hexChars = "ABCDEF0123456789";
            const string secondChars = "26AE";
            var random = new Random();
            var result = new StringBuilder(17);

            result.Append(hexChars[random.Next(hexChars.Length)]);
            result.Append(secondChars[random.Next(secondChars.Length)]);

            for (int i = 0; i < 5; i++)
            {
                result.Append('-');
                result.Append(hexChars[random.Next(hexChars.Length)]);
                result.Append(hexChars[random.Next(hexChars.Length)]);
            }

            return result.ToString();
        }

        public static void SetNetworkAdapterState(string adapterId, bool enable = true)
        {
            string adapterName = "Ethernet";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Id == adapterId)
                {
                    adapterName = nic.Name;
                    break;
                }
            }

            string action = enable ? "enable" : "disable";
            var startInfo = new ProcessStartInfo("netsh", $"interface set interface \"{adapterName}\" {action}")
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
        }

        public static void SpoofDisks()
        {
            // Spoof SCSI disk identifiers
            using (var scsiKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\Scsi"))
            {
                foreach (string portKeyName in scsiKey.GetSubKeyNames())
                {
                    using (var portKey = scsiKey.OpenSubKey(portKeyName))
                    {
                        foreach (string busKeyName in portKey.GetSubKeyNames())
                        {
                            string keyPath = $@"HARDWARE\DEVICEMAP\Scsi\{portKeyName}\{busKeyName}\Target Id 0\Logical Unit Id 0";
                            using (var targetKey = Registry.LocalMachine.OpenSubKey(keyPath, true))
                            {
                                if (targetKey?.GetValue("DeviceType")?.ToString() == "DiskPeripheral")
                                {
                                    string id = GenerateRandomId(14);
                                    string serial = GenerateRandomId(14);

                                    targetKey.SetValue("DeviceIdentifierPage", Encoding.UTF8.GetBytes(serial));
                                    targetKey.SetValue("Identifier", id);
                                    targetKey.SetValue("InquiryData", Encoding.UTF8.GetBytes(id));
                                    targetKey.SetValue("SerialNumber", serial);
                                }
                            }
                        }
                    }
                }
            }

            // Spoof disk peripheral identifiers
            using (var diskKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\MultifunctionAdapter\0\DiskController\0\DiskPeripheral"))
            {
                foreach (string diskId in diskKey.GetSubKeyNames())
                {
                    using (var peripheralKey = diskKey.OpenSubKey(diskId, true))
                    {
                        peripheralKey.SetValue("Identifier", $"{GenerateRandomId(8)}-{GenerateRandomId(8)}-A");
                    }
                }
            }
        }

        public static void SpoofGUIDs()
        {
            // Spoof hardware profile GUID
            using (var hwProfileKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001", true))
            {
                hwProfileKey.SetValue("HwProfileGuid", $"{{{Guid.NewGuid()}}}");
            }

            // Spoof cryptography machine GUID
            using (var cryptoKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true))
            {
                cryptoKey.SetValue("MachineGuid", Guid.NewGuid().ToString());
            }

            // Spoof SQM machine ID
            using (var sqmKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\SQMClient", true))
            {
                sqmKey.SetValue("MachineId", $"{{{Guid.NewGuid()}}}");
            }

            // Spoof system information
            using (var sysInfoKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SystemInformation", true))
            {
                var random = new Random();
                int day = random.Next(1, 31);
                int month = random.Next(1, 13);
                int year = random.Next(2000, 2023);

                sysInfoKey.SetValue("BIOSReleaseDate", $"{day:D2}/{month:D2}/{year}");
                sysInfoKey.SetValue("BIOSVersion", GenerateRandomId(10));
                sysInfoKey.SetValue("ComputerHardwareId", $"{{{Guid.NewGuid()}}}");
                sysInfoKey.SetValue("ComputerHardwareIds", $"{{{Guid.NewGuid()}}}\n{{{Guid.NewGuid()}}}\n{{{Guid.NewGuid()}}}\n");
                sysInfoKey.SetValue("SystemManufacturer", GenerateRandomId(15));
                sysInfoKey.SetValue("SystemProductName", GenerateRandomId(6));
            }

            // Spoof Windows Update IDs
            using (var wuKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate", true))
            {
                wuKey.SetValue("SusClientId", Guid.NewGuid().ToString());
                wuKey.SetValue("SusClientIdValidation", Encoding.UTF8.GetBytes(GenerateRandomId(25)));
            }
        }

        public static void SpoofComputerName()
        {
            string newName = GenerateRandomId(10);

            using (var computerNameKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ComputerName\ComputerName", true))
            {
                computerNameKey.SetValue("ComputerName", newName);
            }

            using (var activeNameKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName", true))
            {
                activeNameKey.SetValue("ComputerName", newName);
            }
        }

        public static void SpoofProductId()
        {
            using (var currentVersionKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", true))
            {
                currentVersionKey.SetValue("ProductId", $"{GenerateRandomId(5)}-{GenerateRandomId(5)}-{GenerateRandomId(5)}-{GenerateRandomId(5)}");
            }
        }

        public static void SpoofOwner()
        {
            using (var currentVersionKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", true))
            {
                currentVersionKey.SetValue("RegisteredOwner", GenerateRandomId(12));
            }
        }

        public static bool SpoofMAC()
        {
            const string networkAdaptersKeyPath = @"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}";

            try
            {
                using (var networkClassKey = Registry.LocalMachine.OpenSubKey(networkAdaptersKeyPath))
                {
                    foreach (string adapterKeyName in networkClassKey.GetSubKeyNames())
                    {
                        if (adapterKeyName == "Properties") continue;

                        try
                        {
                            using (var adapterKey = Registry.LocalMachine.OpenSubKey($"{networkAdaptersKeyPath}\\{adapterKeyName}", true))
                            {
                                if (adapterKey?.GetValue("BusType") != null)
                                {
                                    adapterKey.SetValue("NetworkAddress", GenerateRandomMac());
                                    string adapterId = adapterKey.GetValue("NetCfgInstanceId").ToString();
                                    SetNetworkAdapterState(adapterId, false);
                                    SetNetworkAdapterState(adapterId, true);
                                }
                            }
                        }
                        catch (SecurityException)
                        {
                            Console.WriteLine("\n[X] Start the spoofer in admin mode to spoof your MAC address!");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[X] Error spoofing MAC address: {ex.Message}");
                return true;
            }
        }
    }
}

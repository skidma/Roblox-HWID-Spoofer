# Information
Not much to say just a basic hwid spoofer made for roblox ig, found it in https://valex.io/. It's free so not much to say.

<img width="957" height="232" alt="image" src="https://github.com/user-attachments/assets/98cf7261-a172-4bf2-9d61-cb72ad692ea7" />
<img width="923" height="98" alt="image" src="https://github.com/user-attachments/assets/cff15578-86d2-49cb-ac6e-9837dc596ad8" />
<img width="881" height="337" alt="image" src="https://github.com/user-attachments/assets/5cf5e7b4-0427-4e92-952b-c6c62fb1b304" />
<img width="875" height="612" alt="image" src="https://github.com/user-attachments/assets/255644cf-d3a3-41db-8a97-403c7eb4cd07" />
<img width="1048" height="560" alt="image" src="https://github.com/user-attachments/assets/941b3a13-0cb1-41e5-9866-5d0813c538bc" />
<img width="905" height="128" alt="image" src="https://github.com/user-attachments/assets/031fd2fb-11aa-4843-8756-c7c21b418398" />
<img width="682" height="330" alt="image" src="https://github.com/user-attachments/assets/112d3407-d73f-453a-aab4-c4873ea5f8a7" />

# 
```
public static void Enable_LocalAreaConection(string adapterId, bool enable = true)
{
	string str = "Ethernet";
	foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
	{
		bool flag = networkInterface.Id == adapterId;
		if (flag)
		{
			str = networkInterface.Name;
			break;
		}
	}
	string str2;
	if (enable)
	{
		str2 = "enable";
	}
	else
	{
		str2 = "disable";
	}
	ProcessStartInfo startInfo = new ProcessStartInfo("netsh", "interface set interface \"" + str + "\" " + str2);
	Process process = new Process();
	process.StartInfo = startInfo;
	process.Start();
	process.WaitForExit();
}
```

# 
```
public static string RandomId(int length)
{
	string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	string text2 = "";
	Random random = new Random();
	for (int i = 0; i < length; i++)
	{
		text2 += text[random.Next(text.Length)].ToString();
	}
	return text2;
}
```

# 
```
public static string RandomMac()
{
	string text = "ABCDEF0123456789";
	string text2 = "26AE";
	string text3 = "";
	Random random = new Random();
	text3 += text[random.Next(text.Length)].ToString();
	text3 += text2[random.Next(text2.Length)].ToString();
	for (int i = 0; i < 5; i++)
	{
		text3 += "-";
		text3 += text[random.Next(text.Length)].ToString();
		text3 += text[random.Next(text.Length)].ToString();
	}
	return text3;
}
```

# 
```
public static void SpoofComputerName()
{
	using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName", true))
	{
		registryKey.SetValue("ComputerName", Spoofer.RandomId(10));
	}
	using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ActiveComputerName", true))
	{
		registryKey2.SetValue("ComputerName", Spoofer.RandomId(10));
	}
}
```

# 
```
public static void SpoofDisks()
{
	using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\Scsi"))
	{
		foreach (string text in registryKey.GetSubKeyNames())
		{
			using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\Scsi\\" + text))
			{
				foreach (string text2 in registryKey2.GetSubKeyNames())
				{
					using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey(string.Concat(new string[]
					{
						"HARDWARE\\DEVICEMAP\\Scsi\\",
						text,
						"\\",
						text2,
						"\\Target Id 0\\Logical Unit Id 0"
					}), true))
					{
						bool flag = registryKey3 != null;
						if (flag)
						{
							bool flag2 = registryKey3.GetValue("DeviceType").ToString() == "DiskPeripheral";
							if (flag2)
							{
								string text3 = Spoofer.RandomId(14);
								string text4 = Spoofer.RandomId(14);
								registryKey3.SetValue("DeviceIdentifierPage", Encoding.UTF8.GetBytes(text4));
								registryKey3.SetValue("Identifier", text3);
								registryKey3.SetValue("InquiryData", Encoding.UTF8.GetBytes(text3));
								registryKey3.SetValue("SerialNumber", text4);
							}
						}
					}
				}
			}
		}
	}
	using (RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\MultifunctionAdapter\\0\\DiskController\\0\\DiskPeripheral"))
	{
		foreach (string str in registryKey4.GetSubKeyNames())
		{
			using (RegistryKey registryKey5 = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\MultifunctionAdapter\\0\\DiskController\\0\\DiskPeripheral\\" + str, true))
			{
				registryKey5.SetValue("Identifier", Spoofer.RandomId(8) + "-" + Spoofer.RandomId(8) + "-A");
			}
		}
	}
}
```

# 
```
public static void SpoofGUIDs()
{
	using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001", true))
	{
		registryKey.SetValue("HwProfileGuid", string.Format("{{{0}}}", Guid.NewGuid()));
	}
	using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", true))
	{
		registryKey2.SetValue("MachineGuid", Guid.NewGuid().ToString());
	}
	using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", true))
	{
		registryKey3.SetValue("MachineId", string.Format("{{{0}}}", Guid.NewGuid()));
	}
	using (RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\SystemInformation", true))
	{
		Random random = new Random();
		int num = random.Next(1, 31);
		bool flag = num < 10;
		string arg;
		if (flag)
		{
			arg = string.Format("0{0}", num);
		}
		else
		{
			arg = num.ToString();
		}
		int num2 = random.Next(1, 13);
		bool flag2 = num2 < 10;
		string arg2;
		if (flag2)
		{
			arg2 = string.Format("0{0}", num2);
		}
		else
		{
			arg2 = num2.ToString();
		}
		registryKey4.SetValue("BIOSReleaseDate", string.Format("{0}/{1}/{2}", arg, arg2, random.Next(2000, 2023)));
		registryKey4.SetValue("BIOSVersion", Spoofer.RandomId(10));
		registryKey4.SetValue("ComputerHardwareId", string.Format("{{{0}}}", Guid.NewGuid()));
		registryKey4.SetValue("ComputerHardwareIds", string.Format("{{{0}}}\n{{{1}}}\n{{{2}}}\n", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
		registryKey4.SetValue("SystemManufacturer", Spoofer.RandomId(15));
		registryKey4.SetValue("SystemProductName", Spoofer.RandomId(6));
		using (RegistryKey registryKey5 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate", true))
		{
			registryKey5.SetValue("SusClientId", Guid.NewGuid().ToString());
			registryKey5.SetValue("SusClientIdValidation", Encoding.UTF8.GetBytes(Spoofer.RandomId(25)));
		}
	}
}
```

# 
```
public static bool SpoofMAC()
{
	bool result = false;
	using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e972-e325-11ce-bfc1-08002be10318}"))
	{
		foreach (string text in registryKey.GetSubKeyNames())
		{
			bool flag = text != "Properties";
			if (flag)
			{
				try
				{
					using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e972-e325-11ce-bfc1-08002be10318}\\" + text, true))
					{
						bool flag2 = registryKey2.GetValue("BusType") != null;
						if (flag2)
						{
							registryKey2.SetValue("NetworkAddress", Spoofer.RandomMac());
							string adapterId = registryKey2.GetValue("NetCfgInstanceId").ToString();
							Spoofer.Enable_LocalAreaConection(adapterId, false);
							Spoofer.Enable_LocalAreaConection(adapterId, true);
						}
					}
				}
				catch (SecurityException ex)
				{
					Console.WriteLine("\n[X] Start the spoofer in admin mode to spoof your MAC address!");
					result = true;
					break;
				}
			}
		}
	}
	return result;
}
```

# 
```
public static void SpoofOwner()
{
	using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", true))
	{
		registryKey.SetValue("RegisteredOwner", Spoofer.RandomId(12));
	}
}
```

# 
```
public static void SpoofProductId()
{
	using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", true))
	{
		registryKey.SetValue("ProductId", string.Concat(new string[]
		{
			Spoofer.RandomId(5),
			"-",
			Spoofer.RandomId(5),
			"-",
			Spoofer.RandomId(5),
			"-",
			Spoofer.RandomId(5)
		}));
	}
}
```

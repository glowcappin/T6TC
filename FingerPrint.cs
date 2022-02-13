using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace T6TC
{
	// Token: 0x02000066 RID: 102
	public static class FingerPrint
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0004F828 File Offset: 0x0004DA28
		public static string Value(string salt)
		{
			if (string.IsNullOrEmpty(FingerPrint.fingerPrint))
			{
				FingerPrint.fingerPrint = FingerPrint.GetHash(string.Concat(new string[]
				{
					"CPU >> ",
					FingerPrint.cpuId(),
					"\nBIOS >> ",
					FingerPrint.biosId(),
					"\nBASE >> ",
					FingerPrint.baseId(),
					FingerPrint.videoId(),
					"\nMAC >> ",
					FingerPrint.macId(),
					"+D_b@.6g@oNHb7FxR3-9_tr^)amc74@$'PDN):N'7*pQqfjr]m;Rac`J(jkfQ76&D<L]RSge_(zcsK;xB>8",
					salt
				}));
			}
			return FingerPrint.fingerPrint;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0004F8B4 File Offset: 0x0004DAB4
		private static string GetHash(string s)
		{
			HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
			byte[] bytes = new ASCIIEncoding().GetBytes(s);
			return FingerPrint.GetHexString(hashAlgorithm.ComputeHash(bytes));
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0004F8E0 File Offset: 0x0004DAE0
		private static string GetHexString(byte[] bt)
		{
			string text = string.Empty;
			for (int i = 0; i < bt.Length; i++)
			{
				byte b = bt[i];
				int num = (int)(b & 15);
				int num2 = b >> 4 & 15;
				if (num2 > 9)
				{
					text += ((char)(num2 - 10 + 65)).ToString();
				}
				else
				{
					text += num2.ToString();
				}
				if (num > 9)
				{
					text += ((char)(num - 10 + 65)).ToString();
				}
				else
				{
					text += num.ToString();
				}
				if (i + 1 != bt.Length && (i + 1) % 2 == 0)
				{
					text += "-";
				}
			}
			return text;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0004F98C File Offset: 0x0004DB8C
		private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (managementObject[wmiMustBeTrue].ToString() == "True" && text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0004FA28 File Offset: 0x0004DC28
		private static string identifier(string wmiClass, string wmiProperty)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0004FAAC File Offset: 0x0004DCAC
		private static string cpuId()
		{
			string text = FingerPrint.identifier("Win32_Processor", "UniqueId");
			if (text == "")
			{
				text = FingerPrint.identifier("Win32_Processor", "ProcessorId");
				if (text == "")
				{
					text = FingerPrint.identifier("Win32_Processor", "Name");
					if (text == "")
					{
						text = FingerPrint.identifier("Win32_Processor", "Manufacturer");
					}
					text += FingerPrint.identifier("Win32_Processor", "MaxClockSpeed");
				}
			}
			return text;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0004FB38 File Offset: 0x0004DD38
		private static string biosId()
		{
			return string.Concat(new string[]
			{
				FingerPrint.identifier("Win32_BIOS", "Manufacturer"),
				FingerPrint.identifier("Win32_BIOS", "SMBIOSBIOSVersion"),
				FingerPrint.identifier("Win32_BIOS", "IdentificationCode"),
				FingerPrint.identifier("Win32_BIOS", "SerialNumber"),
				FingerPrint.identifier("Win32_BIOS", "ReleaseDate"),
				FingerPrint.identifier("Win32_BIOS", "Version")
			});
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0004FBBC File Offset: 0x0004DDBC
		private static string diskId()
		{
			return FingerPrint.identifier("Win32_DiskDrive", "Model") + FingerPrint.identifier("Win32_DiskDrive", "Manufacturer") + FingerPrint.identifier("Win32_DiskDrive", "Signature") + FingerPrint.identifier("Win32_DiskDrive", "TotalHeads");
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0004FC0C File Offset: 0x0004DE0C
		private static string baseId()
		{
			return FingerPrint.identifier("Win32_BaseBoard", "Model") + FingerPrint.identifier("Win32_BaseBoard", "Manufacturer") + FingerPrint.identifier("Win32_BaseBoard", "Name") + FingerPrint.identifier("Win32_BaseBoard", "SerialNumber");
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0004FC5A File Offset: 0x0004DE5A
		private static string videoId()
		{
			return FingerPrint.identifier("Win32_VideoController", "DriverVersion") + FingerPrint.identifier("Win32_VideoController", "Name");
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0004FC7F File Offset: 0x0004DE7F
		private static string macId()
		{
			return FingerPrint.identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
		}

		// Token: 0x0400036F RID: 879
		private static string fingerPrint = string.Empty;
	}
}

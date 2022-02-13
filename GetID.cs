using System;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace T6TC
{
	// Token: 0x02000067 RID: 103
	public class GetUniqueID
	{
		// Token: 0x06000223 RID: 547 RVA: 0x0004FCA4 File Offset: 0x0004DEA4
		public static string getUniqueID(string drive)
		{
			if (drive == string.Empty)
			{
				foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
				{
					if (driveInfo.IsReady)
					{
						drive = driveInfo.RootDirectory.ToString();
						break;
					}
				}
			}
			if (drive.EndsWith(":\\"))
			{
				drive = drive.Substring(0, drive.Length - 2);
			}
			string volumeSerial = GetUniqueID.getVolumeSerial(drive);
			string cpuid = GetUniqueID.getCPUID();
			return cpuid.Substring(13) + cpuid.Substring(1, 4) + volumeSerial + cpuid.Substring(4, 4);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0004FD3C File Offset: 0x0004DF3C
		public static string getVolumeSerial(string drive)
		{
			ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
			managementObject.Get();
			string result = managementObject["VolumeSerialNumber"].ToString();
			managementObject.Dispose();
			return result;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0004FD7C File Offset: 0x0004DF7C
		public static string getCPUID()
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass("win32_processor").GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (text == "")
				{
					text = managementObject.Properties["processorID"].Value.ToString();
					break;
				}
			}
			return text;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0004FE04 File Offset: 0x0004E004
		public static string EnPCName()
		{
			string result = "";
			string machineName = Environment.MachineName;
			return result;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0004FE14 File Offset: 0x0004E014
		public static string LastID()
		{
			string result;
			try
			{
				result = GetUniqueID.getUniqueID("C");
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Concat(new string[]
				{
					"An error occured while generating ID. this most likely means your PC has System.Management disabled. Please contact support and attach an image of this message. The console will now close",
					Environment.NewLine,
					Environment.NewLine,
					Environment.NewLine,
					ex.GetType().ToString(),
					Environment.NewLine,
					ex.Message
				}));
				Environment.Exit(0);
				result = FingerPrint.Value("X3L9Ai4Hgx5h6NPasXY67n4BdCrR").Replace("-", "").Substring(0, 19);
			}
			return result;
		}
	}
}

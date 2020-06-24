using System;
using System.IO;
using System.Text;
using xor.Properties;

namespace xor
{
	internal class XorTool
	{
		public static Action<string> SetText;

		public static Action enablebutton;

		public static void DecryptFolder(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileInfo[] files = directoryInfo.GetFiles();
			DirectoryInfo[] dirs = directoryInfo.GetDirectories();
			FileInfo[] array = files;
			foreach (FileInfo file in array)
			{
				if (file.Extension == ".asb")
				{
					Decrypt(file.FullName);
				}
			}
			DirectoryInfo[] array2 = dirs;
			for (int i = 0; i < array2.Length; i++)
			{
				DecryptFolder(array2[i].FullName);
			}
		}

		public static void Decrypt(string path)
		{
			byte[] buffer0 = new byte[612];
			int count = 0;
			int[] bytecount = new int[5]
			{
				612,
				612,
				612,
				612,
				112
			};
			int index = 0;
			byte[] key64 = Resources.key64;
			
			FileStream inputstream = File.OpenRead(path);
			FileStream outputfile = new FileStream(path + ".decrypted", FileMode.Create);
			byte[] mark = new byte[4];
			inputstream.Read(mark, 0, 4);
			if (!(Encoding.ASCII.GetString(mark) == "mark"))
			{
				SetText(Path.GetFileName(path) + " 不是加密文件.跳过..");
				return;
			}
			while (true)
			{
				count = inputstream.Read(buffer0, 0, bytecount[index]);
				int offset = (int)(outputfile.Position % 64);
				for (int i = 0; i < count; i++)
				{
					buffer0[i] ^= key64[(offset + i) % 64];
				}
				outputfile.Write(buffer0, 0, count);
				if (count < bytecount[index])
				{
					break;
				}
				inputstream.Position += 4L;
				index = (index + 1) % 5;
			}
			SetText(Path.GetFileName(path) + " 解密完成.");
			inputstream.Close();
			outputfile.Close();
		}

		public static void Encrypt(string path)
		{
			byte[] buffer = new byte[616];
			int count2 = 0;
			int[] bytecount = new int[5]
			{
				612,
				612,
				612,
				612,
				112
			};
			int index = 0;
			byte[] key64 = Resources.key64;
			FileStream inputstream = new FileStream(path, FileMode.Open);
			if (path.EndsWith(".decrypted"))
			{
				path = path.Replace(".decrypted", string.Empty);
			}
			FileStream outputstream = new FileStream(path + ".encrypted", FileMode.Create);
			outputstream.Write(Encoding.Default.GetBytes("mark"), 0, 4);
			while (inputstream.Position < inputstream.Length)
			{
				long pos = inputstream.Position;
				count2 = inputstream.Read(buffer, 0, bytecount[index]);
				for (int i = 0; i < count2; i++)
				{
					buffer[i] ^= key64[(pos + i) % 64];
				}
				outputstream.Write(buffer, 0, count2);
				if (count2 == bytecount[index])
				{
					outputstream.Write(buffer, 0, 4);
				}
				else if (count2 < bytecount[index])
				{
					break;
				}
				index = (index + 1) % 5;
			}
			SetText(Path.GetFileName(path) + " 加密完成.");
			inputstream.Close();
			outputstream.Close();
		}
	}
}

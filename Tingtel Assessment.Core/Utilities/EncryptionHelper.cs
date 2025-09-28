using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.Utilities
{
	public static class EncryptionHelper
	{
		private static readonly string EncryptionKey = "MySpecialEncryptionKey123!";

		public static string Encrypt(string text)
		{
			byte[] clearBytes = Encoding.UTF8.GetBytes(text);

			using (Aes aes = Aes.Create())
			{
				var keyBytes = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32));
				aes.Key = keyBytes;
				aes.IV = new byte[16];

				using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
				{
					byte[] encrypted = encryptor.TransformFinalBlock(clearBytes, 0, clearBytes.Length);
					return Convert.ToBase64String(encrypted);
				}
			}
		}

		public static string Decrypt(string encryptedText)
		{
			byte[] cipherBytes = Convert.FromBase64String(encryptedText);
			using (Aes aes = Aes.Create())
			{
				var keyBytes = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32));
				aes.Key = keyBytes;
				aes.IV = new byte[16];

				using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
				{
					byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
					return Encoding.UTF8.GetString(decrypted);
				}
			}
		}
	}
}

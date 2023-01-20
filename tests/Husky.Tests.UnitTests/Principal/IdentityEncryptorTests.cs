﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Husky.Principal.Implementations.Tests
{
	[TestClass()]
	public class IdentityEncryptorTests
	{
		[TestMethod()]
		public void EncryptTest() {
			var token = Crypto.RandomString();
			var identity = new Identity { DisplayName = "Weixing", Id = 123, IsConsolidated = true };
			var encryptor = new IdentityEncryptor();
			var encrypted = encryptor.Encrypt(identity, token);
			var decrypted = encryptor.Decrypt(encrypted, token);

			Assert.AreEqual(identity.DisplayName, decrypted.DisplayName);
			Assert.AreEqual(identity.Id, decrypted.Id);
			Assert.AreEqual(identity.IsConsolidated, decrypted.IsConsolidated);

			var nullResult = encryptor.Decrypt(encrypted, "invalid token");
			Assert.IsNull(nullResult);
		}
	}
}
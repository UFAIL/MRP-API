using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Threading.Tasks;

namespace MRP_API.Utilities
{
    internal class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 3;
        private const int MemoryKb = 65536;       // 64 MB in KB
        private static readonly int Parallelism = Math.Max(1, Environment.ProcessorCount);

        // $argon2id$v=19$m=<mem>,t=<iter>,p=<parallel>$<base64salt>$<base64hash>
        public static string HashPassword(string password)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));

            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                Iterations = Iterations,
                MemorySize = MemoryKb
            };

            var hash = argon.GetBytes(HashSize);

            var b64Salt = Convert.ToBase64String(salt);
            var b64Hash = Convert.ToBase64String(hash);

            var phc = $"$argon2id$v=19$m={MemoryKb},t={Iterations},p={Parallelism}${b64Salt}${b64Hash}";
            return phc;
        }

        public static bool VerifyPassword(string password, string phcString)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));
            if (phcString is null) throw new ArgumentNullException(nameof(phcString));

            // erwartet: $argon2id$v=19$m=...,t=...,p=...$<salt>$<hash>
            try
            {
                var parts = phcString.Split('$', StringSplitOptions.RemoveEmptyEntries);
                // parts[0] = "argon2id"; parts[1] = "v=19"; parts[2] = "m=...,t=...,p=..."; parts[3]=salt; parts[4]=hash
                if (parts.Length != 5) return false;
                if (parts[0] != "argon2id") return false;

                var paramPart = parts[2];
                int mem = 0, iter = 0, parallel = 0;
                foreach (var kv in paramPart.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    var pair = kv.Split('=', 2);
                    if (pair.Length != 2) continue;
                    switch (pair[0])
                    {
                        case "m": mem = int.Parse(pair[1]); break;
                        case "t": iter = int.Parse(pair[1]); break;
                        case "p": parallel = int.Parse(pair[1]); break;
                    }
                }

                var salt = Convert.FromBase64String(parts[3]);
                var storedHash = Convert.FromBase64String(parts[4]);

                var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = Math.Max(1, parallel),
                    Iterations = Math.Max(1, iter),
                    MemorySize = Math.Max(1, mem)
                };

                var computed = argon.GetBytes(storedHash.Length);

                return CryptographicOperations.FixedTimeEquals(computed, storedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}

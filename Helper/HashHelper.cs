using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UGB.MVC.Aplicaciones.Seguras.Helper
{
   public static class HashHelper
    {
       /// <summary>
       /// Método para convertir una cadena a un hash
       /// </summary>
       /// <param name="password">Cadena a convertir</param>
       /// <returns>HashedPassword object</returns>
        public static HashedPassword Hash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return new HashedPassword() { Password = hashed, Salt = Convert.ToBase64String(salt) };
        }

        /// <summary>
        /// Método para validar una contraseña
        /// </summary>
        /// <param name="attemptedPassword">Contraseña ingresada</param>
        /// <param name="hash">Hash generado</param>
        /// <param name="salt">Sal generada</param>
        /// <returns>Verdadero si la contraseña es válida</returns>
        public static bool CheckHash(string attemptedPassword, string hash, string salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: attemptedPassword,
                 salt: Convert.FromBase64String(salt),
                 prf: KeyDerivationPrf.HMACSHA256,
                 iterationCount: 10000,
                 numBytesRequested: 256 / 8));
            return hash == hashed;
        }

        /// <summary>
        /// Método que retorna el hash a partir de la palabra y la sal
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static byte[] GetHash(string password, string salt)
        {
            byte[] unhashedBytes = Encoding.Unicode.GetBytes(string.Concat(salt, password));
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);
                return hashedBytes;
            }
        }
    }

    public class HashedPassword
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
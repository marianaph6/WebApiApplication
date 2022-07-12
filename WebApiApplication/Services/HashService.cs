using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApiApplication.Models;

namespace WebApiApplication.Services
{
    //Servicio de Hash → Se debe registrar el servicio en el contenedor de inversion de controles (program.cs)
    public class HashService
    {
        //Recibe texto plano y genera un salt aleatorio
        public HashResult Hash(string textoPlano)
        {
            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return Hash(textoPlano, salt);
        }

        //Recibe texto plano y salt. Se usa una salt existente
        public HashResult Hash(string textoPlano, byte[] salt)
        {
            //Se usa el algoritmo Pbkdf2 para realizar el hash
            var llaveDerivada = KeyDerivation.Pbkdf2(password: textoPlano,
                salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(llaveDerivada);

            //Se retorna el hash y el salt
            return new HashResult()
            {
                Hash = hash,
                Salt = salt
            };
        }
    }
}
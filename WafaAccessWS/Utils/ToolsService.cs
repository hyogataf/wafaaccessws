using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace WafaAccessWS.Utils
{
    public class ToolsService
    {
        private const int PAD_LIMIT = 8192;

        public const string SEPARATOR = "+";
        // const string key = "54421341544545467678789459000024554664";
        // public string keyApp = ConfigurationManager.AppSettings["AppKey"];


        //Verify(Text_A_Signer_Puis_Verifier, Element_De_Comparaison)
        public static bool Verify(string text, string signature)
        {
            try
            {
                var path = Path.Combine(HttpRuntime.AppDomainAppPath, "Contents/sigca.crt");
                Debug.WriteLine("path = " + path);
                /* var path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Contents/sigca.crt");
                 Debug.WriteLine("sigca.crt path1= " + path1);*/
                X509Certificate2 cert = new X509Certificate2(path);
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

                // Hash the data
                SHA1Managed sha1 = new SHA1Managed();
                UnicodeEncoding encoding = new UnicodeEncoding();//ASCIIEncoding ??? //byte [] hashed = Sha.ComputeHash(Encoding.UTF8.GetBytes(info));
                byte[] data = encoding.GetBytes(text);
                byte[] sign = Convert.FromBase64String(signature);
                byte[] hash = sha1.ComputeHash(data);

                return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), sign);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ToolsService Verify signature = " + e.StackTrace);
                return false;
            }
        }


        public static String createSignature(String data)
        {
            // Debug.WriteLine(" keyApp = " + getKey());
            string toSign = data + SEPARATOR + getKey();
            return encode(toSign);

        }

        public static string getKey()
        {
            return ConfigurationManager.AppSettings["AppKey"];
        }


        /*  
           JAVA version de la methode Encode juste en dessous
         * 
           public static String encode(String src) {
           try {
               MessageDigest md;
               md = MessageDigest.getInstance("SHA-1");
               byte bytes[] = src.getBytes("iso-8859-1");
               md.update(bytes, 0, bytes.length);
               byte[] sha1hash = md.digest();
               return convertToHex(sha1hash);
           } catch (Exception e) {
               throw new RuntimeException(e);
           }
       }*/
        public static string encode(String src)
        {
            try
            {
                byte[] buffer = Encoding.GetEncoding(28591).GetBytes(src);
                SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
                var sha1hash = cryptoTransformSHA1.ComputeHash(buffer);

                /*methode utilisée ds la version originale, mais renvoie un string.
                //TODO si probleme, considerer de transformer le hashText renvoyé en byte[]
                 * */
                // String hashText = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

                return convertToHex(sha1hash);
            }
            catch (Exception e)
            {
                Debug.WriteLine("toolsservice encode error = " + e.StackTrace);
                throw;
            }
        }


        private static String convertToHex(byte[] sha1hash)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < sha1hash.Length; i++)
            {
                byte c = sha1hash[i];
                addHex(builder, (c >> 4) & 0xf);
                addHex(builder, c & 0xf);
            }
            return builder.ToString();
        }

        private static void addHex(StringBuilder builder, int c)
        {
            if (c < 10)
                builder.Append((char)(c + '0'));
            else
                builder.Append((char)(c + 'a' - 10));
        }

        /*   
         * JAVA version de la methode en dessous.
         * Ne peut pas reproduire le principe final char[] buf = new char[repeat]; en c#.
         * 
         * 
         * private static string padding1(int repeat, char padChar)
               throws IndexOutOfBoundsException {
           if (repeat < 0) {
               throw new IndexOutOfBoundsException(
                       "Cannot pad a negative amount: " + repeat);
           }
           final char[] buf = new char[repeat];
           for (int i = 0; i < buf.length; i++) {
               buf[i] = padChar;
           }
           return new String(buf);
       }*/
        private static string padding(int repeat, char padChar)
        {
            try
            {
                if (repeat < 0)
                {
                    throw new IndexOutOfRangeException(
                            "Cannot pad a negative amount: " + repeat);
                }
                // readonly char[] buf1 = new char[repeat];
                /*const*/
                char[] buf = new char[repeat];
                for (int i = 0; i < buf.Length; i++)
                {
                    buf[i] = padChar;
                }
                return new String(buf);
            }
            catch (Exception e)
            {
                Debug.WriteLine("padding error = " + e.StackTrace);
                throw;
            }
        }



        public static bool isEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        public static string leftPad(string str, int size, char padChar)
        {
            if (str == null)
            {
                return null;
            }
            int pads = size - str.Length;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (pads > PAD_LIMIT)
            {
                return leftPad(str, size, Convert.ToString(padChar));
            }
            return string.Concat(padding(pads, padChar), str);
        }

        public static string leftPad(string str, int size, string padStr)
        {
            if (str == null)
            {
                return null;
            }
            if (isEmpty(padStr))
            {
                padStr = " ";
            }
            int padLen = padStr.Length;
            int strLen = str.Length;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (padLen == 1 && pads <= PAD_LIMIT)
            {
                return leftPad(str, size, padStr[0]);
            }

            if (pads == padLen)
            {
                return string.Concat(padStr, str);
            }
            else if (pads < padLen)
            {
                return string.Concat(padStr.Substring(0, pads), str);
            }
            else
            {
                char[] padding = new char[pads];
                char[] padChars = padStr.ToCharArray();
                for (int i = 0; i < pads; i++)
                {
                    padding[i] = padChars[i % padLen];
                }
                return string.Concat(new string(padding), str);
            }
        }


        public static string Sign(string text)
        {
            // Access Personal (MY) certificate store of current user
            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);

            // Find the certificate we'll use to sign            
            //   RSACryptoServiceProvider csp = null;
            /*  foreach (X509Certificate2 cert in my.Certificates)
              {
                  if (cert.Subject.Contains(certSubject))
                  {
                      // We found it. 
                      // Get its associated CSP and private key
                      csp = (RSACryptoServiceProvider)cert.PrivateKey;
                  }
              }*/
            var path = Path.Combine(HttpRuntime.AppDomainAppPath, "Contents/sigca.crt");
            X509Certificate2 cert = new X509Certificate2(path);
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;

            if (csp == null)
            {
                throw new Exception("No valid cert was found");
            }

            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = sha1.ComputeHash(data);

            // Sign the hash
            byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            return Convert.ToBase64String(signature);

        }


        static byte[] SignOfficial(string text, string certSubject)
        {
            // Access Personal (MY) certificate store of current user
            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);

            // Find the certificate we'll use to sign            
            //   RSACryptoServiceProvider csp = null;
            /*  foreach (X509Certificate2 cert in my.Certificates)
              {
                  if (cert.Subject.Contains(certSubject))
                  {
                      // We found it. 
                      // Get its associated CSP and private key
                      csp = (RSACryptoServiceProvider)cert.PrivateKey;
                  }
              }*/
            var path = Path.Combine(HttpRuntime.AppDomainAppPath, "Contents/sigca.crt");
            X509Certificate2 cert = new X509Certificate2(path);
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;

            if (csp == null)
            {
                throw new Exception("No valid cert was found");
            }

            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = sha1.ComputeHash(data);

            // Sign the hash
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }


        public static bool VerifyHashes(string hashCalculated, string hashReceived)
        {
            bool IsOk = false;
            if (string.IsNullOrWhiteSpace(hashCalculated) || string.IsNullOrWhiteSpace(hashReceived) || hashCalculated == "null" || hashReceived == "null")
            {
                return IsOk;
            }

            if (hashCalculated.Equals(hashReceived))
            {
                IsOk = true;
            }
            else
            {
                IsOk = false;
            }
            return IsOk;
        }
    }



}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JianTongFun
{
    public class OperationFunction
    {
        private static OperationFunction _operation = null;
        public static OperationFunction CreateInstance
        {
            get
            {
                if (_operation == null)
                {
                    _operation = new OperationFunction();
                }
                return _operation;
            }
        }

        #region DES 加密解密
        /// <summary>
        /// DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="EncryptString">待加密的密文</param>
        /// <param name="EncryptKey">加密的密钥</param>
        /// <returns>returns</returns>
        public string DESEncrypt(string EncryptString, string EncryptKey)
        {
            if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }
            if (EncryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strEncrypt = "";
            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();
            try
            {
                byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);
                m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);
                m_cstream.FlushFinalBlock();
                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
                m_stream.Close(); m_stream.Dispose();
                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }

            return m_strEncrypt;
        }
        /// <summary>
        /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="DecryptString">待解密的密文</param>
        /// <param name="DecryptKey">解密的密钥</param>
        /// <returns>returns</returns>
        public string DESDecrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
            if (DecryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strDecrypt = "";
            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();
            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);
                m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                m_cstream.FlushFinalBlock();
                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                m_stream.Close(); m_stream.Dispose();
                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }

            return m_strDecrypt;
        }
        #endregion

        #region string和int类型转换
        public int StringConvertToInt(string value, int firstValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return firstValue;
            }
            int i = 0;
            bool result = Int32.TryParse(value, out i);
            return result ? i : firstValue;
        }

        public float StringConvertToFloat(string value, float firstValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return firstValue;
            }
            float i = 0;
            bool result = float.TryParse(value, out i);
            return result ? i : firstValue;
        }

        public decimal StringConvertToDecimal(string value, decimal firstValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return firstValue;
            }
            decimal i = 0;
            bool result = decimal.TryParse(value, out i);
            return result ? i : firstValue;
        }

        public DateTime StringConvertToDateTime(string value, DateTime firstValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return firstValue;
            }
            DateTime i = DateTime.MinValue;
            bool result = DateTime.TryParse(value, out i);
            return result ? i : firstValue;
        }
        #endregion
    }
}

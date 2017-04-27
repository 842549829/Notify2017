namespace Notify.Code.Encrypt
{
    /// <summary>
    /// RSA 密钥。公钥and私钥
    /// </summary>
    public class RSAKey
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }
    }
}
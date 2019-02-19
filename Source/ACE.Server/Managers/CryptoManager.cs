using ACE.Common;
using log4net;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ACE.Server.Managers
{
    /// <summary>
    /// ServerManager handles cryptography properly.
    /// </summary>
    /// <remarks>
    ///   Subordinates:
    ///     1. Transfer Manager
    ///     2. Web Api
    ///   Known issue:
    ///     1. self-signed certificates for convenience
    /// </remarks>
    public static class CryptoManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The thumbprint of the certified public key
        /// </summary>
        public static string Thumbprint => CertificateTransferManager.Thumbprint;
        public static X509Certificate2 CertificateWebApi { get; private set; } = null;
        private static X509Certificate2 CertificateTransferManager { get; set; } = null;
        private const string CertFileNameWebApi = "webapi.pfx";
        private const string CertFileNameCharTransferSigning = "transfer.pfx";
        private const string CNWebApi = "ACEmulator WebApi";
        private const string CNCharTransfer = "ACEmulator Transfer Manager";

        public static void Initialize()
        {
            EnsureCert(CertFileNameWebApi, CNWebApi, 3650);
            EnsureCert(CertFileNameCharTransferSigning, CNCharTransfer, 3650);

            CertificateWebApi = new X509Certificate2(CertFilePathWebApi);
            CertificateTransferManager = new X509Certificate2(CertFilePathCharTransferSigning);
            log.Info($"Server certificate thumbprint: {CertificateTransferManager.Thumbprint}");

            int trustedCount = ConfigManager.Config.Transfer.AllowMigrationFrom.Count;
            string plural = (trustedCount > 1 || trustedCount == 0) ? "s" : "";
            log.Info($"Found {trustedCount} trusted server{plural} in TrustedServerCertThumbprints configuration.");

            foreach (string trusted in ConfigManager.Config.Transfer.AllowMigrationFrom)
            {
                log.Debug($"Trusted server certificate thumbprint: {trusted}");
            }
        }
        public static string CertificatePath
        {
            get
            {
                var u = Path.Combine(ServerManager.BasePath, "Certificates");
                if (!Directory.Exists(u))
                    try
                    {
                        Directory.CreateDirectory(u);
                        log.Info($"Created directory {u}");
                    }
                    catch { }
                return u;
            }
        }
        private static string CertFilePathWebApi => Path.Combine(CertificatePath, CertFileNameWebApi);
        private static string CertFilePathCharTransferSigning => Path.Combine(CertificatePath, CertFileNameCharTransferSigning);
        public static void EnsureCert(string certFileName, string certCN, int daysUntilExpire)
        {
            string CertFile = Path.Combine(CertificatePath, certFileName);
            if (!File.Exists(CertFile))
            {
                X509Certificate2 newCert = BuildSelfSignedServerCertificate(certCN, daysUntilExpire);
                File.WriteAllBytes(CertFile, newCert.Export(X509ContentType.Pkcs12));
                log.Info($"{certCN} certificate generated and saved to {CertFile}");
                newCert.Dispose();
            }
        }

        /// <summary>
        /// Generate a new 2048 bit RSA keypair and self sign the public key with the private key
        /// https://stackoverflow.com/a/50138133/6620171
        /// </summary>
        /// <param name="CommonName"></param>
        /// <returns></returns>
        private static X509Certificate2 BuildSelfSignedServerCertificate(string CommonName, int daysUntilExpire)
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);
            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={CommonName}");
            using (RSA rsa = RSA.Create(2048))
            {
                CertificateRequest request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));
                request.CertificateExtensions.Add(
                   new X509EnhancedKeyUsageExtension(
                       new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
                request.CertificateExtensions.Add(sanBuilder.Build());
                X509Certificate2 certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(daysUntilExpire)));
                certificate.FriendlyName = CommonName;
                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, "WeNeedASaf3rPassword"), "WeNeedASaf3rPassword", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            }
        }

        /// <summary>
        /// Save the signed public key as an X.509 certificate file
        /// </summary>
        /// <param name="filePath"></param>
        public static void ExportCert(string filePath)
        {
            File.WriteAllBytes(filePath, CertificateTransferManager.Export(X509ContentType.Cert));
        }

        /// <summary>
        /// return the signed public key as an X.509 certificate file as a byte array
        /// </summary>
        public static byte[] ExportCertAsBytes()
        {
            return CertificateTransferManager.Export(X509ContentType.Cert);
        }

        /// <summary>
        /// Sign a file with the private key and save the signature in a file alongside
        /// the signed file with the same file name as the signed file and a file extension of '.signature'
        /// </summary>
        /// <param name="filePath"></param>
        public static void SignFile(string filePath)
        {
            RSA csp = CertificateTransferManager.GetRSAPrivateKey();
            string sigPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".signature");
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                File.WriteAllBytes(sigPath, csp.SignData(fs, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }

        /// <summary>
        /// Verify the signature of a file using the signature file alongside
        /// the signed file with the same file name as the signed file and a file extension of '.signature'
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="signer"></param>
        /// <returns></returns>
        public static bool VerifySignedFile(string filePath, X509Certificate2 signer)
        {
            string sigPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".signature");
            if (!File.Exists(sigPath))
            {
                return false;
            }
            RSA csp = signer.GetRSAPublicKey();
            using (FileStream fsData = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return csp.VerifyData(fsData, File.ReadAllBytes(sigPath), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        /// <summary>
        /// Sign data with the private key
        /// </summary>
        /// <param name="data">the data to sign</param>
        /// <returns>the signature</returns>
        public static byte[] SignData(string data)
        {
            RSA csp = CertificateTransferManager.GetRSAPrivateKey();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                return csp.SignData(ms, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        /// <summary>
        /// Verify a signature of some data
        /// </summary>
        /// <param name="data">the signed data</param>
        /// <param name="signature">the signature</param>
        /// <param name="signer">the certified public key</param>
        /// <returns>whether the signature is valid or not</returns>
        public static bool VerifySignedData(string data, byte[] signature, X509Certificate2 signer)
        {
            RSA csp = signer.GetRSAPublicKey();
            return csp.VerifyData(Encoding.UTF8.GetBytes(data), signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}

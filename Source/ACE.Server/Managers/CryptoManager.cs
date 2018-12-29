using ACE.Common;
using log4net;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ACE.Server.Managers
{
    /// <summary>
    /// ServerManager handles cryptography properly.
    /// </summary>
    /// <remarks>
    ///   Possibly useful for:
    ///     1. Character transfers
    ///   Known issue:
    ///     1. Uses only self-signed certificates.
    /// </remarks>
    public static class CryptoManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The thumbprint of the certified public key
        /// </summary>
        public static string Thumbprint => Certificate.Thumbprint;
        private static X509Certificate2 Certificate { get; set; } = null;
        private const string CertFileName = "server.pfx";

        public static void Initialize()
        {
            string CertFile = Path.Combine(ServerManager.CertificatePath, CertFileName);
            if (!File.Exists(CertFile))
            {
                X509Certificate2 newCert = BuildSelfSignedServerCertificate("ACEmulator");
                File.WriteAllBytes(CertFile, newCert.Export(X509ContentType.Pkcs12));
                log.Info($"New certificate generated and saved to {CertFile}");
                newCert.Dispose();
            }
            Certificate = new X509Certificate2(CertFile);
            log.Info($"Server certificate thumbprint: {Certificate.Thumbprint}");

            int trustedCount = ConfigManager.Config.Server.TrustedServerCertThumbprints.Count;
            string plural = (trustedCount > 1 || trustedCount == 0) ? "s" : "";
            log.Info($"Found {trustedCount} trusted server{plural} in TrustedServerCertThumbprints configuration.");

            foreach (string trusted in ConfigManager.Config.Server.TrustedServerCertThumbprints)
            {
                log.Debug($"Trusted server certificate thumbprint: {trusted}");
            }
        }

        /// <summary>
        /// Generate a new 2048 bit RSA keypair and self sign the public key with the private key
        /// https://stackoverflow.com/a/50138133/6620171
        /// </summary>
        /// <param name="CommonName"></param>
        /// <returns></returns>
        private static X509Certificate2 BuildSelfSignedServerCertificate(string CommonName)
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
                X509Certificate2 certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
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
            File.WriteAllBytes(filePath, Certificate.Export(X509ContentType.Cert));
        }

        /// <summary>
        /// Sign a file with the private key and save the signature in a file alongside
        /// the signed file with the same file name as the signed file and a file extension of '.signature'
        /// </summary>
        /// <param name="filePath"></param>
        public static void SignFile(string filePath)
        {
            RSA csp = Certificate.GetRSAPrivateKey();
            string sigPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".signature");
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                File.WriteAllBytes(sigPath, csp.SignData(fs, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }
    }
}

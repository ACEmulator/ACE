using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using log4net;

using ACE.Common;

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

        public static void Initialize()
        {
            var CertFile = Path.Combine(ServerManager.CertificatePath, CertFileName);
            if (!File.Exists(CertFile))
            {
                var newCert = BuildSelfSignedServerCertificate("ACEmulator");
                File.WriteAllBytes(CertFile, newCert.Export(X509ContentType.Pkcs12));
                log.Info($"New certificate generated and saved to {CertFile}");
                newCert.Dispose();
            }
            Certificate = new X509Certificate2(CertFile);
            log.Info($"Server certificate thumbprint: {Certificate.Thumbprint}");

            var trustedCount = ConfigManager.Config.Server.TrustedServerCertThumbprints.Count;
            var plural = (trustedCount > 1 || trustedCount == 0) ? "s" : "";
            log.Info($"Found {trustedCount} trusted server{plural} in TrustedServerCertThumbprints configuration.");

            foreach(var trusted in ConfigManager.Config.Server.TrustedServerCertThumbprints)
            {
                log.Debug($"Trusted server certificate thumbprint: {trusted}");
            }
        }
        public static X509Certificate2 Certificate { get; private set; } = null;
        public const string CertFileName = "server.pfx";

        /// <summary>
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

    }
}

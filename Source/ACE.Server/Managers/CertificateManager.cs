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
    /// CertificateManager handles cryptography properly.
    /// </summary>
    /// <remarks>
    ///   Subordinates:
    ///     1. Transfer Manager
    ///     2. Web Api
    ///   Known issue:
    ///     1. self-signed certificates for convenience when operator doesn't supply their own
    /// </remarks>
    public static class CertificateManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The thumbprint of the certified public key
        /// </summary>
        public static string Thumbprint => CertificateDataSigner?.Thumbprint;
        public static X509Certificate2 CertificateWebApi { get; private set; } = null;
        private static X509Certificate2 CertificateDataSigner { get; set; } = null;
        private const string AutoFileNameKeyAndCertBundleWebApi = "webapi.pfx";
        private const string AutoFileNameKeyAndCertBundleDataSigner = "datasigner.pfx";
        private const string AutoCNWebApi = "ACEmulator WebApi";
        private const string AutoCNDataSigner = "ACEmulator Data Signer";

        public static void Initialize()
        {
            try
            {
                if (ConfigManager.Config.WebApi.Enabled)
                {
                    if (ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi == null)
                    {
                        EnsureCert(AutoFileNameKeyAndCertBundleWebApi, AutoCNWebApi, 3650);
                        CertificateWebApi = new X509Certificate2(Path.Combine(EnsureCertificatePath(log), AutoFileNameKeyAndCertBundleWebApi));
                    }
                    else
                    {
                        if (File.Exists(ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi))
                        {
                            string password = null;
                            if (!string.IsNullOrEmpty(ConfigManager.Config.Server.CertificateConfiguration.PasswordKeyAndCertBundleWebApi))
                            {
                                password = ConfigManager.Config.Server.CertificateConfiguration.PasswordKeyAndCertBundleWebApi;
                            }
                            else if (ConfigManager.Config.Server.CertificateConfiguration.PasswordStartupPromptKeyAndCertBundleWebApi)
                            {
                                password = PasswordPrompt("supplied WebAPI key and cert bundle file");
                            }
                            try
                            {
                                if (string.IsNullOrEmpty(password))
                                {
                                    CertificateWebApi = new X509Certificate2(ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi);
                                }
                                else
                                {
                                    CertificateWebApi = new X509Certificate2(ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi, password);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error($"Unable to use the supplied WebAPI key and cert bundle file: {ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi}", ex);
                            }
                        }
                        else
                        {
                            log.Error($"File doesn't exist.  FilePathKeyAndCertBundleWebApi: {ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi}");
                        }
                    }
                }
                if (ConfigManager.Config.Transfer.AllowBackup || ConfigManager.Config.Transfer.AllowImport || ConfigManager.Config.Transfer.AllowMigrate)
                {
                    if (ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleDataSigner == null)
                    {
                        EnsureCert(AutoFileNameKeyAndCertBundleDataSigner, AutoCNDataSigner, 3650);
                        CertificateDataSigner = new X509Certificate2(Path.Combine(EnsureCertificatePath(log), AutoFileNameKeyAndCertBundleDataSigner));
                        log.Info($"Server thumbprint: {CertificateDataSigner.Thumbprint}");
                    }
                    else
                    {
                        if (File.Exists(ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleDataSigner))
                        {
                            string password = null;
                            if (!string.IsNullOrEmpty(ConfigManager.Config.Server.CertificateConfiguration.PasswordKeyAndCertBundleDataSigner))
                            {
                                password = ConfigManager.Config.Server.CertificateConfiguration.PasswordKeyAndCertBundleDataSigner;
                            }
                            else if (ConfigManager.Config.Server.CertificateConfiguration.PasswordStartupPromptKeyAndCertBundleDataSigner)
                            {
                                password = PasswordPrompt("supplied DataSigner key and cert bundle file");
                            }
                            try
                            {
                                if (string.IsNullOrEmpty(password))
                                {
                                    CertificateDataSigner = new X509Certificate2(ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleDataSigner);
                                }
                                else
                                {
                                    CertificateDataSigner = new X509Certificate2(ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleDataSigner, password);
                                }
                                log.Info($"Server thumbprint: {Thumbprint}");
                            }
                            catch (Exception ex)
                            {
                                log.Error($"Unable to use the supplied DataSigner key and cert bundle file: {ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleWebApi}", ex);
                            }
                        }
                        else
                        {
                            log.Fatal($"File doesn't exist.  FilePathKeyAndCertBundleDataSigner: {ConfigManager.Config.Server.CertificateConfiguration.FilePathKeyAndCertBundleDataSigner}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Certificate manager failed to initialize.", ex);
            }
        }
        private static string PasswordPrompt(string subject)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Enter password for {subject}: ");
            string pass = PasswordPrompt();
            Console.WriteLine();
            return pass;
        }
        /// <summary>
        /// https://stackoverflow.com/a/3404522
        /// </summary>
        /// <returns></returns>
        private static string PasswordPrompt()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return pass;
        }

        public static string EnsureCertificatePath(ILog log = null)
        {
            string u = Path.Combine(ServerManager.EnsureBasePath(log), "certificates");
            if (!Directory.Exists(u))
            {
                try
                {
                    Directory.CreateDirectory(u);
                    log?.Info($"Created certificates directory {u}");
                }
                catch (Exception ex) { log?.Fatal($"Failed to create certificates directory {u}", ex); }
            }
            return u;
        }

        public static void EnsureCert(string certFileName, string certCN, int daysUntilExpire)
        {
            string CertFile = Path.Combine(EnsureCertificatePath(log), certFileName);
            if (!File.Exists(CertFile))
            {
                X509Certificate2 newCert = BuildSelfSignedServerCertificate(certCN, daysUntilExpire);
                File.WriteAllBytes(CertFile, newCert.Export(X509ContentType.Pkcs12));
                log.Info($"{certCN} key and cert bundle generated and saved to {CertFile}");
                newCert.Dispose();
            }
        }

        /// <summary>
        /// Generate a new 2048 bit RSA key and cert bundle and self sign the public key with the private key
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
            File.WriteAllBytes(filePath, CertificateDataSigner.Export(X509ContentType.Cert));
        }

        /// <summary>
        /// return the signed public key as an X.509 certificate file as a byte array
        /// </summary>
        public static byte[] ExportCertAsBytes()
        {
            return CertificateDataSigner.Export(X509ContentType.Cert);
        }

        /// <summary>
        /// Sign a file with the private key and save the signature in a file alongside
        /// the signed file with the same file name as the signed file and a file extension of '.signature'
        /// </summary>
        /// <param name="filePath"></param>
        public static void SignFile(string filePath)
        {
            RSA csp = CertificateDataSigner.GetRSAPrivateKey();
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
            RSA csp = CertificateDataSigner.GetRSAPrivateKey();
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

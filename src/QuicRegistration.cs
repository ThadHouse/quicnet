using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using QuicNet.Interop;

namespace QuicNet
{
    public readonly struct QuicRegistrationConfig : IEquatable<QuicRegistrationConfig>
    {
        public QuicRegistrationConfig(string? appName, QuicExecutionProfile executionProfile = 0)
        {
            AppName = appName;
            ExecutionProfile = executionProfile;
        }

        public string? AppName { get; }
        public QuicExecutionProfile ExecutionProfile { get; }

        public override bool Equals(object? obj)
        {
            return obj is QuicRegistrationConfig config && Equals(config);
        }

        public bool Equals(QuicRegistrationConfig other)
        {
            return AppName == other.AppName &&
                   ExecutionProfile == other.ExecutionProfile;
        }

        public override int GetHashCode()
        {
            var hashCode = -125507555;
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(AppName);
            hashCode = hashCode * -1521134295 + ExecutionProfile.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(QuicRegistrationConfig left, QuicRegistrationConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(QuicRegistrationConfig left, QuicRegistrationConfig right)
        {
            return !(left == right);
        }
    }

    public sealed class QuicRegistration : IDisposable
    {

        private readonly IQuicInteropApi m_nativeApi;
        internal unsafe readonly QuicHandle* m_handle;

        internal unsafe QuicRegistration(IQuicInteropApi nativeApi)
        {
            m_nativeApi = nativeApi;
            QuicHandle* handle = null;
            m_nativeApi.RegistrationOpen(null, &handle);
            m_handle = handle;
        }

        internal unsafe QuicRegistration(QuicRegistrationConfig config, IQuicInteropApi nativeApi)
        {
            m_nativeApi = nativeApi;
            QuicHandle* handle = null;
            QuicNativeRegistrationConfig nativeRegConfig = new QuicNativeRegistrationConfig
            {
                AppName = null,
                ExecutionProfile = config.ExecutionProfile
            };

            if (config.AppName != null)
            {
                int appNameLength = config.AppName.Length;
                int maxAppNameLength = Encoding.UTF8.GetMaxByteCount(appNameLength);
                Span<byte> appNameSpan = appNameLength < 256 ? stackalloc byte[256] : new byte[maxAppNameLength];

                fixed (byte* appNamePtr = appNameSpan)
                {
                    fixed (char* strNamePtr = config.AppName)
                    {
                        int actualLength = Encoding.UTF8.GetBytes(strNamePtr, appNameLength, appNamePtr, maxAppNameLength);
                        appNameSpan[actualLength] = 0; // null terminator
                    }
                    nativeRegConfig.AppName = appNamePtr;
                    m_nativeApi.RegistrationOpen(&nativeRegConfig, &handle);
                }
            }
            else
            {
                m_nativeApi.RegistrationOpen(&nativeRegConfig, &handle);
            }
            m_handle = handle;
        }

        public QuicSession CreateSession()
        {
            return new QuicSession(m_nativeApi, this, Array.Empty<byte[]>());
        }

        public unsafe Task<QuicSecurityConfiguration> CreateSecurityConfiguration()
        {
            return QuicSecurityConfiguration.CreateQuicSecurityConfig(m_nativeApi, this);
        }

        public unsafe Task<QuicSecurityConfiguration> CreateSecurityConfiguration(string certFile, string keyName)
        {
            return QuicSecurityConfiguration.CreateQuicSecurityConfig(m_nativeApi, this, certFile, keyName, false);
        }

        public unsafe Task<QuicSecurityConfiguration> CreateSecurityConfiguration(X509Certificate certificate)
        {
            return QuicSecurityConfiguration.CreateQuicSecurityConfig(m_nativeApi, this, certificate);
        }

        public unsafe Task<QuicSecurityConfiguration> CreateSecurityConfiguration(ReadOnlySpan<byte> hash)
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            if (hash.Length != 20) throw new ArgumentOutOfRangeException(nameof(hash), "Hash must be 20 bytes long");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

            var nativeHash = new QuicNativeCertificateHash();
            hash.CopyTo(new Span<byte>(nativeHash.ShaHash, 20));


            return QuicSecurityConfiguration.CreateQuicSecurityConfig(m_nativeApi, this, nativeHash, null, false);
        }

        public unsafe Task<QuicConnection> ConnectToRemote(QuicAddressFamily addressFamily, string serverName, ushort serverPort)
        {

        }

        #region IDisposable Support

        public unsafe void Dispose()
        {
            m_nativeApi.RegistrationClose(m_handle);
        }
        #endregion
    }
}

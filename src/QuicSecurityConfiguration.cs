﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using QuicNet.Interop;

namespace QuicNet
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public sealed class QuicSecurityConfiguration : IDisposable
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        private readonly IQuicInteropApi m_nativeApi;
        internal unsafe QuicNativeSecConfig* m_secConfig;

        internal static async Task<QuicSecurityConfiguration> CreateQuicSecurityConfig(IQuicInteropApi api, QuicRegistration registration, string certFile, string keyFile, bool enableOcsp)
        {
            var flags = QuicSecConfigFlags.CertificateFile;
            if (enableOcsp) flags |= QuicSecConfigFlags.EnableOcsp;

            var completionSource = new TaskCompletionSource<QuicSecurityConfiguration>(TaskCreationOptions.RunContinuationsAsynchronously);

            unsafe QuicSecConfigCreateComplete RunSecConfig()
            {
                QuicSecConfigCreateComplete configComplete = (void* context, int status, QuicNativeSecConfig* config) =>
                {
                    completionSource.SetResult(new QuicSecurityConfiguration(api, config));
                };

                var callback = Marshal.GetFunctionPointerForDelegate(configComplete);

                int maxCertFileLength = Encoding.UTF8.GetMaxByteCount(certFile.Length);
                Span<byte> maxCertFileSpan = maxCertFileLength < 256 ? stackalloc byte[maxCertFileLength] : new byte[maxCertFileLength];

                int maxKeyFileLength = Encoding.UTF8.GetMaxByteCount(keyFile.Length);
                Span<byte> maxKeyFileSpan = maxKeyFileLength < 256 ? stackalloc byte[maxKeyFileLength] : new byte[maxKeyFileLength];
                fixed (byte* certFileBytePtr = maxCertFileSpan)
                fixed (byte* keyFileBytePtr = maxKeyFileSpan)
                {
                    fixed (char* certFileStrPtr = certFile)
                    {
                        int actualLength = Encoding.UTF8.GetBytes(certFileStrPtr, certFile.Length, certFileBytePtr, maxCertFileSpan.Length);
                        maxCertFileSpan[actualLength] = 0;
                    }

                    fixed (char* keyFileStrPtr = keyFile)
                    {
                        int actualLength = Encoding.UTF8.GetBytes(keyFileStrPtr, keyFile.Length, keyFileBytePtr, maxKeyFileSpan.Length);
                        maxKeyFileSpan[actualLength] = 0;
                    }

                    var file = new QuicNativeCertificateFile();
                    file.CertificateFile = certFileBytePtr;
                    file.PrivateKeyFile = keyFileBytePtr;

                    api.SecConfigCreate(registration.m_handle, flags, &file, null, null, (void*)callback);
                }

                return configComplete;
            }

            var configCompleteDelegate = RunSecConfig();

            var secConfig = await completionSource.Task.ConfigureAwait(false);

            GC.KeepAlive(configCompleteDelegate);

            return secConfig;
        }

        internal static async Task<QuicSecurityConfiguration> CreateQuicSecurityConfig(IQuicInteropApi api, QuicRegistration registration, QuicNativeCertificateHashStore certHashStore, string? principal, bool enableOcsp)
        {
            var flags = QuicSecConfigFlags.CertificateHashStore;
            if (enableOcsp) flags |= QuicSecConfigFlags.EnableOcsp;

            var completionSource = new TaskCompletionSource<QuicSecurityConfiguration>(TaskCreationOptions.RunContinuationsAsynchronously);

            unsafe QuicSecConfigCreateComplete RunSecConfig()
            {
                QuicSecConfigCreateComplete configComplete = (void* context, int status, QuicNativeSecConfig* config) =>
                {
                    completionSource.SetResult(new QuicSecurityConfiguration(api, config));
                };

                var callback = Marshal.GetFunctionPointerForDelegate(configComplete);

                var certHashCopy = certHashStore;

                if (principal == null)
                {
                    api.SecConfigCreate(registration.m_handle, flags, &certHashCopy, null, null, (void*)callback);
                }
                else
                {
                    int maxPrincipalLength = Encoding.UTF8.GetMaxByteCount(principal.Length);
                    Span<byte> principalSpan = maxPrincipalLength < 256 ? stackalloc byte[maxPrincipalLength] : new byte[maxPrincipalLength];
                    fixed (byte* principalBytePtr = principalSpan)
                    {
                        fixed (char* principalStrPtr = principal)
                        {
                            int actualLength = Encoding.UTF8.GetBytes(principalStrPtr, principal.Length, principalBytePtr, principalSpan.Length);
                            principalSpan[actualLength] = 0;
                        }
                        api.SecConfigCreate(registration.m_handle, flags, &certHashCopy, principalBytePtr, null, (void*)callback);
                    }
                }

                return configComplete;
            }

            var configCompleteDelegate = RunSecConfig();

            var secConfig = await completionSource.Task.ConfigureAwait(false);

            GC.KeepAlive(configCompleteDelegate);

            return secConfig;
        }

        internal static async Task<QuicSecurityConfiguration> CreateQuicSecurityConfig(IQuicInteropApi api, QuicRegistration registration, X509Certificate certificate)
        {
            var completionSource = new TaskCompletionSource<QuicSecurityConfiguration>(TaskCreationOptions.RunContinuationsAsynchronously);

            unsafe QuicSecConfigCreateComplete RunSecConfig()
            {
                QuicSecConfigCreateComplete configComplete = (void* context, int status, QuicNativeSecConfig* config) =>
                {
                    completionSource.SetResult(new QuicSecurityConfiguration(api, config));
                };

                var callback = Marshal.GetFunctionPointerForDelegate(configComplete);

                api.SecConfigCreate(registration.m_handle, QuicSecConfigFlags.CertificateContext, (void*)certificate.Handle, null, null, (void*)callback);

                return configComplete;
            }

            var configCompleteDelegate = RunSecConfig();

            var secConfig = await completionSource.Task.ConfigureAwait(false);

            GC.KeepAlive(configCompleteDelegate);

            return secConfig;
        }

        internal static async Task<QuicSecurityConfiguration> CreateQuicSecurityConfig(IQuicInteropApi api, QuicRegistration registration)
        {
            var completionSource = new TaskCompletionSource<QuicSecurityConfiguration>(TaskCreationOptions.RunContinuationsAsynchronously);

            unsafe QuicSecConfigCreateComplete RunSecConfig()
            {
                QuicSecConfigCreateComplete configComplete = (void* context, int status, QuicNativeSecConfig* config) =>
                {
                    completionSource.SetResult(new QuicSecurityConfiguration(api, config));
                };

                var callback = Marshal.GetFunctionPointerForDelegate(configComplete);

                api.SecConfigCreate(registration.m_handle, QuicSecConfigFlags.None, null, null, null, (void*)callback);

                return configComplete;
            }

            var configCompleteDelegate = RunSecConfig();

            var secConfig = await completionSource.Task.ConfigureAwait(false);

            GC.KeepAlive(configCompleteDelegate);

            return secConfig;
        }

        internal static async Task<QuicSecurityConfiguration> CreateQuicSecurityConfig(IQuicInteropApi api, QuicRegistration registration, QuicNativeCertificateHash certHash, string? principal, bool enableOcsp)
        {
            var flags = QuicSecConfigFlags.CertificateHash;
            if (enableOcsp) flags |= QuicSecConfigFlags.EnableOcsp;

            var completionSource = new TaskCompletionSource<QuicSecurityConfiguration>(TaskCreationOptions.RunContinuationsAsynchronously);

            unsafe QuicSecConfigCreateComplete RunSecConfig()
            {
                QuicSecConfigCreateComplete configComplete = (void* context, int status, QuicNativeSecConfig* config) =>
                {
                    completionSource.SetResult(new QuicSecurityConfiguration(api, config));
                };

                var callback = Marshal.GetFunctionPointerForDelegate(configComplete);

                var certHashCopy = certHash;

                if (principal == null)
                {
                    api.SecConfigCreate(registration.m_handle, flags, &certHashCopy, null, null, (void*)callback);
                }
                else
                {
                    int maxPrincipalLength = Encoding.UTF8.GetMaxByteCount(principal.Length);
                    Span<byte> principalSpan = maxPrincipalLength < 256 ? stackalloc byte[maxPrincipalLength] : new byte[maxPrincipalLength];
                    fixed (byte* principalBytePtr = principalSpan)
                    {
                        fixed (char* principalStrPtr = principal)
                        {
                            int actualLength = Encoding.UTF8.GetBytes(principalStrPtr, principal.Length, principalBytePtr, principalSpan.Length);
                            principalSpan[actualLength] = 0;
                        }
                        api.SecConfigCreate(registration.m_handle, flags, &certHashCopy, principalBytePtr, null, (void*)callback);
                    }
                }

                return configComplete;
            }

            var configCompleteDelegate = RunSecConfig();

            var secConfig = await completionSource.Task.ConfigureAwait(false);

            GC.KeepAlive(configCompleteDelegate);

            return secConfig;
        }

        internal unsafe QuicSecurityConfiguration(IQuicInteropApi api, QuicNativeSecConfig* secConfig)
        {
            m_nativeApi = api;
            m_secConfig = secConfig;
        }

        public unsafe void Dispose()
        {
            m_nativeApi.SecConfigDelete(m_secConfig);
        }
    }
}

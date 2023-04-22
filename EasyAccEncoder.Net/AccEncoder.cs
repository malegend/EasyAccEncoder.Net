using System;
using System.Buffers;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace EasyAccEncoder
{
    public unsafe class AccEncoder : IDisposable
    {
        private const string DllName = "AccEncoder";
        private IntPtr handle;
        private readonly AccInitOptions options;
        private byte[]? buffer;
        static AccEncoder()
        {
            string ext;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ext = ".dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (!Environment.Is64BitProcess)
                    throw new PlatformNotSupportedException("Not support 32-bit operating systems.");
                ext = ".so";
            }
            else
                throw new PlatformNotSupportedException($"Not support [{RuntimeInformation.OSDescription}].");
            var file = $"{DllName}{ext}";
            if (!File.Exists(file))
            {
                File.WriteAllBytes(file, ext == ".so" ?
                    Properties.Resources.Linux64 :
                    Environment.Is64BitProcess ? 
                        Properties.Resources.Win64 : 
                        Properties.Resources.Win86);
            }
        }

        #region Native Initialize

        /// <summary>
        /// Create and initialize AAC Encoder.
        /// </summary>
        /// <param name="initPar"></param>
        /// <returns></returns>
        [DllImport(DllName, EntryPoint = "Easy_AACEncoder_Init", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr NativeInit(AccInitOptions initPar);

        /// <summary>
        /// Encode
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="inbuf"></param>
        /// <param name="inlen"></param>
        /// <param name="outbuf"></param>
        /// <param name="outlen"></param>
        /// <returns></returns>
        [DllImport(DllName, EntryPoint = "Easy_AACEncoder_Encode", CallingConvention = CallingConvention.StdCall)]
        private static extern int NativeEncode(IntPtr handle, byte[] inbuf, int inlen, byte[] outbuf, out int outlen);

        [DllImport(DllName, EntryPoint = "Easy_AACEncoder_Encode", CallingConvention = CallingConvention.StdCall)]
        private static extern int NativeEncode(IntPtr handle, byte* inbuf, int inlen, byte* outbuf, out int outlen);


        /// <summary>
        /// Release AAC Encoder.
        /// </summary>
        /// <param name="handle"></param>
        [DllImport(DllName, EntryPoint = "Easy_AACEncoder_Release", CallingConvention = CallingConvention.StdCall)]
        private static extern void NativeRelease(IntPtr handle);
        #endregion

        #region Constructors and Releases

        public AccEncoder(AccInitOptions options)
        {
            this.options = options;
            handle = NativeInit(options);
            if (handle == IntPtr.Zero)
                throw new ArgumentException("Encoder initialization failed.");
        }

        public AccEncoder(Rate rate, byte channel = 1, uint audioSamplerate = 8000, uint pcmBitSize = 16)
            : this(new AccInitOptions()
            {
                AudioCodec = AudioCodec.G726,
                AudioChannel = channel,
                AudioSamplerate = audioSamplerate,
                PCMBitSize = pcmBitSize,
                G726Rate = rate,
            })
        { }

        public AccEncoder(AudioCodec codec, byte channel = 1, uint audioSamplerate = 8000, uint pcmBitSize = 16)
            : this(new AccInitOptions()
            {
                AudioCodec = codec,
                AudioChannel = channel,
                AudioSamplerate = audioSamplerate,
                PCMBitSize = pcmBitSize,
            })
        {
        }

        public void Dispose()
        {
            NativeRelease(handle);
            handle = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Encode
        /// <summary>
        /// Encode input data
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns>If the size is less than 0, the encoding fails</returns>
        public (byte[] encoded, int size) Encode(byte[] input, int length)
        {
            int ll = length * 4;
            if (buffer == default || buffer.Length < ll)
            {
                buffer = ArrayPool<byte>.Shared.Rent(ll); // realloc buffer
            }
            NativeEncode(handle, input, length, buffer, out var ret);
            return (buffer, ret);
        }

        /// <summary>
        /// Encode input data
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns>If the size is less than 0, the encoding fails</returns>
        public (byte[] encoded, int size) Encode(byte[] input, int startIndex, int length)
        {
            int ll = length * 4;
            if (buffer == default || buffer.Length < ll)
            {
                buffer = ArrayPool<byte>.Shared.Rent(ll); // realloc buffer
            }

            fixed (byte* itr = &input[startIndex])
            fixed (byte* otr = &buffer[0])
            {
                int size = NativeEncode(handle, itr, length, otr, out var ret);
                return (buffer, ret);
            }
        }
        #endregion
    }

}

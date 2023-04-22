using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyAccEncoder
{

    [StructLayout(LayoutKind.Sequential)]
    public struct AccInitOptions
    {
        public AudioCodec AudioCodec;   // ULaw ALaw PCM16 G726
        public byte AudioChannel;       //1
        public uint AudioSamplerate;    //8000
        public uint PCMBitSize;			//16
        public Rate G726Rate;
    }

    /// <summary>
    /// Audio Codec
    /// </summary>
    public enum AudioCodec : byte
    {
        /// <summary>G711 U law</summary>
        G711U = 0,
        /// <summary>G711 A law</summary>
        G711A = 1,
        /// <summary>16 bit uniform PCM values. Raw pcm Data</summary>
        PCM16 = 2,
        /// <summary>G726</summary>
        G726 = 3
    };

    /// <summary>
    /// Rate Bits
    /// </summary>
    public enum Rate : byte
    {
        /// <summary>16k bits per second (2 bits per ADPCM sample)</summary>
        Rate16kBits = 2,
        /// <summary>24k bits per second (3 bits per ADPCM sample)</summary>
        Rate24kBits = 3,
        /// <summary>32k bits per second (4 bits per ADPCM sample)</summary>
        Rate32kBits = 4,
        /// <summary>40k bits per second (5 bits per ADPCM sample)</summary>
        Rate40kBits = 5,
    };
}

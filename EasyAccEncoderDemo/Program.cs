using EasyAccEncoder;
using System.Net.Sockets;

namespace EasyAccEncoderDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("EasyAccEncoder .net standard wrapper.");
            TestG711A();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            TestG726();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            TestPCM16();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void TestG711A()
        {
            DoTest("src.g711a", AudioCodec.G711A);
        }
        static void TestG726()
        {
            DoTest("encode_out_40.g726", AudioCodec.G726, Rate.Rate40kBits);
        }
        static void TestPCM16()
        {
            DoTest("playback.pcm", AudioCodec.PCM16);
        }

        static void DoTest(string fileName, AudioCodec codec, Rate rate = Rate.Rate40kBits)
        {
            Console.WriteLine($"Test {codec} to Acc format.");
            try
            {
                var fn = $"{Path.GetFileNameWithoutExtension(fileName)}.aac";
                using var fo = File.Open(fn, FileMode.Create);

                AccEncoder encoder;
                if (codec == AudioCodec.G726)
                    encoder = new AccEncoder(rate, 1, 8000, 16);
                else
                    encoder = new AccEncoder(codec, 1, 8000, 16);
                using var file = File.OpenRead($"files{Path.DirectorySeparatorChar}{fileName}");
                var buffSize = 500;
                var fileBuffer = new byte[buffSize];

                while (true)
                {
                    var ll = file.Read(fileBuffer, 0, buffSize);
                    var (encoded, size) = encoder.Encode(fileBuffer, 0, buffSize);
                    if (size < 0)
                        Console.WriteLine("Encode failed.");
                    else if (size > 0)
                    {
                        Console.WriteLine($"Input {buffSize} bytes, output {size} bytes.");
                        //Console.WriteLine($"Input {buffSize} bytes, output {size} bytes:\r\n{BitConverter.ToString(encoded, 0, size)}");
                        fo.Write(encoded, 0, size);
                    }
                    else
                        Console.WriteLine($"Input {buffSize} bytes, no output.");
                    if (ll < buffSize) break;
                }
                fo.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
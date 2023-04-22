# EasyAccEncoder.Net #
A .net standard AccEncoder, encode PCM(G711,G726,Raw) data To AAC(ADTS).

From: https://github.com/EasyDarwin/EasyAACEncoder

# Introduction #
 The C# API encapsulation of the [EasyDarwin/EasyAACEncoder](https://github.com/EasyDarwin/EasyAACEncoder).
 
 Built-in AACEncodder P/Invoke engine reference, no need to copy library files to the runtime directory.

# Supported Platforms #
	Windows x64/x86

	Linux x64
 
# Composition #
 This project contains two C# projects, EasyAACEncoder.Net and EasyAACEncoderDemo. Based on .Net Standard 2.1, it can be used with most .NET versions.
 
 ## EasyAACEncoder.Net ##
 The main class library.
 
 ## EasyAACEncoderDemo ##
 The demo project.

# Demo #

## Encode From byte[] ##

	static void BufferTest(byte[] inBuffer, AccInitOptions options)
	{
	    using AccEncoder encoder = new (options);
	    var buffSize = 500;
	    var fileBuffer = new byte[buffSize];
	    var len = inBuffer.Length;
	    var idx = 0;
	    while (idx < len)
	    {
		var (encoded, size) = encoder.Encode(fileBuffer, idx, buffSize);
		if (size < 0)
		    Console.WriteLine("Encode failed.");
		else if (size > 0)
		{
		    Console.WriteLine($"Input {buffSize} bytes, output {size} bytes:\r\n{BitConverter.ToString(encoded, 0, size)}");
		}
		else
		    Console.WriteLine($"Input {buffSize} bytes, no output.");
		idx += buffSize;
	    }
	}

## Encode form file ##

	static void FileTest(string fileName, AudioCodec codec, Rate rate = Rate.Rate40kBits)
	{
	    Console.WriteLine($"Test {codec} to Acc format.");
	    try
	    {
		var fn = $"{Path.GetFileNameWithoutExtension(fileName)}.aac";
		using var fo = File.Open(fn, FileMode.Create);

		using AccEncoder encoder =(codec == AudioCodec.G726)?
		    new (rate, 1, 8000, 16):
		    new (codec, 1, 8000, 16);

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
	
# See Also #

## EasyAACEncoder ##

**EasyAACEncoder** 是EasyDarwin开源流媒体服务团队整理、开发的一款音频转码到AAC的工具库，目前支持G711a/G711u/G726/PCM等音频格式的转码，跨平台，支持Windows（32&64）/Linux（32&64）/ARM各平台；


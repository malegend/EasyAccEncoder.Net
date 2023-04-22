# EasyAccEncoder.Net
A .net wrapper PCM(G711,G726,Raw) data To AAC(ADTS) Encoder.

From: https://github.com/EasyDarwin/EasyAACEncoder

# Project Introduction
 The C# API encapsulation of the [EasyDarwin/EasyAACEncoder](https://github.com/EasyDarwin/EasyAACEncoder).
 
 Built-in AACEncodder P/Invoke engine reference, no need to copy library files to the runtime directory.

# Supported Platforms:
Windows x64/x86

Linux x64
 
# Project composition
 This project contains two C# projects, EasyAACEncoder.Net and EasyAACEncoderDemo. Based on .Net Standard 2.1, it can be used with most .NET versions.
 
 ## EasyAACEncoder.Net
 The main class library.
 
 ## EasyAACEncoderDemo
 The demo project.

# See Also

## EasyAACEncoder ##

**EasyAACEncoder** 是EasyDarwin开源流媒体服务团队整理、开发的一款音频转码到AAC的工具库，目前支持G711a/G711u/G726/PCM等音频格式的转码，跨平台，支持Windows（32&64）/Linux（32&64）/ARM各平台；

## 调用示例 ##

- **testEasyAACEncoder**：通过EasyAACEncoderAPI对G711A/G711U/G726进行AAC转码；

	Windows编译方法，

    	Visual Studio 2010 编译：./EasyAACEncoder-master/src/EasyAACEncoder.sln

	Linux编译方法，
		
		chmod +x ./Buildit
		./Buildit


## 调用过程 ##
![](http://www.easydarwin.org/skin/easydarwin/images/easyaacencoder20180822.png)


## 特殊说明 ##
EasyAACEncoder目前支持的音视频格式：

	/* Audio Codec */
	enum Law
	{
		Law_ULaw	=	0, 		/**< U law */
		Law_ALaw	=	1, 		/**< A law */
		Law_PCM16	=	2, 		/**< 16 bit uniform PCM values. 原始 pcm 数据 */  
		Law_G726	=	3		/**< G726 */
	};
	
	/* Rate Bits */
	enum Rate
	{
		Rate16kBits=2,	/**< 16k bits per second (2 bits per ADPCM sample) */
		Rate24kBits=3,	/**< 24k bits per second (3 bits per ADPCM sample) */
		Rate32kBits=4,	/**< 32k bits per second (4 bits per ADPCM sample) */
		Rate40kBits=5	/**< 40k bits per second (5 bits per ADPCM sample) */
	};

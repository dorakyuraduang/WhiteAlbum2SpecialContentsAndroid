using Godot;
using System;
using System.Text;
using System.Collections.Generic;
public class FileEntry
{
	public uint Crypted { get; set; }
	public string FileName { get; set; }
	public uint Offset { get; set; }
	public uint Size { get; set; }
	public string PkgPath { get; set; }
}
public class Wa2Resource
{
	public static Wa2EngineMain _engine;
	public static string BgmPath = "res://assets/bgm/";
	public static string MoviePath = "res://assets/movie/";
	public static string ScriptPath = "res://assets/script/";
	public static string SePath = "res://assets/se/";
	public static string VoicePath = "res://assets/voice/";
	public static string BgPath = "res://assets/bak/";
	public static string CharPath = "res://assets/char/";
	public static string CgPath = "res://assets/grp/";
	public static Dictionary<string, FileEntry> FileDic { get; private set; } = new();
	public static void Init(Wa2EngineMain e)
	{
		_engine = e;
	}
	public class BgImage
	{
		public Texture2D texture;
		public string Effect;
		public string Mask;
	}
	public static Texture2D GetBgImage(int id, int type)
	{
		if (type == 0)
		{
			return GetTgaImage(BgPath+string.Format("b{0:D4}{1:D1}{2:D1}.png", id / 10, id % 10,_engine.TimeMode));
		}
		else if (type == 1)
		{
			return GetTgaImage(CgPath+string.Format("v{0:D6}.png", id));
		}
		return null;
	}
	public static byte[] GetBnrBuffer(string name)
	{
		return FileAccess.GetFileAsBytes(ScriptPath + name.ToLower() + ".bnr");
	}
	public static byte[] GetTextBuffer(string name)
	{
		return FileAccess.GetFileAsBytes(ScriptPath + name.ToLower() + ".txt");
	}
	public static Texture2D GetTvImage(int id)
	{
		return GetTgaImage(string.Format("tv{0:D6}.tga", id));
	}
	public static Texture2D GetMaskImage(int id)
	{
		return GetBmpImage(id);
	}
	public static AudioStream GetVoiceStream(int label, int id, int chr)
	{
		// GD.Print(string.Format("{0:D4}_{1:D4}_{2:D2}.ogg", label, id, chr));
		return GetAudioStream(VoicePath + string.Format("{0:D4}_{1:D4}_{2:D2}.ogg", label, id, chr));
	}
	public static AudioStream GetSeStream(int id)
	{
		if (ResourceLoader.Exists(string.Format(SePath + "se_{0:D4}.wav", id)))
		{
			return GetAudioStream(string.Format(SePath + "se_{0:D4}.wav", id));
		}
		else
		{
			return GetAudioStream(string.Format(SePath + "se_{0:D4}.ogg", id));
		}
	}

	public static AudioStream GetAudioStream(string path)
	{
		path = path.ToLower();
		// GD.Print(SoundDic.GetValueOrDefault(path));
		if (!ResourceLoader.Exists(path))
		{
			GD.Print("缺失音频:", path);
		}
		return ResourceLoader.Load<AudioStream>(path);
	}

	public static Texture2D GetChrImage(int id, int type)
	{
		string path = string.Format(CharPath + "{0:S}{1:D6}.png", Wa2Def.CharDict[id], type);
		// GD.Print(path);
		return GetTgaImage(path);

	}
	public static Texture2D GetBmpImage(int id)
	{;
		return GetTgaImage(CgPath+string.Format("f0{0:D3}.png",id));
	}
	public static Texture2D GetImageTexture(string path)
	{
		path = path.ToLower();
		if (!ResourceLoader.Exists(path))
		{
			GD.Print("缺失图片:", path);
		}
		return ResourceLoader.Load<Texture2D>(path);
	}
	public static Texture2D GetTgaImage(string path)
	{
		Image image = GetImageTexture(path).GetImage();
		image.Convert(Image.Format.Rgba8);
		if (Wa2EngineMain.Engine.EffectMode != "")
		{
			SetImageEffect(image);
		}
		Texture2D tgaImage = ImageTexture.CreateFromImage(image);
		return tgaImage;
	}
	public static byte[] GetAmpBuffer(string mode)
	{

		return FileAccess.GetFileAsBytes((CgPath + mode).ToLower());
	}
	public static void SetImageEffect(Image image)
	{
		byte[] data = image.GetData();
		byte[] bytes = GetAmpBuffer(Wa2EngineMain.Engine.EffectMode);
		if (bytes.Length == 1280)
		{
			if (bytes != null)
			{
				for (int i = 0; i < data.Length; i += 4)
				{
					int gray = (77 * data[i] + 151 * data[i + 1] + 28 * data[i + 2]) >> 8;
					data[i] = bytes[256 + gray];
					data[i + 1] = bytes[512 + gray];
					data[i + 2] = bytes[768 + gray];
				}
				image.SetData(image.GetWidth(), image.GetHeight(), false, image.GetFormat(), data);
			}
		}
		else if (bytes.Length == 768)
		{
			for (int i = 0; i < data.Length; i += 4)
			{
				int gray = (77 * data[i] + 151 * data[i + 1] + 28 * data[i + 2]) >> 8;
				data[i] = bytes[0 + gray];
				data[i + 1] = bytes[256 + gray];
				data[i + 2] = bytes[512 + gray];
			}
			image.SetData(image.GetWidth(), image.GetHeight(), false, image.GetFormat(), data);
		}
		else if (bytes.Length == 256)
		{
			for (int i = 0; i < data.Length; i += 4)
			{
				int gray = (77 * data[i] + 151 * data[i + 1] + 28 * data[i + 2]) >> 8;
				data[i] = bytes[gray];
				data[i + 1] = bytes[gray];
				data[i + 2] = bytes[gray];
			}
			image.SetData(image.GetWidth(), image.GetHeight(), false, image.GetFormat(), data);
		}

	}
	public static AudioStream GetBgmStream(string path)
	{
		return GetAudioStream(BgmPath+path);
	}
	public static AudioStream GetBgmStream(int id, bool loop = false)
	{
		if (ResourceLoader.Exists(string.Format(BgmPath + "BGM_{0:D3}.OGG", id)))
		{
			return GetAudioStream(string.Format(BgmPath + "BGM_{0:D3}.OGG", id));
		}
		else
		{
			if (!loop)
			{
				return GetAudioStream(string.Format(BgmPath + "BGM_{0:D3}_A.OGG", id));
			}
			else
			{
				return GetAudioStream(string.Format(BgmPath + "BGM_{0:D3}_B.OGG", id));
			}
		}
	}
}

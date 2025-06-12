using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Channels;
public struct Calender
{
	public int Year;
	public int Month;
	public int Day;
	public int DayOfWeek;
}
public struct BgmInfo
{
	public int Id;
	public int Idx;
	public bool Loop;
	public int Volume;
}
public struct SelectItem
{
	public string Text;
	public int V1;
	public int V2;
	public int V3;
}
public struct BgInfo
{
	public Vector2 Scale;
	public Vector2 Offset;
	public int Id;
	public int Type;
	public int Frame;
	public BgInfo()
	{
		Scale = Vector2.One;
		Offset = Vector2.Zero;
		Frame = 0;
	}
}
public struct CharItem
{
	public int pos;
	public int id;
	public int no;
}
public class VoiceInfo
{
	public int Chr;
	public int Id;
	public int Label;
	public int Volume;
}
public class SeInfo
{
	public int Id;
	public int Volume;
	public bool Loop;
	public int Channel;
	public float Time;
}
public class Wa2GameSav
{
	private Wa2EngineMain _engine;

	public Wa2GameSav(Wa2EngineMain e)
	{
		_engine = e;
	}

	public void SaveData(int idx)
	{
		FileAccess file = FileAccess.Open(string.Format("user://sav{0:D2}.sav", idx), FileAccess.ModeFlags.Write);

		//存档版本						
		file.Store32(4);
		//存档flag
		file.Store32((uint)(2*(5*_engine.GameFlags[1]+30)));
		//保存系统时间
		DateTime SystemTime = DateTime.Now;
		file.Store16((ushort)SystemTime.Year);
		file.Store16((ushort)SystemTime.Month);
		file.Store16((ushort)SystemTime.DayOfWeek);
		file.Store16((ushort)SystemTime.Day);
		file.Store16((ushort)SystemTime.Hour);
		file.Store16((ushort)SystemTime.Minute);
		file.Store16((ushort)SystemTime.Second);
		file.Store16((ushort)SystemTime.Millisecond);
		//第一句话
		file.StoreBuffer([.. Encoding.Unicode.GetBytes(_engine.AdvMain.TextLabel.Text.Substr(0, 16)).Concat(new byte[32]).Take(32)]);
		//保存存档缩略图
		Image image = _engine.Viewport.GetTexture().GetImage();
		image.Resize(256, 144);
		file.StoreBuffer(image.GetData());
		file.Store32((uint)_engine.BgInfo.Id);
		file.Store32((uint)_engine.BgInfo.Offset.X);
		file.Store32((uint)_engine.BgInfo.Offset.Y);
		file.StoreFloat(_engine.BgInfo.Scale.X);
		file.StoreFloat(_engine.BgInfo.Scale.Y);
		file.Store32((uint)_engine.BgInfo.Type);
		file.Store32((uint)_engine.TimeMode);
		file.Store32((uint)(_engine.BgChromaMode ? 1 : 0));
		file.Store32((uint)(_engine.CharChromaMode ? 1 : 0));
		file.StoreBuffer([.. Encoding.ASCII.GetBytes(_engine.EffectMode).Concat(new byte[16]).Take(16)]);
		file.Store32((uint)_engine.CharItems.Count);
		for (int i = 0; i < _engine.CharItems.Count; i++)
		{

			file.Store32((uint)_engine.CharItems[i].pos);
			file.Store32((uint)_engine.CharItems[i].id);
			file.Store32((uint)_engine.CharItems[i].no);
		}
		file.Store32((uint)_engine.AdvMain.TextLabel.Segment);
		file.StoreBuffer([.. Encoding.Unicode.GetBytes(_engine.AdvMain.TextLabel.Text).Concat(new byte[1024]).Take(1024)]);
		file.StoreBuffer([.. Encoding.Unicode.GetBytes(_engine.AdvMain.NameLabel.Text).Concat(new byte[16]).Take(16)]);
		file.Store32((uint)(_engine.NovelMode ? 1 : 0));
		file.Store32((uint)_engine.SelectItems.Count);
		for (int i = 0; i < _engine.SelectItems.Count; i++)
		{

			file.StoreBuffer([.. Encoding.Unicode.GetBytes(_engine.SelectItems[i].Text).Concat(new byte[256]).Take(256)]);
			file.Store32((uint)_engine.SelectItems[i].V1);
			file.Store32((uint)_engine.SelectItems[i].V2);
			file.Store32((uint)_engine.SelectItems[i].V3);
		}
		file.Store32((uint)_engine.SelectIdx);
		file.Store32((uint)_engine.ScriptIdx);
		//特效参数
		file.StoreBuffer(new byte[24]);
		file.Store32((uint)(_engine.EroMode ? 1 : 0));
		file.Store32((uint)_engine.StartTime);
		file.Store32((uint)_engine.BgmInfo.Id);
		file.Store32((uint)(_engine.BgmInfo.Loop ? 1 : 0));
		file.Store32((uint)_engine.BgmInfo.Volume);
		int seCount = _engine.SoundMgr.GetLoopSeAudioCount();
		Wa2SeAudio[] seAudios = _engine.SoundMgr.SeAudios;
		file.Store32((uint)seCount);
		for (int i = 0; i < seAudios.Length; i++)
		{
			if (seAudios[i].Loop && seAudios[i].Id >= 0)
			{
				file.Store32((uint)i);
				file.Store32((uint)seAudios[i].Id);
				file.Store32((uint)seAudios[i].Volume);
				file.Store32((byte)(seAudios[i].Loop ? 1 : 0));
			}
		}
		file.Store32((uint)_engine.Label);
		file.Store32((uint)_engine.Va);
		file.Store32((uint)_engine.Calender.Year);
		file.Store32((uint)_engine.Calender.Month);
		file.Store32((uint)_engine.Calender.DayOfWeek);
		file.Store32((uint)_engine.Calender.Day);
		// SaveScript();
		List<Wa2Script> scriptList = _engine.ScriptStack.ToList();
		file.Store32((uint)scriptList.Count);
		for (int i = 0; i < scriptList.Count; i++)
		{

			SaveScript(file, scriptList[i]);

		}
		for (int i = 0; i < _engine.GameFlags.Length; i++)
		{
			file.Store32((uint)_engine.GameFlags[i]);
		}
		file.Store32((uint)(_engine.AdvMain.WaitKey ? 1:0));
		file.Close();
	}
	public void LoadData(int idx)
	{
		GD.Print("位置", idx);
		_engine.Reset();
		FileAccess file = FileAccess.Open(string.Format("user://sav{0:D2}.sav", idx), FileAccess.ModeFlags.Read);
		file.Seek(0x1b000 + 56);
		_engine.BgInfo.Id = (int)file.Get32();
		_engine.BgInfo.Offset.X = (int)file.Get32();
		_engine.BgInfo.Offset.Y = (int)file.Get32();
		_engine.BgInfo.Scale.X = file.GetFloat();
		_engine.BgInfo.Scale.Y = file.GetFloat();
		_engine.BgInfo.Type = (int)file.Get32();
		_engine.TimeMode = (int)file.Get32();
		_engine.BgChromaMode = file.Get32() == 1;
		_engine.CharChromaMode = file.Get32() == 1;
		_engine.EffectMode = file.GetBuffer(16).GetStringFromAscii().Replace("\0", "");
		int charCount = (int)file.Get32();
		for (int i = 0; i < charCount; i++)
		{
			_engine.CharItems.Add(new CharItem()
			{
				pos = (int)file.Get32(),
				id = (int)file.Get32(),
				no = (int)file.Get32()
			});
		}
		_engine.AdvMain.TextLabel.Segment = (int)file.Get32();
		_engine.AdvMain.TextLabel.Text = Encoding.Unicode.GetString(file.GetBuffer(1024)).Replace("\0", "");
		_engine.AdvMain.NameLabel.Text = Encoding.Unicode.GetString(file.GetBuffer(16)).Replace("\0", "");
		_engine.AdvMain.SetNovelMode(file.Get32() == 1);
		int selectCount = (int)file.Get32();
		for (int i = 0; i < selectCount; i++)
		{
			_engine.SelectItems.Add(new SelectItem()
			{
				Text = Encoding.Unicode.GetString(file.GetBuffer(256)).Replace("\0", ""),
				V1 = (int)file.Get32(),
				V2 = (int)file.Get32(),
				V3 = (int)file.Get32()
			});
		}
		_engine.SelectIdx = (int)file.Get32();
		_engine.ScriptIdx = (int)file.Get32();
		//特效参数
		file.GetBuffer(24);
		_engine.EroMode = file.Get32() == 1;
		_engine.StartTime = -(int)file.Get32();
		_engine.BgmInfo.Id = (int)file.Get32();
		_engine.BgmInfo.Loop = file.Get32() == 1;
		_engine.BgmInfo.Volume = (int)file.Get32();
		int seCount = (int)file.Get32();
		for (int i = 0; i < seCount; i++)
		{
			int channel = (int)file.Get32();
			int id = (int)file.Get32();
			int volume = (int)file.Get32();
			bool loop = file.Get32() == 1;
			_engine.SoundMgr.PlaySe(channel, id, loop, 0, volume);
		}
		_engine.Label = (int)file.Get32();
		_engine.Va = (int)file.Get32();
		_engine.Calender = new Calender()
		{
			Year = (int)file.Get32(),
			Month = (int)file.Get32(),
			Day = (int)file.Get32(),
			DayOfWeek = (int)file.Get32()
		};
		List<Wa2Script> scripts = new();
		int scriptCount = (int)file.Get32();
		for (int i = 0; i < scriptCount; i++)
		{
			Wa2Script script = LoadScript(file);
			scripts.Add(script);
		}
		scripts.Reverse();
		_engine.ScriptStack = new Stack<Wa2Script>(scripts);
		_engine.Script = _engine.ScriptStack.Peek();
		_engine.SetScriptIdx(_engine.Script.ScriptName);
		_engine.GameFlags = new int[0x1d];
		for (int i = 0; i < 0x1d; i++)
		{
			_engine.GameFlags[i] = (int)file.Get32();
		}
		_engine.AdvMain.ParseMode = 2;
		if (_engine.SelectItems.Count > 0)
		{
			_engine.ShowSelectMessage();
		}
		_engine.AdvMain.WaitKey= file.Get32() == 1;
		_engine.AdvMain.ShowText(false);
		_engine.SoundMgr.PlayBgm(_engine.BgmInfo.Id, _engine.BgmInfo.Loop, _engine.BgmInfo.Volume);
		_engine.BgTexture.SetCurTexture(Wa2Resource.GetBgImage(_engine.BgInfo.Id, _engine.BgInfo.Type));
		_engine.BgTexture.SetCurScale(_engine.BgInfo.Scale);
		_engine.BgTexture.SetCurOffset(_engine.BgInfo.Offset);
		_engine.UpdateChar(0f);
		_engine.HasReadMessage = true;
		_engine.Backlogs.Clear();
		file.Close();
	}
	public Wa2Script LoadScript(FileAccess file)
	{
		Wa2Script script = new(file.GetBuffer(8).GetStringFromAscii().Replace("\0", ""));
		script.ScriptPos = file.Get32();
		for (int i = 0; i < 26; i++)
		{
			script.GloInts[i] = (int)file.Get32();
		}
		for (int i = 0; i < 26; i++)
		{
			script.GloFloats[i] = file.GetFloat();
		}

		int JumpEntryCount = (int)file.Get32();
		GD.Print("跳转数量:", JumpEntryCount);
		for (int i = 0; i < JumpEntryCount; i++)
		{

			script.JumpEntrys.Add(new());
			script.JumpEntrys[i].Type = file.Get32();
			script.JumpEntrys[i].Count = file.Get32();
			script.JumpEntrys[i].Pos = file.Get32();
			script.JumpEntrys[i].Flag = (int)file.Get32();
			for (int k = 0; k < 64; k++)
			{
				script.JumpEntrys[i].PosArr[k] = file.Get32();
			}
			for (int k = 0; k < 64; k++)
			{
				script.JumpEntrys[i].FlagArr[k] = file.Get32();
			}

		}
		int ArgsCount = (int)file.Get32();
		for (int i = 0; i < ArgsCount; i++)
		{
			script.Args.Add(new());
			script.Args[i].CmdType = (CmdType)file.Get32();
			script.Args[i].ValType = (ValueType)file.Get32();
			script.Args[i].Value0 = (int)file.Get32();
			script.Args[i].IntValue = (int)file.Get32();
			script.Args[i].FloatValue = file.GetFloat();
		}
		return script;
	}
	public void SaveScript(FileAccess file, Wa2Script script)
	{
		file.StoreBuffer([.. Encoding.ASCII.GetBytes(script.ScriptName).Concat(new byte[8]).Take(8)]);
		file.Store32(script.ScriptPos);
		for (int i = 0; i < script.GloInts.Length; i++)
		{
			file.Store32((uint)script.GloInts[i]);
		}
		for (int i = 0; i < script.GloFloats.Length; i++)
		{
			file.StoreFloat(script.GloFloats[i]);
		}
		file.Store32((uint)script.JumpEntrys.Count);
		for (int i = 0; i < script.JumpEntrys.Count; i++)
		{

			file.Store32(script.JumpEntrys[i].Type);
			file.Store32(script.JumpEntrys[i].Count);
			file.Store32(script.JumpEntrys[i].Pos);
			file.Store32((uint)script.JumpEntrys[i].Flag);
			for (int k = 0; k < 64; k++)
			{
				file.Store32(script.JumpEntrys[i].PosArr[k]);
			}
			for (int k = 0; k < 64; k++)
			{
				file.Store32(script.JumpEntrys[i].FlagArr[k]);
			}
		}
		file.Store32((uint)script.Args.Count);
		for (int i = 0; i < script.Args.Count; i++)
		{

			file.Store32((uint)script.Args[i].CmdType);
			file.Store32((uint)script.Args[i].ValType);
			file.Store32((uint)script.Args[i].Value0);
			file.Store32((uint)script.Args[i].IntValue);
			file.Store32((uint)script.Args[i].FloatValue);
		}
	}
}
using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
[GlobalClass]
public partial class Wa2SoundMgr : Node
{
	const int MAX_SE_CHANNELS = 10;
	const int MAX_VOICE_CHANNELS = 10;
	public Wa2BgmAudio BgmAudio { private set; get; } = new();

	private AudioStreamPlayer _sysSeAudio = new();
	private Wa2VoiceAudio[] _voiceAudios;
	public Wa2SeAudio[] SeAudios { private set; get; }
	private Wa2EngineMain _engine;
	public int BgmId { private set; get; }
	public Wa2Audio GetVoicePlayer(int idx)
	{
		return _voiceAudios[idx];
	}
	public void Init(Wa2EngineMain e)
	{
		_engine = e;
	}
	public void SetVoiceVolume(int idx, int volume, int frame)
	{
		_voiceAudios[idx].SetVolume(volume, frame * _engine.FrameTime);

	}
	public void StopVoice(int idx, float time = 0.0f)
	{
		_voiceAudios[idx].StopStream(time);
	}
	public void StopAll()
	{
		for (int i = 0; i < MAX_VOICE_CHANNELS; i++)
		{
			_voiceAudios[i].Stream = null;
		}
		StopBgm();
		for (int i = 0; i < MAX_SE_CHANNELS; i++)
		{
			SeAudios[i].StopSound(0);
		}
	}
	public float GetVoiceRemainingTime(int idx)
	{
		Wa2Audio audio = _voiceAudios[idx];
		if (audio.Stream == null)
		{
			return 0;
		}
		return (float)audio.Stream.GetLength() - audio.GetPlaybackPosition();
	}
	public void SetSeVolume(int channel, int volume, float time)
	{
		if (channel >= MAX_SE_CHANNELS)
		{
			return;
		}
		SeAudios[channel].SetVolume(volume, time);
	}
	public void PlayVoiceDrama(int id, int chapter)
	{
		_engine.SoundMgr.BgmId = -1;
		_engine.SubtitleMgr.ListenVoice(80+id, chapter,_engine.SoundMgr.BgmAudio);
		if (Wa2Def.VoiceDramaData[id].Length ==1)
		{
			_engine.SoundMgr.BgmAudio.PlaySound(Wa2Resource.GetBgmStream(string.Format("bgm_{0:D3}.ogg", 80 + id)), false,0, 255);
		}
		else
		{
		_engine.SoundMgr.BgmAudio.PlaySound(Wa2Resource.GetBgmStream(string.Format("bgm_{0:D3}_{1:S}.ogg", 80 + id,(char)('a'+chapter))), false,0, 255);
		}


	}
	public void PlayVoice(int label, int id, int chr, int volume = 256, bool loop = false, int channel = 0)
	{

		Wa2VoiceAudio audio = _voiceAudios[channel];
		if (label == -1)
		{
			label = _engine.Label;
		}
		if (channel == 0)
		{
			_engine.VoiceInfos.Add(new()
			{
				Id = id,
				Chr = chr,
				Label = label,
				Volume = volume
			});
		}
		if (_engine.Prefs.CanPlayCharVoice(chr))
		{
			if (!_engine.CanSkip() || _engine.DemoMode || channel != 0)
			{
				if (_engine.EroMode && Array.IndexOf(Wa2Def.EroChar, chr) < 0 && _engine.Prefs.GetConfig("ero_voice") == 1)
				{
					return;
				}
				audio.PlaySound(Wa2Resource.GetVoiceStream(label, id, chr), false, volume);
				(audio.Stream as AudioStreamOggVorbis).Loop = loop;
				if (channel != 0)
				{
					_engine.SubtitleMgr.ListenVoice(label, id, audio);
				}
			}
		}
	}
	public void PlayBgm(int id, bool loopFlag = true, int volume = 255)
	{
		if (id < 0)
		{
			return;
		}
		BgmId = id;
		Wa2EngineMain.Engine.WirtSysFlag(100 + id, 1);
		BgmAudio.PlaySound(Wa2Resource.GetBgmStream(id, false), loopFlag, 0, volume);
		BgmAudio.SetLoopStream(Wa2Resource.GetBgmStream(id, true));
	}
	public void StopBgm(float time = 0.0f)
	{
		BgmAudio.StopStream(time);
		Wa2EngineMain.Engine.BgmInfo.Id = -1;
		BgmId = -1;
	}
	public float GetVoiceTime()
	{
		Wa2Audio audio = _voiceAudios[0];
		if (audio.Stream == null)
		{
			return 0;
		}
		return (float)audio.Stream.GetLength() - audio.GetPlaybackPosition();
	}
	public float GetSeTime(int channel)
	{
		if (SeAudios[channel].Stream == null)
		{
			return 0;
		}
		return (float)SeAudios[channel].Stream.GetLength() - SeAudios[channel].GetPlaybackPosition();
	}
	public static Wa2SoundMgr Instance
	{
		private set; get;

	}
	public void OnVoiceFinished(int idx)
	{
		_voiceAudios[idx].Stream = null;
	}
	public override void _Ready()
	{
		Instance = this;
		BgmAudio.Bus = "BGM";
		_sysSeAudio.Bus = "SE";
		_sysSeAudio.Stream = new AudioStreamPolyphonic();
		BgmAudio.Name = "BgmAudio";
		_sysSeAudio.Name = "SysSeAudio";
		AddChild(BgmAudio);
		AddChild(_sysSeAudio);
		SeAudios = new Wa2SeAudio[MAX_SE_CHANNELS];
		_voiceAudios = new Wa2VoiceAudio[MAX_VOICE_CHANNELS];
		for (int i = 0; i < MAX_SE_CHANNELS; i++)
		{
			Wa2SeAudio audio = new();
			audio.Bus = "SE";
			audio.Name = "SeAudio" + i;
			SeAudios[i] = audio;
			AddChild(audio);
		}
		for (int i = 0; i < MAX_VOICE_CHANNELS; i++)
		{
			Wa2VoiceAudio audio = new();
			audio.Bus = "VOICE";
			audio.Name = "VoiceAudio" + i;
			_voiceAudios[i] = audio;
			int idx = i;
			_voiceAudios[i].Finished += () => OnVoiceFinished(idx);
			AddChild(audio);
		}
		_sysSeAudio.Play();
	}
	public void PlaySysSe(AudioStream stream)
	{
		AudioStreamPlaybackPolyphonic playBack = (AudioStreamPlaybackPolyphonic)_sysSeAudio.GetStreamPlayback();
		if (stream != null)
		{
			playBack.PlayStream(stream);
		}
	}
	public void PlaySe(int channel, int id, bool loopFlag = false, float time = 0.0f, int volume = 255)
	{
		// GD.Print("播放音效2");
		SeAudios[channel].PlaySound(id, loopFlag, time, volume);
	}
	// public void PlaySe(SeInfo seInfo)
	// {
	// 	PlaySe(seInfo.Channel, seInfo.Id, seInfo.Loop, seInfo.Time, seInfo.Volume);
	// }
	public void StopSe(int channel, float time = 0.0f)
	{
		if (channel >= MAX_SE_CHANNELS)
		{
			return;
		}
		SeAudios[channel].StopSound(time);
	}
	public int GetLoopSeAudioCount()
	{
		int r = 0;
		foreach (Wa2SeAudio audio in SeAudios)
		{
			if (audio.Loop && audio.Id >= 0)
			{
				r++;
			}
		}
		return r;
	}

}

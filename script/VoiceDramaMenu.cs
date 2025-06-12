using Godot;
using System;

public partial class VoiceDramaMenu : BasePage
{
	[Export]
	public TextureRect Cover;
	[Export]
	public VBoxContainer VoiceDramaButtons;
	[Export]
	public TextureRect VdTitle;
	[Export]
	public TextureRect VdInfo;
	[Export]
	public GpuParticles2D Particles;
	public int Idx;
	public override void _Ready()
	{
		base._Ready();
		for (int i = 0; i < 7; i++)
		{
			Wa2Button btn = VoiceDramaButtons.GetChild<Wa2Button>(i);
			int idx = i;
			btn.ButtonDown += () => OnVoiceDramaButtonDown(idx);
		}
	}
	public void OnVoiceDramaButtonDown(int chapter)
	{
		AtlasTexture texture = (AtlasTexture)VdInfo.Texture;
		VdInfo.Show();
		texture.Region = new Rect2(0, 24 * Wa2Def.VoiceDramaData[Idx][chapter], 344, 24);
		_engine.SoundMgr.PlayVoiceDrama(Idx, chapter);
	}
	public void Open(int idx)
	{
		base.Open();
		Particles.Restart();
		_engine.SoundMgr.StopBgm();
		VdInfo.Hide();
		AtlasTexture vdTexture = (AtlasTexture)VdTitle.Texture;
		vdTexture.Region = new Rect2(0, 32 * idx, 384, 32);

		Idx = idx;
		Cover.Texture = ResourceLoader.Load<Texture2D>(Wa2Resource.CgPath + string.Format("vd{0:D2}.png", idx + 1));
		for (int i = 0; i < 7; i++)
		{
			Wa2Button btn = VoiceDramaButtons.GetChild<Wa2Button>(i);
			btn.ButtonPressed = false;
			if (i < Wa2Def.VoiceDramaData[idx].Length)
			{
				btn.Show();

				AtlasTexture texture = (AtlasTexture)btn.GetChild<TextureRect>(0).Texture;
				texture.Region = new Rect2(0, 24 * Wa2Def.VoiceDramaData[idx][i], 344, 24);
			}
			else
			{
				btn.Hide();
			}
		}
	}
	public override void Close()
	{
		base.Close();
		_engine.SubtitleMgr.StopListen();
  }
	public override void OnCloseAnimationFinished()
	{
		base.OnCloseAnimationFinished();

		_engine.SoundMgr.PlayBgm(31);
	}
	//   public void OnBackButtonDown()
	// {
	// 	_engine.SoundMgr.StopBgm();
	// }
}

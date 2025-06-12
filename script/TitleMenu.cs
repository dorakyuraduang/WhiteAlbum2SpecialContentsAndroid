using Godot;



public partial class TitleMenu : Control
{
	// [Export]
	// public Wa2Button CgModeButton;
	// [Export]
	// public Wa2Button BgmModeButton;
	[Export]
	public Control DigitalNovel;
	[Export]
	public Control MenuButtons;
	[Export]
	public Control As;
	[Export]
	public Control ExtraEpisode;
	[Export]
	public Control VoiceDrama;
	[Export]
	public AnimationPlayer AnimationPlayer;
		[Export]
	public Wa2Button OptionsButton;
	[ExportGroup("MenuButtons")]
	[Export]
	public Wa2Button AsButton;
	[Export]
	public Wa2Button ExtraEpisodeButton;
	[Export]
	public Wa2Button DigitalNovelButton;
	[Export]
	public Wa2Button VoiceDramaButton;
	[ExportGroup("AsButtons")]
	[Export]
	public Wa2Button AsKazButton;
	[Export]
	public Wa2Button AsSetButton;
	[Export]
	public Wa2Button AsLoadButton;
	[Export]
	public Wa2Button AsBackButton;
	[Export]
	public Wa2Button AsOptionsButton;
	[ExportGroup("ExtraEpisodeButtons")]
	[Export]
	public Wa2Button FuDaButton;
	[Export]
	public Wa2Button ExtraEpisodeLoadButton;
	[Export]
	public Wa2Button ExtraEpisodeOptionsButton;
	[Export]
	public Wa2Button ExtraEpisodeBackButton;
	[ExportGroup("DigitalNovel")]
	[Export]
	public Wa2Button NovelButton1;
	[Export]
	public Wa2Button NovelButton2;
	[Export]
	public Wa2Button NovelButton3;
	[Export]
	public Wa2Button NovelButton4;
	[Export]
	public Wa2Button NovelButton5;
	[Export]
	public Wa2Button NovelButton6;
	[Export]
	public Wa2Button NovelButton7;
	[Export]
	public Wa2Button NovelButton8;
	[Export]
	public Wa2Button NovelButton9;
	[Export]
	public Wa2Button DigitalNovelLoadButton;
	[Export]
	public Wa2Button DigitalNovelBackButton;
	[Export]
	public Wa2Button DigitalNovelOptionsButton;
	[ExportGroup("DigitalNovel")]
	[Export]
	public Wa2Button QuitButton;
	// [Export]
	// public Wa2Button LoadtButton;
	// [Export]
	// public Wa2Button OptionsButton;
	[ExportGroup("VoiceDrama")]
	[Export]
	public Wa2Button VoiceDramaBackButton;
		[Export]
	public Wa2Button VoiceDramaOptionsButton;

	// [Export]
	// public Wa2Button ICButton;
	// [Export]
	// public Wa2Button CcButton;
	// [Export]
	// public Wa2Button CodeaButton;

	// [Export]
	// public Wa2Button SpecialBackButton;
	// [Export]
	// public Wa2Button SpecialButton;
	// [Export]
	// public Control Special;

	// [Export]
	// public Wa2Button DigitalNovelButton;
	// [Export]
	// public Wa2Button DigitalNovelBackButton;
	// [Export]
	// public Wa2Button DigitalNovel1Button;
	// 	[Export]
	// public Wa2Button DigitalNovel2Button;
	private Wa2EngineMain _engine;


	public override void _Ready()
	{
		_engine = Wa2EngineMain.Engine;
		AsButton.ButtonDown += OnAsButtonDown;
		ExtraEpisodeButton.ButtonDown += OnExtraEpisodeButtonDown;
		AsBackButton.ButtonDown += OnAsBackButtonDown;
		VoiceDramaBackButton.ButtonDown += OnVoiceDramaBackButtonDown;
		AsKazButton.ButtonDown += () => OnScriptButtonDown("6001");
		AsSetButton.ButtonDown += () => OnScriptButtonDown("6101");
		AsLoadButton.ButtonDown += OnLoadButtonDown;
		ExtraEpisodeLoadButton.ButtonDown += OnLoadButtonDown;
		OptionsButton.ButtonDown += OnOptionsButtonDown;
		AsOptionsButton.ButtonDown += OnOptionsButtonDown;
		VoiceDramaOptionsButton.ButtonDown += OnOptionsButtonDown;
		ExtraEpisodeOptionsButton.ButtonDown += OnOptionsButtonDown;
		ExtraEpisodeBackButton.ButtonDown += OnExtraEpisodeBackButtonDown;
		DigitalNovelButton.ButtonDown += OnDigitalNovelButtonDown;
		VoiceDramaButton.ButtonDown += OnVoiceDramaButtonDown;
		FuDaButton.ButtonDown += () => OnScriptButtonDown("4000");
		NovelButton1.ButtonDown += () => OnScriptButtonDown("5400");
		NovelButton2.ButtonDown += () => OnScriptButtonDown("5000");
		NovelButton3.ButtonDown += () => OnScriptButtonDown("5300");
		NovelButton4.ButtonDown += () => OnScriptButtonDown("5200");
		NovelButton5.ButtonDown += () => OnScriptButtonDown("5100");
		NovelButton6.ButtonDown += () => OnScriptButtonDown("7300");
		NovelButton7.ButtonDown += () => OnScriptButtonDown("7000");
		NovelButton8.ButtonDown += () => OnScriptButtonDown("7100");
		NovelButton9.ButtonDown += () => OnScriptButtonDown("7200");
		DigitalNovelLoadButton.ButtonDown += OnLoadButtonDown;
		DigitalNovelOptionsButton.ButtonDown += OnOptionsButtonDown;
		DigitalNovelBackButton.ButtonDown += OnDigitalNovelBackButtonDown;
		QuitButton.ButtonDown += OnQuitButtonDown;
		for (int i = 0; i < 5; i++)
		{
			int idx = i;
			VoiceDrama.GetChild<Wa2Button>(i).ButtonDown += () => _engine.UiMgr.OpenVoiceDramaMenu(idx);
		}

	}


	// 	QuitButton.ButtonDown += OnQuitButtonDown;
	// 	ICButton.ButtonDown += OnIcButtonDown;
	// 	CcButton.ButtonDown += OnCCButtonDown;
	// 	SpecialButton.ButtonDown += OnSpecialButtonDown;
	// 	SpecialBackButton.ButtonDown += OnSpecialBackButtonDown;
	// 	// As1Button.ButtonDown += OnAs1ButtonDown;
	// 	// As2Button.ButtonDown += OnAs2ButtonDown;
	// 	DigitalNovel1Button.ButtonDown +=OnDigitalNovel1ButtonDown;
	// 		DigitalNovel2Button.ButtonDown +=OnDigitalNovel2ButtonDown;
	// 	DigitalNovelButton.ButtonDown += OnDigitalNovelButtonDown;
	// 	DigitalNovelBackButton.ButtonDown += OnDigitalNovelBackButtonDown;
	// 	OptionsButton.ButtonDown += OnOptionsButtonDown;
	// 	BgmModeButton.ButtonDown += OnBgmModeButtonDown;
	// 	CodeaButton.ButtonDown += OnCodeaButtonDown;
	// 	LoadtButton.ButtonDown += OnLoadButtonDown;
	// 	CgModeButton.ButtonDown += OnCgModeButtonDown;
	// }
	// public void OnDigitalNovelButtonDown()
	// {
	// 	Special.Hide();
	// 	DigitalNovel.Show();
	// }
	// public void OnDigitalNovelBackButtonDown()
	// {
	// 	DigitalNovel.Hide();
	// 	Special.Show();

	// }
	public void OnDigitalNovelButtonDown()
	{
		MenuButtons.Hide();
		DigitalNovel.Show();
	}
	public void OnVoiceDramaButtonDown()
	{
		MenuButtons.Hide();
		VoiceDrama.Show();
	}
	public async void OnScriptButtonDown(string s)
	{
		_engine.SoundMgr.StopBgm();
		AnimationPlayer.Play("close");
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		_engine.StartScript(s);
		_engine.UiMgr.OpenGame();
	}
	public void OnOptionsButtonDown()
	{
		_engine.UiMgr.OpenOptionsMenu();
	}
	// public void OnCgModeButtonDown()
	// {
	// 	_engine.UiMgr.OpenCGModeMenu();
	// }
	// public void OnBgmModeButtonDown()
	// {
	// 	_engine.UiMgr.OpenBgmModeMenu();
	// }
	public void OnLoadButtonDown()
	{
		_engine.UiMgr.OpenLoadMenu();
	}
	// public void OnSpecialButtonDown()
	// {
	// 	MenuBttons.Hide();
	// 	Special.Show();
	// }
	// public async void OnCodeaButtonDown()
	// {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("3001");
	// 	_engine.UiMgr.OpenGame();
	// }
	// public async void OnCCButtonDown()
	// {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("2001");
	// 	_engine.UiMgr.OpenGame();
	// }
	// public async void OnDigitalNovel1ButtonDown() {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("5000");
	// 	_engine.UiMgr.OpenGame();

	// }
	// 	public async void OnDigitalNovel2ButtonDown() {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("5100");
	// 	_engine.UiMgr.OpenGame();

	// }
	// public async void OnIcButtonDown()
	// {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("1001");
	// 	_engine.UiMgr.OpenGame();
	// }
	// public async void OnAsKazButtonDown()
	// {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("6001");
	// 	_engine.UiMgr.OpenGame();

	// }
	// public async void OnAsSetButtonDown()
	// {
	// 	_engine.SoundMgr.StopBgm();
	// 	AnimationPlayer.Play("close");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.StartScript("6101");
	// 	_engine.UiMgr.OpenGame();

	// }
	public void OnQuitButtonDown()
	{
		GetTree().Quit();
	}
	public void OnAsBackButtonDown()
	{
		MenuButtons.Show();
		As.Hide();
	}
	public void OnVoiceDramaBackButtonDown()
	{
		VoiceDrama.Hide();
		MenuButtons.Show();
	}
	public void OnExtraEpisodeBackButtonDown()
	{
		MenuButtons.Show();
		ExtraEpisode.Hide();
	}
	public void OnDigitalNovelBackButtonDown()
	{
		MenuButtons.Show();
		DigitalNovel.Hide();
	}
	// public void OnSpecialBackButtonDown()
	// {
	// 	MenuBttons.Show();
	// 	Special.Hide();
	// }
	public void OnExtraEpisodeButtonDown()
	{
		MenuButtons.Hide();
		ExtraEpisode.Show();
	}
	public void OnAsButtonDown()
	{
		MenuButtons.Hide();
		As.Show();
	}
	public async void Open()
	{
		_engine.SoundMgr.StopBgm();
		Show();
		AnimationPlayer.Play("RESET");
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		AnimationPlayer.Play("open");
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		_engine.SoundMgr.PlayBgm(31);
	}
	// 	_engine.SoundMgr.StopBgm();
	// 	Show();
	// 	AnimationPlayer.Play("RESET");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	// AnimationPlayer.Play("logo");
	// 	// await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	AnimationPlayer.Play("open");
	// 	await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	// 	_engine.SoundMgr.PlayBgm(31);
	// }

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// // public override void _Process(double delta)
	// // {
	// // }
	public override void _GuiInput(InputEvent @event)
	{

		if (@event is InputEventMouseButton && (@event as InputEventMouseButton).ButtonIndex == MouseButton.Left && @event.IsPressed())
		{
			if (AnimationPlayer.CurrentAnimation != "close")
			{
				AnimationPlayer.Advance(AnimationPlayer.CurrentAnimation.Length);
			}
		}

	}
}

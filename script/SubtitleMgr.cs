using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using Godot;
using System;
public class ContentSegment
{
  public int Begin { get; set; }
  public int End { get; set; }
  public string Text { get; set; }
  public string Text2 { get; set; } = "";
}
public class ListenContent
{
  public ContentSegment Content;
  public int Type = 0;
}
public class VoiceDramaSubtitle
{
  public int Id { get; set; }
  public List<ContentSegment> Content { get; set; }
  public int Chapter { get; set; }
  public int Type { get; set; } = 0;
}
public class SubtitleRoot
{
  public List<VoiceDramaSubtitle> VoiceDramaSubtitle { get; set; }
  public List<VoiceDramaSubtitle> SongDramaSubtitle { get; set; }

  // public List<VoiceDramaSubtitle> Song { get; set; }
}

public partial class SubtitleMgr : Node
{
  public List<VoiceDramaSubtitle> VoiceDramaSubtitleList;
  public List<VoiceDramaSubtitle> SongDramaSubtitleList;
  public Wa2Audio ListenAudio;
  public List<ListenContent> ListenContent=new();
  [Export]
  public Label TextLabel;
  [Export]
  public Label SongLabel;

  public override void _Ready()
  {
    var file = FileAccess.Open("res://assets/sub.yaml", FileAccess.ModeFlags.Read);
    string yamlText = file.GetAsText();
    file.Close();

    var deserializer = new DeserializerBuilder()
     .WithNamingConvention(CamelCaseNamingConvention.Instance)
     .IgnoreUnmatchedProperties()
    .Build();

    var result = deserializer.Deserialize<SubtitleRoot>(yamlText);
    VoiceDramaSubtitleList = result.VoiceDramaSubtitle;
    SongDramaSubtitleList = result.SongDramaSubtitle;
    GD.Print(SongDramaSubtitleList.Count);
    // foreach (var se in VoiceDramaSubtitleList)
    // {
    //   GD.Print($"SoundEffect ID: {se.Id}");
    //   foreach (var segment in se.Content)
    //   {
    //     GD.Print($"  {segment.Begin}-{segment.End}: {segment.Text}");
    //   }
    // }
  }
  public void ListenVoice(int id, int chapter, Wa2Audio audio)
  {
    TextLabel.Text = "";
    SongLabel.Text = "";
    ListenAudio = null;
    ListenContent.Clear();
    for (int i = 0; i < VoiceDramaSubtitleList.Count; i++)
    {
      if (VoiceDramaSubtitleList[i].Id == id && VoiceDramaSubtitleList[i].Chapter == chapter)
      {
        for (int k = 0; k < VoiceDramaSubtitleList[i].Content.Count; k++)
        {
          ListenContent content = new();
          content.Type = 0;
          content.Content = VoiceDramaSubtitleList[i].Content[k];
          ListenContent.Add(content);
        }
        ListenAudio = audio;
        break;
      }
    }
    for (int i = 0; i <SongDramaSubtitleList.Count; i++)
    {
      if (SongDramaSubtitleList[i].Id == id && SongDramaSubtitleList[i].Chapter == chapter)
      {
        for (int k = 0; k < SongDramaSubtitleList[i].Content.Count; k++)
        {
          ListenContent content = new();
          content.Type = 1;
          content.Content = SongDramaSubtitleList[i].Content[k];
          ListenContent.Add(content);
        }
        ListenAudio = audio;
        break;
      }
    }
  }
  public void StopListen()
  {
      TextLabel.Text = "";
      SongLabel.Text = "";
      ListenAudio = null;
      ListenContent.Clear();

  }
  public override void _Process(double delta)
  {
    if (ListenAudio != null && ListenAudio.Stream != null && ListenAudio.Playing && ListenContent != null && ListenContent.Count > 0)
    {
      foreach (ListenContent content in ListenContent)
      {
        ContentSegment segment = content.Content;
        if (ListenAudio.GetPlaybackPosition() * 1000 >= segment.Begin && ListenAudio.GetPlaybackPosition() * 1000 <= segment.End)
        {
          if (content.Type == 0)
          {
            TextLabel.Text = segment.Text;
            if (segment.Text2 != "")
            {
              TextLabel.Text += "\n" + segment.Text2;
            }
          }
          else
          {
            SongLabel.Text = segment.Text;
          }

        }
      }
    }
    else
    {
      TextLabel.Text = "";
      SongLabel.Text = "";
      ListenAudio = null;
      ListenContent.Clear();
    }
  }
}

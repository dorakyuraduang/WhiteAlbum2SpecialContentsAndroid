using Godot;
using System;
using System.Text;
public partial class DataSlot : Wa2Button
{
  [Export]
  public TextureRect NoData;
  [Export]
  public TextureRect ExistData;
  [Export]
  public TextureRect SaveTexture;
  [Export]
  public Label IdxLabel;
  [Export]
  public Label DateLabel;
  [Export]
  public TextureRect Category;
  [Export]
  public Label DayLabel;
  [Export]
  public TextureRect Month;
  [Export]
  public Wa2Label FirstSentenceLabel;
  public void Update(int idx)
  {
    IdxLabel.Text = string.Format("{0:D2}", idx + 1);
    if (FileAccess.FileExists(string.Format("user://sav{0:D2}.sav", idx)))
    {
      FileAccess file = FileAccess.Open(string.Format("user://sav{0:D2}.sav", idx), FileAccess.ModeFlags.Read);
      int type = (int)file.Get32();
      int flag = (int)file.Get32();
      int year = (int)file.Get16();
      int month = (int)file.Get16();
      int dayOfWeek = (int)file.Get16();
      int day = (int)file.Get16();
      int hour = (int)file.Get16();
      int minute = (int)file.Get16();
      int second = (int)file.Get16();
      int millisecond = (int)file.Get16();
      DateLabel.Text = string.Format("{0:D4} {1:D2}/{2:D2} {3:D2}:{4:D2}", year, month, day, hour, minute);
      string text = Encoding.Unicode.GetString(file.GetBuffer(32)).Replace("\n", "").Replace("\\n", "").Replace("\0", "");
      if (text.Length >= 14)
      {
        text = text.Substring(0, 13) + "…";
      }
      FirstSentenceLabel.SetText(text);
      SaveTexture.Texture = ImageTexture.CreateFromImage(Image.CreateFromData(256, 144, false, Image.Format.Rgb8, file.GetBuffer(0x1b000)));
      AtlasTexture texture = (AtlasTexture)Category.Texture;
      texture.Region = new Rect2(0, 24*flag/10, 248, 24);
      NoData.Hide();
      DateLabel.Show();
      ExistData.Show();
      Category.Show();
      file.Close();
    }
    else
    {
      FirstSentenceLabel.SetText("");
      FirstSentenceLabel.Clear();
      Category.Hide();
      SaveTexture.Texture = null;
      NoData.Show();
      ExistData.Hide();
      DateLabel.Hide();
      Month.Hide();
      DayLabel.Hide();
    }
  }
}
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Wa2Def
{
	public static List<Rect2> FontSliceData = new();
	public static List<Rect2> FontShadowSliceData = new();
	public static int[][] VoiceDramaData = [
		[0,1,2,3,4,5,6],
		[7,8,9,0xa,0xb],
		[0xc,0xd,0xe,0xf],
		[0x10],
		[0x11,0x12],
		[0x13,0x14,0x15,0x16,0x17]
		];
	public static int[] SelectScript = [
		2003,
		2004,
		2005,
		2007,
		2009,
		2011,
		2013,
		2014,
		2015,
		2016,
		2017,
		2018,
		2019,
		2024,
		2503,
		3003,
		3005,
		3009,
		3012,
		3013,
		3014,
		3016,
		3904
];
	public static int[] EroChar = [1, 2, 3, 4, 5];
	public static string[] ScriptList = [
		"1001",
		"1002",
		"1003",
		"1004",
		"1005",
		"1006",
		"1007",
		"1008",
		"1008_020",
		"1008_030",
		"1008_040",
		"1008_050",
		"1009",
		"1009_020",
		"1009_030",
		"1010",
		"1010_020",
		"1010_030",
		"1010_040",
		"1010_050",
		"1010_060",
		"1010_070",
		"1011",
		"1011_020",
		"1011_030",
		"1012",
		"1012_020",
		"1012_030",
		"1013",
		"catch",
		"catch2",
		"catch3",
		"1012_030_2",
		"2001",
		"2002",
		"2003",
		"2004",
		"2005",
		"2006",
		"2007",
		"2008",
		"2009",
		"2010",
		"2011",
		"2012",
		"2013",
		"2014",
		"2015",
		"2016",
		"2017",
		"2018",
		"2019",
		"2020",
		"2021",
		"2022",
		"2023",
		"2024",
		"2025",
		"2026",
		"2027",
		"2028",
		"2029",
		"2030",
		"2031",
		"2032",
		"2033",
		"2301",
		"2302",
		"2303",
		"2304",
		"2305",
		"2306",
		"2307",
		"2308",
		"2309",
		"2310",
		"2311",
		"2312",
		"2313",
		"2314",
		"2315",
		"2316",
		"2317",
		"2318",
		"2319",
		"2320",
		"2321",
		"2322",
		"2401",
		"2402",
		"2403",
		"2404",
		"2405",
		"2406",
		"2407",
		"2408",
		"2409",
		"2410",
		"2411",
		"2412",
		"2413",
		"2501",
		"2502",
		"2503",
		"2504",
		"2505",
		"2506",
		"2507",
		"2508",
		"2509",
		"2510",
		"2511",
		"2512",
		"2513",
		"2514",
		"2515",
		"2516",
		"2517",
		"3001",
		"3002",
		"3003",
		"3004",
		"3005",
		"3006",
		"3007",
		"3008",
		"3009",
		"3010",
		"3011",
		"3012",
		"3013",
		"3014",
		"3014_2",
		"3014_3",
		"3015",
		"3016",
		"3017",
		"3018",
		"3019",
		"3020",
		"3021",
		"3022",
		"3023",
		"3024",
		"3101",
		"3102",
		"3103",
		"3104",
		"3105",
		"3106",
		"3107",
		"3108",
		"3109",
		"3110",
		"3111",
		"3201",
		"3202",
		"3203",
		"3204",
		"3205",
		"3206",
		"3207",
		"3208",
		"3209",
		"3210",
		"3211",
		"3901",
		"3902",
		"3903",
		"3904",
		"3905",
		"3906",
		"3907",
		"3908",
		"3909",
		"5000",
		"5001",
		"5002",
		"5003",
		"5004",
		"5100",
		"5101",
		"5102",
		"5103",
		"5104",
		"5200",
		"5201",
		"5202",
		"5203",
		"5204",
		"5205",
		"5206",
		"5207",
		"5208",
		"5400",
		"5401",
		"6001",
		"6002",
		"6003",
		"6004",
		"6005",
		"6101",
		"6102",
		"6103",
		"6104",
		"7000",
		"7100",
		"7200",
		"7300"
	];
	public static Dictionary<char, int> FontMap = new();
	public static readonly int[] CharPos = new int[]{
		-288,
		0,
		288,
		-384,
		384,
		-480,
		480,
		-480,
		-160,
		160,
		480
	};


	public static Dictionary<int, string> CharDict = new(){
		{0, "har"},
		{1, "kaz"},
		{2, "set"},
		{3, "koh"},
		{4, "izu"},
		{5, "mar"},
		{10, "tak"},
		{11, "ioo"},
		{12, "chi"},
		{13, "pap"},
		{14, "mam"},
		{15, "oto"},
		{16, "you"},
		{17, "tan"},
		{18, "shi"},
		{19, "tom"},
		{20,"sat"},
		{21,"hon"},
		{22,"nak"},
		{23,"say"},
		{24,"aco"},
		{25,"mih"},
		{26,"mhh"},
		{27,"ueh"},
		{28,"yos"},
		{29,"tan"},
		{30,"ham"},
		{31,"mat"},
		{32,"kiz"},
		{33,"suz"},
		{34,"saw"},
		{35,"miy"},
		{36,"yan"},
		{37,"mas"},
	};
	public static int[] BgmSlot = [
		4,
		1,
		2,
		3,
		5,
		6,
		12,
		7,
		8,
		9,
		0xa,
		0xb,
		0xc,
		0xd,
		0xe,
		0xf,
		0x10,
		0x11,
		0x1b,
		0x21,
		0x20,
		0x1c,
		0x13,
		0x14,
		0x15,
		0x1a,
		0x1d,
		0x17,
		0x18,
		0x16,
		0x22,
		0x3c,
		0x1f,
		0x28,
		0x29,
		0x2a,
		0x2b,
		0x2c,
		0x2d,
		0x2e,
		0x2f,
		0x45,
		0x4a,
		0x34,
		0x35,
		0x3d,
		0x32,
		0x33,
		0x3e,
		0x3f,
		0x43,
		0x44,
		0x46,
		0x47,
		0x36,
		0x37,
		0x40,
		0x3a,
		0x3b,
		0x42,
		0x38,
		0x39,
		0x41,
	];
	// public static void LoadSliceData(string path, List<Rect2> rects)
	// {
	// 	GD.Print(Time.GetTicksMsec());
	// 	var tex = GD.Load<Texture2D>(path);
	// 	var img = tex.GetImage();
	// 	var bitmap = new Bitmap();
	// 	bitmap.CreateFromImageAlpha(img);
	// 	// const int SIZE = 40;
	// 	int width = img.GetWidth();
	// 	int height = img.GetHeight();

	// 	const int SIZE = 32;


	// 	for (int i = 0; i < width / SIZE; i++)
	// 	{
	// 		for (int j = 0; j < height / SIZE; j++)
	// 		{
	// 			var rect = new Rect2I(i * SIZE, j * SIZE, SIZE, SIZE);
	// 			var polygons = bitmap.OpaqueToPolygons(rect);

	// 			if (polygons.Count == 0)
	// 				continue;

	// 			var extractedRect = new Rect2(polygons[0][0], Vector2.Zero);

	// 			foreach (var polygon in polygons)
	// 			{
	// 				for (int k = 1; k < polygon.Count(); k++)
	// 				{
	// 					extractedRect = extractedRect.Expand(polygon[k]);
	// 				}
	// 			}

	// 			// 加回偏移（因为局部 rect 是从 0,0 开始）
	// 			extractedRect.Position += new Vector2(i * SIZE, j * SIZE);

	// 			rects.Add(extractedRect);
	// 		}
	// 	}

	// 	GD.Print(rects);
	// }

	public static void LoadFontMap()
	{
		byte[] buffer = FileAccess.GetFileAsBytes("res://assets/font.map");
		//new EncoderReplacementFallback(strUniRepChr), new DecoderReplacementFallback(strUniRepChr)
		// GD.Print("长度",Encoding.GetEncoding("shift_jis").GetString(buffer).Length);
		string str = Wa2EngineMain.Engine.Wa2Encoding.GetString(buffer).Replace("\n", "").Replace("\r", "");
		// GD.Print(str);
		for (int i = 0; i < str.Length; i++)
		{
			FontMap.TryAdd(str[i], i);
		}
	}
}

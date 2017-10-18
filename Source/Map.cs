using System;
using System.IO;

namespace KF2MapListTool
{
	public class Map
	{
		public string Name { get; private set; }
		public string WorkshopID { get; private set; }
		public string WorkshopLink
		{
			get
			{
				return $"https://steamcommunity.com/sharedfiles/filedetails/?id={WorkshopID}";
			}
		}
		public bool IsChecked { get; set; } = true;

		public Map(string path)
		{
			Name = Path.GetFileNameWithoutExtension(path);
			WorkshopID = path.Split('\\')[0];
		}
	}
}

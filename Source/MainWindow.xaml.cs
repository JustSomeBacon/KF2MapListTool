using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace KF2MapListTool
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string cacheLocation;
		private ObservableCollection<Map> mapList = new ObservableCollection<Map>();

		public MainWindow()
		{
			InitializeComponent();
			MapsListBox.ItemsSource = mapList;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cacheLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
				"\\My Games\\KillingFloor2\\KFGame\\Cache\\";

			if (Directory.Exists(cacheLocation))
				FindMaps();
			else
				SelectDirectory();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			SaveServerEngineConfig();
			SaveServerGameConfig();
			WinForms.MessageBox.Show("Done!");
		}
		
		private void SelectDirectory()
		{
			WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog()
			{
				Description = "Select the location of the KF2 cache.",
				ShowNewFolderButton = false
			};

			if (dialog.ShowDialog() != WinForms.DialogResult.OK)
				Close();
			else
			{
				cacheLocation = dialog.SelectedPath;
				FindMaps();
			}
		}

		/// <summary>
		/// Finds all the maps in the cache and adds them to a list.
		/// </summary>
		private void FindMaps()
		{
			// Search the cache.
			try
			{
				foreach (string file in Directory.GetFiles(cacheLocation, "*.kfm", SearchOption.AllDirectories))
				{
                    string MapPath = file.Replace(cacheLocation, "");

                    if (!IsDuplicate(MapPath)){
                        Map map = new Map(MapPath);
                        mapList.Add(map);
                    }
				}
			}
			catch(Exception e)
			{
				WinForms.MessageBox.Show($"We encountered a problem while scanning the directory!\n\n{e.Message}",
									"Whoops...", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
				Close();
				return;
			}
			
			// No maps found, might as well just close.
			if (mapList.Count == 0)
			{
				WinForms.MessageBox.Show("No maps found!\n\nAssuming the directory is correct, you may need to launch the game for the cache to fill up.", 
					"Oopsy!", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
				Close();
			}
		}

		/// <summary>
		/// Write the config needed in the KFEngine file.
		/// </summary>
		private void SaveServerEngineConfig()
		{
			if (File.Exists(".\\KFEngine.txt"))
			{
				WinForms.DialogResult result = WinForms.MessageBox.Show("The file 'KFEngine.txt' already exists. Overwrite?",
					"Decisions, decisions!", WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Asterisk);

				if (result == WinForms.DialogResult.No)
					return;
			}

			try
			{
				using (StreamWriter config = new StreamWriter(".\\KFEngine.txt"))
				{
					config.WriteLine("[OnlineSubsystemSteamworks.KFWorkshopSteamworks]");

					foreach(Map map in mapList)
					{
						if (!map.IsChecked)
							continue;

						config.WriteLine($"ServerSubscribedWorkshopItems={map.WorkshopID}");
					}
				}
			}
			catch (Exception e)
			{
				WinForms.MessageBox.Show($"There was a problem writing the 'KFEngine.txt' file!\n\n{e.Message}",
									"This can't be good...", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
				Close();
			}
		}

		/// <summary>
		/// Write the config needed in the KFGame file.
		/// </summary>
		private void SaveServerGameConfig()
		{
			if (File.Exists(".\\KFGame.txt"))
			{
				WinForms.DialogResult result = WinForms.MessageBox.Show("The file 'KFGame.txt' already exists. Overwrite?",
					"Decisions, decisions!", WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Asterisk);

				if (result == WinForms.DialogResult.No)
					return;
			}

			try
			{
				using (StreamWriter config = new StreamWriter(".\\KFGame.txt"))
				{
					foreach (Map map in mapList)
					{
						if (!map.IsChecked)
							continue;

						config.WriteLine($"[{map.Name} KFMapSummary]");
						config.WriteLine($"MapName={map.Name}");
						config.WriteLine($"ScreenshotPathName=UI_MapPreview_TEX.UI_MapPreview_Nuked");
						config.WriteLine();
					}
				}
			}
			catch (Exception e)
			{
				WinForms.MessageBox.Show($"There was a problem writing the 'KFGame.txt' file!\n\n{e.Message}",
									"This can't be good...", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
				Close();
			}
		}

        private bool IsDuplicate(string path)
        {
            string WorkshopID = path.Split('\\')[0];

            for(int i = 0; i<mapList.Count; i++)
            {
                if (mapList[i].WorkshopID == WorkshopID)
                {
                    return true;
                }
            }

            return false;
        }
	}
}

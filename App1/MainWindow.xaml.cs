using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppWindow.Resize(new Windows.Graphics.SizeInt32(400, 210));
            PositionWindow();
            LoadStartMenuApps();

            if (AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
            }

        }

        String selectedCommand = "";

        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(selectedCommand);
            ExecuteCommand();

        }

        // List of commands
        private List<string> Commands = new List<string>()
        {
            "notepad",
            "calc",
            "cmd",
            "powershell",
            "wt",
            "explorer",
            "regedit",
            "taskmgr",
            "control",
            "msconfig",

            "services.msc",
            "devmgmt.msc",
            "diskmgmt.msc",
            "compmgmt.msc",
            "eventvwr.msc",
            "gpedit.msc",

            "shell:startup",
            "shell:downloads",
            "shell:documents",
            "shell:desktop",

            "ms-settings:",
            "appwiz.cpl",
            "ncpa.cpl",
            "sysdm.cpl"
        };

        private List<QuickRun.StartMenuApp> InstalledApps = new List<QuickRun.StartMenuApp>();

        private void LoadStartMenuApps()
        {
            string[] startMenuPaths =
            {
                Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)
            };

            foreach (string folder in startMenuPaths.Distinct())
            {
                Debug.WriteLine($"Scansione: {folder}");

                foreach (string shortcut in EnumerateShortcutsSafely(folder))
                {
                    InstalledApps.Add(new QuickRun.StartMenuApp
                    {
                        Name = Path.GetFileNameWithoutExtension(shortcut),
                        Path = shortcut
                    });
                }
            }
        }

        private static IEnumerable<string> EnumerateShortcutsSafely(string folder)
        {
            IEnumerable<string> files;

            try
            {
                files = Directory.EnumerateFiles(folder, "*.lnk", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine($"Accesso negato: {folder} - {ex.Message}");
                yield break;
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Errore I/O: {folder} - {ex.Message}");
                yield break;
            }

            foreach (string file in files)
            {
                yield return file;
            }

            IEnumerable<string> directories;

            try
            {
                directories = Directory.EnumerateDirectories(folder);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine($"Accesso negato alle sottocartelle: {folder} - {ex.Message}");
                yield break;
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Errore I/O nelle sottocartelle: {folder} - {ex.Message}");
                yield break;
            }

            foreach (string directory in directories)
            {
                foreach (string shortcut in EnumerateShortcutsSafely(directory))
                {
                    yield return shortcut;
                }
            }
        }

        // Handle text change and present suitable items
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                var allCommands = Commands
                    .Concat(InstalledApps.Select(a => a.Name))
                    .Distinct()
                    .ToList();

                foreach (var cmd in allCommands)
                {
                    var found = splitText.All((key) =>
                    {
                        return cmd.ToLower().Contains(key);
                    });

                    if (found)
                    {
                        suitableItems.Add(cmd);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }

            selectedCommand = AutoSuggestBox1.Text.ToString();
            Debug.WriteLine(AutoSuggestBox1.Text.ToString());
        }

        // Handle user selecting an item, in our case just output the selected item.
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            selectedCommand = args.SelectedItem.ToString();
            Debug.WriteLine(AutoSuggestBox1.Text.ToString());
            
        }

        // Handles the Click event on the Button on the page and opens the Popup.
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }

        // Handles the Click event on the Button inside the Popup control and closes the Popup.
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            // Close the application
            Close();
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            var app = InstalledApps.FirstOrDefault(x => x.Name.Equals(selectedCommand, StringComparison.OrdinalIgnoreCase));

            if (app != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = app.Path,
                    UseShellExecute = true
                });

                Close();
                return;
            }

            if (IsMathExpression(selectedCommand))
            {
                try
                {
                    var result = new DataTable().Compute(selectedCommand, null);
                    AutoSuggestBox1.Text = result.ToString();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    //ShowPopupOffsetClicked(null, null);
                }
            }
            else
            {
                try
                {
                    string input = Environment.ExpandEnvironmentVariables(AutoSuggestBox1.Text.Trim());

                    int firstSpace = input.IndexOf(' ');

                    string fileName;
                    string arguments;

                    if (firstSpace == -1)
                    {
                        fileName = input;
                        arguments = "";
                    }
                    else
                    {
                        fileName = input.Substring(0, firstSpace);
                        arguments = input.Substring(firstSpace + 1);
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        UseShellExecute = true
                    });

                    Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ShowPopupOffsetClicked(null, null);
                }
            }
        }
        

        private void AutoSuggestBox1_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ExecuteCommand();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            AutoSuggestBox1.Focus(FocusState.Programmatic);
            //code to set the window icon
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            AppWindow appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));
            appWindow.SetIcon("Assets\\QuickRun.ico");
        }

        private bool IsMathExpression(string input)
        {
            return input.Any(char.IsDigit) &&
                   input.Any(c => "+-*/()".Contains(c));
        }

        private void PositionWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);

            AppWindow appWindow =
                AppWindow.GetFromWindowId(
                    Win32Interop.GetWindowIdFromWindow(hWnd));

            var displayArea = DisplayArea.GetFromWindowId(
                appWindow.Id,
                DisplayAreaFallback.Primary);

            int x = 10;

            int y = displayArea.WorkArea.Height -
                    appWindow.Size.Height - 10;

            appWindow.Move(new Windows.Graphics.PointInt32(x, y));
        }
    }
}

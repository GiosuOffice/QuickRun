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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;

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
            AppWindow.Resize(new Windows.Graphics.SizeInt32(400, 200));
            
            if (AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
            }
            
            //TestCommands();
        }

        String selectedCommand = "";

        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(selectedCommand);
            if (string.IsNullOrWhiteSpace(selectedCommand))
            {
                ShowPopupOffsetClicked(sender, e);
                try
                {
                    string command = Environment.ExpandEnvironmentVariables(AutoSuggestBox1.Text.Trim());

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = command,
                        UseShellExecute = true
                    });

                    Close();
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex.Message);
                    ShowPopupOffsetClicked(sender, e);
                }
            }

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

        // Handle text change and present suitable items
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var cmd in Commands)
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
            selectedCommand = AutoSuggestBox1.Text.ToString();
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
            try
            {
                string command = Environment.ExpandEnvironmentVariables(AutoSuggestBox1.Text.Trim());

                Process.Start(new ProcessStartInfo
                {
                    FileName = command,
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
        

        private void AutoSuggestBox1_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ExecuteCommand();
        }

        private void TestCommands()
        {
            string[] commands =
            {
                "notepad",
                "calc",
                "cmd",
                "regedit",
                "taskmgr",
                "control",
                "services.msc",
                "devmgmt.msc",
                "diskmgmt.msc",
                "shell:startup",
                "shell:downloads",
                "ms-settings:",
                "%temp%",
                "C:\\Windows"
            };

            foreach (string command in commands)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Environment
                            .ExpandEnvironmentVariables(command),
                        UseShellExecute = true
                    });

                    Debug.WriteLine($"OK: {command}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR: {command}");
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            AutoSuggestBox1.Focus(FocusState.Programmatic);
        }
    }
}

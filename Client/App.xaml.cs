using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;

        public App()
        {
            this.navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            this.navigationStore.CurrentViewModel = new HomeViewModel();

            MainViewModel mainViewModel = new MainViewModel(this.navigationStore);
            this.MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };
            this.MainWindow.Show();

            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(mainViewModel.OnPreviewMouseClick));
            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewMouseMoveEvent, new MouseEventHandler(mainViewModel.OnPreviewMouseMove));
            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyDownEvent, new KeyEventHandler(mainViewModel.OnPreviewKeyDown));

            base.OnStartup(e);
        }
    }
}

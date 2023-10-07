using System.Windows;

namespace ProyectoBodega
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ChangeMainWindow(Window newMainWindow)
        {
            MainWindow.Close();

            MainWindow = newMainWindow;

            MainWindow.Show();
        }
    }
}

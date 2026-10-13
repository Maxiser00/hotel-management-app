using HegreHotel.Views;

namespace HegreHotel;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Task.Run(async () => await Database.CreateDatabaseAsync()).Wait();

        MainPage = new NavigationPage(new MainPage());
    }
}

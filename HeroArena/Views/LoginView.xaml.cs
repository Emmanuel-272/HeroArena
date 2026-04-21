using HeroArena.ViewModels;
using System.Windows;

namespace HeroArena.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginVMX();
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        var vm = (LoginVMX)DataContext;
        vm.ExecuteLoginPublic(PwdPassword.Password);
    }
}
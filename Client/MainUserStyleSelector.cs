using System.Windows;
using System.Windows.Controls;

namespace TCPClientWPF
{
    public class MainUserStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            Message message = item as Message;

            return message.Author == ViewModel.MainClientUsername
                ? elemnt.FindResource("MainUserMessageStyle") as Style
                : elemnt.FindResource("UserMessageStyle") as Style;
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace TCPClientWPF
{
    public class MainUserDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            Message message = item as Message;

            return message.Author == ViewModel.MainClientUsername 
                ? elemnt.FindResource("MainUserMessageTemplate") as DataTemplate
                : elemnt.FindResource("UserMessageTemplate") as DataTemplate;
        }
    }
}

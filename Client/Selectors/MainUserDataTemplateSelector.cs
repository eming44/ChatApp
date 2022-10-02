using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public class MainUserDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            Message message = item as Message;

            return message.Author == MainViewModel.MainClientUsername 
                ? element.FindResource("MainUserMessageTemplate") as DataTemplate
                : element.FindResource("UserMessageTemplate") as DataTemplate;
        }
    }
}

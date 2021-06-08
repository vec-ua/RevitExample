using CommonLibrary.Models;
using System.Windows;
using System.Windows.Controls;

namespace RevitApp.Views.TemplateSelectors
{
    /// <summary>
    /// Класс для выбора шаблона плагина
    /// </summary>
    public sealed class PluginTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement felement = container as FrameworkElement;
            AbstractPluginViewModel plugin = null;

            if (felement is ContentPresenter)
                plugin = (felement as ContentPresenter).Content as AbstractPluginViewModel;

            if (plugin == null)
                return null;

            try
            {
                ResourceDictionary dictionary = new ResourceDictionary() { Source = plugin.ResourceDictionary };
                return dictionary["pluginTemplate"] as DataTemplate;
            }
            catch
            { return null; }
        }
    }
}

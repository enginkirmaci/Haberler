using Haberler.Services;
using System;
using System.Windows;

namespace Haberler.SourceTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static private NewsService _service = null;
        static public NewsService Service
        {
            get
            {
                if (_service == null)
                    _service = new NewsService();

                return _service;
            }
        }
    }
}
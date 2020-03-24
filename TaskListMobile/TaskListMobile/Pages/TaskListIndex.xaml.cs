using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskListMobile.Sevices;
using TaskListMobile.ViewModels;
using TaskListMobileData.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskListMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskListIndex : ContentPage
    {
        public TaskListIndexViewModel ViewModel { get; set; }
        public TaskListIndex()
        {
            InitializeComponent();
            ViewModel = new TaskListIndexViewModel(new TaskListRepository(FileService.GetDbFilePath()),
                DateTime.Now.Date);
            BindingContext = ViewModel;
        }
    }
}
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
    public partial class TaskListDetail : ContentPage
    {
        public TaskListDetailViewModel ViewModel { get; set; }
        public TaskListDetail()
        {
            ViewModel = new TaskListDetailViewModel(
                new TaskListRepository(FileService.GetDbFilePath()),
                DateTime.Now.Date);
            InitializeComponent();
            BindingContext = ViewModel;
            //TaskItemsListView.ItemsSource = ViewModel.Model.TaskItems;
        }
    }
}
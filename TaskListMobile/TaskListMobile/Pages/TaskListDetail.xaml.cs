using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskListMobile.Configuration;
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
        public TaskListDetail(TaskListDetailViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            BindingContext = ViewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ViewModel.DeleteTaskItemCommand.Execute( ((Xamarin.Forms.Button)sender).CommandParameter.ToString());
        }
    }
}
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

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            ViewModel.DisplayEditDialogCommand.Execute(((Button)sender).Text);

        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            ViewModel.GoListForAnotherDayCommand.Execute(e.NewDate);
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            ViewModel.RescheduleDialogCommand.Execute(((Xamarin.Forms.Button)sender).CommandParameter.ToString());

        }
    }
}
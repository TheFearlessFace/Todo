using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reset the 'resume' id, since we just want to re-start here
            ((App)App.Current).ResumeAtTodoListId = -1;
            TodoListsView.ItemsSource = await App.Database.GetTodoListAsync();
        }

        async void OnListAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoListPage
            {
                BindingContext = new TodoList()
            });
        }

        async void OnListSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //((App)App.Current).ResumeAtTodoListId = (e.SelectedItem as TodoList).ID;
            Debug.WriteLine("setting ResumeAtTodoListId = " + (e.SelectedItem as TodoList).ID);
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new TodoListPage
                {
                    BindingContext = e.SelectedItem as TodoList
                });
            }
        }
    }
}
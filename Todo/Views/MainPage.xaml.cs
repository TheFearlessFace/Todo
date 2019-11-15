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
        TodoList todoList;
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
            if (todoList == null)
                todoList = new TodoList();
            //Change ID, because SQLite does not  autoincrement, even though the [autoincrement...] is set.
            int count =  App.Database.GetTodoListAsync().Result.Count();
            if (todoList.ID < count + 1)
            {
                todoList.ID = count + 1;
            }
            todoList.Name = String.Empty;
            //insert new todoList
            await App.Database.InsertItemAsync(todoList);

            await Navigation.PushAsync(new TodoListPage
            {
                BindingContext = todoList
            });
        }
        async void AboutSelected(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage
            {
                //BindingContext = todoList
            });
        }

            async void OnListSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //((App)App.Current).ResumeAtTodoListId = (e.SelectedItem as TodoList).ID;
            Debug.WriteLine("setting ResumeAtTodoListId = " + (e.SelectedItem as TodoList).ID);
            //todoList = e.SelectedItem as TodoList;
            if (todoList == null)
            {
                todoList = new TodoList();
            }
            if (todoList != null && e.SelectedItem != null)
            {
                todoList.ID = (e.SelectedItem as TodoList).ID;
                todoList.Name = (e.SelectedItem as TodoList).Name;
                todoList.Date = (e.SelectedItem as TodoList).Date;
                todoList.Done = (e.SelectedItem as TodoList).Done;

                await Navigation.PushAsync(new TodoListPage(todoList)
                {
                    BindingContext = e.SelectedItem as TodoList
                });
            }
        }
    }
}
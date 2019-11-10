using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Todo
{
	public partial class TodoListPage : ContentPage
	{
        public TodoList todoList;
		public TodoListPage()
		{
			InitializeComponent();
		}

        public TodoListPage(TodoList todoList)
        {
            this.todoList = todoList;

            InitializeComponent();
        }
        protected override async void OnAppearing()
		{
			base.OnAppearing();

            if (todoList != null)
            {
                this.BindingContext = todoList;
                ((App)App.Current).ResumeAtTodoListId = todoList.ID;
                listView.ItemsSource = await App.Database.GetItemsByTodoListIdAsync(todoList.ID);
            }
		}
        //todo: add button for new TodoList
		async void OnItemAdded(object sender, EventArgs e)
		{
            if (todoList == null)
            {
                //todoList = new TodoList();
                
                //int count = App.Database.GetTodoListAsync().Result.Count();

                todoList = (TodoList)BindingContext;
                //if (todoList.ID < count)
                //{
                //    todoList.ID = count;
                //}
            }
            
            
            await Navigation.PushAsync(new TodoItemPage(todoList)
            {
                BindingContext = new TodoItem()
            });
        }

		async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
            //((App)App.Current).ResumeAtTodoId = (e.SelectedItem as TodoItem).ID;
            Debug.WriteLine("setting ResumeAtTodoId = " + (e.SelectedItem as TodoItem).ID);
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new TodoItemPage(todoList)
                {
                    BindingContext = e.SelectedItem as TodoItem
                });
            }
		}
        async void OnSaveClicked(object sender, EventArgs e)
        {
            var todoList = (TodoList)BindingContext;
            todoList.Date = DateTime.Now;
            await App.Database.SaveItemAsync(todoList);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var todoItem = (TodoList)BindingContext;
            await App.Database.DeleteItemAsync(todoItem);
            await Navigation.PopAsync();
        }
    }
}

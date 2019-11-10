using System;
using Xamarin.Forms;

namespace Todo
{
	public partial class TodoItemPage : ContentPage
	{
        public TodoList todoList;
		public TodoItemPage(TodoList todoList)
		{
            this.todoList = todoList;
			InitializeComponent();
		}

		async void OnSaveClicked(object sender, EventArgs e)
		{
			var todoItem = (TodoItem)BindingContext;
            if (todoList != null)
            {
                todoItem.TodoListId = todoList.ID;
               
                await App.Database.SaveItemAsync(todoItem);
                //Check if all items are done, if they are then set todoList.Done = true else false
                todoList.Done = App.Database.IsItemsDoneByTodoListIdAsync(todoList.ID);
                await App.Database.SaveItemAsync(todoList);
            }
			await Navigation.PopAsync();
		}

		async void OnDeleteClicked(object sender, EventArgs e)
		{
			var todoItem = (TodoItem)BindingContext;
			await App.Database.DeleteItemAsync(todoItem);
			await Navigation.PopAsync();
		}

		async void OnCancelClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		void OnSpeakClicked(object sender, EventArgs e)
		{
			var todoItem = (TodoItem)BindingContext;
			DependencyService.Get<ITextToSpeech>().Speak(todoItem.Name + " " + todoItem.Notes);
		}
	}
}

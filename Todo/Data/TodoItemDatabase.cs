using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Todo
{
	public class TodoItemDatabase
	{
		readonly SQLiteAsyncConnection database;

		public TodoItemDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TodoList>().Wait();
            database.CreateTableAsync<TodoItem>().Wait();
		}

		public Task<List<TodoItem>> GetItemsAsync()
		{
			return database.Table<TodoItem>().ToListAsync();
		}
        public Task<List<TodoItem>> GetItemsByTodoListIdAsync(int id)
        {
            return database.Table<TodoItem>().Where(item => item.TodoListId == id).ToListAsync();
        }
        public Task<List<TodoList>> GetTodoListAsync()
        {
            return database.Table<TodoList>().ToListAsync();
        }

        public Task<List<TodoItem>> GetItemsNotDoneAsync()
		{
			return database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}
        public bool IsItemsDoneByTodoListIdAsync(int todoListId)
        {
            bool returner = false;
            Task<List<TodoItem>> items = database.Table<TodoItem>().Where(item => (item.TodoListId == todoListId) && (item.Done == false)).ToListAsync();//SElect * from todoItem where done = false and TodoListId = id
            if (items.Result.Count > 0)
                returner = false;
            else returner = true;
            return returner;
        }
        public Task<TodoItem> GetItemAsync(int id)
		{
			return database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(TodoItem item)
		{
			if (item.ID != 0)
			{
				return database.UpdateAsync(item);
			}
			else {
				return database.InsertAsync(item);
			}
		}

        public Task<int> SaveItemAsync(TodoList item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        public Task<int> InsertItemAsync(TodoList item)
        {
                return database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(TodoItem item)
		{
			return database.DeleteAsync(item);
		}
        public Task<int> DeleteItemAsync(TodoList item)
        {
            return database.DeleteAsync(item);
        }
    }
}


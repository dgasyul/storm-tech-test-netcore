using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool ShouldShowDoneItems { get; }
        public TodoListSortOrder SortOrder { get; }

        public TodoListDetailViewmodel(
            int todoListId,
            string title,
            ICollection<TodoItemSummaryViewmodel> items,
            bool shouldShowDoneItems,
            TodoListSortOrder sortOrder)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            ShouldShowDoneItems = shouldShowDoneItems;
            SortOrder = sortOrder;
        }
    }
}
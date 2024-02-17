using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ToDoListClassLibrary;

namespace ToDoListUnitTests
{
    [TestClass]
    public class ToDoItemTests
    {
       private static ToDoContext _ToDoContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseSqlite("Data Source=/workspaces/API/TodoList.API/todolist.db;")
                .Options;

            _ToDoContext = new ToDoContext(options);
            _ToDoContext.Database.EnsureCreated();
        }


   [TestMethod]
public void Test_SaveNewToDoItemWithNonAsciiCharactersInDescription_Variant1()
{
    string nonAsciiDescription = "Description with non-ASCII characters: àèìòù";
    SaveToDoItemAndAssertDescription(nonAsciiDescription, nonAsciiDescription);
}


[TestMethod]
public void Test_SaveNewToDoItemWithEmojisInDescription_Variant1()
{
    string emojiDescription = "Description with emojis: 😀🚀🎉";
    SaveToDoItemAndAssertDescription(emojiDescription, emojiDescription);
}

[TestMethod]
public void Test_SaveNewToDoItemWithMixedCharactersInDescription_Variant1()
{
    string mixedDescription = "Description with mixed characters: ABC123!@#";
    SaveToDoItemAndAssertDescription(mixedDescription, mixedDescription);
}

[TestMethod]
public void Test_SaveNewToDoItemWithNonAsciiAndNumbersInDescription_Variant2()
{
    string nonAsciiNumberDescription = "Description with non-ASCII characters and numbers: àèì123";
    SaveToDoItemAndAssertDescription(nonAsciiNumberDescription, nonAsciiNumberDescription);
}



private void SaveToDoItemAndAssertDescription(string description, string expectedDescription)
{
    // Create a new ToDoItem with the provided description
    var newItem = new ToDoItem { Description = description };

    // Add the new item to the database context
    _ToDoContext.Add(newItem);

    // Save changes to the database
    _ToDoContext.SaveChanges();

    // Retrieve the item from the database using LINQ
    var itemFromDb = _ToDoContext.ToDoItems.FirstOrDefault(item => item.Description == expectedDescription);

    // Assert that the item retrieved from the database is not null
    Assert.IsNotNull(itemFromDb);

    // Assert that the description of the item from the database matches the expected description
    Assert.AreEqual(expectedDescription, itemFromDb.Description);
}
    }

    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }
    }
}
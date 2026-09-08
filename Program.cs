using TaskManager;
using var db = new database_context();

Multi_menu main = new(db);
main.load_interactive_menu();

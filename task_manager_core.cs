namespace TaskManager;

public class Multi_menu
{
    private database_context _db;

    public Multi_menu(database_context db)
    {
        _db = db;
    }
    private string[] main_menu = ["1.Add Tasks", "2.View Tasks", "3.Task Log", "4.Edit Task", "5.Exit"];


    private string option = "";
    bool valid_input = false;
    bool exit = false;
    public void load_interactive_menu()
    {
        do
        {
            foreach (string option in main_menu)
                Console.WriteLine(option);
            do
            {
                Console.WriteLine("Choose an option from the above or enter '5' to exit the program");
                string? user_input = Console.ReadLine();
                if (user_input != null && (user_input.Length == 1))
                {
                    if (user_input[0] >= '1' && user_input[0] <= '5')
                    {
                        option = user_input;
                        valid_input = true;
                    }
                }
            }
            while (!valid_input);

            Options display_info = new(_db);
            
            switch (option)
            {
                case "1":
                    display_info.add_task();
                    break;
                case "2":
                    display_info.view_tasks();
                    break;
                case "3":
                    display_info.task_log();
                    break;
                case "4":
                    display_info.edit_task();
                    break;
                case "5":
                    exit = true;
                    break;
            }
        }
        while (!exit);
    }

}
public class Options
{
    private database_context _db;
    private Random randi = new();

    public Options(database_context db)
    {
        _db = db;
    }
    public void add_task()
    {
        Console.WriteLine("Please type in your task title: ");
        string? user_title = Console.ReadLine();
        Console.WriteLine("Kindly enter the contents, or description of your task");
        string? user_content = Console.ReadLine();
        if (user_title != null && user_content != null)
        {
            int i = randi.Next(10000);
            var task_var = new taskItem { title = user_title, content = user_content, ID = i };
            Console.WriteLine("Press Enter or Return to continue");
            Console.ReadLine();
            _db.tasks.Add(task_var);
            _db.SaveChanges();
        }

    }

    public void view_tasks()
    {
        List<taskItem> all_tasks = _db.tasks.ToList();

        bool valid_id = false;
        bool valid_finish = false;
        bool checker = false;
        int identity = 0;

        foreach (taskItem task_title in all_tasks)
        {
            if (task_title.is_done == false)
            {
                Console.WriteLine(task_title.ID + "." + task_title.title);
            }
        }
        do
        {
            Console.WriteLine("Please enter the ID preceding the task you wish to view or 'q' to quit");
            string? id_input = Console.ReadLine();
            if (id_input != null)
            {
                if (int.TryParse(id_input, out identity) && identity < 10000)
                {
                    foreach (taskItem task_view in all_tasks)
                    {
                    	if (task_view.ID == identity)
                        {
                            Console.WriteLine("\n" + task_view.content + "\n");
                    		valid_id = true;
                        }
                    }
                }
                else if (id_input.ToLower() == "q")
                {
                	checker = true;
                    valid_id = true;
                }
            }
        }
        while (!valid_id);
        if (checker == false)
        {
        	
			do
			{
				Console.WriteLine("Type 'Finish' or 'Done' to set the task as done or 'No' to leave it as pending");
				string? status_input = Console.ReadLine();
				if (status_input != null)
				{
					if (status_input.ToLower() == "finish" || status_input.ToLower() == "done")
					{
						(_db.tasks.Find(identity)).is_done = true;
						_db.SaveChanges();
						valid_finish = true;
					}
					else if (status_input.ToLower() == "no")
						valid_finish = true;
				}
			}
			while (!valid_finish);
        }

        Console.WriteLine("Please press Enter or Return to continue");
        Console.ReadLine();
    }

    public void task_log()
    {
        List<taskItem> logs = _db.tasks.ToList();
        foreach (taskItem log in logs)
		{
            if (log.is_done == true)
            {
            	Console.WriteLine($"{log.ID}.{log.title} [FINISHED]");
            }
            else
            {
            	Console.WriteLine($"{log.ID}.{log.title} [PENDING]");
            }
        }
        Console.WriteLine("Press 'Enter' or 'Return' to continue");
        Console.ReadLine();
    }

    public void edit_task()
    {
        List<taskItem> edits = _db.tasks.ToList();
        bool valid_edit = false;
        bool sub_valid = false;
        int edit_id = 0;

        foreach (taskItem edit in edits)
        {
            if (edit.is_done == false)
            {
				Console.WriteLine ($"{edit.ID}.{edit.title}");
            }
        }
        Console.WriteLine("\n--------------------------------");
        do
        {
            Console.WriteLine("\nType the ID of the task you wish to edit");
            string? user_choice = Console.ReadLine();
            if (user_choice != null)
            {
                if (int.TryParse(user_choice, out edit_id) && edit_id < 10000)
                {
                	foreach (taskItem edit in edits)
					{
                        if (edit.ID == edit_id)
                        {
                        	valid_edit = true;
                        }
                    }
                }
                Console.WriteLine("\t\t\t" + edit_id + "." + (_db.tasks.Find(edit_id)).title + "\n");
                Console.WriteLine((_db.tasks.Find(edit_id)).content + "\n");
                Console.WriteLine("1.Change Title\n2.Change Content");

                do
                {
					Console.WriteLine("Choose one of the options above or enter '0' to cancel");
					user_choice = Console.ReadLine();
                    if (user_choice != null)
                    {
                        if (int.TryParse(user_choice, out int c))
                        {
                            if (c == 1)
                            {
                                Console.WriteLine("Enter your new title");
                                string? change = Console.ReadLine();
                                if (change != null)
                                {
                                    _db.tasks.Find(edit_id).title = change;
                                    _db.SaveChanges();
                                    sub_valid = true;
                                }
                            }
                            else if (c == 2)
                            {
                                Console.WriteLine("Enter the new content of this task");
                                string? change = Console.ReadLine();
                                if (change != null)
                                {
                                    _db.tasks.Find(edit_id).content = change;
                                    _db.SaveChanges();
                                    sub_valid = true;
                                }
                            }
                            else if (c == 0)
								sub_valid = true;
                        }
                    }
                }
                while (!sub_valid);
            }
        }
		while (!valid_edit);
    }
}
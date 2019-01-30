Users
    .Where(u => u.Name == "NameA")
    .Select(u => u.SecondName);
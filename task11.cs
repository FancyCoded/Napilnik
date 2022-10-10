public static void SpawnNewObject()
{
    //Создание объекта на карте
}

public static void GenerateChance()
{
    _chance = Random.Range(0, 100);
}

public static int SetSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}
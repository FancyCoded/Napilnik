public static void SpawnNewObject()
{
    //�������� ������� �� �����
}

public static void GenerateChance()
{
    _chance = Random.Range(0, 100);
}

public static int GetSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}
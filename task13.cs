private void OnCheckButtonClicked(object sender, EventArgs e)
{
    if (this.passportTextbox.Text.Trim() == string.Empty)
    {
        int massageBox = (int)MessageBox.Show("Введите серию и номер паспорта");
        return;
    }

    string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
    int correctDataLength = 10;

    if (rawData.Length < correctDataLength)
    {
        this.textResult.Text = "Неверный формат серии или номера паспорта";
        return;
    }

    try
    {
        string connectionString = string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        DataTable dataTable = new DataTable();
        
        connection.Open();
        
        FindPassportInDataTable(connection, DataTable dataTable);
        SetResult(dataTable);
        
        connection.Close();
    }
    catch (SQLiteException exeption)
        TryHandle(exeption);
}

private void FindPassportInDataTable(SQLiteConnection connection, DataTable dataTable)
{
    string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(rawData));
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));

    sqLiteDataAdapter.Fill(dataTable);
    
    if (dataTable.Rows.Count < 0)
    {
        this.textResult.Text = "Паспорт «" + this.passportTextbox.Text + "» в списке участников дистанционного голосования НЕ НАЙДЕН";
        return;
    }
}

private void SetResult(DataTable dataTable)
{
    if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
        this.textResult.Text = "По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН";
    else
        this.textResult.Text = "По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
}

private void TryHandle(SQLiteException exception)
{
    int errorCode = 1;

    if (exeption.ErrorCode != errorCode)
        return;

    int massageBox = (int)MessageBox.Show("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
}

private void OnCheckButtonClicked(object sender, EventArgs e)
{
    if (this.passportTextbox.Text.Trim() == string.Empty)
    {
        int massageBox = (int)MessageBox.Show("������� ����� � ����� ��������");
        return;
    }

    string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
    int correctDataLength = 10;

    if (rawData.Length < correctDataLength)
    {
        this.textResult.Text = "�������� ������ ����� ��� ������ ��������";
        return;
    }

    try
    {
        string connectionString = string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        
        connection.Open();
        FindPassportInDataTable(connection);
        connection.Close();
    }
    catch (SQLiteException exeption)
        TryHandle(exeption);
}

private void FindPassportInDataTable(SQLiteConnection connection)
{
    string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(rawData));
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
    DataTable dataTable = new DataTable();

    sqLiteDataAdapter.Fill(dataTable);
    
    if (dataTable.Rows.Count < 0)
    {
        this.textResult.Text = "������� �" + this.passportTextbox.Text + "� � ������ ���������� �������������� ����������� �� ������";
        return;
    }

    SetResult();
}

private void SetResult()
{
    if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
        this.textResult.Text = "�� �������� �" + this.passportTextbox.Text + "� ������ � ��������� �� ������������� ����������� ����������� ������������";
    else
        this.textResult.Text = "�� �������� �" + this.passportTextbox.Text + "� ������ � ��������� �� ������������� ����������� ����������� �� ��������������";
}

private void TryHandle(SQLiteException exception)
{
    int errorCode = 1;

    if (exeption.ErrorCode != errorCode)
        return;

    int massageBox = (int)MessageBox.Show("���� db.sqlite �� ������. �������� ���� � ����� ������ � exe.");
}
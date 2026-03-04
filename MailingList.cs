namespace ASD;

// Класс адресатов 
public class MailingList
{
    // Имя адресата (максимум 20 символов + \0)
    public readonly char[] Name = new char[21];

    // Адрес (максимум 50 символов + \0)
    public readonly char[] Address = new char[51];

    public MailingList(string name, string address)
    {
        // Копируем имя
        int nameSize = Math.Min(name.Length, 20);
        for (int i = 0; i < nameSize; i++)
        {
            Name[i] = name[i];
        }

        Name[nameSize] = '\0'; // завершающий нуль

        // Копируем адрес
        int addressSize = Math.Min(address.Length, 50);
        for (int i = 0; i < addressSize; i++)
        {
            Address[i] = address[i];
        }

        Address[addressSize] = '\0'; // завершающий нуль
    }
    
    public override bool Equals(object? obj)
    {
        MailingList? other = (MailingList)obj;

        // Сравниваем Name
        for (int i = 0; i < 20; i++)
        {
            if (Name[i] != other.Name[i]) 
                return false;
            if (Name[i] == '\0') 
                break; // Достигли конца строки
        }

        // Сравниваем Address
        for (int i = 0; i < 50; i++)
        {
            if (Address[i] != other.Address[i]) 
                return false;
            if (Address[i] == '\0') 
                break; // Достигли конца строки
        }
        return true;
    }
    
    public override string ToString()
    {
        string nameStr = new string(Name).TrimEnd('\0');
        string addressStr = new string(Address).TrimEnd('\0');
        return $"{nameStr}: {addressStr}";
    }
}
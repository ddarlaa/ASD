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

    // переопределить метод для почтовой рассылки
    public override bool Equals(object? obj)
    {
        MailingList? other = (MailingList)obj!;

        // Сравнение поля Name
        int i = 0;
        while (true)
        {
            bool endThisName = (i >= this.Name.Length) || (this.Name[i] == '\0');
            bool endOtherName = (i >= other.Name.Length) || (other.Name[i] == '\0');

            if (endThisName && endOtherName)
                break; // Оба завершились – переходим к Address
            if (endThisName != endOtherName)
                return false; // Один завершился раньше другого
            if (this.Name[i] != other.Name[i])
                return false;
            i++;
        }

        // Сравнение поля Address
        i = 0;
        while (true)
        {
            bool endThisAddr = (i >= this.Address.Length) || (this.Address[i] == '\0');
            bool endOtherAddr = (i >= other.Address.Length) || (other.Address[i] == '\0');

            if (endThisAddr && endOtherAddr)
                return true; // Оба завершились – объекты равны
            if (endThisAddr != endOtherAddr)
                return false;
            if (this.Address[i] != other.Address[i])
                return false;
            i++;
        }
    }

    protected bool Equals(MailingList other)
    {
        return Name.Equals(other.Name) && Address.Equals(other.Address);
    }
    public override string ToString()
    {
        string nameStr = new string(Name).TrimEnd('\0');
        string addressStr = new string(Address).TrimEnd('\0');
        return $"{nameStr}: {addressStr}";
    }
    
}
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
    
    // Реализация интерфейса IEquatable<Addressee>
    public bool Equals(MailingList? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        for (int i = 0; i < Name.Length; i++)
        {
            if (Name[i] != other.Name[i]) return false;
        }

        for (int i = 0; i < Address.Length; i++)
        {
            if (Address[i] != other.Address[i]) return false;
        }

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as MailingList);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            new string(Name).TrimEnd('\0'),
            new string(Address).TrimEnd('\0')
        );
    }

    public override string ToString()
    {
        string nameStr = new string(Name).TrimEnd('\0');
        string addressStr = new string(Address).TrimEnd('\0');
        return $"{nameStr}: {addressStr}";
    }
}
using ASD.DoubleLinked;
//using ASD.Cursors;

namespace ASD;
public class Program
{
     public static void Main()
     {
    
    var list = new MyList<MailingList>(); // создаем пустой лист
    MakeList(list);
    list.PrintList(); // вывод3
    Console.WriteLine();
    
    DeleteDuplicates(list);
    list.PrintList(); // вывод 
    
    }
    
    private static void MakeList(MyList<MailingList> list)
    {
        // Добавляем адресатов с повторениями
        list.Insert(list.End(), new MailingList("Иванов Иван", "ул. Ленина, 10"));
        list.Insert(list.End(), new MailingList("Петров Петр", "ул. Мира, 15"));
        list.Insert(list.End(), new MailingList("Иванов Иван", "ул. Ленина, 10"));
        list.Insert(list.End(), new MailingList("Сидоров Сидор", "ул. Гагарина, 20"));
        list.Insert(list.End(), new MailingList("Петров Петр", "ул. Мира, 15"));
        list.Insert(list.End(), new MailingList("Васильев Василий", "ул. Пушкина, 5"));
        list.Insert(list.End(), new MailingList("Иванов Иван", "ул. Ленина, 10"));
    }
    
    private static void DeleteDuplicates(MyList<MailingList> list)
    {  
        Position<MailingList> p = list.First(); // получение первой позиции списка
        Position<MailingList> end = list.End(); // получение позиции после последнего
    
        while (!Equals(p, end)) // пока список не кончится
        {
            Position<MailingList> q = list.Next(p); // получаем следующую позицию за проверяемой
            while (!Equals(q, end))
            {
                if (list.Retrieve(p).Equals(list.Retrieve(q))) // если объекты в разных позициях совпали
                {
                    Position<MailingList> deleted = q; // запоминаем удаляемую позицию, чтобы могли перейти к следующей
                    q = list.Next(q); 
                    list.Delete(deleted); // удаляем дубликат
                }
                else
                {
                    q = list.Next(q); // если не удаляли дубликат все равно переходим к следующей
                }
            }
            p = list.Next(p); // ищем дубликаты для объекта на следующей позиции
        }
    }
}
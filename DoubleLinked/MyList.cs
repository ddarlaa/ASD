using ASD.DoubleLinked;

namespace ASD.DoubleLinked;



//Класс двусвязного списка хранит начало и конец
// Класс MyList<T> — реализация двусвязного списка.
// Каждый элемент хранит ссылки на предыдущий и следующий узлы.
// Работает только с ссылочными типами

public class MyList<T> where T : class
{
    //Голова списка (первый элемент)
    private Node? _head;
    //Хвост списка (последний элемент)
    private Node? _tail;
    
    //Условный "конец списка" (как маркер отсутствия позиции)
    // Возвращает фиктивную позицию конца списка (это будет null)
    public Position End()
    {
        return new Position(null);;
    }

    //Проверка, существует ли указанная позиция в списке (ищем текущий элемент)
    //никаких других проверок нет
    private bool CheckPos(Position p)
    {
       Node? cur = _head;
       //перебираем список от начала до конца
       while (cur != null)
       {
           if (Equals(cur, (Node)p.Pos!)) // нашли совпадение
           {
               return true;
           }
           cur = cur.Next;
       }

       return false; // позиция не найдена
    }
    
    
    
    //Вставка элемента obj перед позицией p
    
    //проверка на пустой 
    //позиция в голове нас не интересует
    //случай вставки в голову и в середину объединяем.
    
    //вставка в хвост и в позицию после последнего - разные случаи.
    //если после последнего -- пустой/не пустой список.
    public void Insert(Position p, T obj)
    {
        Node newNode;
        
        // Если вставляем в позицию после последнего
        if (p.Equals(End()))
        {
            //Если список пустой 
            if (_head == null)
            {
                _head = _tail = new Node(obj); // новый элемент является и головой, и хвостом
                return;
            }

            //Если список не пустой — спокойно добавляем новый узел в конец
            newNode = new Node(obj);
                _tail.Next = newNode;
                newNode.Previous = _tail;
                _tail = newNode;
                return;
            
        }

        //Если позиция некорректная - кидаем ПРЕДУПРЕЖДЕНИЕ, а не ИСКЛЮЧЕНИЕ
            if (!CheckPos(p))
                throw new Exception("Данная позиция не существует в списке");
            
        //В других сценариях всё хорошо
        
        // Переставляем ссылки для текущего узла, ПОСЛЕ которого вставляем
        // и нового узла, В который переносим данные

        Node cur = (Node)p.Pos!;

        newNode = new Node(cur.Data);
        
        newNode.Next = cur.Next;
        newNode.Next?.Previous = newNode;
        cur.Next = newNode;
        newNode.Previous = cur;
        
        // В текущий узел записываем новые данные
        cur.Data = obj;
        
        // Если вставляли в хвост, меняем его позицию
        if (p.Pos == _tail)
        {
            _tail = _tail!.Next;
        }
    }

    // Поиск первого вхождения элемента obj в списке
    public Position Locate(T obj)
    {
        
        Node? cur = _head;

        while (cur != null)
        {
            if (cur.Data.Equals(obj))
            {
                return new Position(cur);
            }
            cur = cur.Next;
        }
        return End(); // не найден
    }

    // Получение данных по позиции
    public T Retrieve(Position p)
    {
        
        //найдём позицию
        if (!CheckPos(p)) 
            throw new Exception("Данная позиция не существует в списке");
        
        //вернули значение из конкретного узла
        Node node = (Node)p.Pos!;
        return node.Data;
    }

    
    
    //удаление элемента по позиции
    public void Delete(Position p)
    {
        //проверка на позицию после последнего
        if (p.Equals(End()))
            throw new Exception("Удаление позиции после последнего невозможно");
        
        //проверка на голову 
        if (Equals(p.Pos!,_head))
        {
            //проверка на единственность элемента, тогда становится пустой (хвост и голова стали нулями)
            // в этом же случае если список пустой - он остаётся пустым
            if (_head == _tail)
            {
                _tail = _head = null;
                return;
            }
            
            //перемещаем голову на следующий элемент
            _head = _head!.Next;
            if (_head != null)
            {
                _head.Previous = null;
            }
            return;
        }
        
        //проверка на хвост, тогда хвост изменяется 
        if (p.Equals(_tail))
        {
            _tail = _tail!.Previous;
            _tail!.Next = null;
            return;
        }
        
        //проверка позиции - самая последняя проверка 
        if (!CheckPos(p))
        {
            throw new Exception("Данная позиция не существует в списке");
        }
        
        //все остальные случаи - р где-то в середине списка
        Node cur = (Node)p.Pos!;
        cur.Previous!.Next = cur.Next;
        cur.Next!.Previous = cur.Previous;
    
    }

    //получение позиции следующего элемента с ИСКЛЮЧЕНИЕМ
    public Position Next(Position p)
    {
        if (!CheckPos(p)) 
            throw new Exception("Данная позиция не существует в списке");
        Node cur = (Node)p.Pos!;
        return new Position(cur.Next);
    }

    //получение позиции предыдущего элемента с ИСКЛЮЧЕНИЕМ
    public Position Previous(Position p)
    {
        if (p.Pos == _head || !CheckPos(p)) 
            throw new Exception("Данная позиция не существует в списке или не имеет предыдущего");

        Node cur = (Node)p.Pos!;
        return new Position(cur.Previous);
    }

    // Получение позиции первого элемента списка
    public Position First()
    {
        return new Position(_head);
    }

    // Полная очистка списка
    public void MakeNull()
    {
        _head = _tail = null;
    }

    // Вывод списка в консоль
    public void PrintList()
    {
        Console.Write("[");
        
        Node? cur = _head;

        if (cur != null)
        {
            Console.Write(cur.Data); //печатаем первый элемент
            cur = cur.Next;
            
            //печатаем всё остальное через запятую
            while (cur != null)
            {
                Console.Write($",\n {cur.Data}");
                cur = cur.Next;
            }
        }

        Console.WriteLine("]");
    }

    //внутри есть приватный класс узел, где хранится значение и ссылка на следующий узел
    internal class Node
        (T data, Node? prev = null, Node? next = null)
    {

        internal T Data { get; set; } = data; // Объект внутри ноды
        internal Node? Previous { get; set; } = prev; // ссылка на предыдущую ноду
        internal Node? Next { get; set; } = next; // ссылка на следующую ноду
    }
    
}

public class Position
{
    // Храним ссылку как object.
    // internal - чтобы Main не видел это поле, но MyList видел.
    internal object? Pos { get; }

    public Position(object? pos)
    {
        Pos = pos;
    }

    // Переопределяем Equals для корректной работы p == End()
    public override bool Equals(object? obj)
    {
        if (obj is Position other)
        {
            // Сравниваем физические ссылки на объекты в памяти
            return ReferenceEquals(this.Pos, other.Pos);
        }
        return false;
    }
    public override int GetHashCode()
    {
        return Pos?.GetHashCode() ?? 0;
    }
}

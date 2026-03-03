namespace ASD.DoubleLinked;

public class Position<T> where T : class
{
    // Храним ссылку как object.
    // internal - чтобы Main не видел это поле, но MyList видел.
    internal MyList<T>.Node? Node { get; }

    internal Position(MyList<T>.Node? node)
    {
        Node = node;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Position<T> other)
            return Node == other.Node;
        return false;
    }
}



//Класс двусвязного списка хранит начало и конец
// Класс MyList<T> — реализация двусвязного списка.
// Каждый элемент хранит ссылки на предыдущий и следующий узлы.
// Работает только c ссылочными типами

public class MyList<T> where T : class
{
    //Голова списка (первый элемент)
    private Node? _head;

    //Хвост списка (последний элемент)
    private Node? _tail;

    //Условный "конец списка" (как маркер отсутствия позиции)
    // Возвращает фиктивную позицию конца списка (это будет null)
    public Position<T> End()
    {
        return new Position<T>(null);
    }

    //Проверка, существует ли указанная позиция в списке (ищем текущий элемент)
    //никаких других проверок нет
    private bool CheckPos(Position<T> p)
    {
        Node? cur = _head;
        //перебираем список от начала до конца
        while (cur != null)
        {
            if (cur == p.Node!) // нашли совпадение
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
    public void Insert(Position<T> p, T obj)
    {
        // Если вставляем в позицию после последнего
        if (p.Node == null)
        {
            //Если список пустой 
            if (_head == null)
            {
                _head = _tail = new Node(obj); // новый элемент является и головой, и хвостом
                return;
            }

            //Если список не пустой — спокойно добавляем новый узел в конец (после тейла, в этом случае меняется только хвост)
            Node newNode;
            newNode = new Node(obj);
            _tail!.Next = newNode;
            newNode.Previous = _tail;
            _tail = newNode;
            return;
        }

        //Если вставка происходит в конец списка
        if (p.Node == _tail)
        {
            Node cur = p.Node!;
            Node newNode2 = new Node(cur.Data);
            newNode2.Next = cur.Next; // будет null
            newNode2.Previous = cur;
            cur.Next = newNode2;
            newNode2.Previous = cur;
            cur.Data = obj;
            _tail = _tail!.Next; // теперь _tail указывает на newNode2
            return;
        }

        //Если позиция некорректная - кидаем ИСКЛЮЧЕНИЕ
        if (!CheckPos(p))
            throw new Exception("Данная позиция не существует в списке");

        // Переставляем ссылки для текущего узла, ПОСЛЕ которого вставляем
        // и нового узла, В который переносим данные

        //вставка в голову и в середину - одно и то же, а вставка в голову - другой случай 
        Node newNode3; //создаём объект 

        Node current = p.Node!;
        newNode3 = new Node(current.Data);
        newNode3.Next = current.Next;
        newNode3.Next?.Previous = newNode3;
        current.Next = newNode3;
        newNode3.Previous = current;

        // В текущий узел записываем новые данные
        current.Data = obj;
    }

    // Поиск первого вхождения элемента obj в списке
    public Position<T> Locate(T obj)
    {
        Node? current = _head;

        while (current != null)
        {
            if (current.Data.Equals(obj))
            {
                return new Position<T>(current);
            }

            current = current.Next;
        }

        return End(); // не найден
    }

    // Получение данных по позиции
    public T Retrieve(Position<T> p)
    {
        //найдём позицию
        if (!CheckPos(p))
            throw new Exception("Данная позиция не существует в списке");

        //вернули значение из конкретного узла
        return p.Node!.Data;
    }


    //удаление элемента по позиции
    public void Delete(Position<T> p)
    {
        //проверка на позицию после последнего
        if (p.Node == null)
            throw new Exception("Удаление позиции после последнего невозможно");

        //проверка на голову 
        if (p.Node == _head)
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
        if (p.Node == _tail)
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
        Node cur = p.Node!;
        cur.Previous!.Next = cur.Next;
        cur.Next!.Previous = cur.Previous;
    }

    //получение позиции следующего элемента с ИСКЛЮЧЕНИЕМ
    public Position<T> Next(Position<T> p)
    {
        if (!CheckPos(p))
            throw new Exception("Данная позиция не существует в списке");
        Node cur = p.Node!;
        return new Position<T>(cur.Next);
    }

    //получение позиции предыдущего элемента с ИСКЛЮЧЕНИЕМ
    public Position<T> Previous(Position<T> p)
    {
        if (p.Node == _head)
            throw new Exception("Данная позиция не имеет предыдущего");

        if (!CheckPos(p))
            throw new Exception("Данная позиция не существует в списке");

        Node cur = p.Node!;
        return new Position<T>(cur.Previous);
    }

    // Получение позиции первого элемента списка
    public Position<T> First()
    {
        return new Position<T>(_head);
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
    internal class Node(T data, Node? prev = null, Node? next = null)
    {
        internal T Data { get; set; } = data; // Объект внутри ноды
        internal Node? Previous { get; set; } = prev; // ссылка на предыдущую ноду
        internal Node? Next { get; set; } = next; // ссылка на следующую ноду
    }
}
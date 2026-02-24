namespace ASD.Cursors;

//Класс MyList<T> — реализация АТД списка на курсорах.
//Класс, где мы можем подставить ссылочные типы (подходит для первой лабораторной)
public class MyList<T> where T : class
{
    // Массив объектов списка и указателей
    private static readonly Node[] Elements;

    // Индекс первого элемента списка, -1 означает пустой список
    private int _start = -1;

    // Указатель на первый пустой индекс в массиве (голова "пустого списка")
    private static int _space = 0;

    // Максимальный размер списка
    private const int MaxSize = 100;

    // Статический конструктор — инициализация массивов и списка пустых ячеек
    static MyList()
    {
        Elements = new Node[MaxSize];
        const int size = MaxSize - 1;
        // Каждая ячейка указывает на следующую пустую
        for (int i = 0; i < size; i++)
        {
            Elements[i] = new Node
            {
                Next = i + 1
            };
        }

        // Последняя свободная указывает на конец (-1)
        Elements[size] = new Node
        {
            Next = -1
        };
    }

    // Возвращает позицию конца списка
    public Position End()
    {
        return new Position(-1);
    }

    //возвращает индекс последнего существующего элемента списка
    private int Last()
    {
        //перебором от начала и до конца ищем последнюю заполненную ячейку
        int cur = _start;
        int prev = -1;
        while (cur != -1)
        {
            prev = cur;
            cur = Elements[cur].Next;
        }

        return prev;
    }

    // Возвращает индекс предыдущего элемента относительно данного (ищем предыдущий)
    //нужен для того, чтобы можно было использовать и Insert Delete

    //так как это вспомогательный метод, мы работаем только с индексами,
    //а не с позициями, тк позиции - внешняя абстракция 
    private int GetPrev(int index)
    {
        //идём от начала списка 
        int cur = _start;
        int prev = -1;
        while (cur != -1)
        {
            if (cur == index) return prev;
            prev = cur;
            cur = Elements[cur].Next;
        }

        return -1; //иначе возвращаем -1
    }

    //Вставка элемента obj в позицию p

    //логика вставки: всегда новый узел ПОСЛЕ позиции п,
    //в новый переносится значение из текущего,
    //а в текущий вставляется значение из параметра

    //после вставки позиция сохранилась 
    public void Insert(Position p, T obj)
    {
        int tmp;
        //если ппп - вставляем просто в конец
        if (p.Equals(End()))
        {
            // а) список пустой 
            if (_start == -1)
            {
                _start = _space; // новый старт
                Elements[_space].Data = obj; // записываем объект
                _space = Elements[_space].Next; // передвигаем указатель на свободное место
                Elements[_start].Next = -1; // у последнего элемента next = -1
                return;
            }

            // б)список не пустой (вставляем в конец при помощи Last)

            Elements[_space].Data = obj;
            tmp = _space;
            _space = Elements[_space].Next;

            int last = Last();
            Elements[tmp].Next = Elements[last].Next;
            Elements[last].Next = tmp;
            return;
        }

        //проверка позиции на существование в списке 
        if (p.Pos >= 0 && (p.Pos == _start || GetPrev(p.Pos) != -1))
        {
            throw new Exception("Данная позиция отсутствует в списке");
        }

        // //проверка на голову ее(ВАЖНА)
        // if (Equals(p.Pos, _start))
        // {
        //     // Если мы вставляем в голову - вставляем новый элемент после головы
        //     Elements[_start].Next;
        //
        //     // Значение из головы копируем в этот новый элемент (только что вставленный)
        //
        //     // А в значение головы заносим то, что у нас передаётся в качестве параметра 
        // }


        // 1) Копируем данные из позиции p в свободную ячейку
        Elements[_space].Data = Elements[p.Pos].Data;

        // 2) Запоминаем новую ячейку и обновляем _space
        tmp = _space;
        _space = Elements[_space].Next;

        // 3) Вставляем новую ячейку ПОСЛЕ p
        Elements[tmp].Next = Elements[p.Pos].Next;
        Elements[p.Pos].Next = tmp;

        // 4) Заменяем данные в p на obj
        Elements[p.Pos].Data = obj;
    }


    // Находит позицию элемента obj в списке
    public Position Locate(T obj)
    {
        //идём по циклу пока текущий узел не станет -1
        int cur = _start;
        while (cur != -1)
        {
            if (Elements[cur].Data.Equals(obj)) return new Position(cur);
            cur = Elements[cur].Next;
        }

        return End(); // не найден
    }


    // Возвращает элемент списка по позиции
    public T Retrieve(Position p)
    {
        //проверка на позицию после последней
        if (p.Equals(End()))
        {
            throw new Exception("Данная позиция не существует в списке");
        }

        //возвращаем значение узла позиции р
        return Elements[p.Pos].Data;
    }

    // Удаляет элемент из списка по позиции
    public void Delete(Position p)
    {
        //проверка на позицию после последнего
        if (p.Equals(End()))
        {
            throw new Exception("Данная позиция отсутствует в списке");
        }

        if (_start == -1)
        {
            throw new Exception("Список пуст");
        }

        int tmp;


        //если удаляем голову
        if (p.Pos == _start)
        {
            // проверка на единственный элемент (голова и нет следующего)
            if (Elements[p.Pos].Next == -1)
            {
                // Случай 1: Удаляем единственный элемент
                Elements[p.Pos].Next = _space;
                _space = p.Pos;
                _start = -1;
                return;
            }

            // Случай 2: Удаляем первый элемент (но не единственный)
            tmp = _space;
            _space = _start;
            _start = Elements[_start].Next;
            Elements[_space].Next = tmp;
            return;
        }

        //не для первого элемента находим предыдущий
        int prev = GetPrev(p.Pos);

        if (prev == -1)
        {
            throw new Exception("Нарушена связность списка! У не первого элемента в списке нет предыдущего.");
        }

        //если удаляем последний (но не единственный) элемент
        if (Elements[p.Pos].Next == -1)
        {
            Elements[prev].Next = -1; // предыдущий становится последним

            // Освобождаем удаляемую ячейку
            Elements[p.Pos].Next = _space;
            _space = p.Pos;
            return;
        }

        Elements[prev].Next = Elements[p.Pos].Next;
        tmp = _space;
        Elements[p.Pos].Next = tmp;
        _space = p.Pos;
    }


    //Возвращает позицию следующего элемента
    public Position Next(Position p)
    {
        //проверка на позицию после последнего (если позиция не существует в списке кидаем ИСКЛЮЧЕНИЕ)
        if (p == End())
            throw new Exception("Данная позиция не существует в списке");


        //если р - последняя позиция в списке 
        if (p.Pos == Last())
        {
            return End();
        }


        return new Position(Elements[p.Pos].Next);

        //получаем предыдущий - если есть тогда берем после следущую поз
        //иначе возвращаем следующую
    }

    // Возвращает позицию предыдущего элемента
    public Position Previous(Position p)
    {
        //проверка на позицию после последнего или первую позицию в списке
        if (p == End() || p.Pos == _start)
            throw new Exception("Данная позиция не существует в списке");

        //иначе возвращаем предыдущую заполненную позицию
        return new Position(GetPrev(p.Pos));
    }

    //Очищает список, все элементы возвращаются в свободное пространство
    public void MakeNull()
    {
        if (_start == -1) return;

        Elements[Last()].Next = _space; // цепляем освободившиеся ячейки к списку пустых
        _space = _start; //обновляем новое начало пустого списка
        _start = -1;
    }


    // Возвращает позицию первого элемента списка
    public Position First()
    {
        //если список пустой - возвращает ппп
        if (_start == -1)
        {
            return End();
        }

        return new Position(_start);
    }

    // Печатает список в консоль
    public void PrintList()
    {
        Console.Write("[");
        int cur = _start;
        while (cur != -1)
        {
            Console.Write($",\n{Elements[cur].Data}");
            cur = Elements[cur].Next;
        }

        Console.WriteLine("]");
    }

    //внутри есть вложенный класс узел, где хранится значение и ссылка на следующий узел
    internal class Node
    {
        internal T Data { get; set; }
        internal int Next { get; set; }

        // Конструктор по умолчанию (нужен для инициализации)
        internal Node()
        {
        }

        internal Node(T data, int next)
        {
            Data = data;
            Next = next;
        }
    }
}

//вспомогательный класс позиции (абстракция)
//объект - позиция со свойствами
public class Position(int pos)
{
    public int Pos { get; init; } = pos;

    public override bool Equals(object? obj)
    {
        if (obj is Position other)
        {
            return this.Pos == other.Pos; // Сравниваем по значению индекса
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Pos.GetHashCode();
    }
}
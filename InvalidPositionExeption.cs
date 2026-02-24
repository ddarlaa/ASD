namespace ASD;

public class InvalidPositionExeption
{
    // Класс исключения для некорректной позиции списка
    public class InvalidPositionException(string message) : Exception(message) {}
}
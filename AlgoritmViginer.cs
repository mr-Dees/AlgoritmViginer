// Импорт пространства имен System.
using System;
// Импорт пространства имен System.Net, которое предоставляет простой программный 
// интерфейс для многих протоколов сетевых служб.
using System.Net;
// Импорт пространства имен System.Security.
using System.Security;
// Импорт пространства имен System.Text.RegularExpressions.
using System.Text.RegularExpressions;

// Объявление класса AlgoritmViginer.
public class AlgoritmViginer
{
    // Определение алфавита для шифрования.
    private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
        "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789";
    // Метод для получения ввода от пользователя в виде безопасной строки.
    private static SecureString GetPassword()
    {
        // Создание нового экземпляра безопасной строки.
        SecureString password = new SecureString();

        // Ручное сокрытие пароля в виде бесконечного цикла, 
        // который продолжается до тех пор, пока пользователь не нажмет Enter.
        while (true)
        {
            // Чтение следующего символа или функциональной клавиши, нажатой пользователем.
            ConsoleKeyInfo i = Console.ReadKey(true);
            // Если был нажат Enter
            if (i.Key == ConsoleKey.Enter)
            {
                // Выход из цикла
                break;
            }
            // Если пользователь стер символ
            else if (i.Key == ConsoleKey.Backspace) // Проверка, что пользователь нажал Backspace.
            {
                // Если символы есть в строке
                if (password.Length > 0)
                {
                    // Удаление последнего символа из пароля.
                    password.RemoveAt(password.Length - 1);
                    // Удаление отображения последнего символа в консоли.
                    Console.Write("\b \b");
                }
            }
            // Если нажата другая кнопка
            else
            {
                // Добавляем символ в пароль
                password.AppendChar(i.KeyChar);
                // Вывод символа * в консоль.
                Console.Write("*");
            }
        }
        // Вывод перехода на новую строку.
        Console.Write('\n');
        // Возврат введенного пароля.
        return password;
    }

    // Метод для получения ввода от пользователя с проверкой на соответствие определенным условиям.
    public static string GetInput(string prompt)
    {
        // Бесконечный цикл, который продолжается до тех пор, пока пользователь не введет допустимое значение.
        while (true)
        {
            // Вывод запроса на ввод.
            Console.Write(prompt);
            // Получение ввода от пользователя в виде безопасной строки.
            SecureString input = GetPassword();
            // Преобразование безопасной строки в обычную строку.
            string user_input = new NetworkCredential(string.Empty, input).Password;

            // Проверка, что длина ввода не меньше 4 символов.
            if (user_input.Length < 4)
            {
                // Вывод сообщения об ошибке, если длина ввода меньше 4 символов.
                Console.WriteLine("Ошибка: ввод должен содержать не менее 4 символов.");
            }
            // Проверка, что ввод содержит как минимум одну цифру и одну латинскую букву.
            else if (!Regex.IsMatch(user_input, @"\d") || !Regex.IsMatch(user_input, @"[a-zA-Z]"))
            {
                // Вывод сообщения об ошибке, если ввод не содержит как минимум одну цифру и одну латинскую букву.
                Console.WriteLine("Ошибка: ввод должен содержать как минимум одну цифру и одну латинскую букву. ");
            }
            // Проверка, что ввод не содержит повторяющихся символов.
            else if (Regex.IsMatch(user_input, @"(.)\1"))
            {
                // Вывод сообщения об ошибке, если ввод содержит повторяющиеся символы.
                Console.WriteLine("Ошибка: ввод не должен содержать повторяющиеся символы. ");
            }
            // Проверка, что все символы ввода присутствуют в алфавите.
            else if (AllCharsInAlphabet(user_input))
            {
                // Возврат ввода, если все проверки пройдены успешно.
                return user_input;
            }
            // Если вознакла неопределенная ситуация
            else
            {
                // Вывод сообщения об ошибке, если ввод содержит символы, которых нет в алфавите.
                Console.WriteLine("Ошибка: ввод содержит запрещенные символы. ");
            }
        }
    }

    // Метод для проверки, что все символы ввода присутствуют в алфавите.
    private static bool AllCharsInAlphabet(string input)
    {
        foreach (char c in input) // Цикл по каждому символу ввода
        {
            // Проверка, что символ присутствует в алфавите.
            if (!alphabet.Contains(c.ToString()))
            {
                return false; // Возврат false, если символа нет в алфавите.
            }
        }
        return true; // Возврат true, если все символы ввода присутствуют в алфавите.
    }

    // Метод для шифрования сообщения с использованием ключа.
    public static string Encrypt(string message, string key)
    {
        // Создание пустой строки для хранения зашифрованного сообщения.
        string encrypted = "";
        // Инициализация счетчика для ключа.
        int i = 0;
        // Цикл по каждому символу сообщения.
        foreach (char с in message)
        {
            // Получение позиции символа в алфавите.
            int pos = alphabet.IndexOf(с);
            // Получение позиции символа ключа в алфавите.
            int key_pos = alphabet.IndexOf(key[i]);
            // Вычисление новой позиции символа после шифрования.
            int new_pos = (pos + key_pos) % alphabet.Length;
            // Добавление зашифрованного символа в строку.
            encrypted += alphabet[new_pos];
            // Обновление счетчика для ключа. Остаток от деление берется для зацикливания ключа
            i = (i + 1) % key.Length;
        }
        // Возврат зашифрованного сообщения.
        return encrypted;
    }
}

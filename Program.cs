using System;
using Telegram.Bot;//Библиотека Telegram.Bot
using System.Threading.Tasks;//библиотека для ассинхронных потоков
using System.Data.SQLite;//Библиотека поддержики SQLine баз данных
namespace MyTorrentzBot
{
    class Program
    {
        const string TOKEN = "858759117:AAHPjdQEW1NScJlVnZRjKFdccEr0SopRoeM";//переменная с токеном бота типа константа. TOKEN с ошибкой, так как ещё его не использовали
        public static SQLiteConnection DB;//Переменная с базой данных
        static void Main(string[] args)
        {
            while (true)
            {
                try//обработчик  исключений. Отлавливает исключения, вызванные в ходе выполнения работы
                {
                    GetMessage().Wait();//здесь будет основная задача бота, метод
                }
                catch (Exception ex)//здесь ошибка, если срабатывает исключение. Выводит в консоль
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
        }
        static async Task GetMessage()//метод, выполняющие авторизацию и получающий обновления от телеграм бота. Он статический и ассинхронный. Второй поток типа
        {
            TelegramBotClient bot = new TelegramBotClient(TOKEN);//Переменная авторизации бота.
            int offset = 0;//тупа нужно
            int timeout = 0;
            try
            {
                await bot.SetWebhookAsync("");//Убираем вебхук
                while (true)//Вытаскиваем сообщения из обновления бота
                {
                    var updates = await bot.GetUpdatesAsync(offset, timeout);//Получили обновления
                    foreach (var update in updates)//
                    {
                        var message = update.Message;//Получили сообщение
                        if (message.Text == "MyFirstBot")//проверяем содержимое сообщения
                        {
                            Console.WriteLine("Получено сообщение: " + message.Text + " ; ID отправителя: " + message.Chat.Username + " ; ID чата: " + message.Chat.Id + " ; Дата,время(GMT 0): " + DateTime.UtcNow + "\n");//Вывели сообщение в консоль
                            await bot.SendTextMessageAsync(message.Chat.Id, "Привет, создатель, я твой бот!");//ответ на сообщение
                            await bot.SendTextMessageAsync(message.Chat.Id, "Сообщение отправлено пользователем - " + message.Chat.Username);
                        }
                        if (message.Text == "/reg")
                        {
                            Registration(message.Chat.Id.ToString(), message.Chat.Username.ToString());
                            await bot.SendTextMessageAsync(message.Chat.Id, "Пользователь зарегистрирован");
                        }
                        offset = update.Id + 1;//чтобы не приходило много обновлений
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
        public static void Registration(string chatId, string username )//Метод для регистрации пользователя в базе данных
        {
            try
            {
                DB = new SQLiteConnection("Data Source=DB.db;");//Присваиваем переменной значение базы данных
                DB.Open();//Открыли базу данных
                SQLiteCommand regcmd = DB.CreateCommand();//Создаём команду добавления пользователя
                regcmd.CommandText = "INSERT INTO Regusers VALUES(@chatId, @username)";//Запрос на добавление пользователя
                regcmd.Parameters.AddWithValue("@chatId", chatId);//Добавляем параметры
                regcmd.Parameters.AddWithValue("@username", username);
                regcmd.ExecuteNonQuery();//Подставить и выполнить
                DB.Close();//Закрыли базу данных
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}

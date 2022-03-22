namespace WebSurvey
{
    public static class WordHelp
    {
        /// <summary>
        /// Метод для склонения слов.
        /// </summary>
        /// <param name="number">Само число, к которому надо просклонять слова</param>
        /// <param name="str1">Сколонение слова при 0. Например для слова 'день' - над написать "дней"</param>
        /// <param name="str2">Сколонение слова при 1. Например для слова 'день' - над написать "день"</param>
        /// <param name="str3">Сколонение слова при 2. Например для слова 'день' - над написать "дня"</param>
        /// <returns>Правильной склонение или str1</returns>
        public static string Inflect(int number, string str1, string str2, string str3)
        {
            if (number % 10 == 0 || number % 100 >= 11 && number % 100 <= 19)
            {
                return str1;
            }
            else
            {
                switch (number % 10)
                {
                    case 1:
                        return str2;
                    case 2:
                    case 3:
                    case 4:
                        return str3;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return str1;
                    default:
                        return str1;
                }
            }
        }
    }
}

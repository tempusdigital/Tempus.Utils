using System;

namespace Tempus.Utils
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Retorna o início do ano (dia 1 de Janeiro, às 00:00:00:000)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset NextDayOfWeek(this DateTimeOffset source, DayOfWeek dayOfWeek)
        {
            var daysUntil = (((int)dayOfWeek - (int)source.DayOfWeek + 6) % 7) + 1;
            return source.AddDays(daysUntil);
        }

        /// <summary>
        /// Retorna o início do ano (dia 1 de Janeiro, às 00:00:00:000)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset StartOfTheYear(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, 1, 1, 0, 0, 0, 0, source.Offset);
        }

        /// <summary>
        /// Retorna o fim do ano (dia 31 de Janeiro, às 23:59:59:999)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfTheYear(this DateTimeOffset source)
        {
            var day = DateTime.DaysInMonth(source.Year, 12);
            return new DateTimeOffset(source.Year, 12, day, 23, 59, 59, 999, source.Offset);
        }

        /// <summary>
        /// Retorna o início do mês (dia 1, às 00:00:00:000)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset StartOfTheMonth(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, 1, 0, 0, 0, 0, source.Offset);
        }

        /// <summary>
        /// Retorna o fim do mês (último dia do mês, às 23:59:59:999)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfTheMonth(this DateTimeOffset source)
        {
            var day = DateTime.DaysInMonth(source.Year, source.Month);
            return new DateTimeOffset(source.Year, source.Month, day, 23, 59, 59, 999, source.Offset);
        }

        /// <summary>
        /// Retorna o início do dia (horário 00:00:00:000)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset StartOfTheDay(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, 0, 0, 0, 0, source.Offset);
        }

        /// <summary>
        /// Retorna o fim do dia (horário 23:59:59:999)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfTheDay(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, 23, 59, 59, 999, source.Offset);
        }

        /// <summary>
        /// Retorna o início do segundo, ou seja, no milisegundo 0
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset StartOfTheSecond(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, 0, source.Offset);
        }

        /// <summary>
        /// Retorna o fim do segundo, ou seja, no milisegundo 999
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfTheSecond(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, 999, source.Offset);
        }

        /// <summary>
        /// Troca os segundos para 59 e milisegundos para 999
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfTheMinute(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, source.Hour, source.Minute, 59, 999, source.Offset);
        }

        public static DateTimeOffset StartOfTheMinute(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, source.Hour, source.Minute, 0, 0, source.Offset);
        }

        /// <summary>
        /// Troca os minutos para 59 e segundos para 59 e milisegundos para 999
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfTheHour(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, source.Hour, 59, 59, 999, source.Offset);
        }

        public static DateTimeOffset StartOfTheHour(this DateTimeOffset source)
        {
            return new DateTimeOffset(source.Year, source.Month, source.Day, source.Hour, 0, 0, 0, source.Offset);
        }
    }
}

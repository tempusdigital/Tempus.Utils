namespace Tempus.Utils.FluentValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ValidationConst
    {
        public static Regex SomenteLetrasComAcentosOuNumerosOuEspaco = new Regex(@"^[A-Za-z 0-9ãõáéíóúâêîôûçñüàÃÕÁÉÍÓÚÂÊÎÔÛÇÑÜÀ]+$");

        public static Regex SomenteLetrasComAcentosOuNumerosOuEspacoOuPonto = new Regex(@"^[A-Za-z 0-9ãõáéíóúâêîôûçñüàÃÕÁÉÍÓÚÂÊÎÔÛÇÑÜÀ@@\.]+$");

        public static Regex SomenteNumerosOuLetrasSemAcentos = new Regex(@"^\w+$");

        public static Regex SomenteNumeros = new Regex(@"^[\d]*$");

        public static string TiposDeArquivoSuportados = ".pdf,.jpg,.jpeg,.png,.xls,.xlsx";
    }
}

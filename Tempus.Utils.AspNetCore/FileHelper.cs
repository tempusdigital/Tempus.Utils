namespace Tempus.Utils.AspNetCore
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public static class FileHelper
    {
        public static async Task<string> SaveFile(string salvarArquivoEm, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length <= 0)
                return null;

            var pasta = DateTime.Now.ToString("yyyy-MM-dd");

            var caminho = Path.Combine(salvarArquivoEm, pasta);

            if (!Directory.Exists(caminho))
                Directory.CreateDirectory(caminho);

            var dataHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            var nomeArquivo = dataHora + " - " + Path.GetFileName(arquivo.FileName);

            var filePath = Path.Combine(caminho, nomeArquivo);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await arquivo.CopyToAsync(fileStream);
            }

            return Path.Combine(pasta, nomeArquivo);
        }
    }
}

namespace Tempus.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    [DebuggerStepThrough]
    public static class ChecarSe
    {
        public static void Encontrou(object objeto)
        {
            if (objeto == null)
                throw new NotFoundException();
        }
        

        public static void ArgumentoNaoNulo<TArgumento>(TArgumento argumento, string nomeDoArgumento = null)
            where TArgumento : class
        {
            if (argumento == null)
            {
                if (nomeDoArgumento != null)
                    throw new ArgumentNullException(nomeDoArgumento);

                throw new ArgumentNullException();
            }
        }

        public static void ArgumentoNaoVazio<TArgumento>(IEnumerable<TArgumento> argumento, string nomeDoArgumento = null)
        {
            if (argumento == null || argumento.Count() == 0)
            {
                if (nomeDoArgumento != null)
                    throw new ArgumentNullException(nomeDoArgumento);

                throw new ArgumentNullException();
            }
        }

        public static void ArgumentoNaoVazio(DateTime? argumento, string nomeDoArgumento = null)
        {
            if (argumento.GetValueOrDefault() == default(DateTime))
            {
                if (nomeDoArgumento != null)
                    throw new ArgumentNullException(nomeDoArgumento);

                throw new ArgumentNullException();
            }
        }

        public static void ArgumentoNaoVazio(int? argumento, string nomeDoArgumento = null)
        {
            if (argumento.GetValueOrDefault() == default(int))
            {
                if (nomeDoArgumento != null)
                    throw new ArgumentNullException(nomeDoArgumento);

                throw new ArgumentNullException();
            }
        }

        public static void ArgumentoNaoVazio(string argumento, string nomeDoArgumento = null)
        {
            if (string.IsNullOrWhiteSpace(argumento))
            {
                if (nomeDoArgumento != null)
                    throw new ArgumentNullException(nomeDoArgumento);

                throw new ArgumentNullException();
            }
        }
    }
}

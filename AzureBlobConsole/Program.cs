using System.Diagnostics;

namespace AzureBlobConsole
{
    internal class Program
    {
        const string ConnectionString = "";
        private static AzureBlobLogger logger;

        static async Task Main(string[] args)
        {
            logger = new AzureBlobLogger(ConnectionString, "logs");

            var url = await FazerUploadBase64();

            Console.WriteLine($"URL do arquivo: {url}");

            await FazerDownload(url);

            await CriarLog();

            Console.WriteLine("Pressione qualquer tecla para sair...");
        }

        static async Task CriarLog()
        {
            await logger.Log("Upload e download");
        }

        static async Task<string> FazerUploadArquivoFisico()
        {
            try
            {
                var blobUploader = new AzureBlobUploader(ConnectionString, "uploads");

                var caminhoArquivo = @"";


                var nomeArquivo = Guid.NewGuid().ToString() + "-" + Path.GetFileName(caminhoArquivo);

                var url = await blobUploader.UploadFile(caminhoArquivo, nomeArquivo);

                return url;

            }
            catch (Exception ex)
            {
                await logger.Log(ex.Message);
                return string.Empty;
            }
        }

        static async Task<string> FazerUploadBase64()
        {
            try
            {
                var blobUploader = new AzureBlobUploader(ConnectionString, "uploads");

                var caminhoArquivo = @"";

                var base64String = Convert.ToBase64String(File.ReadAllBytes(caminhoArquivo));

                var nomeArquivo = Guid.NewGuid().ToString() + "-" + Path.GetFileName(caminhoArquivo);

                var url = await blobUploader.UploadBase64(base64String, nomeArquivo);

                return url;

            }
            catch (Exception ex)
            {
                await logger.Log(ex.Message);
                return string.Empty;
            }
        }

        static async Task<string> FazerUploadByteArray()
        {
            try
            {
                var blobUploader = new AzureBlobUploader(ConnectionString, "uploads");

                var caminhoArquivo = @"";

                var arquivoBytes = File.ReadAllBytes(caminhoArquivo);

                var nomeArquivo = Guid.NewGuid().ToString() + "-" + Path.GetFileName(caminhoArquivo);

                var url = await blobUploader.UploadFile(arquivoBytes, nomeArquivo);

                return url;

            }
            catch (Exception ex)
            {
                await logger.Log(ex.Message);
                return string.Empty;
            }
        }

        static async Task FazerDownload(string url)
        {
            try
            {
                var blobUploader = new AzureBlobUploader(ConnectionString, "uploads");

                var caminhoArquivo = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(url));

                await blobUploader.DownloadFile(url, caminhoArquivo);

                // Abre a imagem no visualizador padrão
                string argument = "/open, \"" + caminhoArquivo + "\"";
                Process.Start("explorer.exe", argument);
            }
            catch (Exception ex)
            {
                await logger.Log(ex.Message);
            }
        }
    }
}
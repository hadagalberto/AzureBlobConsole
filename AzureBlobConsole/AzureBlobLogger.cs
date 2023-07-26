using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;

namespace AzureBlobConsole
{
    public class AzureBlobLogger
    {
        private CloudBlobContainer _blobContainer;
        private string _currentFileName;

        public AzureBlobLogger(string connectionString, string containerName)
        {
            // Verifica a connection string
            if (!CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                throw new ArgumentException("Connection string inválida.");
            }

            // Cria o blob client e a referencia do container
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference(containerName);

            _currentFileName = string.Empty;
        }

        public async Task Log(string mensagem)
        {
            string fileName = $"{DateTime.UtcNow:dd-MM-yyyy}.log";

            // Cria um novo arquivo de log se a data mudou
            if (fileName != _currentFileName)
            {
                _currentFileName = fileName;
            }

            // Cria o container se ele não existir
            await _blobContainer.CreateIfNotExistsAsync();


            // Cria ou obtem o blob de log atual
            var blockBlob = _blobContainer.GetAppendBlobReference(_currentFileName);

            // Cria o blob se ele não existir
            if (!await blockBlob.ExistsAsync())
            {
                await blockBlob.CreateOrReplaceAsync();
            }

            // Concatena a mensagem de log com uma quebra de linha
            byte[] logBytes = Encoding.UTF8.GetBytes(mensagem + Environment.NewLine);

            // Faz o upload do log
            using (MemoryStream stream = new MemoryStream(logBytes))
            {
                await blockBlob.AppendFromStreamAsync(stream);
            }
        }
    }
}

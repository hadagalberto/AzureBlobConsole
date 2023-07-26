using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobConsole
{
    public class AzureBlobUploader
    {
        private CloudBlobContainer _blobContainer;

        public AzureBlobUploader(string connectionString, string containerName)
        {
            // Verifica a connection string
            if (!CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                throw new ArgumentException("Connection string inválida.");
            }

            // Cria o blob client e a referencia do container
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference(containerName);
        }

        public async Task<string> UploadFile(string localFilePath, string blobName)
        {
            // Pega a referencia do blob
            CloudBlockBlob blockBlob = _blobContainer.GetBlockBlobReference(blobName);

            // Cria o container se ele não existir
            await _blobContainer.CreateIfNotExistsAsync();

            // Faz o upload do arquivo para o blob
            using (var fileStream = File.OpenRead(localFilePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            return blobName;
        }

        public async Task<string> UploadFile(byte[] byteArray, string blobName)
        {
            // Pega a referencia do blob
            CloudBlockBlob blockBlob = _blobContainer.GetBlockBlobReference(blobName);

            // Cria o container se ele não existir
            await _blobContainer.CreateIfNotExistsAsync();

            // Faz o upload do arquivo para o blob
            await blockBlob.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);

            return blobName;
        }

        public async Task<string> UploadBase64(string base64String, string blobName)
        {
            // Pega a referencia do blob
            CloudBlockBlob blockBlob = _blobContainer.GetBlockBlobReference(blobName);

            // Cria o container se ele não existir
            await _blobContainer.CreateIfNotExistsAsync();

            // Converte a string base64 para um filestream
            var stream = new MemoryStream(Convert.FromBase64String(base64String));

            // Faz o upload do arquivo para o blob
            await blockBlob.UploadFromStreamAsync(stream);

            return blobName;
        }

        public async Task DownloadFile(string blobName, string localFilePath)
        {
            // Pega a referencia do blob
            CloudBlockBlob blockBlob = _blobContainer.GetBlockBlobReference(blobName);

            // Faz o download do blob para o arquivo local
            using (var fileStream = File.OpenWrite(localFilePath))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
            }
        }
    }
}

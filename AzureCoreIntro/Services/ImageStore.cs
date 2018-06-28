using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCoreIntro.Services
{
    public class ImageStore
    {
        CloudBlobClient cloudBlobClient;
        string baseUri = "https://azurestorageintro.blob.core.windows.net/";
        public ImageStore()
        {
            var credentials = new StorageCredentials("azurestorageintro", "ze0STNsjYRx6/4QqhwesMI2B/4ST6tNE5kCjZJb/bM18hmAzZeyhsQ6YGVjpNfzL8xP/gdY2XfAMJl1xb4vuwg==");
            cloudBlobClient = new CloudBlobClient(new Uri(baseUri), credentials);
        }
        public async Task<string> SaveImage(Stream stream)
        {
            string imageId = Guid.NewGuid().ToString();
            var container = cloudBlobClient.GetContainerReference("containerimages");
            var blob = container.GetBlockBlobReference(imageId);
            await blob.UploadFromStreamAsync(stream);
            return imageId;
        }

        public string UriFor(string imageId)
        {
            var sapsPolicy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(15)
            };
            var container = cloudBlobClient.GetContainerReference("containerimages");
            var blob = container.GetBlockBlobReference(imageId);
            var sap = blob.GetSharedAccessSignature(sapsPolicy);

            return $"{baseUri}containerimages/{imageId}{sap}";
        }
    }
}

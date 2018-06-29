using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.WindowsAzure.Storage.Blob;
namespace AzureFunctions
{
    public static class ImageAnalysis
    {
        [FunctionName("ImageAnalysis")]
        public static async Task Run([BlobTrigger("images/{name}",
            Connection = "AzureFunctionImageAnalysis")]CloudBlockBlob blob,
            string name,
            TraceWriter log,
            [CosmosDB("cosmosdbonazure", "images", ConnectionStringSetting = "cosmosdbonazure")]
        IAsyncCollector<FaceAnalysisResults> result)
        {
            log.Info($"C# Blob trigger function Processed blob\n N  ame:{blob.Name} \n Size: {blob.Properties.Length} Bytes");
            var sas = GetSas(blob);
            var url = blob.Uri + sas;
            log.Info($"Blob Url is {url}");
            var faces = await GetAnalyisAsync(url);
            await result.AddAsync(new FaceAnalysisResults { Faces = faces, ImageId = blob.Name });
        }

        public static async Task<Face[]> GetAnalyisAsync(string url)
        {
            var client = new FaceServiceClient("23f833674f6c41c7a4a94651be9bec9b", "https://southcentralus.api.cognitive.microsoft.com/face/v1.0");

            var faceAttributes = new[] { FaceAttributeType.Emotion };
            var result = await client.DetectAsync(url, false, false, faceAttributes);
            return result;

        }
        public static string GetSas(CloudBlockBlob blob)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };


            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return sas;
        }
    }
}

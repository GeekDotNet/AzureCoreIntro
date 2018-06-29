using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AzureCoreIntro.Services
{
    public class ImageAnalyisStore
    {
        private DocumentClient client;

        private Uri imageAnalysisLink;

        public ImageAnalyisStore()
        {
            var uri = new Uri("https://cosmosdbonazure.documents.azure.com:443/");
            var key = "2THIF9Tr4CEkfyZtWgdGuBKhcfBMFYaYb3NUpGILyL8py2GD5y9IthvyXSvXwaLy30FZ78h1o4Z2XAxM0rskrQ==";
            client = new DocumentClient(uri, key);

            imageAnalysisLink = UriFactory.CreateDocumentCollectionUri("cosmosdbonazure", "images");
        }

        public dynamic GetImageAnalysis(string imageId)
        {
            var spec = new SqlQuerySpec();
            spec.QueryText = "SELECT * FROM c where (c.ImageId=@imageId)";
            spec.Parameters.Add(new SqlParameter("@imageid", imageId));
            var result = client.CreateDocumentQuery(imageAnalysisLink, spec).ToList();
            return result;
        }
    }
}

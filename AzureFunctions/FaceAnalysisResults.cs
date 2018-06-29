using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions
{
    public class FaceAnalysisResults
    {
        public Face[] Faces { get; set; }
        public string ImageId { get; set; }
    }
}

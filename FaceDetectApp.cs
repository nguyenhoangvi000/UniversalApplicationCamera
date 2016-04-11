using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using System.IO;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace CameraGetPreviewFrame
{
    public class FaceDetectApp
    {

        private VisionService _vision;


        public FaceDetectApp(VisionService vision)
        {
            this._vision = vision;
        }
        public IList<FaceAnnotation> detectFaces(byte[] input, int maxResults)
        {
            byte[] data = input;

            AnnotateImageRequest request = new AnnotateImageRequest();
            Google.Apis.Vision.v1.Data.Image img = new Google.Apis.Vision.v1.Data.Image();
            img.Content = Convert.ToBase64String(data);
            request.Image = img;

            Feature feature = new Feature();
            feature.Type = "FACE_DETECTION";
            feature.MaxResults = maxResults;

            request.Features = new List<Feature>()
            {
                feature
            };

            BatchAnnotateImagesRequest batchAnnotate = new BatchAnnotateImagesRequest();
            batchAnnotate.Requests = new List<AnnotateImageRequest>() {
                request
            };
            ImagesResource.AnnotateRequest annotate = _vision.Images.Annotate(batchAnnotate);

            BatchAnnotateImagesResponse batchResponse = annotate.Execute();

            AnnotateImageResponse response = batchResponse.Responses[0];

            if (response.FaceAnnotations == null)
            {
                throw new Exception(response.Error.Message);
            }

            return response.FaceAnnotations;

        }
    }
}

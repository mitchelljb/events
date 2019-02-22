using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
namespace CustomVisionMarvel01
{
    
    public sealed class modelInput
    {
        public ImageFeatureValue data; // BitmapPixelFormat: Bgra8, BitmapAlphaMode: Premultiplied, width: 416, height: 416
    }
    
    public sealed class modelOutput
    {
        //public TensorFloat model_outputs0; // shape(-1,40,13,13)
        public TensorFloat Model_outputs0 = TensorFloat.Create(new long[] { 1, 4 });
    }

    public sealed class modelModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<modelModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            var learningModel = new modelModel();
            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<modelOutput> EvaluateAsync(modelInput input)
        {
            binding.Bind("data", input.data);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new modelOutput();
            //output.model_outputs0 = result.Outputs["model_outputs0"] as TensorFloat;
            return output;
        }
    }
}

# Profile a model

The performance of a model depends on the following:

- The complexity of the model.
- Whether the model uses performance-heavy operators such as `Conv` or `MatMul`.
- The features of the platform you run the model on, for example CPU memory, GPU memory, and number of cores.
- Whether Sentis downloads data to CPU memory when you access a tensor. Refer to [Get output from a model](get-the-output.md) for more information.

## Profile a model in the Profiler window

To get performance information when you run a model, you can use the following:

- [The Profiler window](https://docs.unity3d.com/Documentation/Manual/Profiler.html).
- [RenderDoc](https://docs.unity3d.com/Documentation/Manual/RenderDocIntegration.html), a third-party graphics debugger.

The **Profiler** window displays each Sentis layer as a dropdown item in the **Module Details** panel. Open a layer to get a detailed timeline of the execution of the layer. 

When a layer executes methods that include **Download** or **Upload**, Sentis transfers data to or from the CPU or the GPU. This might slow down the model. 

If your model runs slower than you expect, refer to:

- [Understand models in Sentis](models-concept.md) for information about how the complexity of a model might affect performance.
- [Create an engine to run a model](create-an-engine.md) for information about different types of worker.

## Get output from any layer

To help you profile a model, you can get the output from any layer in a model. Follow these steps: 

1. Get the index of the layer want to output from the model inspector.
2. Use [`Model.AddOutput("layer-name", index)`](xref:Unity.Sentis.Model.AddOutput(System.String)) to add the layer to the model outputs, before you create the worker.
3. Run the model.
4. Use [`IWorker.PeekOutput("layer-name")`](xref:Unity.Sentis.IWorker.PeekOutput) to get the output from the layer.

Only use layer outputs to debug your model. The more layers you add as outputs, the more memory the model uses.

For example, to output from a layer named `ConvolutionLayer`:

```
using UnityEngine;
using Unity.Sentis;

public class GetOutputFromALayer : MonoBehaviour
{
    ModelAsset modelAsset;
    Model runtimeModel;
    IWorker worker;

    void Start()
    {
        // Create an input tensor
        TensorFloat inputTensor = new TensorFloat(new TensorShape(4), new[] { 2.0f, 1.0f, 3.0f, 0.0f });

        // Create the runtime model
        runtimeModel = ModelLoader.Load(modelAsset);

        // Add the layer to the model outputs, the layer index is found in the model inspector
        runtimeModel.AddOutput("ConvolutionLayer", "52");

        // Create a worker
        worker = WorkerFactory.CreateWorker(BackendType.GPUCompute, runtimeModel);

        // Run the model with the input data
        worker.Execute(inputTensor);

        // Get the output from the model
        TensorFloat outputTensor = worker.PeekOutput() as TensorFloat;

        // Get the output from the ConvolutionLayer layer
        TensorFloat convolutionLayerOutputTensor = worker.PeekOutput("ConvolutionLayer") as TensorFloat;
    }
}
```

## Additional resources

- [Inspect a model](inspect-a-model.md)
- [How Sentis optimizes a model](models-concept.md#how-sentis-optimizes-a-model)
- [Supported ONNX operators](supported-operators.md)


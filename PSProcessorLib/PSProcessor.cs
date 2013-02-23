using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace PSProcessorLib
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Custom FX Processor")]
    public class PSProcessor : ContentProcessor<PSSourceCode, CompiledEffectContent>
    {
        public override CompiledEffectContent Process(PSSourceCode input, ContentProcessorContext context)
        {
            EffectContent content = new EffectContent();
            content.EffectCode = input.code;
            EffectProcessor compiler = new EffectProcessor();
            return compiler.Process(content, context);
        }
    }
}
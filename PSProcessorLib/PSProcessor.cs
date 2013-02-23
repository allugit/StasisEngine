using System;
using System.IO;
using System.Diagnostics;
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

            /*
            EffectProcessor compiler = new EffectProcessor();
            CompiledEffectContent compiledEffect = compiler.Process(content, context);
            using (FileStream fs = new FileStream(String.Format("E:\\ShaderDebug\\{0}.fxcode", input.name), FileMode.Create))
            {
                byte[] code = compiledEffect.GetEffectCode();
                fs.Write(code, 0, code.Length);
            }*/

            
            CompiledEffectContent compiledEffect;
            string temporaryFilename = context.IntermediateDirectory + "\\" + input.name + ".fx";
            string fxcPath = "C:\\Program Files (x86)\\Microsoft DirectX SDK (June 2010)\\Utilities\\bin\\x86\\fxc.exe";
            string fxcArguments = "/Tfx_2_0 " + input.filename + " /Fo:" + temporaryFilename;
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(fxcPath, fxcArguments);
            process.Start();
            process.WaitForExit();

            compiledEffect = new CompiledEffectContent(File.ReadAllBytes(temporaryFilename));
            
            return compiledEffect;
        }
    }
}
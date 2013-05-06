using System;
using System.IO;
using System.Collections.Generic;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    public class ScriptManager
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Dictionary<string, ScriptBase> _scripts;
        private CodeDomProvider _provider;
        private CompilerParameters _parameters;
        private string _sourcePrefix = @"
using System;
using StasisCore;
using StasisGame.Managers;
using StasisGame.Systems;
namespace StasisGame
{
    public class Script : ScriptBase
    {
        public Script(SystemManager systemManager, EntityManager entityManager)
            : base(systemManager, entityManager)
        {
        }";
        private string _sourcePostfix = @"}}";

        public Dictionary<string, ScriptBase> scripts { get { return _scripts; } }

        public ScriptManager(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _scripts = new Dictionary<string, ScriptBase>();
            _provider = new CSharpCodeProvider();
            _parameters = new CompilerParameters();
            _parameters.GenerateExecutable = false;
            _parameters.GenerateInMemory = true;
            _parameters.ReferencedAssemblies.Add("StasisGame.exe");
            _parameters.ReferencedAssemblies.Add("StasisCore.dll");

            loadGlobalScript();
            _scripts["global"].doAction("omg");
        }

        // loadGlobalScript -- Loads a global script (called by constructor, which is called just after SystemManager and EntityManager are created)
        private void loadGlobalScript()
        {
            string source = string.Format("{0}{1}{2}", _sourcePrefix, File.ReadAllText("data\\global_script.cs"), _sourcePostfix);
            ScriptBase script = compileAndCreateScript(source);
            _scripts.Add("global", script);
        }

        // loadLevelScript -- Loads a level script (called every time a level is loaded)
        public void loadLevelScript(string levelUID)
        {
            string fileName = string.Format("data\\levels\\{0}.cs", levelUID);

            if (!_scripts.ContainsKey(levelUID) && File.Exists(fileName))
            {
                string source = string.Format("{0}{1}{2}", _sourcePrefix, File.ReadAllText(fileName), _sourcePostfix);
                ScriptBase script = compileAndCreateScript(source);
                _scripts.Add(levelUID, script);
            }
        }

        // compileAndCreateScript -- Takes source code in the form of a string and returns a ScriptBase instance
        private ScriptBase compileAndCreateScript(string source)
        {
            CompilerResults results = _provider.CompileAssemblyFromSource(_parameters, source);
            if (results.Errors.Count > 0)
            {
                Console.WriteLine("Script errors:");
                foreach (CompilerError error in results.Errors)
                {
                    Console.WriteLine("[{0}]: \t{1}", error.Line, error.ErrorText);
                }
            }
            ScriptBase script = (ScriptBase)results.CompiledAssembly.CreateInstance("StasisGame.Script", true, BindingFlags.CreateInstance, null, new object[2] { _systemManager, _entityManager }, System.Globalization.CultureInfo.CurrentCulture, null);
            return script;
        }

        // doAction -- Execute an action that isn't hooked to an event. Level scripts take precedence over global scripts.
        public void doAction(string key, string action)
        {
            bool tryGlobal = true;
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
            {
                tryGlobal = script.doAction(action);
                tryGlobal = key == "global" ? false : tryGlobal;    // prevent trying 'global' twice in a row
            }

            if (tryGlobal)
                _scripts["global"].doAction(action);
        }

        // registerGoals -- Hook for registering goals for a specific level
        public void registerGoals(string key, LevelSystem levelSystem)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.registerGoals(levelSystem);
        }

        // onLevelStart -- Hook for the start of the level
        public void onLevelStart(string key)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.onLevelStart();
        }

        // onLevelEnd -- Hook for the end of a level
        public void onLevelEnd(string key)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.onLevelEnd();
        }

        // onReturnToWorldMap -- Hook called when returning to the world map after a level ends
        public void onReturnToWorldMap(string key, LevelSystem levelSystem)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.onReturnToWorldMap(levelSystem);
        }
    }
}
